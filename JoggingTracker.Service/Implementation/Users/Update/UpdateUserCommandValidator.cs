using System;
using FluentValidation;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Infrastructure.Provider.Time;
using JoggingTracker.Model.Entity;
using JoggingTracker.Service.Model;
using JoggingTracker.Shared.Exceptions;
using JoggingTracker.Shared.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.Users.Update
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator(JoggingTrackerDbContext dbContext, IServerTime serverTime, IHttpContextAccessor httpContextAccessor)
        {
            ArgumentChecker.CheckNotNull(new { dbContext, serverTime, httpContextAccessor });

            var userIdentity = UserIdentity.FromPrincipal(httpContextAccessor.HttpContext.User);

            this.Custom(command =>
            {
                if (userIdentity.IsAdmin || userIdentity.IsUserManager || userIdentity.Id == command.Id)
                    return null;

                throw new UnauthorizedAccessException();
            });

            this.RuleFor(u => u.Name)
                .Must(name => !string.IsNullOrWhiteSpace(name))
                .WithMessage($"'{nameof(RegisterUserCommand.Name)}' is required.")
                .Length(User.NameLengthMin, User.NameLengthMax);

            this.RuleFor(u => u.Surname)
                .Must(surname => !string.IsNullOrWhiteSpace(surname))
                .WithMessage($"'{nameof(RegisterUserCommand.Surname)}' is required.")
                .Length(User.SurnameLengthMin, User.SurnameLengthMax);

            this.RuleFor(u => u.BirthDate)
                .NotNull()
                .LessThanOrEqualTo(serverTime.UtcNow)
                .WithMessage("Invalid date of birth.");

            this.RuleFor(u => u.Gender)
                .NotNull();

            this.CustomAsync(async command =>
            {
                if (await dbContext.Users.AnyAsync(u => u.Id == command.Id))
                    return null;

                throw new ObjectNotFoundException($"{nameof(User)} not found.");
            });
        }
    }
}
