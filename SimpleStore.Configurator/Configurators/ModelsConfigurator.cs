using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SimpleStore.Domain.Model;

namespace SimpleStore.Configurator.Configurators
{
    public class ModelsConfigurator : IConfigurator
    {
        #region IConfigurator Members

        public void Configure(IWindsorContainer container)
        {
            IEnumerable<Type> repositoryServices = typeof(ICreateWorkerModel).Assembly
                .GetTypes().Where(t => t.IsInterface && t.Namespace.EndsWith("Model"));

            IEnumerable<Type> repositoryImpl = Assembly.Load("SimpleStore.Domain.Impl").GetTypes()
                .Where(t => t.GetInterfaces().Any(i => repositoryServices.Contains(i)));

            foreach (Type service in repositoryServices)
                foreach (Type impl in repositoryImpl)
                    if (service.IsAssignableFrom(impl))
                        container.Register(Component.For(service).ImplementedBy(impl).LifeStyle.Transient);
        }

        #endregion
    }
}