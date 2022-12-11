﻿using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Data
{
    public class RestaurantDbContext : DbContext
    {
        private string _connectionString =
            "Data Source=DESKTOP-ODOFPG3\\MYSSQLSERVER;Initial Catalog=RestaurantAPI;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name);
            modelBuilder.Entity<Dish>()
                .Property(r => r.Name)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(r => r.FirstName);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
