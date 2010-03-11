using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using DataModel.Domain;

namespace DataModel.Mapping
{
    public class ProducerMap:ClassMap<Producer>
    {
        public ProducerMap()
        {
            Table("Producer");
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Caption);

        }
    }
}
