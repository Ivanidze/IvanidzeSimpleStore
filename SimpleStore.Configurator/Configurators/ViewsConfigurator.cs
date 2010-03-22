using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Infrasturcture;
using SimpleStore.Artifacts;


namespace SimpleStore.Configurator.Configurators
{
    public class ViewsConfigurator : IConfigurator
    {
        #region IConfigurator Members

        public void Configure(IWindsorContainer container)
        {
            container.Register(AllTypes.FromAssemblyNamed("SimpleStore.Gui")
                                   .Where(t => typeof (Window).IsAssignableFrom(t))
                                   .Configure(c => c.LifeStyle.Transient));

            container.Register(Component.For<IViewFactory>()
                                        .ImplementedBy<ViewFactory>());
        }

        #endregion
    }
}