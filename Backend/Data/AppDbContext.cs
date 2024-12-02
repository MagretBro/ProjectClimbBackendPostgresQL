using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) 
        {}

        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Massive> Massives { get; set; }
        public DbSet<Sector> Sectors { get; set; }
        public DbSet<ClimbingRoute> ClimbingRoutes { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<User> Users { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<Country>()
                .HasMany(c => c.Regions)  // 
                .WithOne(r => r.Country)  // 
                .HasForeignKey(r => r.CountryId); // 

            // Настройка сущности Region
            modelBuilder.Entity<Region>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Region>()
                .HasMany(r => r.Massives) 
                .WithOne(m => m.Region) 
                .HasForeignKey(m => m.RegionId);

            modelBuilder.Entity<Massive>()
                .HasKey(m => m.Id);

            modelBuilder.Entity<Massive>()
                .HasMany(m => m.Sectors)
                .WithOne(s => s.Massive)
                .HasForeignKey(s => s.MassiveId);

            modelBuilder.Entity<Sector>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Sector>()
                .HasMany(s => s.ClimbingRoutes)
                .WithOne(cr => cr.Sector)
                .HasForeignKey(cr => cr.SectorId);

            modelBuilder.Entity<ClimbingRoute>()
                .HasKey(cr => cr.Id);
        
    // Настройка связи между Sector и Picture через ParentId
            modelBuilder.Entity<Picture>()
            .HasOne<Sector>()
            .WithMany(s => s.Pictures)
            .HasForeignKey(p => p.ParentId);

    // Настройка сущности Picture для использования enum
            modelBuilder.Entity<Picture>(entity =>
            {
                entity.Property(p => p.EntityType)
                    .HasConversion<string>() // Конвертация Enum <-> String
                    .HasMaxLength(50); // Соответствие размеру столбца в БД
            });
            
    // Создание user для аутентификации
            modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        }
    }
}