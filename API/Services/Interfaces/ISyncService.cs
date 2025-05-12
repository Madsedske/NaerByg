using API.Enums;

namespace API.Services.Interfaces
{
    public interface ISyncService
    {
        /// <summary>
        /// Synchronizes data for a specific chain ID using the provided token.
        /// </summary>
        /// <param name="chainId">The ID of the chain to sync.</param>
        /// <param name="token">The authentication token for the external API.</param>
        Task SyncProviderData(int chainId, string token);

        /// <summary>
        /// Gets the last successful sync datetime for a specific chain and data object.
        /// </summary>
        /// <param name="chainId">The ID of the chain.</param>
        /// <param name="dataObject">The type of data object being synced.</param>
        /// <returns>The last successful sync datetime, or null if none found.</returns>
        Task<DateTime> GetLastSynced(int chainId, DataObjectType dataObject);

        /// <summary>
        /// Updates the database for a specific chain and data object.
        /// </summary>
        /// <param name="chainId">The ID of the chain.</param>
        /// <param name="procedureName">The name of the stored procedure to call.</param>
        /// <param name="data">The data to update in the database.</param>
        /// <returns>The number of rows updated.</returns>
        Task<int> UpdateDatabase(int chainId, string procedureName, object data);

        /// <summary>
        /// Logs the synchronization status for a specific chain and data object.
        /// </summary>
        /// <param name="chainId">The ID of the chain.</param>
        /// <param name="dataObject">The type of data object (or null for general errors).</param>
        /// <param name="status">The status of the sync (success, skipped, error).</param>
        /// <param name="message">A message providing details about the sync.</param>
        Task LogSync(int chainId, string? dataObject, string status, string message);
    }
}
