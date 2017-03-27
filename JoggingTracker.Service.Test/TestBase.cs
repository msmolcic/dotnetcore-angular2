using System;
using JoggingTracker.DataAccess.Database;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Test
{
    public abstract class TestBase
    {
        protected readonly DbContextOptions<JoggingTrackerDbContext> _options;

        public TestBase()
        {
            var builder = new DbContextOptionsBuilder<JoggingTrackerDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _options = builder.Options;
        }
    }
}
