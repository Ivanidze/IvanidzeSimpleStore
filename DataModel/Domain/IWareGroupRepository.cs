using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel.Domain
{
    public  interface IWareGroupRepository
    {
        void Add(WareGroup wareGroup);
        void Update(WareGroup wareGroup);
        void Remove(WareGroup wareGroup);
        WareGroup GetAggregateById(int nodeId);
    } 
   
}
