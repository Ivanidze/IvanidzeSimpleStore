using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;
using uNhAddIns.Entities;

namespace Infrasturcture
{
    public class EntityFactory:IEntityFactory
    {
        private readonly IServiceLocator _serviceLocator;
        public EntityFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }
        public  T Create<T>()where T:Entity
        {
            try
            {
                return _serviceLocator.GetInstance<T>();
            }
            catch(ActivationException)
            {
                return Activator.CreateInstance<T>();
            }
        }
    }
}
