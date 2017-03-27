using System.Threading.Tasks;
using FluentValidation;
using JoggingTracker.Shared.Helper;
using MediatR;

namespace JoggingTracker.Service.Infrastructure.PipelineBehavior
{
    public class ValidatorBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IValidatorFactory _validatorFactory;

        public ValidatorBehavior(IValidatorFactory validatorFactory)
        {
            ArgumentChecker.CheckNotNull(new { validatorFactory });

            this._validatorFactory = validatorFactory;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            var validator = this._validatorFactory.GetValidator<TRequest>();

            if (validator != null)
                await validator.ValidateAndThrowAsync(request);

            return await next();
        }
    }
}
