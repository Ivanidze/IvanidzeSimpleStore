using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.Domain;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace DataModel.Mapping
{
    public class WareTypeMap : ClassMap<WareType>
    {
        public WareTypeMap()
        {
            Table("WareType");
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.Name);
            References(x => x.WareGroup);
            References(x => x.Producer);
        }
    }
}
