using System;
using FluentValidation;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Model.Entity;
using JoggingTracker.Service.Model;
using JoggingTracker.Shared.Exceptions;
using JoggingTracker.Shared.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.Service.Implementation.Users.GetSingle
{
    public class GetSingleUserQueryValidator : AbstractValidator<GetSingleUserQuery>
    {
        public GetSingleUserQueryValidator(JoggingTrackerDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            ArgumentChecker.CheckNotNull(new { dbContext, httpContextAccessor });

            var userIdentity = UserIdentity.FromPrincipal(httpContextAccessor.HttpContext.User);

            this.Custom(command =>
            {
                if (userIdentity.IsAdmin || userIdentity.IsUserManager || userIdentity.Id == command.Id)
                    return null;

                throw new UnauthorizedAccessException();
            });

            this.CustomAsync(async query =>
            {
                if (await dbContext.Users.AnyAsync(u => u.Id == query.Id))
                    return null;

                throw new ObjectNotFoundException($"{nameof(User)} not found.");
            });
        }
    }
}
