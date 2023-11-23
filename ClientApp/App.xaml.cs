﻿using ClientApp.ViewModels.Base;
using ClientApp.ViewModels.Main;
using ClientApp.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SimpleInjector;

namespace ClientApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {

        public static Container Container { get; set; } = new Container();

        protected override void OnStartup(StartupEventArgs e)
        {
            RegisterContainer();
            Start<HomeViewModel>();

            base.OnStartup(e);
        }

        private static void Start<T>() where T : ViewModelBase
        {
            var mainView = Container.GetInstance<MainWindow>();
            var mainViewModel = Container.GetInstance<MainViewModel>();

            mainView.DataContext = mainViewModel;
            mainViewModel.ActiveViewModel = Container.GetInstance<T>();

            mainView.ShowDialog();
        }

        private static void RegisterContainer()
        {
            Container.RegisterSingleton<MainWindow>();
            Container.RegisterSingleton<MainViewModel>();
            Container.RegisterSingleton<HomeViewModel>();

            Container.Verify();
        }
    }
}