using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JoggingTracker.DataAccess.Database;
using JoggingTracker.Infrastructure.Provider.Security;
using JoggingTracker.Infrastructure.Provider.Time;
using JoggingTracker.Service.Model;
using JoggingTracker.Shared.Configuration;
using JoggingTracker.Shared.Helper;
using JoggingTracker.Shared.Identity;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JoggingTracker.Service.Implementation.Users
{
    public class LoginUserCommandHandler : IAsyncRequestHandler<LoginUserCommand, LoginResponse>
    {
        private readonly IServerTime _serverTime;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IConfigurationRoot _configuration;
        private readonly JoggingTrackerDbContext _dbContext;

        public LoginUserCommandHandler(
            ISecurityProvider securityProvider,
            IServerTime serverTime,
            IOptions<TokenValidationParameters> tokenValidationParameterOptions,
            IConfigurationRoot configuration,
            JoggingTrackerDbContext dbContext)
        {
            ArgumentChecker.CheckNotNull(new
            {
                serverTime,
                tokenValidationParameterOptions,
                configuration,
                dbContext
            });

            this._serverTime = serverTime;
            this._tokenValidationParameters = tokenValidationParameterOptions.Value;
            this._configuration = configuration;
            this._dbContext = dbContext;
        }

        public async Task<LoginResponse> Handle(LoginUserCommand message)
        {
            var user = await this._dbContext.Users
                .Include(u => u.UserRoles)
                .ThenInclude(userRole => userRole.Role)
                .Where(u => u.Username == message.Username)
                .SingleAsync();

            var claimsIdentity = new ClaimsIdentity(
                JwtBearerDefaults.AuthenticationScheme,
                ClaimTypes.Name,
                ClaimTypes.Role);

            var now = this._serverTime.UtcNow;

            claimsIdentity.AddClaims(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            });

            claimsIdentity.AddClaim(new Claim(JoggingTrackerClaimTypes.UserId, user.Id.ToString()));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Username));
            claimsIdentity.AddClaim(new Claim(JoggingTrackerClaimTypes.Name, user.Name));
            claimsIdentity.AddClaim(new Claim(JoggingTrackerClaimTypes.Surname, user.Surname));

            foreach (var role in user.UserRoles.Select(userRole => userRole.Role.Name))
                claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));

            var expiresAt = now.Add(TimeSpan.FromDays(
                Convert.ToInt32(this._configuration[UserSecrets.TokenExpirationTimeDays])));

            var jsonWebToken = new JwtSecurityToken(
                issuer: this._tokenValidationParameters.ValidIssuer,
                audience: this._tokenValidationParameters.ValidAudience,
                claims: claimsIdentity.Claims,
                notBefore: now,
                expires: expiresAt,
                signingCredentials: new SigningCredentials(
                    this._tokenValidationParameters.IssuerSigningKey,
                    SecurityAlgorithms.HmacSha256));

            var userIdentity = new UserIdentity(user);
            var encodedTokenValue = new JwtSecurityTokenHandler().WriteToken(jsonWebToken);

            return new LoginResponse(
                userIdentity: userIdentity,
                jsonWebToken: encodedTokenValue);
        }
    }
}
