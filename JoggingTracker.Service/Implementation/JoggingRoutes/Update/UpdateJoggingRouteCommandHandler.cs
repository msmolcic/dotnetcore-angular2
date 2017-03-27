using System.Linq;
using System.Threading.Tasks;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class UpdateJoggingRouteCommandHandler : IAsyncRequestHandler<UpdateJoggingRouteCommand>
    {
        private readonly JoggingTrackerDbContext _dbContext;

        public UpdateJoggingRouteCommandHandler(JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { dbContext });

            this._dbContext = dbContext;
        }

        public async Task Handle(UpdateJoggingRouteCommand command)
        {
            var joggingRoute = await this._dbContext
                .JoggingRoutes
                .Where(r => r.Id == command.Id)
                .SingleAsync();

            joggingRoute.DistanceKilometers = command.DistanceKilometers.Value;
            joggingRoute.StartTime = command.StartTime.Value;
            joggingRoute.EndTime = command.EndTime.Value;
            joggingRoute.Date = command.StartTime.Value.Date;

            this._dbContext.Entry(joggingRoute).State = EntityState.Modified;
            await this._dbContext.SaveChangesAsync();
        }
    }
}
