namespace JoggingTracker.Shared.Configuration
{
    public static class UserSecrets
    {
        public const string JsonWebTokenSecretKey = nameof(UserSecrets.JsonWebTokenSecretKey);
        public const string JwtAudience = nameof(UserSecrets.JwtAudience);
        public const string JwtIssuer = nameof(UserSecrets.JwtIssuer);
        public const string PasswordIterationCount = nameof(UserSecrets.PasswordIterationCount);
        public const string TokenExpirationTimeDays = nameof(UserSecrets.TokenExpirationTimeDays);
    }
}
