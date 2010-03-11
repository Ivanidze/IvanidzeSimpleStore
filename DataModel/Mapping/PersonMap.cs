using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.Domain;
using FluentNHibernate.Mapping;

namespace DataModel.Mapping
{
    public class PersonMap:ClassMap<Person>
    {
        public PersonMap()
        {Table("Contragent");
            Id(x => x.Id).GeneratedBy.Native();
            Map(x => x.FIO).Not.Nullable();
            Map(x => x.ContactPhone);

        }
    }
}
