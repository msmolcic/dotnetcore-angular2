using System;
using MediatR;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class DeleteJoggingRouteCommand : IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
    }
}
