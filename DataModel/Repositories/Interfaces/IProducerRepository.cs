using DataModel.Domain;

namespace DataModel.Repositories
{
    public interface IProducerRepository:IBaseRepository<Producer>
    {
        Producer GetByCaption(string caption);
     
    }
}
