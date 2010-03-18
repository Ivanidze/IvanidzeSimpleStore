using Castle.Windsor;
using Infrasturcture;
using NHibernate.Validator.Engine;
using Castle.MicroKernel.Registration;
using SimpleStore.Data.Impl.Validators;
using uNhAddIns.Adapters;
using uNhAddIns.NHibernateValidator;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using uNhAddIns.NHibernateTypeResolver;
namespace SimpleStore.Configurator.Configurators
{
    /// <summary>
    /// Конфигуратор для валидации модели через валидатор хибернейта
    /// </summary>
    public class NHValidatorConfigurator:IConfigurator
    {
        public void Configure(IWindsorContainer container)
        {
            var validatorEngine = new ValidatorEngine();
            container.Register(Component.For<IEntityValidator>().ImplementedBy<EntityValidator>());
            container.Register(Component.For<ValidatorEngine>().Instance(validatorEngine).LifeStyle.Singleton);

            container.Register(Component.For<ISharedEngineProvider>().ImplementedBy<NhValidatorSharedEngineProvider>());
            Environment.SharedEngineProvider = container.Resolve<ISharedEngineProvider>();

            var configure = new FluentConfiguration();
            configure.Register(typeof(WareGroupValidationDefeniton).Assembly.ValidationDefinitions()).SetDefaultValidatorMode(
                ValidatorMode.OverrideAttributeWithExternal).AddEntityTypeInspector<NHVTypeInspector>().
                IntegrateWithNHibernate.ApplyingDDLConstraints().And.RegisteringListeners();
            validatorEngine.Configure(configure);
        }
    }
}
