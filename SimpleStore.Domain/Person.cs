using uNhAddIns.Entities;

namespace SimpleStore.Domain
{

    public class Person:Entity
    {
        public virtual string FIO { get; set; }
        public virtual string ContactPhone{ get; set; }
    }
}