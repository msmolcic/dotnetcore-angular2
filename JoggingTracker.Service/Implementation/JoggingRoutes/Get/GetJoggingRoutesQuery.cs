using System;
using System.Collections.Generic;
using JoggingTracker.Service.Model;
using MediatR;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class GetJoggingRoutesQuery : IRequest<List<JoggingRouteViewModel>>
    {
        public Guid UserId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? UntilDate { get; set; }
    }
}
