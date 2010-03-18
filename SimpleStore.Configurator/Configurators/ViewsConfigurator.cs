using Castle.Windsor;
using Castle.MicroKernel.Registration;
using System.Windows;
using Infrasturcture;
using SimpleStore.Artifacts;

namespace SimpleStore.Configurator.Configurators
{
    public class ViewsConfigurator:IConfigurator
    {
        public void Configure(IWindsorContainer container)
        {
            
            container.Register(
                AllTypes.FromAssemblyNamed("SimpleStore.Gui").Where(type => typeof(Window).IsAssignableFrom(type)).
                    Configure(c => c.LifeStyle.Transient));
            container.Register(Component.For<IViewFactory>().ImplementedBy<ViewFactory>());
        }
    }
}
