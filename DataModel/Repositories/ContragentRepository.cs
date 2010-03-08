using System;
using System.Collections.Generic;
using DataModel.Domain;
using NHibernate;
using NHibernate.Criterion;
namespace DataModel.Repositories
{
    public class ContragentRepository:IContragentRepository
    {
        
        public void Add(Contragent contragent)
        {
           using (ISession session = NHibernateHelper.OpenSession())
           using (ITransaction transaction = session.BeginTransaction())
           {
               session.Save(contragent);
               transaction.Commit();
           }
        }

        public void Update(Contragent contragent)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(contragent);
                transaction.Commit();
            }
        }

        public void Remove(Contragent contragent)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(contragent);
                transaction.Commit();
            }
        }

        public Contragent GetById(int contragentId)
        {
          using (var session = NHibernateHelper.OpenSession())
          {
              return session.Get<Contragent>(contragentId);
          }
        }

        public Contragent GetByFio(string contragentFio)
        {
           using (ISession session = NHibernateHelper.OpenSession())
           {
               Contragent contragent =
                   session.CreateCriteria(typeof (Contragent)).Add(Restrictions.Eq("FIO", contragentFio)).UniqueResult
                       <Contragent>();
               return contragent;
           }
        }

        public ICollection<Contragent> GetByPartFio(string contragentFioPart)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
               var contragent =
                    session.CreateCriteria(typeof (Contragent)).Add(Restrictions.Like("FIO", string.Format("%{0}%",contragentFioPart))).List<Contragent>();
                return contragent;
            }
        }

        
    }
}
