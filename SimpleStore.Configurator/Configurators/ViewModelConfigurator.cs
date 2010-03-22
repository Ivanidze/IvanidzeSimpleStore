using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SimpleStore.ViewModels;


namespace SimpleStore.Configurator.Configurators
{
    public class ViewModelConfigurator : IConfigurator
    {
        #region IConfigurator Members

        public void Configure(IWindsorContainer container)
        {
            container.Register(Component.For<CreateWorkerViewModel>()
                                   .LifeStyle.Transient);
            container.Register(Component.For<WareGroupViewModel>().LifeStyle.Transient);
            container.Register(Component.For<EditWareGroupViewModel>().LifeStyle.Transient);
        }

        #endregion
    }
}