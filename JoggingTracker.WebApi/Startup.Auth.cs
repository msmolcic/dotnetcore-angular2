using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JoggingTracker.WebApi
{
    public partial class Startup
    {
        private void ConfigureAuth(IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<TokenValidationParameters>>();

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                TokenValidationParameters = options.Value
            });
        }
    }
}
