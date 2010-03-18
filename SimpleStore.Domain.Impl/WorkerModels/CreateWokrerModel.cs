using Infrasturcture;
using SimpleStore.Data;
using SimpleStore.Domain.Model.WorkerModels;
using uNhAddIns.Adapters;

namespace SimpleStore.Domain.Impl.WorkerModels
{
    [PersistenceConversational]
    public class CreateWorkerModel:ICreateWorkerModel
    {
        private readonly IDao<Worker> _workerDao;
        private readonly IEntityFactory _entityFactory;
        private readonly IEntityValidator _validator;
      

        public CreateWorkerModel(IDao<Worker> workerDao, IEntityFactory entityFactory, IEntityValidator validator)
        {
            _workerDao = workerDao;
            _entityFactory = entityFactory;
            _validator = validator;
        }

        public void SaveWorker(Worker worker)
        {
           if (IsValid(worker))
               _workerDao.Save(worker);
        }
        private bool IsValid(Worker worker)
        {
           return _validator.IsValid(worker);
        }

        public void CancelWorker(Worker worker)
        {
            _workerDao.Refresh(worker);
        }
        public Worker CreateWorker()
        {
            var worker = _entityFactory.Create<Worker>();
            return worker;
        }

    }
}
