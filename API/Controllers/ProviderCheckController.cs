﻿using API.Enums;
using API.Services;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MySqlConnector;
using Shared.DTOs;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderCheckController : ControllerBase
    {
        private readonly IProviderCheckService _providerCheckService;
        private readonly IProviderDispatcherService _providerDispatcherService;
        private readonly ISyncService _syncService;

        public ProviderCheckController(IProviderCheckService ProviderCheckService, IProviderDispatcherService ProviderDispatcherService, ISyncService SyncService)
        {
            _providerDispatcherService = ProviderDispatcherService;
            _providerCheckService = ProviderCheckService;
            _syncService = SyncService;
        }

        [HttpPost("GetProviderData")]
        public async Task<IActionResult> GetProviderData([FromQuery] int chainId)
        {
            try
            {
                var u = Environment.GetEnvironmentVariable("var_username");
                var p = Environment.GetEnvironmentVariable("var_password");

                if (string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
                {
                    await _syncService.LogSync(chainId, null, "error", "Credentials not found");
                    return BadRequest("Credentials not found");
                }

                var authResponse = await _providerCheckService.LoginAsync(u, p);
                if (authResponse == null)
                {
                    await _syncService.LogSync(chainId, null, "error", "Authentication failed");
                    return Unauthorized("Authentication failed");
                }

                await _syncService.SyncProviderData(chainId, authResponse.Token);
                return Ok("Data sync completed.");
            }
            catch (Exception ex)
            {
                await _syncService.LogSync(chainId, null, "error", $"Unexpected error: {ex.Message}");
                return StatusCode(500, $"Unexpected error: {ex.Message}");
            }
        }

    }
}
