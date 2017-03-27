using Microsoft.AspNetCore.Authorization;

namespace JoggingTracker.WebApi.Infrastructure.Attributes
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles)
            : base()
        {
            this.Roles = string.Join(",", roles);
        }
    }
}
