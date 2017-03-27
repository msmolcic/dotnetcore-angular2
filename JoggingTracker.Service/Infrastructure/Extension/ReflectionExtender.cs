using System;
using System.Linq;
using System.Reflection;

namespace JoggingTracker.Service.Infrastructure.Extension
{
    public static class ReflectionExtender
    {
        public static bool HasAttribute<TAttribute>(this Type type)
            where TAttribute : Attribute
        {
            return type
                .GetTypeInfo()
                .GetCustomAttributes<TAttribute>(false)
                .Any();
        }
    }
}
