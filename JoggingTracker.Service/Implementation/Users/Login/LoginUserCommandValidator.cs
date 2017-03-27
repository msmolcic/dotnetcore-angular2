using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Infrastructure.Provider.Security;
using JoggingTracker.Shared.Helper;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.Users
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator(JoggingTrackerDbContext dbContext, ISecurityProvider securityProvider)
        {
            ArgumentChecker.CheckNotNull(new { dbContext, securityProvider });

            this.CustomAsync(async command =>
            {
                var user = await dbContext.Users
                    .Where(u => u.Username == command.Username)
                    .SingleOrDefaultAsync();

                if (user == null || !securityProvider.IsValidPassword(command.Password.Trim(), user.Password))
                    return new ValidationFailure(string.Empty, "Invalid credentials.");

                return null;
            });
        }
    }
}
