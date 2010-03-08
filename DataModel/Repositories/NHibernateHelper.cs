using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using DataModel.Domain;

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
                    _configurator = new Configuration();
                    _configurator.Configure();
                    _configurator.AddAssembly(typeof(Contragent).Assembly);
                    
                    
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
