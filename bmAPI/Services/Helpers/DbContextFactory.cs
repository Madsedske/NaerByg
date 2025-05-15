using bmAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace bmAPI.Services.Helpers
{
    public class DbContextFactory :IDbContextFactory
    {
        private readonly IConfiguration _configuration;

        public DbContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DatabaseContext Create(int chainId)
        {
            var connCheck = new[] { "db_staerk", "db_harrag_nybold", "db_jex_og_fim" };
            foreach (var conn in connCheck)
            {
                if (string.IsNullOrWhiteSpace(_configuration.GetConnectionString(conn)))
                    throw new InvalidOperationException($"Connection string '{conn}' is missing!");
            }

            var connectionName = chainId switch
            {
                1 => "db_staerk",
                2 => "db_harrag_nybold",
                3 => "db_jex_og_fim",
                _ => throw new ArgumentException("Invalid Chain Id.")
            };

            var connectionString = _configuration.GetConnectionString(connectionName);

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException($"Connection string '{connectionName}' not found.");

            Console.WriteLine($"Bruger connection string for chainId {chainId}: {connectionString}");

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
