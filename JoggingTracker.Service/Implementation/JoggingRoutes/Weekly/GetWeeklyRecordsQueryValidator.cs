using System;
using FluentValidation;
using JoggingTracker.Service.Model;
using JoggingTracker.Shared.Helper;
using Microsoft.AspNetCore.Http;

namespace JoggingTracker.Service.Implementation.JoggingRoutes.Weekly
{
    public class GetWeeklyRecordsQueryValidator : AbstractValidator<GetWeeklyRecordsQuery>
    {
        public GetWeeklyRecordsQueryValidator(IHttpContextAccessor httpContextAccessor)
        {
            ArgumentChecker.CheckNotNull(new { httpContextAccessor });

            var userIdentity = UserIdentity.FromPrincipal(httpContextAccessor.HttpContext.User);

            this.Custom(query =>
            {
                if (userIdentity.Id == query.UserId)
                    return null;

                throw new UnauthorizedAccessException();
            });
        }
    }
}
