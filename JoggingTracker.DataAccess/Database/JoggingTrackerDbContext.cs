using JoggingTracker.DataAccess.Extension;
using JoggingTracker.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.DataAccess.Database
{
    public class JoggingTrackerDbContext : DbContext
    {
        public JoggingTrackerDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseEntityTypeConfiguration();
        }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<JoggingRoute> JoggingRoutes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
