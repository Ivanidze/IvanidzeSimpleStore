using System;
using Castle.Windsor;
using NHibernate.Validator.Cfg;
using uNhAddIns.CastleAdapters;
using uNhAddIns.CastleAdapters.AutomaticConversationManagement;
using uNhAddIns.CastleAdapters.EnhancedBytecodeProvider;
using uNhAddIns.NHibernateTypeResolver;
using uNhAddIns.SessionEasier;
using Environment = NHibernate.Cfg.Environment;
using uNhAddIns.WPF.Collections;
using Castle.MicroKernel.Registration;
using NHibernate;
using NHibernate.Engine;
using uNhAddIns.SessionEasier.Conversations;


namespace SimpleStore.Configurator.Configurators
{
    public class NHibernateConfigurator:IConfigurator
    {
        public void Configure(IWindsorContainer container)
        {

            container.AddFacility<PersistenceConversationFacility>();

            Environment.BytecodeProvider = new EnhancedBytecode(container);

            var nhConfigurator = new DefaultSessionFactoryConfigurationProvider();
            nhConfigurator.BeforeConfigure += (sender, e) =>
                                                  {
                                                      ValidatorInitializer.Initialize(e.Configuration);
                                                      e.Configuration.RegisterEntityNameResolver();
                                                     
                                                          e.Configuration.Properties[Environment.CollectionTypeFactoryClass]
                                                                                  = typeof(WpfCollectionTypeFactory).AssemblyQualifiedName;
                                                     
                                                      

                                                  };
            var sessionFactoryProvider = new SessionFactoryProvider(nhConfigurator);
            container.Register(Component.For<ISessionFactoryProvider>().Instance(sessionFactoryProvider));
            
            container.Register(
                Component.For<ISessionFactory>().UsingFactoryMethod(
                    () => 
                        container.Resolve<ISessionFactoryProvider>().GetFactory(null))
                        );
            container.Register(
                Component.For<ISessionFactoryImplementor>().UsingFactoryMethod(
                    () => (ISessionFactoryImplementor) container.Resolve<ISessionFactoryProvider>().GetFactory(null)));

            container.Register(Component.For<ISessionWrapper>().ImplementedBy<SessionWrapper>());
            container.Register(Component.For<IConversationFactory>().ImplementedBy<DefaultConversationFactory>());
            container.Register(
                Component.For<IConversationsContainerAccessor>().ImplementedBy<NhConversationsContainerAccessor>());

        }
    }
}
