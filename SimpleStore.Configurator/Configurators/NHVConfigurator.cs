using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Infrasturcture;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using SimpleStore.Data.Impl.Validators;
using uNhAddIns.Adapters;
using uNhAddIns.NHibernateTypeResolver;
using uNhAddIns.NHibernateValidator;

namespace SimpleStore.Configurator.Configurators
{
    public class NHVConfigurator : IConfigurator
    {
        #region IConfigurator Members

        public void Configure(IWindsorContainer container)
        {
            var ve = new ValidatorEngine();

            container.Register(Component.For<IEntityValidator>()
                                   .ImplementedBy<EntityValidator>());

            container.Register(Component.For<ValidatorEngine>()
                                   .Instance(ve)
                                   .LifeStyle.Singleton);

            //Register the service for ISharedEngineProvider
            container.Register(Component.For<ISharedEngineProvider>()
                                   .ImplementedBy<NHVSharedEngineProvider>());

            //Assign the shared engine provider for NHV.
            Environment.SharedEngineProvider =
                container.Resolve<ISharedEngineProvider>();

            //Configure validation framework fluently
            var configure = new FluentConfiguration();

            configure.Register(typeof (WorkerValidationDefenition).Assembly.ValidationDefinitions())
                .SetDefaultValidatorMode(ValidatorMode.OverrideAttributeWithExternal)
				.AddEntityTypeInspector<NHVTypeInspector>()
                .IntegrateWithNHibernate.ApplyingDDLConstraints().And.RegisteringListeners();

            ve.Configure(configure);
        }

        #endregion
    }
}