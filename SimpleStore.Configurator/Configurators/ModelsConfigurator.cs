using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using SimpleStore.Domain.Model.WorkerModels;
using SimpleStore.Domain.Impl.WorkerModels;
using Castle.MicroKernel.Registration;

namespace SimpleStore.Configurator.Configurators
{
    public class ModelsConfigurator:IConfigurator
    {
        public void Configure(IWindsorContainer container)
        {
            //берем все интерфейсы модели
            IEnumerable<Type> repositryServices =
                typeof (ICreateWorkerModel).Assembly.GetTypes().Where(
                    t => t.IsInterface && t.Name.EndsWith("Model"));
            //все реализации моделей
            IEnumerable<Type> repositoryImpl =
                typeof (CreateWorkerModel).Assembly.GetTypes().Where(
                    t => t.GetInterfaces().Any(i => repositryServices.Contains(i)));
            //и регистрируем
            foreach (Type service in repositryServices)
            {
                foreach (Type impl in repositoryImpl)
                {
                    if (service.IsAssignableFrom(impl))
                    {
                        container.Register(Component.For(service).ImplementedBy(impl).LifeStyle.Transient);

                    }

                }
            }
        }
    }
}
