using System;

namespace JoggingTracker.Service.Infrastructure.Attributes
{
    /// <summary>
    /// Attribute used to indicate IAsyncRequestHandler that will be wrapped within database transaction at runtime.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RequireTransactionAttribute : Attribute { }
}
