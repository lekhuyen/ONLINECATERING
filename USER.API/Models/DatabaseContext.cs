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
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<FavoriteList> FavoriteLists { get; set; }

    }
}
