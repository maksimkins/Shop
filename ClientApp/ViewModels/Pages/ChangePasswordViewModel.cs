using ClientApp.Utilities.Command.Base;
using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.Mediator.Messages;
using ClientApp.Utilities.MyHttpClient;
using ClientApp.ViewModels.Base;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Pages;

public class ChangePasswordViewModel : ViewModelBase
{
    #region Fields

    private readonly MyHttpClient _httpClient;
    private readonly IMessenger _messenger;

    private string? password;
    public string? Password
    {
        get => password;
        set => base.PropertyChangeMethod(out password, value);
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

    public ChangePasswordViewModel(IMessenger messenger)
    {
        _httpClient = App.Container.GetInstance<MyHttpClient>();
        _messenger = messenger;
    }

    #endregion

    #region Commands

    private CommandBase? savePasswordCommand;
    public CommandBase SavePasswordCommand => savePasswordCommand ??= new CommandBase(
            execute: async () => {
                if (string.IsNullOrEmpty(Password))
                {
                    SuccessMessage = string.Empty;
                    ErrorMessage = "Invalid password!";
                    return;
                }

                var userToUpdate = new User()
                {
                    Id = App.Container.GetInstance<User>().Id,
                    Login = App.Container.GetInstance<User>().Login,
                    Password = this.Password,
                };

                var response = await _httpClient.PutAsync("http://localhost:8080/User/", userToUpdate);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    SuccessMessage = string.Empty;
                    ErrorMessage = "Cannot create product!";
                    return;
                }

                App.Container.GetInstance<User>().Password = this.Password;
                SuccessMessage = "Password was successfully changed!";

            },
            canExecute: () => true);

    #endregion


    #region Methods
    public override void RefreshViewModel()
    {
        this.Password = App.Container.GetInstance<User>().Password;
        this.ErrorMessage = string.Empty;
        this.SuccessMessage = string.Empty;
    }

    #endregion
}
