using System;
using FluentValidation;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Model.Entity;
using JoggingTracker.Service.Model;
using JoggingTracker.Shared.Exceptions;
using JoggingTracker.Shared.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class GetSingleJoggingRouteQueryValidator : AbstractValidator<GetSingleJoggingRouteQuery>
    {
        public GetSingleJoggingRouteQueryValidator(JoggingTrackerDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            ArgumentChecker.CheckNotNull(new { dbContext, httpContextAccessor });

            var userIdentity = UserIdentity.FromPrincipal(httpContextAccessor.HttpContext.User);

            this.Custom(query =>
            {
                if (userIdentity.IsAdmin || userIdentity.Id == query.UserId)
                    return null;

                throw new UnauthorizedAccessException();
            });

            this.CustomAsync(async query =>
            {
                if (await dbContext.JoggingRoutes.AnyAsync(r => r.Id == query.Id))
                    return null;

                throw new ObjectNotFoundException($"{nameof(JoggingRoute)} not found.");
            });
        }
    }
}
