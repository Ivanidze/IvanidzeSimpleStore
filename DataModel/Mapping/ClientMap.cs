using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;

namespace DataModel.Domain.Mapping
{
    public class ClientMap : SubclassMap<Client>
    {
        public ClientMap()
        {
            Table("Client");
            Map(x => x.Identification);
        }
    }
}