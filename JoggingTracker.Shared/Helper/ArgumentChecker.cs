using System;
using System.Reflection;
using JoggingTracker.Shared.Exceptions;

namespace JoggingTracker.Shared.Helper
{
    /// <summary>
    /// Provides the set of helper methods to validate object properties.
    /// </summary>
    public class ArgumentChecker
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if any <paramref name="source"/> object property is null.
        /// </summary>
        /// <param name="source">Object whose properties are being checked.</param>
        public static void CheckNotNull(object source)
        {
            ArgumentChecker.CheckProperties(
                source: source,
                throwCondition: value => value == null,
                exceptionType: typeof(ArgumentNullException));
        }

        /// <summary>
        /// Checks the <paramref name="throwCondition"/> against every property in <paramref name="source"/> object.
        /// If throw condition is satisfied an exception of provided <paramref name="exceptionType"/> is thrown.
        /// </summary>
        /// <param name="source">Object whose properties are being checked.</param>
        /// <param name="throwCondition">Exception throw condition.</param>
        /// <param name="exceptionType">Type of the exception thrown.</param>
        private static void CheckProperties(
            object source,
            Func<object, bool> throwCondition,
            Type exceptionType)
        {
            foreach (var property in source.GetType().GetProperties())
            {
                if (throwCondition.Invoke(property.GetValue(source)))
                {
                    var exception = (Exception)Activator.CreateInstance(
                        type: exceptionType,
                        args: property.Name);

                    throw exception;
                }
            }
        }
    }
}
