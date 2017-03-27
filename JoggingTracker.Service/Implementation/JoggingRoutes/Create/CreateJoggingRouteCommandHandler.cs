using System;
using System.Threading.Tasks;
using AutoMapper;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Model.Entity;
using JoggingTracker.Shared.Helper;
using MediatR;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class CreateJoggingRouteCommandHandler : IAsyncRequestHandler<CreateJoggingRouteCommand, UpdateJoggingRouteCommand>
    {
        private readonly IMapper _mapper;
        private readonly JoggingTrackerDbContext _dbContext;

        public CreateJoggingRouteCommandHandler(IMapper mapper, JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { mapper, dbContext });

            this._mapper = mapper;
            this._dbContext = dbContext;
        }

        public async Task<UpdateJoggingRouteCommand> Handle(CreateJoggingRouteCommand command)
        {
            var joggingRoute = new JoggingRoute()
            {
                Id = Guid.NewGuid(),
                DistanceKilometers = command.DistanceKilometers.Value,
                StartTime = command.StartTime.Value,
                EndTime = command.EndTime.Value,
                Date = command.StartTime.Value.Date,
                UserId = command.UserId
            };

            this._dbContext.JoggingRoutes.Add(joggingRoute);
            await this._dbContext.SaveChangesAsync();

            return this._mapper.Map<UpdateJoggingRouteCommand>(joggingRoute);
        }
    }
}
