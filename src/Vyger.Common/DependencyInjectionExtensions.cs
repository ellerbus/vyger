using System;
using System.Linq;
using System.Reflection;
using Augment.Caching;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Vyger.Common.Configuration;

namespace Vyger.Common
{
    public static class DependencyInjectionExtensions
    {
        public static void AddVyger(this IServiceCollection services)
        {
            AddDependencies(services);
            AddRepositories(services);
            AddServices(services);
            AddValidators(services);
        }

        private static void AddDependencies(IServiceCollection services)
        {
            services.AddSingleton<IVygerConfiguration, VygerConfiguration>();
            services.AddSingleton<ICacheProvider>(provider => new MemoryCacheProvider());
            services.AddSingleton<ICacheManager>(provider => new CacheManager(provider.GetService<ICacheProvider>()));
        }

        private static void AddRepositories(IServiceCollection services)
        {
            Type[] types = GetTypesIn(".Repositories");

            foreach (Type interfaceType in types.Where(x => x.IsInterface && !x.IsNested))
            {
                foreach (Type concreteType in types.Where(x => !x.IsInterface))
                {
                    if (interfaceType.IsAssignableFrom(concreteType))
                    {
                        services.AddScoped(interfaceType, concreteType);
                        break;
                    }
                }
            }
        }

        private static void AddServices(IServiceCollection services)
        {
            Type[] types = GetTypesIn(".Services");

            foreach (Type interfaceType in types.Where(x => x.IsInterface && !x.IsNested))
            {
                foreach (Type concreteType in types.Where(x => !x.IsInterface))
                {
                    if (interfaceType.IsAssignableFrom(concreteType))
                    {
                        services.AddScoped(interfaceType, concreteType);
                        break;
                    }
                }
            }
        }

        private static void AddValidators(IServiceCollection services)
        {
            Type[] types = GetTypesIn(".Validators");

            Type validatorType = typeof(IValidator<>);

            foreach (Type concreteType in types.Where(x => !x.IsInterface && !x.IsNested))
            {
                Type modelType = concreteType.BaseType.GetGenericArguments().First();

                Type interfaceType = validatorType.MakeGenericType(modelType);

                services.AddScoped(interfaceType, concreteType);
            }
        }

        private static Type[] GetTypesIn(string suffix)
        {
            Assembly assembly = typeof(DependencyInjectionExtensions).Assembly;

            string ns = assembly.GetName().Name + suffix;

            Type[] types = assembly.GetTypes()
                .Where(x => x.Namespace == ns)
                .ToArray();

            return types;
        }
    }
}