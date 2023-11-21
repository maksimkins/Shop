using ClientApp.ViewModels.Base;
using ClientApp.ViewModels.Main;
using ClientApp.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            this.Start();

            base.OnStartup(e);
        }

        private void Start()
        {
            var mainView = new MainWindow();
            var mainViewModel = new MainViewModel();

            mainView.DataContext = mainViewModel;
            mainViewModel.ActiveViewModel = new HomeViewModel();

            mainView.ShowDialog();
        }
    }
}
