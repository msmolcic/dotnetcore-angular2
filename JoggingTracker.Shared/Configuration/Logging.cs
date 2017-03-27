namespace JoggingTracker.Shared.Configuration
{
    public class Logging
    {
        /// <summary>
        /// Returns the name of the 'Logging' section inside appsettings.json file.
        /// </summary>
        public const string SectionName = nameof(Logging);

        public bool IncludeScopes { get; set; }
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }
}
