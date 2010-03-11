using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using DataModel.Domain;

namespace DataModel.Mapping
{
    public class WareForTestingMap:SubclassMap<WareForTesting>
    {
        public WareForTestingMap()
        {
            Table("WareForTesting");
            Map(x => x.Priority);
        }
    }
}
