using ClientApp.Utilities.Command.Base;
using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.Mediator.Messages;
using ClientApp.Utilities.MyHttpClient;
using ClientApp.Utilities.Validation;
using ClientApp.ViewModels.Base;
using ClientApp.ViewModels.Pages;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Authentication;

public class SignInViewModel : ViewModelBase
{
    #region Fields

    private readonly MyHttpClient _httpClient;

    private readonly IMessenger _messenger;


    private string? userInput;
    public string? UsernameInput
    {
        get => userInput;
        set => base.PropertyChangeMethod(out userInput, value);
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

    #endregion Fields

    #region Constructor
    public SignInViewModel(IMessenger messenger)
    {
        _httpClient = App.Container.GetInstance<MyHttpClient>();
        _messenger = messenger;
    }
    #endregion


    #region Commands

    private CommandBase? loginCommand;
    public CommandBase LoginCommand => this.loginCommand ??= new CommandBase(
            execute: () => {
                this.ErrorMessage = string.Empty;

                if (Validator.ValidateCredentials(UsernameInput, PasswordInput) == false)
                {
                    this.ErrorMessage = "Invalid credentials!";
                    return;
                }

                //var response = await _httpClient.PostAsync<User>("http://localhost:8080/User/", userToCreate)

                //var user = AuthenticateUser();

                //if (user == null)
                //{
                //    this.ErrorMessage = "Couldn't login in the system!";
                //    return;
                //}

                //App.Container.GetInstance<User>().Id = user.Id;
                //App.Container.GetInstance<User>().Login = user.Username;
                //App.Container.GetInstance<User>().Password = user.Password;

            },
            canExecute: () => true);

    private CommandBase? signUpCommand;
    public CommandBase SignUpCommand => this.signUpCommand ??= new CommandBase(
            execute: () => {
                _messenger.Send(new NavigationMessage(App.Container.GetInstance<SignUpViewModel>()));
            },
            canExecute: () => true);
    #endregion
}
