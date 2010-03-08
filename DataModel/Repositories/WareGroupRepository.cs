using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.Repositories;
using NHibernate;

namespace DataModel.Domain
{
    public class WareGroupRepository : IWareGroupRepository
    {
        public void Add(WareGroup wareGroup)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(wareGroup);
                transaction.Commit();
            }
        }

        public void Update(WareGroup wareGroup)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Update(wareGroup);
                transaction.Commit();
            }
        }

        public void Remove(WareGroup wareGroup)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(wareGroup);
                transaction.Commit();
            }
        }
        public WareGroup GetAggregateById(int WareGroupId)
        {
            using (var session = NHibernateHelper.OpenSession())
            {
                var sql = "from WareGroup e" +
                          " left join fetch e.Parent p" +
                          " left join fetch e.Children c" +
                          " where e.Id = :id";
                var node = session.CreateQuery(sql)
                    .SetInt32("id", WareGroupId)
                    .UniqueResult<WareGroup>();

                // load the ancestors
                var sql2 = "from WareGroup e" +
                          " left join fetch e.Ancestors a" +
                          " where e.Id = :id";
                node = session.CreateQuery(sql2)
                    .SetInt32("id", WareGroupId)
                    .UniqueResult<WareGroup>();

                // load the descendants
                var sql3 = "from WareGroup e" +
                          " left join fetch e.Descendants d" +
                          " where e.Id = :id";
                node = session.CreateQuery(sql3)
                    .SetInt32("id", WareGroupId)
                    .UniqueResult<WareGroup>();

                return node;
            }
        }

     
    }
}
