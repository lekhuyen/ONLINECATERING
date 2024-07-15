using Microsoft.EntityFrameworkCore;

namespace USER.API.Models
{
    public class DatabaseContext:DbContext
    {
        
        public DatabaseContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteLists)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .HasOne(u => u.Grade)
                .WithOne(u => u.User)
                .HasForeignKey<Grade>(u => u.UserId);
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserBookings)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);


            modelBuilder.Entity<MenuBooking>()
                .HasKey(m => new { m.MenuId, m.BookingId });
            modelBuilder.Entity<MenuBooking>()
                .HasOne(m => m.Booking)
                .WithMany(m => m.MenuBookings)
                .HasForeignKey(m => m.BookingId);

            modelBuilder.Entity<MenuBooking>()
                .HasOne(m => m.Menu)
                .WithMany(m => m.MenuBookings)
                .HasForeignKey(m => m.MenuId);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<FavoriteList> FavoriteLists { get; set; }
        public DbSet<Booking> UserBooking { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuBooking> MenuBookings { get; set; }



    }
}
