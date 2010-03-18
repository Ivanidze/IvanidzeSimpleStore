using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Infrasturcture;
using Microsoft.Practices.ServiceLocation;
using SimpleStore.Domain.Model.WorkerModels;
using uNhAddIns.Adapters;
using SimpleStore.ViewModels.Workers;
using SimpleStore.Domain;
using NHibernate;
namespace SimpleStore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IGuyWire configurator = ApplicationConfiguration.GetGuyWire();
        public App()
        {
            configurator.Wire();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            var viewFactory = ServiceLocator.Current.GetInstance<IViewFactory>();
            var t = ServiceLocator.Current.GetInstance<ISimpleModel>();
            
            viewFactory.ShowView<CreateWorkerViewModel>();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            configurator.Dewire();
        }
    }
}
