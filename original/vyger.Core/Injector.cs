using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using Augment.Caching;
using FluentValidation;
using SimpleInjector;

namespace vyger.Core
{
    public class Injector : Container
    {
        #region Constructor

        public Injector(ScopedLifestyle lifestyle) : base()
        {
            Options.DefaultScopedLifestyle = lifestyle;

            RegisterAll();
        }

        #endregion

        #region Register Mainline

        private void RegisterAll()
        {
            RegisterSupportItems();

            RegisterValidators();

            RegisterRepositories();

            RegisterServices();
        }

        private void RegisterSupportItems()
        {
            Register<ISecurityActor>(() => GetSecurityActor());

            Register<ICacheProvider, HttpRuntimeCacheProvider>();
            Register<ICacheManager, CacheManager>();
        }

        private void RegisterValidators()
        {
            Type[] types = GetTypesOf("Validators")
                .Where(x => !x.IsInterface && !x.IsNested)
                .ToArray();

            Type validatorType = typeof(IValidator<>);

            foreach (Type concreteType in types)
            {
                Type modelType = concreteType.BaseType.GetGenericArguments().First();

                Type interfaceType = validatorType.MakeGenericType(modelType);

                Register(interfaceType, concreteType);
            }

        }

        private void RegisterRepositories()
        {
            Type[] types = GetTypesOf("Repositories").ToArray();

            foreach (Type interfaceType in types.Where(x => x.IsInterface && !x.IsNested))
            {
                foreach (Type concreteType in types.Where(x => !x.IsInterface))
                {
                    if (interfaceType.IsAssignableFrom(concreteType))
                    {
                        Register(interfaceType, concreteType);
                        break;
                    }
                }
            }
        }

        private void RegisterServices()
        {
            Type[] types = GetTypesOf("Services").ToArray();

            foreach (Type interfaceType in types.Where(x => x.IsInterface && !x.IsNested))
            {
                foreach (Type concreteType in types.Where(x => !x.IsInterface))
                {
                    if (interfaceType.IsAssignableFrom(concreteType))
                    {
                        Register(interfaceType, concreteType);
                        break;
                    }
                }
            }
        }

        private IEnumerable<Type> GetTypesOf(string nmspace)
        {
            Assembly assembly = GetType().Assembly;

            string ns = assembly.GetName().Name + "." + nmspace;

            return assembly.GetTypes().Where(x => x.Namespace == ns);
        }

        #endregion

        #region Actor Support

        public ISecurityActor GetSecurityActor()
        {
            ISecurityActor p = GetPrincipal() as SecurityActor;

            if (p != null && p.Identity != null)
            {
                return p;
            }

            return new SecurityActor("", false, "");
        }

        private static IPrincipal GetPrincipal()
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                IPrincipal p = HttpContext.Current.User;

                if (p.Identity != null)
                {
                    return p;
                }
            }

            return null;
        }

        #endregion
    }
}