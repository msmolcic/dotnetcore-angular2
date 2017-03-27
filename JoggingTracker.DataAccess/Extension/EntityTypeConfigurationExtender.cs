using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JoggingTracker.DataAccess.Database;
using Microsoft.EntityFrameworkCore;

namespace JoggingTracker.DataAccess.Extension
{
    public static class EntityTypeConfigurationExtender
    {
        private const string Entity = "Entity";
        private static readonly Dictionary<Assembly, IEnumerable<Type>> AssemblyTypes;

        static EntityTypeConfigurationExtender()
        {
            EntityTypeConfigurationExtender.AssemblyTypes = new Dictionary<Assembly, IEnumerable<Type>>();
        }

        public static ModelBuilder UseEntityTypeConfiguration(this ModelBuilder modelBuilder)
        {
            IEnumerable<Type> configurationTypes;
            var assembly = typeof(JoggingTrackerDbContext).GetTypeInfo().Assembly;

            if (!AssemblyTypes.TryGetValue(assembly, out configurationTypes))
            {
                configurationTypes = assembly
                    .GetExportedTypes()
                    .Where(p => p.GetTypeInfo().IsClass)
                    .Where(p => !p.GetTypeInfo().IsAbstract)
                    .Where(p => p.GetInterfaces()
                                 .Where(i => i.GetTypeInfo().IsGenericType)
                                 .Where(i => i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                                 .Any());

                EntityTypeConfigurationExtender.AssemblyTypes[assembly] = configurationTypes;
            }

            var configurations = configurationTypes.Select(p => Activator.CreateInstance(p));

            foreach (dynamic configuration in configurations)
            {
                EntityTypeConfigurationExtender.ApplyConfiguration(modelBuilder, configuration);
            }

            return modelBuilder;
        }

        private static ModelBuilder ApplyConfiguration<T>(
            this ModelBuilder modelBuilder,
            IEntityTypeConfiguration<T> configuration)
            where T : class
        {
            var entityType = FindEntityType(configuration.GetType());

            dynamic entityTypeBuilder = EntityTypeConfigurationExtender
                .GetEntityMethod()
                .MakeGenericMethod(entityType)
                .Invoke(modelBuilder, null);

            configuration.Configure(entityTypeBuilder);

            return modelBuilder;
        }

        private static Type FindEntityType(Type type)
        {
            var typeInterfaces = type.GetInterfaces();
            var interfaceType = typeInterfaces
                .Where(p => p.GetTypeInfo().IsGenericType)
                .Where(p => p.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                .First();

            return interfaceType
                .GetGenericArguments()
                .First();
        }

        private static MethodInfo GetEntityMethod()
        {
            var modelBuilderType = typeof(ModelBuilder);
            var typeInfo = modelBuilderType.GetTypeInfo();
            var entityMethods = typeInfo.GetMethods();

            return entityMethods
                .Where(p => p.Name == EntityTypeConfigurationExtender.Entity)
                .Where(p => p.IsGenericMethod)
                .Where(p => p.GetParameters().Length == 0)
                .Single();
        }
    }
}
