using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.Domain;
using FluentNHibernate.Mapping;

namespace DataModel.Mapping
{
public class WareGroupMap:ClassMap<WareGroup>
    {
    public WareGroupMap()
    {
        Table("WareGroup");
        Id(x => x.Id).GeneratedBy.Native();
        Map(x => x.Name);
        References(x => x.Parent).Column("ParentId");
        HasMany(x => x.Children)
            .KeyColumn("ParentId")
            .ForeignKeyConstraintName("fk_WareGroup_ParentWareGroup")
            .Cascade.All()
            .AsSet();
        HasManyToMany(x => x.Descendants)
            .ParentKeyColumn("ParentId")
            .ChildKeyColumn("ChildId")
            .Table("WareGroupHierarchy")
            .AsSet()
            .Inverse();
        HasManyToMany(x => x.Ancestors)
            .ParentKeyColumn("ChildId")
            .ChildKeyColumn("ParentId")
            .Table("WareGroupHierarchy")
            .AsSet();
    }
    }
}
