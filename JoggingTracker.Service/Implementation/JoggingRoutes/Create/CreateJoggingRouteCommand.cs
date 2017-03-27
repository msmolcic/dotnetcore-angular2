using System;
using MediatR;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class CreateJoggingRouteCommand : IRequest<UpdateJoggingRouteCommand>
    {
        public decimal? DistanceKilometers { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Guid UserId { get; set; }
    }
}
