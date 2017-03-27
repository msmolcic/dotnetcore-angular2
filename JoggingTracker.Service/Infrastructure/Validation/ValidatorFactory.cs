using System;
using Autofac;
using FluentValidation;
using JoggingTracker.Shared.Helper;

namespace JoggingTracker.Service.Infrastructure.Validation
{
    public class ValidatorFactory : IValidatorFactory
    {
        private readonly IComponentContext _componentContext;

        public ValidatorFactory(IComponentContext componentContext)
        {
            ArgumentChecker.CheckNotNull(new { componentContext });

            this._componentContext = componentContext;
        }

        public IValidator GetValidator(Type type)
        {
            ArgumentChecker.CheckNotNull(new { type });

            var validator = typeof(IValidator<>).MakeGenericType(type);

            if (!this._componentContext.IsRegistered(validator))
                return null;

            return this._componentContext.Resolve(validator) as IValidator;
        }

        public IValidator<T> GetValidator<T>()
        {
            if (!this._componentContext.IsRegistered<IValidator<T>>())
                return null;
            
            return this._componentContext.Resolve<IValidator<T>>();
        }
    }
}
