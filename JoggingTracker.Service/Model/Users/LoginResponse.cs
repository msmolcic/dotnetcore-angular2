namespace JoggingTracker.Service.Model
{
    public class LoginResponse
    {
        public LoginResponse(UserIdentity userIdentity, string jsonWebToken)
        {
            this.UserIdentity = userIdentity;
            this.JsonWebToken = jsonWebToken;
        }

        public UserIdentity UserIdentity { get; }
        public string JsonWebToken { get; }
    }
}
