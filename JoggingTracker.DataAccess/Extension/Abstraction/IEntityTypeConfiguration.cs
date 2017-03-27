using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JoggingTracker.DataAccess.Extension
{
    public interface IEntityTypeConfiguration<T> where T : class
    {
        void Configure(EntityTypeBuilder<T> entityTypeBuilder);
    }
}
