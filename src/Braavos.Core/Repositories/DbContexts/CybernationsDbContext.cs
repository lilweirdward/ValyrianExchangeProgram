using Braavos.Core.Infrastructure;
using Braavos.Core.Repositories.DataObjects;
using Braavos.Core.Repositories.DbContexts.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Braavos.Core.Repositories.DbContexts
{
    public class CybernationsDbContext : DbContext, ICybernationsDbContext
    {
        private readonly string _connectionString;

        public DbSet<Nation> Nations { get; set; }
        private DbSet<TodaysNationData> TodaysNationData { get; set; }

        public CybernationsDbContext(IOptions<FunctionOptions> options) => _connectionString = options.Value.DbConnectionString;
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Nation>(entity =>
            {
                entity.ToTable("nation");
                entity.HasKey(e => e.NationId);
                entity.Property(e => e.NationId).HasColumnName("id");
                entity.Property(e => e.NationName).HasColumnName("nation_name");
                entity.Property(e => e.RulerName).HasColumnName("ruler_name");
                entity.Property(e => e.AllianceId).HasColumnName("alliance_id");
                entity.Property(e => e.AllianceStatus).HasColumnName("alliance_status");
                entity.Property(e => e.AllianceDate).HasColumnName("alliance_date");
                entity.Property(e => e.GovernmentType).HasColumnName("government_type");
                entity.Property(e => e.Religion).HasColumnName("religion");
                entity.Property(e => e.Team).HasColumnName("team");
                entity.Property(e => e.Created).HasColumnName("created");
                entity.Property(e => e.Technology).HasColumnName("technology");
                entity.Property(e => e.Infrastructure).HasColumnName("infrastructure");
                entity.Property(e => e.BaseLand).HasColumnName("base_land");
                entity.Property(e => e.Votes).HasColumnName("votes");
                entity.Property(e => e.Defcon).HasColumnName("defcon");
                entity.Property(e => e.BaseSoldiers).HasColumnName("base_soldiers");
                entity.Property(e => e.Tanks).HasColumnName("tanks");
                entity.Property(e => e.CruiseMissiles).HasColumnName("cruise_missiles");
                entity.Property(e => e.Nukes).HasColumnName("nukes");
                entity.Property(e => e.RecentActivity).HasColumnName("recent_activity");
                entity.Property(e => e.UpdatedBy).HasColumnName("audit_updated_by");
                entity.Property(e => e.UpdatedOn).HasColumnName("audit_updated_on");
            });

            #region Internal Tables

            modelBuilder.Entity<TodaysNationData>(entity =>
            {
                entity.ToTable("todays_file_nation");
                entity.HasKey(e => e.NationId);
                entity.Property(e => e.NationId).HasColumnName("nation_id");
                entity.Property(e => e.NationName).HasColumnName("nation_name");
                entity.Property(e => e.RulerName).HasColumnName("ruler_name");
                entity.Property(e => e.AllianceId).HasColumnName("alliance_id");
                entity.Property(e => e.AllianceStatus).HasColumnName("alliance_status");
                entity.Property(e => e.AllianceDate).HasColumnName("alliance_date");
                entity.Property(e => e.GovernmentType).HasColumnName("government_type");
                entity.Property(e => e.Religion).HasColumnName("religion");
                entity.Property(e => e.Team).HasColumnName("team");
                entity.Property(e => e.Created).HasColumnName("created");
                entity.Property(e => e.Technology).HasColumnName("technology");
                entity.Property(e => e.Infrastructure).HasColumnName("infrastructure");
                entity.Property(e => e.BaseLand).HasColumnName("base_land");
                entity.Property(e => e.Votes).HasColumnName("votes");
                entity.Property(e => e.Defcon).HasColumnName("defcon");
                entity.Property(e => e.BaseSoldiers).HasColumnName("base_soldiers");
                entity.Property(e => e.Tanks).HasColumnName("tanks");
                entity.Property(e => e.CruiseMissiles).HasColumnName("cruise_missiles");
                entity.Property(e => e.Nukes).HasColumnName("nukes");
                entity.Property(e => e.RecentActivity).HasColumnName("recent_activity");
                entity.Property(e => e.ConnectedResource1).HasColumnName("connected_resource_1");
                entity.Property(e => e.ConnectedResource2).HasColumnName("connected_resource_2");
                entity.Property(e => e.ConnectedResource3).HasColumnName("connected_resource_3");
                entity.Property(e => e.ConnectedResource4).HasColumnName("connected_resource_4");
                entity.Property(e => e.ConnectedResource5).HasColumnName("connected_resource_5");
                entity.Property(e => e.ConnectedResource6).HasColumnName("connected_resource_6");
                entity.Property(e => e.ConnectedResource7).HasColumnName("connected_resource_7");
                entity.Property(e => e.ConnectedResource8).HasColumnName("connected_resource_8");
                entity.Property(e => e.ConnectedResource9).HasColumnName("connected_resource_9");
                entity.Property(e => e.ConnectedResource10).HasColumnName("connected_resource_10");
                entity.Property(e => e.FileName).HasColumnName("file_name");
            });

            #endregion
        }

        public async Task InsertTempData(IReadOnlyCollection<TodaysNationData> todaysNationData)
        {
            // Truncate the table since it's only for temp data
            await Database.ExecuteSqlRawAsync("TRUNCATE TABLE todays_file_nation;");

            // Turn of change detection to speed up EF performance
            ChangeTracker.AutoDetectChangesEnabled = false;

            // Add the data and save
            TodaysNationData.AddRange(todaysNationData);
            await SaveChangesAsync();
        }

        public async Task ExecuteSqlCommand(string sql, params object[] parameters)
        {
            await Database.ExecuteSqlRawAsync(sql, parameters);
        }
    }
}
