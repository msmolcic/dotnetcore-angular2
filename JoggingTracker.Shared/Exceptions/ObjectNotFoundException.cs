using System;

namespace JoggingTracker.Shared.Exceptions
{
    public class ObjectNotFoundException : Exception
    {
        public ObjectNotFoundException(string message)
            : base(message)
        {
        }
    }
}
