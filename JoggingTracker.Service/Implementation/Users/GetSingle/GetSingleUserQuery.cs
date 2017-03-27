using System;
using MediatR;

namespace JoggingTracker.Service.Implementation.Users
{
    public class GetSingleUserQuery : IRequest<UpdateUserCommand>
    {
        public GetSingleUserQuery(Guid id)
        {
            this.Id = id;
        }

        public Guid Id { get; }
    }
}
