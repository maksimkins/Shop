using ClientApp.Utilities.Command.Base;
using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.Mediator.Messages;
using ClientApp.Utilities.MyHttpClient;
using ClientApp.Utilities.Validation;
using ClientApp.ViewModels.Base;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Pages;

public class AddProductViewModel : ViewModelBase
{
    #region Fields

    private readonly MyHttpClient _httpClient;

    private string? titleInput;
    public string? TitleInput
    {
        get => titleInput;
        set => base.PropertyChangeMethod(out titleInput, value);
    }

    private string? descriptionInput;
    public string? DescriptionInput
    {
        get => descriptionInput;
        set => base.PropertyChangeMethod(out descriptionInput, value);
    }

    private double priceInput;
    public double PriceInput
    {
        get => priceInput;
        set => base.PropertyChangeMethod(out priceInput, value);
    }

    private string? errorMessage;
    public string? ErrorMessage
    {
        get => errorMessage;
        set => base.PropertyChangeMethod(out errorMessage, value);
    }

    private string? successMessage;
    public string? SuccessMessage
    {
        get => successMessage;
        set => base.PropertyChangeMethod(out successMessage, value);
    }

    #endregion

    #region Constructor
    public AddProductViewModel()
    {
        _httpClient = App.Container.GetInstance<MyHttpClient>();
        PriceInput = 0;
    }
    #endregion

    #region Commands
    private CommandBase? addProductCommand;
    public CommandBase AddProductCommand => this.addProductCommand ??= new CommandBase(
            execute: async () => {

                if (Validator.ValidateProductInput(TitleInput, DescriptionInput, PriceInput) == false)
                {
                    SuccessMessage = string.Empty;
                    ErrorMessage = "Invalid product info input!";
                    return;
                }

                var productToAdd = new Product()
                {
                    Title = TitleInput!,
                    Text = DescriptionInput!,
                    Price = PriceInput,
                    UserId = App.Container.GetInstance<User>().Id,
                };

                var response = await _httpClient.PostAsync("http://localhost:8080/Product", productToAdd);

                if (response.StatusCode != System.Net.HttpStatusCode.Created) 
                { 
                    ErrorMessage = "Cannot create product!";
                    return;
                }

                RefreshViewModel();
                SuccessMessage = "Product was successfully created!";
            },
            canExecute: () => true);
    #endregion


    #region Methods
    public override void RefreshViewModel()
    {
        this.TitleInput = string.Empty;
        this.DescriptionInput = string.Empty;
        this.PriceInput = 0;
        this.ErrorMessage = string.Empty;
        this.SuccessMessage = string.Empty;
    }
    #endregion
}
