using System.Linq;
using System.Threading.Tasks;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class DeleteJoggingRouteCommandHandler : IAsyncRequestHandler<DeleteJoggingRouteCommand>
    {
        private readonly JoggingTrackerDbContext _dbContext;

        public DeleteJoggingRouteCommandHandler(JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { dbContext });

            this._dbContext = dbContext;
        }

        public async Task Handle(DeleteJoggingRouteCommand command)
        {
            var joggingRoute = await this._dbContext
                .JoggingRoutes
                .Where(r => r.Id == command.Id)
                .SingleAsync();

            this._dbContext.JoggingRoutes.Remove(joggingRoute);
            await this._dbContext.SaveChangesAsync();
        }
    }
}
