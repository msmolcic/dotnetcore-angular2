using System;
using FluentValidation;
using JoggingTracker.Service.Model;
using JoggingTracker.Shared.Helper;
using Microsoft.AspNetCore.Http;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class CreateJoggingRouteCommandValidator : AbstractValidator<CreateJoggingRouteCommand>
    {
        public CreateJoggingRouteCommandValidator(IHttpContextAccessor httpContextAccessor)
        {
            ArgumentChecker.CheckNotNull(new { httpContextAccessor });

            var userIdentity = UserIdentity.FromPrincipal(httpContextAccessor.HttpContext.User);

            this.Custom(command =>
            {
                if (userIdentity.IsAdmin || userIdentity.Id == command.UserId)
                    return null;

                throw new UnauthorizedAccessException();
            });

            this.RuleFor(r => r.DistanceKilometers)
                .NotNull()
                .GreaterThan(0);

            this.RuleFor(r => r.StartTime)
                .NotNull()
                .LessThan(r => r.EndTime)
                .WithMessage("'Start time' must be less than 'End time'");

            this.RuleFor(r => r.EndTime)
                .NotNull()
                .GreaterThan(r => r.StartTime)
                .WithMessage("'End time' must be greater than 'Start time'");
        }
    }
}
