using Braavos.Core.Infrastructure;
using Braavos.Core.Repositories.DataObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Repositories.DbContexts
{
    public class CybernationsDbContext : DbContext, ICybernationsDbContext
    {
        private readonly string _connectionString;

        public DbSet<Nation> Nations { get; set; }

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
        }
    }
}
