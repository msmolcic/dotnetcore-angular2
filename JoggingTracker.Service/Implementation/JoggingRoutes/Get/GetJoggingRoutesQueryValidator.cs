using System;
using FluentValidation;
using FluentValidation.Results;
using JoggingTracker.Service.Model;
using JoggingTracker.Shared.Helper;
using Microsoft.AspNetCore.Http;

namespace JoggingTracker.Service.Implementation.JoggingRoutes
{
    public class GetJoggingRoutesQueryValidator : AbstractValidator<GetJoggingRoutesQuery>
    {
        public GetJoggingRoutesQueryValidator(IHttpContextAccessor httpContextAccessor)
        {
            ArgumentChecker.CheckNotNull(new { httpContextAccessor });

            var userIdentity = UserIdentity.FromPrincipal(httpContextAccessor.HttpContext.User);

            this.Custom(query =>
            {
                if (userIdentity.IsAdmin || userIdentity.Id == query.UserId)
                    return null;

                throw new UnauthorizedAccessException();
            });

            this.Custom(query =>
            {
                if (!query.FromDate.HasValue || !query.UntilDate.HasValue || (query.FromDate <= query.UntilDate))
                    return null;

                return new ValidationFailure(
                    propertyName: string.Empty,
                    error: "'From date' must be less than or equal to 'Until date'");
            });
        }
    }
}
