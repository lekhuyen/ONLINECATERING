using Microsoft.EntityFrameworkCore;

namespace BOOKING.API.Models
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext(DbContextOptions options): base(options) { }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    //modelBuilder.Entity<UserBooking>()
        //    //    .HasOne(u => u.Booking)
        //    //    .WithOne(u => u.UserBooking)
        //    //    .HasForeignKey<UserBooking>(u => u.BookingId);

        //    //modelBuilder.Entity<Booking>()
        //    //    .HasOne(u => u.UserBooking)
        //    //    .WithOne(u => u.Booking)
        //    //    .HasForeignKey<UserBooking>(u => u.BookingId);
        //}
        public DbSet<Booking> Bookings { get; set; }
        //public DbSet<UserBooking> UserBookings { get; set; }
    }
}
