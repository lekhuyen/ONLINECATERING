using Microsoft.EntityFrameworkCore;

namespace RESTAURANT.API.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            

            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.Category)
                .WithMany(r => r.Restaurants)
                .HasForeignKey(r => r.CategoryId);

            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Rating)
                .WithOne(r => r.Restaurant)
                .HasForeignKey(r => r.RestaurantId);

            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Comment)
                .WithOne(r => r.Restaurant)
                .HasForeignKey(r => r.RestaurantId);

            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.RestaurantImages)
                .WithOne(r => r.Restaurant)
                .HasForeignKey(r => r.RestaurantId);

            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Menus)
                .WithOne(r => r.Restaurant)
                .HasForeignKey(r => r.RestaurantId);

            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Descriptions)
                .WithOne(r => r.Restaurant)
                .HasForeignKey(r => r.RestaurantId);

            modelBuilder.Entity<Restaurant>()
                .HasMany(r => r.Bookings)
                .WithOne(r => r.Restaurant)
                .HasForeignKey(r => r.RestaurantId);

            


            modelBuilder.Entity<Comment>()
                .HasMany(m => m.CommentChildren)
                .WithOne(m => m.Comment)
                .HasForeignKey(m => m.CommentId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(b => b.Booking)
                .HasForeignKey(b => b.UserId);
            
            // ComboDish
            modelBuilder.Entity<ComboDish>()
             .HasKey(cd => new { cd.DishId, cd.ComboId });

            modelBuilder.Entity<ComboDish>()
                .HasOne(cd => cd.Dish)
                .WithMany(d => d.ComboDishes)
                .HasForeignKey(cd => cd.DishId);

            modelBuilder.Entity<ComboDish>()
                .HasOne(cd => cd.Combo)
                .WithMany(c => c.ComboDishes)
                .HasForeignKey(cd => cd.ComboId);

            //Order
            modelBuilder.Entity<Order>()
               .HasOne(o => o.User)
               .WithMany(u => u.Orders)
               .HasForeignKey(o => o.UserId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.CustomCombo)
                .WithOne(cc => cc.Order)
                .HasForeignKey<CustomCombo>(o => o.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Promotion)
                .WithOne(cc => cc.Order)
                .HasForeignKey<Order>(o => o.PromotionId);

            // Orderdish
            modelBuilder.Entity<OrderDish>()
                .HasKey(od => new { od.DishId, od.OrderId });

            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDishes)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict cascade delete for Order

            modelBuilder.Entity<OrderDish>()
                .HasOne(od => od.Dish)
                .WithMany(d => d.OrderDishes)
                .HasForeignKey(od => od.DishId)
                .OnDelete(DeleteBehavior.Restrict);


            //CustomCombo
            modelBuilder.Entity<CustomCombo>()
               .HasKey(od => new { od.DishId, od.UserId });

            modelBuilder.Entity<CustomCombo>()
                .HasOne(cc => cc.Dish)
                .WithMany(d => d.CustomCombos)
                .HasForeignKey(cc => cc.DishId);

            modelBuilder.Entity<CustomCombo>()
                .HasOne(cc => cc.User)
                .WithMany(u => u.CustomCombos)
                .HasForeignKey(cc => cc.UserId);

            // Service
            modelBuilder.Entity<Service>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<LobbyImages>()
               .HasOne(li => li.Lobby) // Navigation property to Lobby
               .WithMany(l => l.LobbyImages) // One lobby can have many images
               .HasForeignKey(li => li.LobbyId); // Foreign key property in LobbyImages


            modelBuilder.Entity<ComboAppetizer>()
               .HasKey(od => new { od.ComboId, od.AppetizerId });
            modelBuilder.Entity<ComboAppetizer>()
                .HasOne(c => c.Combo)
                .WithMany(c => c.ComboAppetizers)
                .HasForeignKey(c => c.ComboId);
            modelBuilder.Entity<ComboAppetizer>()
                .HasOne(c => c.Appetizer)
                .WithMany(c => c.ComboAppetizers)
                .HasForeignKey(c => c.AppetizerId);


            modelBuilder.Entity<ComboDessert>()
              .HasKey(od => new { od.ComboId, od.DessertId });
            modelBuilder.Entity<ComboDessert>()
                .HasOne(c => c.Dessert)
                .WithMany(c => c.ComboDesserts)
                .HasForeignKey(c => c.DessertId);
            modelBuilder.Entity<ComboDessert>()
                .HasOne(c => c.Combo)
                .WithMany(c => c.ComboDesserts)
                .HasForeignKey(c => c.ComboId);

            modelBuilder.Entity<Order>()
                .HasOne(c => c.Combo)
                .WithMany(c => c.Order)
                .HasForeignKey(c => c.ComboId);

            modelBuilder.Entity<Combo>()
                .HasMany(c => c.Promotions)
                .WithOne(c => c.Combo)
                .HasForeignKey(c => c.ComboId);

            modelBuilder.Entity<Lobby>()
                .HasMany(c => c.Order)
                .WithOne(c => c.Lobby)
                .HasForeignKey(c => c.LobbyId);

            modelBuilder.Entity<Dish>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.Dish)
                .HasForeignKey(c => c.DishId);
            modelBuilder.Entity<Appetizer>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.Appetizer)
                .HasForeignKey(c => c.AppetizerId);

            modelBuilder.Entity<Dessert>()
                .HasMany(c => c.Comments)
                .WithOne(c => c.Dessert)
                .HasForeignKey(c => c.DessertId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(c => c.Comments)
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<CommentChild>()
                .HasOne(c => c.User)
                .WithMany(c => c.CommentChildren)
                .HasForeignKey(c => c.UserId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Appetizer>()
                .HasMany(r => r.Rating)
                .WithOne(r => r.Appetizer)
                .HasForeignKey(r => r.AppetizerId);

            modelBuilder.Entity<Dish>()
                .HasMany(r => r.Rating)
                .WithOne(r => r.Dish)
                .HasForeignKey(r => r.DishId);

            modelBuilder.Entity<Dessert>()
                .HasMany(r => r.Rating)
                .WithOne(r => r.Dessert)
                .HasForeignKey(r => r.DessertId);
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.User)
                .WithMany(r => r.Ratings)
                .HasForeignKey(r => r.UserId);

        }

        public DbSet<Restaurant> Restaurants { get; set;}
        public DbSet<Category> Categories { get; set;}
        public DbSet<Description> Descriptions { get; set;}
        public DbSet<Menu> Menus { get; set;}
        public DbSet<Rating> Ratings { get; set;}
        public DbSet<Comment> Comments { get; set;}
        public DbSet<Booking> Booking { get; set;}
        public DbSet<MenuImages> MenuImages { get; set;}
        public DbSet<User> User { get; set;}
        public DbSet<CommentChild> CommentChildren { get; set;}
        public DbSet<RestaurantImages> RestaurantImages { get; set;}


        //Order
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboDish> ComboDishes { get; set; }
        public DbSet<CustomCombo> CustomCombos { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set;}
        public DbSet<Lobby> Lobbies { get; set; }
        public DbSet<LobbyImages> LobbiesImages { get; set; }

        public DbSet<OrderDish> OrderDishes { get; set; }
        public DbSet<Dessert> Desserts { get; set; }
        public DbSet<Appetizer> Appetizers { get; set; }
        public DbSet<ComboAppetizer> ComboAppetizers { get; set; }
        public DbSet<ComboDessert> ComboDesserts { get; set; }

    }
}
