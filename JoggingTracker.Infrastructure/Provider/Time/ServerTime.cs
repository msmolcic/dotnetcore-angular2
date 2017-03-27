using System;

namespace JoggingTracker.Infrastructure.Provider.Time
{
    /// <summary>
    /// Provides the access to the server time.
    /// </summary>
    public class ServerTime : IServerTime
    {
        /// <summary>
        /// Gets a DateTime structure that is set to the current date and time on the server machine,
        /// expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        public DateTime UtcNow
        {
            get
            {
                return DateTime.UtcNow;
            }
        }
    }
}
