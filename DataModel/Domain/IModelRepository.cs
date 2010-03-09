using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel.Domain
{
    public interface IModelRepository
    {
        void Add(Model model);
        void Update(Model model);
        void Remove(Model model);
    }

}
