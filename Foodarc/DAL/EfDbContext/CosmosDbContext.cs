using Microsoft.EntityFrameworkCore;

namespace Foodarc.DAL.EfDbContext { 

    public class CosmosDbContext : DbContext
    {
        public CosmosDbContext(DbContextOptions<CosmosDbContext> options)
            : base(options) {
            Database.EnsureCreated();
        }

        public DbSet<DbBasket> Baskets { get; set; } 
        public DbSet<DbOrder> Orders { get; set; }
        public DbSet<DbRestaurant> Restaurant { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<DbBasket>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<DbBasket>()
                .Property(p => p.UserId);
            modelBuilder.Entity<DbBasket>()
                .ToContainer("Baskets");
            modelBuilder.Entity<DbBasket>()
                 .HasNoDiscriminator();
            modelBuilder.Entity<DbBasket>()
                .HasPartitionKey(b => b.UserId);
            modelBuilder.Entity<DbBasket>()
                .Property(b => b.LastEdited);
            modelBuilder.Entity<DbBasket>()
                .Property(b => b.TotalCost);
            modelBuilder.Entity<DbBasket>()
                .OwnsMany(b => b.Foods, y => {
                    y.OwnsOne(food => food.OrderedFood);
                });
            //modelBuilder.HasDefaultContainer("Baskets");

            //modelBuilder.Entity<DbFood>()
            //    .Property(p => p.Name).HasMaxLength(150);
            //modelBuilder.Entity<DbFood>()
            //    .Property(p => p.Description).HasMaxLength(1000);
            //modelBuilder.Entity<DbFood>()
            //    .Property(p => p.Price);
            //modelBuilder.Entity<DbFood>()
            //    .Property(p => p.Calories);
            //modelBuilder.Entity<DbFood>()
            //    .Property(p => p.ImagePath);
            //
            //modelBuilder.Entity<DbBasketFood>()
            //    .Property(p => p.AddTime);
            //modelBuilder.Entity<DbBasketFood>()
            //    .Property(p => p.RestaurantUrl);
            //modelBuilder.Entity<DbBasketFood>()
            //    .OwnsOne(p => p.OrderedFood);

            modelBuilder.Entity<DbOrder>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<DbOrder>()
                    .HasNoDiscriminator();
            modelBuilder.Entity<DbOrder>()
                .ToContainer("Orders");
            modelBuilder.Entity<DbOrder>()
                .HasPartitionKey(p => p.UserId);
            modelBuilder.Entity<DbOrder>()
                .Property(p => p.OrderDate);
            //modelBuilder.Entity<DbOrder>()
            //    .OwnsMany(p => p.Orders, y => {
            //        y.OwnsMany(food => food.Foods, x =>
            //        {
            //            x.OwnsOne(f => f.OrderedFood);
            //        });
            //    });

            modelBuilder.Entity<DbRestaurant>()
                .HasKey(p => p.Id);
            modelBuilder.Entity<DbRestaurant>()
                .ToContainer("Restaurants");
            modelBuilder.Entity<DbRestaurant>()
                    .HasNoDiscriminator();
            modelBuilder.Entity<DbRestaurant>()
                .HasPartitionKey(p => p.Id);
            modelBuilder.Entity<DbRestaurant>()
                .Property(p => p.ZipCode);
            modelBuilder.Entity<DbRestaurant>()
                .Property(p => p.City).HasMaxLength(100);
            modelBuilder.Entity<DbRestaurant>()
                .Property(p => p.Country).HasMaxLength(100);
            modelBuilder.Entity<DbRestaurant>()
                .Property(p => p.Address).HasMaxLength(500);
            modelBuilder.Entity<DbRestaurant>()
                .Property(p => p.Name).HasMaxLength(200);
            modelBuilder.Entity<DbRestaurant>()
                .Property(p => p.OwnerId).IsRequired(required: true);
            modelBuilder.Entity<DbRestaurant>()
                .Property(p => p.Description).HasMaxLength(500);
            modelBuilder.Entity<DbRestaurant>()
                .Property(p => p.ImagePath);
            modelBuilder.Entity<DbRestaurant>()
                .OwnsMany(p => p.Orders);
                
            //modelBuilder.Entity<DbRestaurant>()
            //.OwnsOne(p => p.Orders 
            // ,x =>
            // {
            //     x.ToJsonProperty("orders");
            //     x.WithOwner();
            //     x.Property(p => p.OrderDate);
            //     x.Property(p => p.UserId);
            //     x.Property(p => p.Id);
            //     x.OwnsMany(order => order.Orders, y =>
            //     {
            //         y.ToJsonProperty("orders");
            //         y.OwnsMany(foods => foods.Foods, z =>
            //         {
            //             z.ToJsonProperty("foods");
            //             z.OwnsOne(f => f.OrderedFood).WithOwner();
            //         }).WithOwner();
            //     }).WithOwner();
            // }
            //);
            modelBuilder.Entity<DbRestaurant>()
                .OwnsMany(p => p.AvailableFoods);

            modelBuilder.HasManualThroughput(1000);
        }
         
    }
}

