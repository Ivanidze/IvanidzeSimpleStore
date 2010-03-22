using Castle.Facilities.FactorySupport;
using Castle.MicroKernel.Facilities.OnCreate;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using SimpleStore.Configurator.Configurators;
using uNhAddIns.Adapters;

namespace SimpleStore.Configurator
{
    public class GeneralGuyWire : IGuyWire
    {
        private readonly IConfigurator[] _configurators = new IConfigurator[]
                                                             {
                                                                 new NHibernateConfigurator(),
																 new NHVConfigurator(),
                                                                 new EntitiesConfigurator(),
                                                                 new ModelsConfigurator(),
                                                                 new ViewModelConfigurator(),
                                                                 new ViewsConfigurator(),
                                                                 new DaoConfigurator()
                                                             };

        private IWindsorContainer _container;

        #region IGuyWire Members

        /// <summary>
        /// Application wire.
        /// </summary>
        /// <remarks>
        /// IoC container configuration (more probably conf. by code).
        /// </remarks>
        public void Wire()
        {
            if (_container != null)
                Dewire();
            _container = new WindsorContainer();
        	_container.AddFacility<OnCreateFacility>();
        	_container.AddFacility<FactorySupportFacility>();

            //container.Register(Component.For<IWindsorContainer>().Instance(container));

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(_container));
            _container.Register(Component.For<IServiceLocator>().Instance(ServiceLocator.Current));

            foreach (IConfigurator configurator in _configurators)
            {
                configurator.Configure(_container);
            }
        }

        /// <summary>
        /// Application dewire
        /// </summary>
        /// <remarks>
        /// IoC container dispose.
        /// </remarks>
        public void Dewire()
        {
            if (_container != null)
                _container.Dispose();
        }

        #endregion
    }
}