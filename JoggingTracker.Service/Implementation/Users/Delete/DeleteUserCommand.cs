using System;
using JoggingTracker.Service.Infrastructure.Attributes;
using MediatR;

namespace JoggingTracker.Service.Implementation.Users
{
    [RequireTransaction]
    public class DeleteUserCommand : IRequest
    {
        public DeleteUserCommand(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
