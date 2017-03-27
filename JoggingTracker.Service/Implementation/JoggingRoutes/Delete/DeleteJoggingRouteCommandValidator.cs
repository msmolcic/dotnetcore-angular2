using System;
using FluentValidation;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Model.Entity;
using JoggingTracker.Service.Model;
using JoggingTracker.Shared.Exceptions;
using JoggingTracker.Shared.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.JoggingRoutes.Delete
{
    public class DeleteJoggingRouteCommandValidator : AbstractValidator<DeleteJoggingRouteCommand>
    {
        public DeleteJoggingRouteCommandValidator(JoggingTrackerDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            ArgumentChecker.CheckNotNull(new { dbContext, httpContextAccessor });

            var userIdentity = UserIdentity.FromPrincipal(httpContextAccessor.HttpContext.User);

            this.Custom(command =>
            {
                if (userIdentity.IsAdmin || userIdentity.Id == command.UserId)
                    return null;

                throw new UnauthorizedAccessException();
            });

            this.CustomAsync(async command =>
            {
                if (await dbContext.JoggingRoutes.AnyAsync(r => r.Id == command.Id))
                    return null;

                throw new ObjectNotFoundException($"{nameof(JoggingRoute)} not found.");
            });
        }
    }
}
