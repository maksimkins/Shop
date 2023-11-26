using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.MyHttpClient;
using ClientApp.ViewModels.Base;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Pages;

public class MyProductsViewModel : ViewModelBase
{
    #region Fields

    private readonly MyHttpClient _httpClient;

    private readonly IMessenger _messenger;

    public ObservableCollection<Product> MyProducts { get; set; }


    private int selectedIndex;
    public int SelectedIndex
    {
        get => selectedIndex;
        set => base.PropertyChangeMethod(out selectedIndex, value);
    }

    #endregion


    #region Constructor
    public MyProductsViewModel(IMessenger messenger)
    {
        _httpClient = App.Container.GetInstance<MyHttpClient>();
        _messenger = messenger;


        this.MyProducts = new ObservableCollection<Product>();

    }
    #endregion


    #region Commands

    #endregion

    #region Methods
    public async void GetProducts()
    {
        var response = await _httpClient.GetAsync("http://localhost:8080/Product/?" + $"user_id={App.Container.GetInstance<User>().Id}");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return;

        var json = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<ObservableCollection<Product>>(json) ??
            throw new Exception();

        foreach (var product in products)
            this.MyProducts.Add(product);
    }

    public override void RefreshViewModel()
    {
        MyProducts.Clear();
        GetProducts();
    }

    #endregion
}
