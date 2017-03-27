using System.Linq;
using System.Threading.Tasks;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.Users.Delete
{
    public class DeleteUserCommandHandler : IAsyncRequestHandler<DeleteUserCommand>
    {
        private readonly JoggingTrackerDbContext _dbContext;

        public DeleteUserCommandHandler(JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { dbContext });

            this._dbContext = dbContext;
        }

        public async Task Handle(DeleteUserCommand command)
        {
            var userJoggingRoutes = await this._dbContext
                .JoggingRoutes
                .Where(r => r.UserId == command.Id)
                .ToListAsync();

            this._dbContext.JoggingRoutes.RemoveRange(userJoggingRoutes);

            var userRoles = await this._dbContext
                .UserRoles
                .Where(r => r.UserId == command.Id)
                .ToListAsync();

            this._dbContext.UserRoles.RemoveRange(userRoles);

            var user = await this._dbContext
                .Users
                .Where(u => u.Id == command.Id)
                .SingleAsync();

            this._dbContext.Users.Remove(user);

            await this._dbContext.SaveChangesAsync();
        }
    }
}
