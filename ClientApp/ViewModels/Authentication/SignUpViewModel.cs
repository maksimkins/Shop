using ClientApp.Utilities.Command.Base;
using ClientApp.Utilities.Validation;
using ClientApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Authentication;

public class SignUpViewModel : ViewModelBase
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
    #endregion


    #region Constructor



    #endregion


    #region Commands
    private CommandBase? signUpCommand;
    public CommandBase SignUpCommand => this.signUpCommand ??= new CommandBase(
            execute: () =>
            {
                this.ErrorMessage = string.Empty;

                if (Validator.ValidateCredentials(UsernameInput, PasswordInput) == false)
                {
                    this.ErrorMessage = "Invalid credentials!";
                    return;
                }

                
            },
            canExecute: () => true);
    #endregion


    #region Methods
    public override void RefreshViewModel()
    {
        this.UsernameInput = string.Empty;
        this.PasswordInput = string.Empty;
        this.ErrorMessage = string.Empty;
    }
    #endregion

}
