using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataModel.Domain
{
    public class WareType
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual WareGroup WareGroup { get; set; }
        public virtual Producer Producer { get; set; }
    }
}
