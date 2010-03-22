using System.Collections.Generic;
using uNhAddIns.Entities;

namespace SimpleStore.Domain
{
    
    public class WareGroup:Entity
    {
        public WareGroup()
        {
            Children = new List<WareGroup>();
        }

        public virtual string Name { get; set; }
        public virtual WareGroup Parent { get; set; }
        public virtual IList<WareGroup> Children { get; set; }

        
        public virtual void AddChild(WareGroup children)
        {
            children.Parent = this;
            Children.Add(children);
        }
        public virtual void RemoveChild(WareGroup children)
        {
            children.Parent = null;
            Children.Remove(children);
        }
    }
}
