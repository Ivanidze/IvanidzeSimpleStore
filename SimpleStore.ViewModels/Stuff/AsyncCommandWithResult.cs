using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Diagnostics;

namespace SimpleStore.ViewModels.Stuff
{
    /// <summary>
    /// Команда для асинхронной работы
    /// </summary>
    /// <typeparam name="TParameter">тип данных</typeparam>
    /// <typeparam name="TResult">тип результата </typeparam>
    public class AsyncCommandWithResult<TParameter, TResult> : IAsyncCommandWithResult<TParameter,TResult>
    {
        private readonly Func<TParameter, bool> _canAction;
        private readonly Func<TParameter, TResult> _action;
        private readonly BackgroundWorker worker= new BackgroundWorker();
        public AsyncCommandWithResult(Func<TParameter,TResult> action)
        {
            _action = action;
        }
        public AsyncCommandWithResult(Func<TParameter,TResult> action, Func<TParameter,bool > canAction)
        {
            _action = action;
            _canAction = canAction;
        }
        public void Execute(object parameter)
        {
            if (Preview!=null)
            Preview((TParameter)parameter);
            worker.DoWork+=
            (sender, args) =>
            {
                args.Result = _action((TParameter)parameter);

            };
            worker.RunWorkerCompleted += (sender, args) =>
            {
                Completed((TParameter)parameter, (TResult)args.Result);
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

        public Action<TParameter, TResult> Completed { get; set;}

        public Action<TParameter> Preview { get; set; }

        public bool BlockInteraction { get; set; }

        public TResult ExecuteSync(TParameter obj)
        {
            return _action(obj);
        }
    }
}
