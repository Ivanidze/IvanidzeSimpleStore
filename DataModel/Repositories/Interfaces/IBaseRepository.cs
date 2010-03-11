using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel.Repositories
{
    public interface IBaseRepository<T>
    {
        void Add(T obj);
        void Update(T obj);
        void Remove(T obj);
        T GetById(int id);
        ICollection<T> GetAll();

    }
}
