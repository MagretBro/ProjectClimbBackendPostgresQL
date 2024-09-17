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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
            .HasKey(c => c.Id);

            modelBuilder.Entity<Country>()
            .HasMany(c => c.Regions)  // Категория имеет много продуктов
            .WithOne(p => p.Country)  // Продукт имеет одну категорию
            .HasForeignKey(p => p.CountryId); // Внешний ключ в таблице Products

            // Настройка сущности Region
            modelBuilder.Entity<Region>()
                .HasKey(p => p.Id);
        }

    }
}