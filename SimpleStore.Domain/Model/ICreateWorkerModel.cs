
namespace SimpleStore.Domain.Model

{
    public interface ICreateWorkerModel
    {
        void SaveWorker(Worker worker);
        bool IsValid(Worker worker);
        void CancelWorker(Worker worker);
        Worker CreateWorker();
    }
    
}
