using System;
using DataModel.Repositories;
using NHibernate;

namespace DataModel.Domain
{
    public class WareGroupRepository : BaseRepository<WareGroup>, IWareGroupRepository
    {
        public override WareGroup GetById(int Id)
        {
        
            using (var session = NHibernateHelper.OpenSession())
            {
                var sql = "from WareGroup e" +
                          " left join fetch e.Parent p" +
                          " left join fetch e.Children c" +
                          " where e.Id = :id";
                var node = session.CreateQuery(sql)
                    .SetInt32("id", Id)
                    .UniqueResult<WareGroup>();

                // load the ancestors
                var sql2 = "from WareGroup e" +
                          " left join fetch e.Ancestors a" +
                          " where e.Id = :id";
                node = session.CreateQuery(sql2)
                    .SetInt32("id", Id)
                    .UniqueResult<WareGroup>();

                // load the descendants
                var sql3 = "from WareGroup e" +
                          " left join fetch e.Descendants d" +
                          " where e.Id = :id";
                node = session.CreateQuery(sql3)
                    .SetInt32("id", Id)
                    .UniqueResult<WareGroup>();

                return node;
            }
        }

    }
}
