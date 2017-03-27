using JoggingTracker.DataAccess.Extension;
using JoggingTracker.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoggingTracker.DataAccess.ModelConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private const string TableName = "Users";

        public void Configure(EntityTypeBuilder<User> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable(UserConfiguration.TableName);

            entityTypeBuilder.HasKey(p => p.Id);
            entityTypeBuilder.HasAlternateKey(p => p.Username);
            entityTypeBuilder.HasAlternateKey(p => p.Email);

            entityTypeBuilder.Property(p => p.Username)
                .IsRequired()
                .HasMaxLength(User.UsernameLengthMax);

            entityTypeBuilder.Property(p => p.Email)
                .IsRequired()
                .HasMaxLength(User.EmailLengthMax);

            entityTypeBuilder.Property(p => p.Password)
                .IsRequired();

            entityTypeBuilder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(User.NameLengthMax);

            entityTypeBuilder.Property(p => p.Surname)
                .IsRequired()
                .HasMaxLength(User.SurnameLengthMax);

            entityTypeBuilder.Property(p => p.BirthDate)
                .IsRequired();

            entityTypeBuilder.Property(p => p.Gender)
                .IsRequired();

            entityTypeBuilder.Property(p => p.RegistrationDate)
                .IsRequired();
        }
    }
}
