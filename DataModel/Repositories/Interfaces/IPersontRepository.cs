using System.Collections.Generic;
using DataModel.Domain;

namespace DataModel.Repositories

{
    public interface IPersontRepository:IBaseRepository<Person>
    {
        
        Person GetByFio(string persronFio);
        ICollection<Person> GetByPartFio(string contragentFioPart);

    }
}
