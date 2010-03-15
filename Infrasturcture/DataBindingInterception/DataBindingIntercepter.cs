using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;

namespace Infrasturcture.DataBindingInterception
{
    /// <summary>
    /// перехватчик привязки к данным
    /// </summary>
    public class DataBindingIntercepter:EmptyInterceptor
    {
        public ISessionFactory SessionFactory { get; set; }
        public override object Instantiate(string clazz, EntityMode entityMode, object id)
        {
            if (entityMode==EntityMode.Poco)
            {
                Type type = Type.GetType(clazz);
                if (type!=null)
                {
                    var instance = DataBindingFactory.Create(type);
                    SessionFactory.GetClassMetadata(clazz).SetIdentifier(instance,id,entityMode);
                }
            }

            return base.Instantiate(clazz, entityMode, id);
        }
        public override string GetEntityName(object entity)
        {
            var markerInterface = entity as DataBindingFactory.IMarkerInterface;
            if (markerInterface != null)
                return markerInterface.TypeName;
            return base.GetEntityName(entity);
        }
    }
}
