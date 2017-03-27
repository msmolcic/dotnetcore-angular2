using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.Users
{
    public class GetSingleUserQueryHandler : IAsyncRequestHandler<GetSingleUserQuery, UpdateUserCommand>
    {
        private readonly IMapper _mapper;
        private readonly JoggingTrackerDbContext _dbContext;

        public GetSingleUserQueryHandler(
            IMapper mapper,
            JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new
            {
                mapper,
                dbContext
            });

            this._mapper = mapper;
            this._dbContext = dbContext;
        }

        public async Task<UpdateUserCommand> Handle(GetSingleUserQuery query)
        {
            return await this._dbContext
                .Users
                .Where(u => u.Id == query.Id)
                .ProjectTo<UpdateUserCommand>()
                .SingleAsync();
        }
    }
}
