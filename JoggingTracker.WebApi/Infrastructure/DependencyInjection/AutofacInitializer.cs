using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using JoggingTracker.Infrastructure.Provider.Security;
using JoggingTracker.Infrastructure.Provider.Time;
using JoggingTracker.Service.Infrastructure.PipelineBehavior;
using JoggingTracker.Service.Infrastructure.Validation;
using JoggingTracker.Shared.Configuration;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JoggingTracker.WebApi.Infrastructure.DependencyInjection
{
    public class AutofacInitializer
    {
        public static IServiceProvider Initialize(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OptionalTransactionBehavior<,>));

            builder.Populate(services);

            builder.RegisterType<ServerTime>()
                .As<IServerTime>()
                .SingleInstance();

            builder.Register(context =>
            {
                var configuration = context.Resolve<IConfigurationRoot>();
                var iterationCount = Convert.ToInt32(configuration[UserSecrets.PasswordIterationCount]);

                return new SecurityProvider(iterationCount);
            })
            .As<ISecurityProvider>();

            builder.RegisterType<ValidatorFactory>()
                .As<IValidatorFactory>();

            builder.RegisterAssemblyTypes((typeof(ValidatorFactory).GetTypeInfo().Assembly))
                .AsClosedTypesOf(typeof(IValidator<>));

            var container = builder.Build();
            return container.Resolve<IServiceProvider>();
        }
    }
}
