using System;
using MediatR;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class GetSingleJoggingRouteQuery : IRequest<UpdateJoggingRouteCommand>
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
