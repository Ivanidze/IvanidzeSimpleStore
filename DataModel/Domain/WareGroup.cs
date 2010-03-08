using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iesi.Collections.Generic;

namespace DataModel.Domain
{
    
    public class WareGroup
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual WareGroup Parent { get; set; }
        public virtual ISet<WareGroup> Children { get; set; }

        public virtual ISet<WareGroup> Ancestors { get; set; }
        public virtual ISet<WareGroup> Descendants { get; set; }

        public WareGroup()
        {
            Children = new HashedSet<WareGroup>();
            Ancestors = new HashedSet<WareGroup>();
            Descendants = new HashedSet<WareGroup>();
        }

        public virtual void AddChild(WareGroup childWareGroup)
        {
            Children.Add(childWareGroup);
            childWareGroup.Parent = this;
            childWareGroup.Ancestors.AddAll(this.Ancestors);
            childWareGroup.Ancestors.Add(this);
            this.Descendants.Add(childWareGroup);
        }
    }
}
