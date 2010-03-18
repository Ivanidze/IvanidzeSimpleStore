using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DataModel.DataAccess;
using SimpleStore.Data;

namespace SimpleStore.Configurator.Configurators
{
    public class DaoConfigurator:IConfigurator
    {
        public void Configure(IWindsorContainer container)
        {
            container.Register(
                Component.For(typeof (IDao<>)).ImplementedBy(typeof (Dao<>)).Forward(typeof (IReadOnlyDao<>)).LifeStyle.
                    Transient);
        }
    }
}
