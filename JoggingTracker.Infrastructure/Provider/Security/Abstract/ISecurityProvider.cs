namespace JoggingTracker.Infrastructure.Provider.Security
{
    public interface ISecurityProvider
    {
        byte[] CalculatePasswordHash(string password);

        bool IsValidPassword(string passwordGuess, byte[] savedHash);
    }
}
