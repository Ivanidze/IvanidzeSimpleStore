using Castle.Windsor;

namespace SimpleStore.Configurator
{
    public interface IConfigurator
    {
        void Configure(IWindsorContainer container);
    }
}