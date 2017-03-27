using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.JoggingRoutes.GetSingle
{
    public class GetSingleJoggingRouteQueryHandler : IAsyncRequestHandler<GetSingleJoggingRouteQuery, UpdateJoggingRouteCommand>
    {
        private readonly JoggingTrackerDbContext _dbContext;

        public GetSingleJoggingRouteQueryHandler(JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { dbContext });

            this._dbContext = dbContext;
        }

        public async Task<UpdateJoggingRouteCommand> Handle(GetSingleJoggingRouteQuery query)
        {
            return await this._dbContext
                .JoggingRoutes
                .Where(r => r.Id == query.Id)
                .ProjectTo<UpdateJoggingRouteCommand>()
                .SingleAsync();
        }
    }
}
