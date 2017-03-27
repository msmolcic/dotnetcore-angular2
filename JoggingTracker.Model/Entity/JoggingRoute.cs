using System;

namespace JoggingTracker.Model.Entity
{
    public class JoggingRoute : EntityBase
    {
        public decimal DistanceKilometers { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime Date { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
