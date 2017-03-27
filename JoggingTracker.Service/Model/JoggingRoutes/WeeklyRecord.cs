using System;

namespace JoggingTracker.Service.Model
{
    public class WeeklyRecord
    {
        public DateTime FromDate { get; set; }
        public DateTime UntilDate { get; set; }
        public decimal AverageSpeed { get; set; }
        public decimal AverageDistance { get; set; }
    }
}
