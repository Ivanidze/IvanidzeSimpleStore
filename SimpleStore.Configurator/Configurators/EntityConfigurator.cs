using Castle.Windsor;
using SimpleStore.Domain;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;
using uNhAddIns.ComponentBehaviors;
using uNhAddIns.ComponentBehaviors.Castle;
using Castle.MicroKernel.Registration;
using Infrasturcture;

namespace SimpleStore.Configurator.Configurators
{
    public class EntityConfigurator:IConfigurator
    {
        public void Configure(IWindsorContainer container)
        {
            container.AddFacility<ComponentBehaviorsFacility>();
            var config = new BehaviorDictionary();
            config.For<Worker>().Add<DataErrorInfoBehavior>().Add<NotifyPropertyChangedBehavior>();
            container.Register(Component.For<Worker>().LifeStyle.Transient);
            container.Register(Component.For<IEntityFactory>().ImplementedBy<EntityFactory>());
        }
    }
}
