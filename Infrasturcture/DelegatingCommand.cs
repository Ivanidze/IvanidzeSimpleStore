using System;
using System.Windows.Input;
using System.Windows.Threading;

namespace Infrasturcture
{
    class DelegatingCommand:ICommand
    {
        private readonly Action _action;
        private readonly Fact _canExecute;
        public DelegatingCommand(Action action, Fact canExecute)
        {
            _action = action;
            _canExecute = canExecute;
            var dispatcher = Dispatcher.CurrentDispatcher;
            if (_canExecute!=null)
            {
                _canExecute.PropertyChanged +=
                    (sender, args) => dispatcher.Invoke(CanExecuteChanged, this, EventArgs.Empty);
            }
        }

        public void Execute(object parameter)
        {
            _action();
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute==null)
                return true;
            return _canExecute.Value;
        }

        public event EventHandler CanExecuteChanged = delegate { }; 
    }
}
