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

    public ObservableCollection<Product> Products { get; set; }

    #endregion


    #region Constructor
    public HomeViewModel()
    {
        _httpClient = App.Container.GetInstance<MyHttpClient>();
        this.Products = new ObservableCollection<Product>();
        GetProducts();
    }
    #endregion


    #region Methods
    public async void GetProducts()
    {
        var response = await _httpClient.GetAsync("http://localhost:8080/Product");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return;
    
        var json = await response.Content.ReadAsStringAsync();
        var products = JsonSerializer.Deserialize<ObservableCollection<Product>>(json) ??
            throw new Exception();

        foreach (var product in products)
            this.Products.Add(product);
    }

    #endregion
}
