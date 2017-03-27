using JoggingTracker.Service.Model;
using MediatR;

namespace JoggingTracker.Service.Implementation.Users
{
    public class LoginUserCommand : IRequest<LoginResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
