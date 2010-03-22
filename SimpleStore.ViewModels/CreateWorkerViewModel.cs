using System;
using System.Windows;
using Infrasturcture;
using System.ComponentModel;
using SimpleStore.Domain.Model;
using SimpleStore.Domain;
using System.Windows.Input;
using SimpleStore.ViewModels.Stuff;

namespace SimpleStore.ViewModels
{
    public class CreateWorkerViewModel : INotifyPropertyChanged
    {
     
        private readonly IViewFactory _viewFactory;
        private readonly ICreateWorkerModel _createWorkerModel;
        private IAsyncCommandWithoutResult<object> _saveCommand;
        private ICommand _quitCommand;
        private Worker _worker;
        public CreateWorkerViewModel(ICreateWorkerModel createWorkerModel, IViewFactory viewFactory)
        {
            _createWorkerModel = createWorkerModel;
            _viewFactory = viewFactory;
            //_worker = createWorkerModel.CreateWorker();
            
        }
        
      
        public Worker Worker
        {
            get { return _worker; }
            set { _worker = value;
            OnPropertyChanged("Worker");
            }
        }
        public void OnException(Exception exception)
        {
            MessageBox.Show(exception.Message);
        }

        private string _status;
        public string Status
        {
            get { return _status;}
            set { _status = value;
                OnPropertyChanged("Status");
            }
        }
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand==null)
                {
                    _saveCommand = new AsyncCommandWithoutResult<object>(o => Save())
                                       {
                                           BlockInteraction = true,
                                           Preview = o => Status = "Сохранение...",
                                           Completed = (o) => { Status = "Сохранено"; }



                                       };
                }
                return _saveCommand;
            }
        }
        public ICommand QuitCommand
        {
            get { if (_quitCommand==null)
            {
                _quitCommand = new RelayCommand(o => Quit());
                
            }
                return _quitCommand;
            }
        }
        private void Quit()
        {
            OnRequestClose();
        }

        private void Save()
        {
            _createWorkerModel.SaveWorker(_worker);
            OnRequestClose();
        }
        public virtual event EventHandler RequestClose;
        private void OnRequestClose()
        {
            EventHandler handler = RequestClose;
            if (handler != null) handler(this, new EventArgs());
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
