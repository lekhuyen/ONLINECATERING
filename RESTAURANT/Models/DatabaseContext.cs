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

            //modelBuilder.Entity<Menu>()
            //    .HasMany(m => m.MenuImages)
            //    .WithOne( m => m.Menu)
            //    .HasForeignKey( m => m.MenuId); 


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
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Payment)
                .WithOne(p => p.Order)
                .HasForeignKey<Payment>(p => p.OrderId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.CustomCombo)
                .WithOne(cc => cc.Order)
                .HasForeignKey<Order>(o => o.ComboCustomId);

            /*modelBuilder.Entity<Order>()
                .HasMany(o => o.Promotions)
                .WithMany(p => p.Order)
                .UsingEntity(j => j.ToTable("OrderPromotions"));*/

            //CustomCombo
            modelBuilder.Entity<CustomCombo>()
                .HasOne(cc => cc.Dish)
                .WithOne(d => d.CustomCombo)
                .HasForeignKey<CustomCombo>(cc => cc.DishId);

            modelBuilder.Entity<CustomCombo>()
                .HasOne(cc => cc.User)
                .WithOne(u => u.CustomCombo)
                .HasForeignKey<CustomCombo>(cc => cc.UserId);

            // Service
            modelBuilder.Entity<Service>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<LobbyImages>()
               .HasOne(li => li.Lobby) // Navigation property to Lobby
               .WithMany(l => l.LobbyImages) // One lobby can have many images
               .HasForeignKey(li => li.LobbyId); // Foreign key property in LobbyImages

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

    }
}
