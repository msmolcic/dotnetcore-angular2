using System.Linq;
using System.Threading.Tasks;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Shared.Helper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.Users.Update
{
    public class UpdateUserCommandHandler : IAsyncRequestHandler<UpdateUserCommand>
    {
        private readonly JoggingTrackerDbContext _dbContext;

        public UpdateUserCommandHandler(JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { dbContext });

            this._dbContext = dbContext;
        }

        public async Task Handle(UpdateUserCommand command)
        {
            var user = await this._dbContext
                .Users
                .Where(u => u.Id == command.Id)
                .SingleAsync();

            user.Name = command.Name;
            user.Surname = command.Surname;
            user.BirthDate = command.BirthDate.Value;
            user.Gender = command.Gender.Value;

            this._dbContext.Entry(user).State = EntityState.Modified;
            await this._dbContext.SaveChangesAsync();
        }
    }
}
