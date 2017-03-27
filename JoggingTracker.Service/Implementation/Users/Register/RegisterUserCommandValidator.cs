using FluentValidation;
using FluentValidation.Results;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Infrastructure.Provider.Time;
using JoggingTracker.Model.Entity;
using JoggingTracker.Shared.Helper;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.Users
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator(JoggingTrackerDbContext dbContext, IServerTime serverTime)
        {
            ArgumentChecker.CheckNotNull(new { dbContext, serverTime });

            this.RuleFor(u => u.Username)
                .Must(username => !string.IsNullOrWhiteSpace(username))
                .WithMessage($"'{nameof(RegisterUserCommand.Username)}' is required.")
                .Length(User.UsernameLengthMin, User.UsernameLengthMax);

            this.RuleFor(u => u.Email)
                .EmailAddress()
                .Must(email => !string.IsNullOrWhiteSpace(email))
                .WithMessage($"'{nameof(RegisterUserCommand.Email)}' is required.")
                .Length(User.EmailLengthMin, User.EmailLengthMax);

            this.RuleFor(u => u.Password)
                .Must(password => !string.IsNullOrWhiteSpace(password))
                .WithMessage($"'{nameof(RegisterUserCommand.Password)}' is required.")
                .Equal(u => u.ConfirmPassword)
                .WithMessage($"'{nameof(RegisterUserCommand.Password)}' and '{nameof(RegisterUserCommand.ConfirmPassword)}' must be equal.")
                .Length(User.PasswordLengthMin, User.PasswordLengthMax)
                .WithMessage($"'{nameof(RegisterUserCommand.Password)}' length must be between {User.PasswordLengthMin} and {User.PasswordLengthMax} characters.");

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
                if (!await dbContext.Users.AnyAsync(u => u.Username == command.Username))
                    return null;

                return new ValidationFailure(
                    propertyName: nameof(User.Username),
                    error: $"{nameof(User.Username)} is already in use.");
            });

            this.CustomAsync(async command =>
            {
                if (!await dbContext.Users.AnyAsync(u => u.Email == command.Email))
                    return null;

                return new ValidationFailure(
                    propertyName: nameof(User.Email),
                    error: $"{nameof(User.Email)} is already in use.");
            });
        }
    }
}
