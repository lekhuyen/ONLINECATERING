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
        public DbSet<Service> Services { get; set;}

    }
}
