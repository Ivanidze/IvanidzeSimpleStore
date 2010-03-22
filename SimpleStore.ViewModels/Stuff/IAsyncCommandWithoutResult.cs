using System;
using System.Windows.Input;

namespace SimpleStore.ViewModels.Stuff
{
    public interface IAsyncCommandWithoutResult<TParameter> : ICommand
    {
        Action<TParameter> Completed { get; set; }
        Action<TParameter> Preview { get; set; }
        bool BlockInteraction { get; set; }
        void ExecuteSync(TParameter obj);
    }
}
