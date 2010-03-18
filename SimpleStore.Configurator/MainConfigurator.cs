using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Facilities.FactorySupport;
using Castle.Windsor;

using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using Castle.MicroKernel.Registration;
using SimpleStore.Configurator.Configurators;
using uNhAddIns.Adapters;

namespace SimpleStore.Configurator
{
    public class MainConfigurator:IGuyWire
    {
        private readonly IConfigurator[] _configurators = new IConfigurator[] { new NHibernateConfigurator(),
            new DaoConfigurator(), 
            new NHValidatorConfigurator(),
        new EntityConfigurator(),
        new ModelsConfigurator(),
        new ViewModelConfigurator(),
        new ViewsConfigurator()
       };
        private IWindsorContainer _container;
        public void Wire()
        {
            if (_container!=null)
                Dewire();
            _container=new WindsorContainer();
            
           _container.AddFacility<FactorySupportFacility>();
            ServiceLocator.SetLocatorProvider(()=> new WindsorServiceLocator(_container));
            _container.Register(Component.For<IServiceLocator>().Instance(ServiceLocator.Current));
            foreach (IConfigurator configurator in _configurators)
            {
                configurator.Configure(_container);
            }
        }

        public void Dewire()
        {
            if (_container != null)
                _container.Dispose();
        }

    }
}
