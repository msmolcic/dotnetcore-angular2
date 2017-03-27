using System;

namespace JoggingTracker.Infrastructure.Provider.Time
{
    /// <summary>
    /// Abstracts the server time to facilitate testing.
    /// </summary>
    public interface IServerTime
    {
        /// <summary>
        /// Gets a DateTime structure that is set to the current date and time on the server machine,
        /// expressed as the Coordinated Universal Time (UTC).
        /// </summary>
        DateTime UtcNow { get; }
    }
}
