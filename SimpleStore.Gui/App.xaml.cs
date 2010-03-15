using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Infrasturcture.Bootstrapping;
using Infrasturcture;

namespace SimpleStore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            BootStrapper.Initialize();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            Presenters.Show("Workers.CreateNew");
        }
    }
}
