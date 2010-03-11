using System;
using System.Collections.Generic;
using DataModel.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;
namespace DataModel.Repositories
{
    public class PersonRepository:BaseRepository<Person>,IPersontRepository
    {
        
       
        public Person GetByFio(string contragentFio)
        {
           using (ISession session = NHibernateHelper.OpenSession())
           {
               Person contragent =
                   session.CreateCriteria(typeof (Person)).Add<Person>(x=>x.FIO==contragentFio).UniqueResult<Person>();
                       
               return contragent;
           }
        }

        public ICollection<Person> GetByPartFio(string contragentFioPart)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
               var contragent =
                    session.CreateCriteria(typeof (Person)).Add(Restrictions.Like("FIO", string.Format("%{0}%",contragentFioPart))).List<Person>();
                return contragent;
            }
        }

        
    }
}
