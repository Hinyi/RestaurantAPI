using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Data
{
    public class RestaurantDbContext : DbContext
    {
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {
            
        }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

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
            modelBuilder.Entity<Role>()
                .Property(u => u.Name)
                .IsRequired();
        }
    }
}
