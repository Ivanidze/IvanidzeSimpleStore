using DataModel.Domain;
using NHibernate;

using NHibernate.LambdaExtensions;
namespace DataModel.Repositories
{
    public class ProducerRepository:BaseRepository<Producer>,IProducerRepository
    {

        public Producer GetByCaption(string caption)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                var producer =
                   session.CreateCriteria<Producer>().Add<Producer>(x => x.Caption == caption).UniqueResult<Producer>();
                return producer;
            }
        }

      
    }
}
