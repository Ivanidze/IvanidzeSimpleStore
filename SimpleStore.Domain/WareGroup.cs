using Iesi.Collections.Generic;
using uNhAddIns.Entities;

namespace SimpleStore.Domain
{
    
    public class WareGroup:Entity
    {
        public virtual string Name { get; set; }
        public virtual WareGroup Parent { get; set; }
        


        public virtual void AddChild(WareGroup childWareGroup)
        {
            childWareGroup.Parent = this;
        }
    }
}
