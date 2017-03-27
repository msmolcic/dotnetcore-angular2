using JoggingTracker.WebApi.Infrastructure.Extension;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace JoggingTracker.WebApi
{
    public partial class Startup
    {
        private const string AllOrigins = "*";
        private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

        private void ConfigureExceptionHandler(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(
              builder =>
              {
                  builder.Run(
                    async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.Headers.Add(Startup.AccessControlAllowOrigin, Startup.AllOrigins);

                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                        }
                    });
              });
        }
    }
}
