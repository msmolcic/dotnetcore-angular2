using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Service.Model;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class GetJoggingRoutesQueryHandler : IAsyncRequestHandler<GetJoggingRoutesQuery, List<JoggingRouteViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly JoggingTrackerDbContext _dbContext;

        public GetJoggingRoutesQueryHandler(IMapper mapper, JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { mapper, dbContext });

            this._mapper = mapper;
            this._dbContext = dbContext;
        }

        public async Task<List<JoggingRouteViewModel>> Handle(GetJoggingRoutesQuery query)
        {
            var joggingRoutes = this._dbContext
                .JoggingRoutes
                .Where(r => r.UserId == query.UserId);

            if (query.FromDate.HasValue)
                joggingRoutes = joggingRoutes.Where(r => r.Date >= query.FromDate.Value);

            if (query.UntilDate.HasValue)
                joggingRoutes = joggingRoutes.Where(r => r.Date <= query.UntilDate.Value);

            var selectedRoutes = await joggingRoutes
                .OrderByDescending(r => r.Date)
                .ToListAsync();

            return selectedRoutes
                .Select(r => new JoggingRouteViewModel(r))
                .ToList();
        }
    }
}
