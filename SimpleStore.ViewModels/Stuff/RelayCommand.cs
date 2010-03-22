using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Diagnostics;

namespace SimpleStore.ViewModels.Stuff
{
    /// <summary>
    /// Класс комманд для ведения логики через делегаты
    /// подробнее:http://msdn.microsoft.com/ru-ru/magazine/dd419663.aspx 
    /// </summary>
    public class RelayCommand:ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;
        public RelayCommand(Action<object> execute):this(execute,null)
        {
        }
        public RelayCommand(Action execute):this(o=>execute())
        {
        }

        public RelayCommand(Action execute,Func<bool> canExecute):this(o=>execute(),o=>canExecute())
        {
        }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute==null)
                throw new ArgumentNullException("Relay command initialize execute is null");
            _execute = execute;
            _canExecute = canExecute;
        }

      
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
