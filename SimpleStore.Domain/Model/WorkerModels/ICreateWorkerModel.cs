
namespace SimpleStore.Domain.Model.WorkerModels

{
    public interface ICreateWorkerModel
    {
        void SaveWorker(Worker worker);
        void CancelWorker(Worker worker);
        Worker CreateWorker();
    }
    
}
