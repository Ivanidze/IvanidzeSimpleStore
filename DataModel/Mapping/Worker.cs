using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.Domain;
using FluentNHibernate.Mapping;

namespace DataModel.Mapping
{
    public class WorkerMap:SubclassMap<Worker>
    {
        public WorkerMap()
        {

        }
    }
}
