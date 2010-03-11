using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Cfg;
using NHibernate;

using DataModel.Domain;
using NHibernate.Cfg;

namespace DataModel.Repositories
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;
        private static Configuration _configurator;
        public static Configuration Configurator
        {
            get
            {
                if (_configurator==null)
                {
                    var fluentConfiguration =
                        Fluently.Configure().Database(
                            FluentNHibernate.Cfg.Db.MsSqlCeConfiguration.Standard.ConnectionString(
                                "Data Source = SimpleStore.sdf"));
                    
                    fluentConfiguration.Mappings(m => m.FluentMappings.AddFromAssemblyOf<WareGroup>());
                    _configurator = fluentConfiguration.BuildConfiguration();
                    
                    
                    
                    
                }
                return _configurator;
            }
        }
        
        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    _sessionFactory = Configurator.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }
        public static ISession OpenSession()
        { 
            return SessionFactory.OpenSession();
        }
    }
}
