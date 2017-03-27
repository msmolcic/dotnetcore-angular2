using System;
using JoggingTracker.Model.Enum;
using JoggingTracker.Service.Infrastructure.Attributes;
using MediatR;

namespace JoggingTracker.Service.Implementation.Users
{
    [RequireTransaction]
    public class RegisterUserCommand : IRequest<UpdateUserCommand>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
