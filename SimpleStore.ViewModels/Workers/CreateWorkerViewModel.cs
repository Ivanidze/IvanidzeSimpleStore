using System;
using System.Windows;
using Infrasturcture;

using SimpleStore.Domain.Model.WorkerModels;
using System.ComponentModel;

namespace SimpleStore.ViewModels.Workers
{
    public class CreateWorkerViewModel : INotifyPropertyChanged
    {
        private readonly ICreateWorkerModel _createWorkerModel;
        private readonly IViewFactory _viewFactory;
      

        public CreateWorkerViewModel(ICreateWorkerModel createWorkerModel,IViewFactory viewFactory)
        {
            _createWorkerModel = createWorkerModel;
            _viewFactory = viewFactory;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler changed = PropertyChanged;
            if (changed != null) changed(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnException(Exception exception)
        {
            MessageBox.Show(exception.Message);
        }
    }
}
