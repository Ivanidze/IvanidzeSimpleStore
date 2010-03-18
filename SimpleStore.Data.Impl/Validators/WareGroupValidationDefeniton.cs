using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Cfg.Loquacious;
using SimpleStore.Domain;

namespace SimpleStore.Data.Impl.Validators
{

    public class WareGroupValidationDefeniton:ValidationDef<WareGroup>
    {
        public WareGroupValidationDefeniton()
        {
            Define(x => x.Name).NotEmpty().WithMessage("Группа товара не должна быть пустой");
        }
    }
}
