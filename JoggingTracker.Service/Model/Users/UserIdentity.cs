using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using JoggingTracker.Model.Entity;
using JoggingTracker.Shared.Identity;
using Newtonsoft.Json;

namespace JoggingTracker.Service.Model
{
    public class UserIdentity
    {
        public static UserIdentity AnonymousUserIdentity => new UserIdentity();

        public UserIdentity(User user)
        {
            this.Id = user.Id;
            this.Username = user.Username;
            this.Name = user.Name;
            this.Surname = user.Surname;
            this.Roles = user.UserRoles.Select(userRole => userRole.Role.Name).ToList();
        }

        private UserIdentity()
        {
            this.Roles = new List<string>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<string> Roles { get; set; }

        [JsonIgnore]
        public bool IsAnonymous => this.Id == Guid.Empty;

        [JsonIgnore]
        public bool IsAdmin => this.Roles.Contains(Role.AdminRole);

        [JsonIgnore]
        public bool IsUser => this.Roles.Contains(Role.UserRole);

        [JsonIgnore]
        public bool IsUserManager => this.Roles.Contains(Role.UserManagerRole);

        public static UserIdentity FromPrincipal(IPrincipal principal)
        {
            var userIdentity = new UserIdentity();

            var claimsIdentity = principal.Identity as ClaimsIdentity;

            if (claimsIdentity?.IsAuthenticated == false)
                return userIdentity;

            try
            {
                var claims = claimsIdentity.Claims;

                userIdentity.Id = Guid.Parse(claims.Single(claim => claim.Type == JoggingTrackerClaimTypes.UserId).Value);
                userIdentity.Username = claims.Single(claim => claim.Type == ClaimTypes.Name).Value;
                userIdentity.Name = claims.Single(claim => claim.Type == JoggingTrackerClaimTypes.Name).Value;
                userIdentity.Surname = claims.Single(claim => claim.Type == JoggingTrackerClaimTypes.Surname).Value;

                userIdentity.Roles = claims
                    .Where(claim => claim.Type == ClaimTypes.Role)
                    .Select(claim => claim.Value)
                    .ToList();

                return userIdentity;
            }
            catch
            {
                return UserIdentity.AnonymousUserIdentity;
            }
        }
    }
}
