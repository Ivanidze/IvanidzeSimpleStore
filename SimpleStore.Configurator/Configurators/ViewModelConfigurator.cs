using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Infrasturcture.ViewModelInterception;
using SimpleStore.ViewModels.Workers;
using Castle.MicroKernel.Registration;
using Castle.Core;

namespace SimpleStore.Configurator.Configurators
{
    public class ViewModelConfigurator:IConfigurator
    {
        public void Configure(IWindsorContainer container)
        {
            IEnumerable<Type> viewmodels =  typeof (CreateWorkerViewModel).Assembly.GetTypes().Where(vm => vm.Name.EndsWith("ViewModel"));
            foreach (Type viewmodel in viewmodels)
            {
                container.Register(Component.For(viewmodel).LifeStyle.Transient);
            }

        }
    }
}
