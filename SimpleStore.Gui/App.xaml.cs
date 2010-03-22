using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Infrasturcture;
using Microsoft.Practices.ServiceLocation;
using SimpleStore.Domain.Model;
using SimpleStore.ViewModels;
using uNhAddIns.Adapters;

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
         //   var t = ServiceLocator.Current.GetInstance<ICreateWorkerModel>();
            
            var create = viewFactory.ShowView<WareGroupViewModel>();
            

        }
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            configurator.Dewire();
        }
    }
}
