using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Service.Model;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class GetWeeklyRecordsQueryHandler : IAsyncRequestHandler<GetWeeklyRecordsQuery, List<WeeklyRecord>>
    {
        private readonly JoggingTrackerDbContext _dbContext;

        public GetWeeklyRecordsQueryHandler(JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { dbContext });

            this._dbContext = dbContext;
        }

        public async Task<List<WeeklyRecord>> Handle(GetWeeklyRecordsQuery query)
        {
            var joggingRoutes = await this._dbContext
                .JoggingRoutes
                .Where(r => r.UserId == query.UserId)
                .ToListAsync();

            return joggingRoutes
                .Select(route =>
                {
                    var weekStart = route.StartTime.AddDays(DayOfWeek.Monday - route.StartTime.DayOfWeek);
                    var weekEnd = weekStart.AddDays(6);

                    return new
                    {
                        JoggingRoute = route,
                        FromDate = weekStart.Date,
                        UntilDate = weekEnd.Date,
                    };
                })
                .GroupBy(group => new { group.FromDate, group.UntilDate })
                .Select(group =>
                {
                    var totalDistance = group.Sum(item => item.JoggingRoute.DistanceKilometers);
                    var totalSpeed = group.Sum(item =>
                    {
                        var totalTime = item.JoggingRoute.EndTime - item.JoggingRoute.StartTime;
                        return Math.Round(item.JoggingRoute.DistanceKilometers / (decimal)totalTime.TotalHours, 2, MidpointRounding.AwayFromZero);
                    });
                    var routesCount = group.Count();

                    return new WeeklyRecord
                    {
                        FromDate = group.Key.FromDate,
                        UntilDate = group.Key.UntilDate,
                        AverageDistance = Math.Round(totalDistance / routesCount, 2, MidpointRounding.AwayFromZero),
                        AverageSpeed = Math.Round(totalSpeed / routesCount, 2, MidpointRounding.AwayFromZero)
                    };
                })
                .OrderByDescending(w => w.FromDate)
                .ToList();
        }
    }
}
