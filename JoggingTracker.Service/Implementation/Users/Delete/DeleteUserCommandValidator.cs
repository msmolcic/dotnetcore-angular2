using FluentValidation;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Model.Entity;
using JoggingTracker.Shared.Exceptions;
using JoggingTracker.Shared.Helper;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.Users.Delete
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator(JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new { dbContext });

            this.CustomAsync(async command =>
            {
                if (await dbContext.Users.AnyAsync(u => u.Id == command.Id))
                    return null;

                throw new ObjectNotFoundException($"{nameof(User)} not found.");
            });
        }
    }
}
