// SyncService implementation for handling data synchronization
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using API.Services.Interfaces;
using API.Services.Helpers;
using System.Text.RegularExpressions;
using API.Enums;
using API.Models;
using Shared.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MySqlConnector;

public class SyncService : ISyncService
{
    private readonly DatabaseContext _databaseContext;
    private readonly IProviderDispatcherService _providerDispatcherService;

    public SyncService(DatabaseContext dbContext, IProviderDispatcherService providerDispatcherService)
    {
        _databaseContext = dbContext;
        _providerDispatcherService = providerDispatcherService;
    }

    // Syncs data for a specific chain using the provided token
    public async Task SyncProviderData(int chainId, string token)
    {
        // Looping through all data objects except null
        foreach (DataObjectType dataObject in Enum.GetValues(typeof(DataObjectType)))
        {
            try
            {
                DateTime lastSynced = await GetLastSynced(chainId, dataObject);

                var data = await _providerDispatcherService.GetDataAsync(
                    dataObject,
                    new ProviderRequest { ChainId = chainId, LastSynced = lastSynced },
                    token
                );

                if (data != null && data is IEnumerable<object> dataList && dataList.Any())
                {
                    var procedureName = Regex.Replace(dataObject.ToString(), "([a-z])([A-Z])", "$1_$2").ToLower();
                    int rowsUpdated = await UpdateDatabase(chainId, procedureName, dataList);
                    await LogSync(chainId, dataObject.ToString(), "success", $"Opdaterede {rowsUpdated} rækker.");
                }
                else
                {
                    await LogSync(chainId, dataObject.ToString(), "skipped", "Ingen ny data.");
                }
            }
            catch (Exception ex)
            {
                await LogSync(chainId, dataObject.ToString(), "error", $"Fejl: {ex.Message}");
            }
        }
    }

    // Fetches the last successful sync datetime from the database
    public async Task<DateTime> GetLastSynced(int chainId, DataObjectType dataObject)
    {
        return await _databaseContext.ApiSyncLog
            .Where(log => log.ChainId == chainId && log.DataObject == dataObject.ToString() && log.Status == "success")
            .OrderByDescending(log => log.SyncedAt)
            .Select(log => log.SyncedAt)
            .FirstOrDefaultAsync();
    }

    // Updates the database using a stored procedure via raw SQL
    public async Task<int> UpdateDatabase(int chainId, string procedureName, object providerData)
    {
        try
        {
            var json = System.Text.Json.JsonSerializer.Serialize(providerData);

            var sql = $"CALL GetData_{procedureName}(@chainId, @jsonData)";
            var parameters = new[]
            {
            new MySqlParameter("@chainId", chainId),
            new MySqlParameter("@jsonData", json)
        };

            return await _databaseContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        catch (Exception ex)
        {
            throw new Exception($"Fejl ved databaseopdatering ({procedureName}): {ex.Message}");
        }
    }


    // Logs the sync status to the database
    public async Task LogSync(int chainId, string dataObject, string status, string message)
    {
        _databaseContext.ApiSyncLog.Add(new ApiSyncLog
        {
            ChainId = chainId,
            DataObject = dataObject,  // Kan være null ved generelle fejl
            SyncedAt = DateTime.UtcNow,
            Status = status,
            Message = message
        });

        await _databaseContext.SaveChangesAsync();
    }
}
