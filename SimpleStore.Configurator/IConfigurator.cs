using Castle.Windsor;

namespace SimpleStore.Configurator
{
   interface IConfigurator
    {
       void Configure(IWindsorContainer container);
    }
}
