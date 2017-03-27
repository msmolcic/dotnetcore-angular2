using Microsoft.AspNetCore.Http;

namespace JoggingTracker.WebApi.Infrastructure.Extension
{
    public static class HttpResponseExtender
    {
        private const string ApplicationError = "Application-Error";
        private const string AccessControlExposeHeaders = "access-control-expose-headers";

        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add(
                HttpResponseExtender.ApplicationError,
                message);

            response.Headers.Add(
                HttpResponseExtender.AccessControlExposeHeaders,
                HttpResponseExtender.ApplicationError);
        }
    }
}
