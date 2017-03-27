using System;
using JoggingTracker.Model.Enum;
using MediatR;

namespace JoggingTracker.Service.Implementation.Users
{
    public class UpdateUserCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
}
