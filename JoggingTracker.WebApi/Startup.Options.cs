using System;
using System.Text;
using JoggingTracker.Shared.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JoggingTracker.WebApi
{
    public partial class Startup
    {
        private void ConfigureOptions(IServiceCollection services)
        {
            services.AddOptions();
            services.AddSingleton(this.Configuration);

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(this.Configuration[UserSecrets.JsonWebTokenSecretKey])),

                ValidateAudience = true,
                ValidAudience = this.Configuration[UserSecrets.JwtAudience],

                ValidateIssuer = true,
                ValidIssuer = this.Configuration[UserSecrets.JwtIssuer],

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(Options.Create(tokenValidationParameters));
        }
    }
}
