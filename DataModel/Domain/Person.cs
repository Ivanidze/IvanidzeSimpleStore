namespace DataModel.Domain
{

    public class Person
    {
        public virtual int Id { get; set; }
        public virtual string FIO { get; set; }
        public virtual string ContactPhone{ get; set; }
    }
}