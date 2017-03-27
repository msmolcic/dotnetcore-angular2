using JoggingTracker.DataAccess.Extension;
using JoggingTracker.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoggingTracker.DataAccess.ModelConfiguration
{
    public class JoggingRouteConfiguration : IEntityTypeConfiguration<JoggingRoute>
    {
        private const string TableName = "JoggingRoutes";

        public void Configure(EntityTypeBuilder<JoggingRoute> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable(JoggingRouteConfiguration.TableName);

            entityTypeBuilder.HasKey(p => p.Id);

            entityTypeBuilder.Property(p => p.DistanceKilometers)
                .IsRequired();
            entityTypeBuilder.Property(p => p.StartTime)
                .IsRequired();
            entityTypeBuilder.Property(p => p.EndTime)
                .IsRequired();
            entityTypeBuilder.Property(p => p.UserId)
                .IsRequired();

            entityTypeBuilder.HasOne(p => p.User)
                .WithMany(p => p.JoggingRoutes)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
