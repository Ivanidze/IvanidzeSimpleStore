
using uNhAddIns.Entities;

namespace SimpleStore.Domain
{
    public class WareType:Entity
    {
        public virtual string Name { get; set; }
        public virtual WareGroup WareGroup { get; set; }
        public virtual Producer Producer { get; set; }
    }
}
