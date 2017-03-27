using JoggingTracker.DataAccess.Extension;
using JoggingTracker.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoggingTracker.DataAccess.ModelConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        private const string TableName = "Roles";

        public void Configure(EntityTypeBuilder<Role> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable(RoleConfiguration.TableName);

            entityTypeBuilder.HasKey(p => p.Id);
            entityTypeBuilder.HasAlternateKey(p => p.Name);

            entityTypeBuilder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(Role.NameLengthMax);
        }
    }
}
