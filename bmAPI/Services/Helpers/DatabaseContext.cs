using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Services.Helpers
{
    public class DatabaseContext : DbContext
    {
        /// <summary>
        /// The database connection string. <-- Old. 
        /// New --> DbContext is changed so it only manages db operations rather than connectionstring and configuration.
        /// </summary>
        /// 
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public IDbConnection CreateConnection()
        {
            return Database.GetDbConnection();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException("Database connection is not configured.");
            }
        }
    }
}
