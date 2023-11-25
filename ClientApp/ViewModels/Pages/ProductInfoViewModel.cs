using ClientApp.Utilities.MyHttpClient;
using ClientApp.ViewModels.Base;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Pages;

public class ProductInfoViewModel : ViewModelBase
{
    #region Fields

    private readonly MyHttpClient _httpClient;

    private int productId;
    public int ProductId
    {
        get => productId;
        set => base.PropertyChangeMethod(out productId, value);
    }

    private string? title;
    public string? Title
    {
        get => title;
        set => base.PropertyChangeMethod(out title, value);
    }

    private string? description;
    public string? Description
    {
        get => description;
        set => base.PropertyChangeMethod(out description, value);
    }

    private double price;
    public double Price
    {
        get => price;
        set => base.PropertyChangeMethod(out price, value);
    }

    private string? creationDate;
    public string? CreationDate
    {
        get => creationDate;
        set => base.PropertyChangeMethod(out creationDate, value);
    }

    private string? authorLogin;
    public string? AuthorLogin
    {
        get => authorLogin;
        set => base.PropertyChangeMethod(out authorLogin, value);
    }

    #endregion


    #region Constructor

    public ProductInfoViewModel()
    {
        _httpClient = App.Container.GetInstance<MyHttpClient>();
    }

    #endregion

    #region Methods
    
    public async void GetProductById(int productId)
    {
        var response = await _httpClient.GetAsync("http://localhost:8080/Product" + $"/{productId}");

        if (response.StatusCode != System.Net.HttpStatusCode.OK)
            return;

        var json = await response.Content.ReadAsStringAsync();
        var product = JsonSerializer.Deserialize<Product>(json) ??
            throw new Exception();

        this.Title = product.Title;
        this.Description = product.Text;
        this.Price = product.Price;
        this.CreationDate = product.CreationalDate.ToShortDateString();
    }


    public override void RefreshViewModel()
    {
        GetProductById(this.ProductId);
    }

    #endregion
}
