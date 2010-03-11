using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.Domain;
using NHibernate;
using NHibernate.Criterion;

namespace DataModel.Repositories
{
    public class ProducerRepository:IProducerRepository
    {
        public void Update(Producer producer)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(producer);
                transaction.Commit();
            }
        }

        public void Remove(Producer producer)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(producer);
                transaction.Commit();
            }
        }

        public Producer GetById(int producerId)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                return session.Get<Producer>(producerId);
            }
        }

        public Producer GetByCaption(string caption)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                Producer producer =
                    session.CreateCriteria(typeof(Producer)).Add(Restrictions.Eq("Caption", caption)).UniqueResult<Producer>();
                return producer;
            }
        }

        public void Add(Producer producer)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(producer);
                transaction.Commit();
            }
        }
    }
}
