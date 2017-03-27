using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.Users.Get
{
    public class GetUsersQueryHandler : IAsyncRequestHandler<GetUsersQuery, List<UpdateUserCommand>>
    {
        private readonly IMapper _mapper;
        private readonly JoggingTrackerDbContext _dbContext;

        public GetUsersQueryHandler(IMapper mapper, JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { mapper, dbContext });

            this._mapper = mapper;
            this._dbContext = dbContext;
        }

        public async Task<List<UpdateUserCommand>> Handle(GetUsersQuery query)
        {
            return await this._dbContext
                .Users
                .OrderBy(u => u.Name)
                .ThenBy(u => u.Surname)
                .ProjectTo<UpdateUserCommand>()
                .ToListAsync();
        }
    }
}
