using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace SimpleStore.ViewModels.Stuff
{
    public class AsyncCommandWithoutResult<TParameter>:IAsyncCommandWithoutResult<TParameter>
    {

        private readonly Func<TParameter, bool> _canAction;
        private readonly Action<TParameter> _action;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public AsyncCommandWithoutResult(Action<TParameter> action)
        {
            _action = action;
        }

        public AsyncCommandWithoutResult(Func<TParameter> action,Func<TParameter,bool> canAction)
        {
            _action = _action;
            _canAction = _canAction;
        }

        public Action<TParameter> Completed{ get; set;}
        

        public Action<TParameter> Preview{ get; set;}

        public bool BlockInteraction { get; set; }

        public void ExecuteSync(TParameter obj)
        {
             _action(obj);
        }

        public void Execute(object parameter)
        {
            Preview((TParameter)parameter);
            worker.DoWork +=
            (sender, args) => _action((TParameter)parameter);
            worker.RunWorkerCompleted += (sender, args) =>
            {
                Completed((TParameter)parameter);
                CommandManager.InvalidateRequerySuggested();
            };

            //Run the async work.
            worker.RunWorkerAsync();
        }
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (BlockInteraction && worker.IsBusy)
                return false;

            return _canAction == null ? true : _canAction((TParameter)parameter);
        }


        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
