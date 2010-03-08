using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel.Domain
{
    public interface IProducerRepository
    {
        void Update(Producer producer);
        void Remove(Producer producer);
        Producer GetById(int id);
        Producer GetByCaption(string caption);
        void Add(Producer producer);
    }
}
