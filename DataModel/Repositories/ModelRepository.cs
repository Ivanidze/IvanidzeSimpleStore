using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.Domain;
using NHibernate;

namespace DataModel.Repositories
{
    public class ModelRepository:IModelRepository
    {
        public void Add(Model model)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(model);
                transaction.Commit();
            }
        }

        public void Update(Model model)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(model);
                transaction.Commit();
            }
        }

        public void Remove(Model model)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(model);
                transaction.Commit();
            }
        }
    }
}
