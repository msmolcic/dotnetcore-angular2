using System.Collections.Generic;
using MediatR;

namespace JoggingTracker.Service.Implementation.Users
{
    public class GetUsersQuery : IRequest<List<UpdateUserCommand>>
    {
    }
}
