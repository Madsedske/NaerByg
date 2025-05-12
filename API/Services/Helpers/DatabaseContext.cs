using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace API.Services.Helpers
{
    /// <summary>
    /// Represents the application's database context, providing access to the underlying database connection 
    /// and configuration for Entity Framework Core.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="DbContext"/> and enforces configuration validation.
    /// Provides a method to create a raw database connection via <see cref="CreateConnection"/>.
    /// </remarks>
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<ApiSyncLog> ApiSyncLog { get; set; }


        /// <summary>
        /// Returns the underlying database connection as an <see cref="IDbConnection"/>.
        /// </summary>
        public IDbConnection CreateConnection()
        {
            return Database.GetDbConnection();
        }

        /// <summary>
        /// Validates that the database context is properly configured.
        /// Throws an exception if no configuration is found.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new InvalidOperationException("Database connection is not configured.");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguration af ApiSyncLog tabellen
            modelBuilder.Entity<ApiSyncLog>(entity =>
            {
                entity.HasKey(e => e.ApiSyncLogId);
                entity.Property(e => e.DataObject).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
            });
        }

    }
}
