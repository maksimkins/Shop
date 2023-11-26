using ClientApp.Utilities.Command.Base;
using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.Mediator.Messages;
using ClientApp.Utilities.MyHttpClient;
using ClientApp.ViewModels.Base;
using ClientApp.ViewModels.Pages;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Main;

public class MainViewModel : ViewModelBase
{
    #region Fields

    private readonly MyHttpClient _httpClient;

    private readonly IMessenger _messenger;


    private bool isAuthenticated;

    public bool IsAuthenticated
    {
        get => isAuthenticated;
        set => base.PropertyChangeMethod(out isAuthenticated, value);
    }


    private ViewModelBase? activeViewModel;
    public ViewModelBase? ActiveViewModel
    {
        get => activeViewModel;
        set => base.PropertyChangeMethod(out activeViewModel, value);
    }


    private string searchInput;

    public string SearchInput
    {
        get => searchInput;
        set => base.PropertyChangeMethod(out searchInput, value);
    }

    #endregion


    #region Constructor
    public MainViewModel(IMessenger messenger)
    {
        _httpClient = App.Container.GetInstance<MyHttpClient>();
        _messenger = messenger;

        _messenger.Subscribe<NavigationMessage>((message) =>
        {
            if (message is NavigationMessage navigationMessage)
            {
                this.ActiveViewModel = navigationMessage.DestinationViewModel;
                this.ActiveViewModel.RefreshViewModel();
            }
        });
    }
    #endregion

    #region Commands

    private CommandBase? homeCommand;
    public CommandBase HomeCommand => this.homeCommand ??= new CommandBase(
            execute: () => {
                _messenger.Send(new NavigationMessage(App.Container.GetInstance<HomeViewModel>()));
            },
            canExecute: () => true);


    private CommandBase? addProductCommand;
    public CommandBase AddProductCommand => this.addProductCommand ??= new CommandBase(
            execute: () => {
                _messenger.Send(new NavigationMessage(App.Container.GetInstance<AddProductViewModel>()));
            },
            canExecute: () => true);

    private CommandBase? userInfoCommand;
    public CommandBase UserInfoCommand => this.userInfoCommand ??= new CommandBase(
            execute: () => {
                _messenger.Send(new NavigationMessage(App.Container.GetInstance<UserInfoViewModel>()));
            },
            canExecute: () => true);


    private CommandBase? searchCommand;
    public CommandBase SearchCommand => this.searchCommand ??= new CommandBase(
            execute: async () => {

                App.Container.GetInstance<HomeViewModel>().TitleToSearch = SearchInput;
                App.Container.GetInstance<HomeViewModel>().RefreshViewModel();

                //var response = await _httpClient.GetAsync("http://localhost:8080/Product/?title=" + $"{SearchInput}");

                //if (response.StatusCode != System.Net.HttpStatusCode.OK)
                //    return;

                //var json = await response.Content.ReadAsStringAsync();
                //var products = JsonSerializer.Deserialize<ObservableCollection<Product>>(json) ??
                //    throw new Exception();

                //App.Container.GetInstance<ObservableCollection<Product>>().Clear();

                //foreach (var product in products)
                //    this.App.Container.GetInstance<ObservableCollection<Product>>().Add(product);

            },
            canExecute: () => true);

    #endregion

}
