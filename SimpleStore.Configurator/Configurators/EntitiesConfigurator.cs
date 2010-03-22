using System.Collections.ObjectModel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Infrasturcture;
using SimpleStore.Domain;
using uNhAddIns.ComponentBehaviors;
using uNhAddIns.ComponentBehaviors.Castle;
using uNhAddIns.ComponentBehaviors.Castle.Configuration;

namespace SimpleStore.Configurator.Configurators
{
	public class EntitiesConfigurator : IConfigurator
	{
		#region IConfigurator Members

		public void Configure(IWindsorContainer container)
		{
			container.AddFacility<ComponentBehaviorsFacility>();
            var config = new BehaviorDictionary();
			config.For<Worker>().Add<DataErrorInfoBehavior>()
							.Add<NotifyPropertyChangedBehavior>();
		    config.For<WareGroup>().Add<DataErrorInfoBehavior>()
		        .Add<NotifyPropertyChangedBehavior>();

			container.Register(Component.For<IBehaviorStore>().Instance(config));

            container.Register(Component.For<WareGroup>()
                                .OnCreate((kernel, wareGroup) => wareGroup.Children = new ObservableCollection<WareGroup>())
                                .LifeStyle.Transient);

			container.Register(Component.For<IEntityFactory>()
			                   	.ImplementedBy<EntityFactory>());
		}

		#endregion
	}
}