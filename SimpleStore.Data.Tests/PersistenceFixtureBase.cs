using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace SimpleStore.Data.Tests
{
    public class  PersistenceFixtureBase
    {
        protected Configuration cfg;
        protected ISessionFactoryImplementor sessions;

        
        public void TestFixtureSetUp()
        {
            cfg = new Configuration();
            cfg.Configure();
            new SchemaExport(cfg).Execute(false, true,false);
            sessions = (ISessionFactoryImplementor)cfg.BuildSessionFactory();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            sessions.Close();
            sessions = null;
            cfg = null;
        }
    }
}
