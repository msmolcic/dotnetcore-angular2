using System;
using JoggingTracker.Model.Entity;

namespace JoggingTracker.Service.Model
{
    public class JoggingRouteViewModel
    {
        public JoggingRouteViewModel(JoggingRoute route)
        {
            this.Id = route.Id;
            this.UserId = route.UserId;
            this.DistanceKilometers = route.DistanceKilometers;

            var totalTime = route.EndTime - route.StartTime;
            this.AverageSpeed = this.CalculateAverageSpeed(route.DistanceKilometers, totalTime);
            this.TotalTime = this.FormatTotalTime(totalTime);
            this.Date = route.Date;
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal DistanceKilometers { get; set; }
        public decimal AverageSpeed { get; set; }
        public string TotalTime { get; set; }
        public DateTime Date { get; set; }

        private decimal CalculateAverageSpeed(decimal distance, TimeSpan time)
        {
            return Math.Round(distance / (decimal)time.TotalHours, 2, MidpointRounding.AwayFromZero);
        }

        private string FormatTotalTime(TimeSpan time)
        {
            var totalTime = string.Empty;

            if (time.Days > 0)
                totalTime += $"{time.Days}d ";

            if (time.Hours > 0)
                totalTime += $"{time.Hours}h ";

            if (time.Minutes > 0)
                totalTime += $"{time.Minutes}m ";

            if (time.Seconds > 0)
                totalTime += $"{time.Seconds}s ";

            return totalTime.TrimEnd();
        }
    }
}
