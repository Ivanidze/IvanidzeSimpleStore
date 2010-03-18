using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrasturcture;
using Microsoft.Practices.ServiceLocation;
using System.Windows;

namespace SimpleStore.Artifacts
{
    /// <summary>
    /// реализация фабрики для создания вьюмоделей
    /// </summary>
    public class ViewFactory:IViewFactory
    {
        private readonly IServiceLocator _serviceLocator;
        public ViewFactory(IServiceLocator serviceLocator)
        {
            _serviceLocator = serviceLocator;
        }

        public TViewModel ShowView<TViewModel>()
        {
            var viewModel = ResolveViewModel<TViewModel>();

            Window view = GetView(viewModel);
            view.Show();
            Application.Current.MainWindow = view;
            return viewModel;
        }

        public TViewModel ResolveViewModel<TViewModel>() 
        {
            return _serviceLocator.GetInstance<TViewModel>();
        }
        private Window GetView<TViewModel>(TViewModel viewModel)
        {
            string viewName = typeof (TViewModel).Name.Replace("ViewModel", "View");
            string viewFullName = string.Format("SimpleStore.Gui.Views.{0}, SimpleStore.Gui", viewName);
            Type viewType = Type.GetType(viewFullName, true);
            var view = (Window) _serviceLocator.GetInstance(viewType);
            view.DataContext = viewModel;
            return view;
        }
    }
}
