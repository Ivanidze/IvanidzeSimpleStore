using DataModel.Domain;
using FluentNHibernate.Mapping;

namespace DataModel.Mapping
{
    public class WorkerMap:SubclassMap<Worker>
    {
        public WorkerMap()
        {
            Table("Worker");
        }
    }
}
