using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.Domain;
using FluentNHibernate.Mapping;

namespace DataModel.Mapping
{
    public class WareMap:ClassMap<Ware>
    {
        public WareMap()
        {
            Table("WareMap");
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Description);
            References(x => x.WareType);
            References(x => x.Worker);

        }
    }
}
