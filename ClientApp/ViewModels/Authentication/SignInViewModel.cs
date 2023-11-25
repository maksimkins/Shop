using ClientApp.Utilities.Command.Base;
using ClientApp.Utilities.Validation;
using ClientApp.ViewModels.Base;
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
    #endregion
}
