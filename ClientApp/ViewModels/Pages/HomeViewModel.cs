using ClientApp.Utilities.Command.Base;
using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.Mediator.Messages;
using ClientApp.Utilities.MyHttpClient;
using ClientApp.ViewModels.Base;
using ClientApp.ViewModels.Main;
using SharedProj.Models;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Pages;

public class HomeViewModel : ViewModelBase
{
    #region Fields

    private readonly MyHttpClient _httpClient;

    private readonly IMessenger _messenger;

    public ObservableCollection<Product> Products { get; set; }


    private int selectedIndex;
    public int SelectedIndex
    {
        get => selectedIndex;
        set => base.PropertyChangeMethod(out selectedIndex, value);
    }

    private string? titleToSearch;
    public string? TitleToSearch
    {
        get => titleToSearch;
        set => base.PropertyChangeMethod(out titleToSearch, value);
    }

    #endregion


    #region Constructor
    public HomeViewModel(IMessenger messenger)
    {
        _httpClient = App.Container.GetInstance<MyHttpClient>();
        _messenger = messenger;


        this.Products = new ObservableCollection<Product>();

    }
    #endregion


    #region Commands

    #endregion

    #region Methods
    public async void GetProducts()
    {
        var response = await _httpClient.GetAsync("http://localhost:8080/Product/?title=" + $"{TitleToSearch}");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return;
    
        var json = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<ObservableCollection<Product>>(json) ??
            throw new Exception();

        foreach (var product in products)
            this.Products.Add(product);
    }

    public override void RefreshViewModel()
    {
        Products.Clear();
        GetProducts();
    }

    #endregion
}
