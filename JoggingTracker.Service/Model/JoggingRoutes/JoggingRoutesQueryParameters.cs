using System;

namespace JoggingTracker.Service.Model
{
    public class JoggingRoutesQueryParameters
    {
        public DateTime? FromDate { get; set; }
        public DateTime? UntilDate { get; set; }
    }
}
