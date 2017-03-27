using System;
using System.Collections.Generic;
using JoggingTracker.Service.Model;
using MediatR;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class GetWeeklyRecordsQuery : IRequest<List<WeeklyRecord>>
    {
        public Guid UserId { get; set; }
    }
}
