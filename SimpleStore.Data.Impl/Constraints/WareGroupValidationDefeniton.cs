using NHibernate.Validator.Cfg.Loquacious;
using SimpleStore.Domain;

namespace SimpleStore.Data.Impl.Constraints
{

    public class WareGroupValidationDefeniton:ValidationDef<WareGroup>
    {
        public WareGroupValidationDefeniton()
        {
            Define(x => x.Name).NotEmpty().WithMessage("Название группы товара не должно быть пустым");
        }
    }
}


