using JoggingTracker.DataAccess.Extension;
using JoggingTracker.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoggingTracker.DataAccess.ModelConfiguration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        private const string TableName = "UserRoles";

        public void Configure(EntityTypeBuilder<UserRole> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable(UserRoleConfiguration.TableName);

            entityTypeBuilder.HasKey(p => new { p.UserId, p.RoleId });

            entityTypeBuilder.HasOne(p => p.User)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entityTypeBuilder.HasOne(p => p.Role)
                .WithMany(p => p.UserRoles)
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
