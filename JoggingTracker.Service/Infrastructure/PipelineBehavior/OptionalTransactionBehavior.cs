using System.Threading.Tasks;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Service.Infrastructure.Attributes;
using JoggingTracker.Service.Infrastructure.Extension;
using JoggingTracker.Shared.Helper;
using MediatR;

namespace JoggingTracker.Service.Infrastructure.PipelineBehavior
{
    public class OptionalTransactionBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly JoggingTrackerDbContext _dbContext;

        public OptionalTransactionBehavior(JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { dbContext });

            this._dbContext = dbContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            if (!typeof(TRequest).HasAttribute<RequireTransactionAttribute>())
                return await next();

            using (var transaction = await this._dbContext.Database.BeginTransactionAsync())
            {
                var result = await next();

                transaction.Commit();

                return result;
            }
        }
    }
}
