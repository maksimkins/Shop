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
using System.Net.Http;
using ClientApp.Utilities.MyHttpClient;
using ClientApp.ViewModels.Authentication;
using SharedProj.Models;
using ClientApp.Views.Authentication;
using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.Mediator;
using ClientApp.Views.Pages;
using System.Collections.ObjectModel;

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
            Start<SignInViewModel>();

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
            Container.RegisterSingleton<MyHttpClient>();

            Container.RegisterSingleton<IMessenger, Messenger>();

            Container.RegisterSingleton<User>();
            Container.RegisterSingleton<SignInViewModel>();
            Container.RegisterSingleton<SignUpViewModel>();

            Container.RegisterSingleton<MainWindow>();

            Container.RegisterSingleton<MainViewModel>();
            Container.RegisterSingleton<HomeViewModel>();
            Container.RegisterSingleton<AddProductViewModel>();
            Container.RegisterSingleton<ProductInfoViewModel>();
            Container.RegisterSingleton<MyProductsViewModel>();

            Container.RegisterSingleton<UserInfoViewModel>();
            Container.RegisterSingleton<ChangePasswordViewModel>();

            Container.Verify();
        }
    }
}
