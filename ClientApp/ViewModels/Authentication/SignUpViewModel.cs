using ClientApp.Utilities.Command.Base;
using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.Mediator.Messages;
using ClientApp.Utilities.MyHttpClient;
using ClientApp.Utilities.Validation;
using ClientApp.ViewModels.Base;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Authentication;

public class SignUpViewModel : ViewModelBase
{
    #region Fields

    private readonly IMessenger _messenger;

    private readonly MyHttpClient _httpClient;

    private string? loginInput;
    public string? LoginInput
    {
        get => loginInput;
        set => base.PropertyChangeMethod(out loginInput, value);
    }

    private string? passwordInput;
    public string? PasswordInput
    {
        get => passwordInput;
        set => base.PropertyChangeMethod(out passwordInput, value);
    }

    private string? errorMessage;
    public string? ErrorMessage
    {
        get => errorMessage;
        set => base.PropertyChangeMethod(out errorMessage, value);
    }
    #endregion


    #region Constructor
    public SignUpViewModel(IMessenger messenger)
    {
        _messenger = messenger;
        _httpClient = App.Container.GetInstance<MyHttpClient>();
    }


    #endregion


    #region Commands

    private CommandBase? signUpCommand;
    public CommandBase SignUpCommand => this.signUpCommand ??= new CommandBase(
            execute: async () =>
            {
                this.ErrorMessage = string.Empty;

                if (Validator.ValidateCredentials(LoginInput, PasswordInput) == false)
                {
                    this.ErrorMessage = "Invalid credentials!";
                    return;
                }

                var userToCreate = new User()
                {
                    Login = LoginInput,
                    Password = PasswordInput,
                };

                var response = await _httpClient.PostAsync<User>("http://localhost:8080/User/", userToCreate);

                if (response.StatusCode != System.Net.HttpStatusCode.Created)
                    return;



                Console.WriteLine(await response.Content.ReadAsStringAsync());
            },
            canExecute: () => true);

    private CommandBase? goBackCommand;
    public CommandBase GoBackCommand => this.goBackCommand ??= new CommandBase(
            execute: async () =>
            {
                _messenger.Send(new NavigationMessage(App.Container.GetInstance<SignInViewModel>()));
            },
            canExecute: () => true);

    #endregion


    #region Methods
    public override void RefreshViewModel()
    {
        this.LoginInput = string.Empty;
        this.PasswordInput = string.Empty;
        this.ErrorMessage = string.Empty;
    }
    #endregion

}
