
namespace SimpleStore.Domain.Model.WorkerModels

{
    public interface ICreateNewWorkerModel
    {
        void SaveWorker(Worker worker);
        void CancelWorker(Worker worker);
    }
    
}
