using System;
using System.Collections.Generic;
using DataModel.Domain;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.LambdaExtensions;
namespace DataModel.Repositories
{
    public class PersonRepository:BaseRepository<Person>,IPersonRepository
    {
        
       
        public Person GetByFio(string contragentFio)
        {
           using (ISession session = NHibernateHelper.OpenSession())
           {
               Person person =
                   session.CreateCriteria<Person>().Add<Person>(x=>x.FIO==contragentFio).UniqueResult<Person>();
                       
               return person;
           }
        }

        public ICollection<Person> GetByPartFio(string contragentFioPart)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
               var contragent =
                    session.CreateCriteria<Person>().Add(SqlExpression.Like<Person>(p => p.FIO, string.Format("%{0}%",contragentFioPart))).List<Person>();
                return contragent;
            }
        }

        
    }
}
