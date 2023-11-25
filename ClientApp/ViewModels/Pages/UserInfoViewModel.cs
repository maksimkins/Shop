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

public class UserInfoViewModel : ViewModelBase
{

    #region Fields

    private readonly MyHttpClient _httpClient;
    private readonly IMessenger _messenger;

    private string? login;
    public string? Login
    {
        get => login;
        set => base.PropertyChangeMethod(out login, value);
    }

    private string? creationDate;
    public string? CreationDate
    {
        get => creationDate;
        set => base.PropertyChangeMethod(out creationDate, value);
    }

    #endregion

    #region Constructor

    public UserInfoViewModel(IMessenger messenger)
    {
        _httpClient = App.Container.GetInstance<MyHttpClient>();
        _messenger = messenger;
    }

    #endregion


    #region Commands

    private CommandBase? changePasswordCommand;
    public CommandBase ChangePasswordCommand => changePasswordCommand ??= new CommandBase(
            execute: () => {
                _messenger.Send(new NavigationMessage(App.Container.GetInstance<ChangePasswordViewModel>()));
            },
            canExecute: () => true);

    #endregion

    #region Methods
    public override void RefreshViewModel()
    {
        this.Login = App.Container.GetInstance<User>().Login;
        this.CreationDate = App.Container.GetInstance<User>().CreationalDate.ToShortDateString();
    }
    #endregion
}
