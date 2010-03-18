using NHibernate.Validator.Cfg.Loquacious;
using SimpleStore.Domain;

namespace SimpleStore.Data.Impl.Validators
{
    public class WorkerValidationDefenition : ValidationDef<Worker>
    {
        public WorkerValidationDefenition()
        {
            Define(x => x.FIO).NotEmpty().WithMessage("ФИО не может быть пустым");
            Define(x => x.ContactPhone).NotEmpty().WithMessage("Контактный телефон не может быть пустым");
        }

    }
}
