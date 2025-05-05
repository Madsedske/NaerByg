using API.Services.Helpers;
using Microsoft.EntityFrameworkCore;

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
            var connectionName = chainId switch
            {
                1 => "ConnectionStrings:db_staerk",
                2 => "ConnectionStrings:db_harrag_nybold",
                3 => "ConnectionStrings:db_jex_og_fim",
                _ => throw new ArgumentException("Invalid Chain Id.")
            };

            var connectionString = _configuration[connectionName];

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new DatabaseContext(optionsBuilder.Options);
        }
    }
}
