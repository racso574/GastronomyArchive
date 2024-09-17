using Microsoft.EntityFrameworkCore;
using GastronomyArchive.Models;

namespace GastronomyArchive.Data
{
    public class GastronomyDbContext : DbContext
    {
        public GastronomyDbContext(DbContextOptions<GastronomyDbContext> options) : base(options) { }

        public DbSet<Food> Foods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food>()
                .Property(f => f.CaloriesPer100g)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Food>()
                .Property(f => f.ProteinsPer100g)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Food>()
                .Property(f => f.FatsPer100g)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Food>()
                .Property(f => f.CarbsPer100g)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Food>()
                .Property(f => f.AverageWeightGrams)
                .HasColumnType("decimal(10,2)");
        }
    }
}
