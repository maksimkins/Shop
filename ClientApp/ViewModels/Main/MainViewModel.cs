using ClientApp.Utilities.Command.Base;
using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.Mediator.Messages;
using ClientApp.ViewModels.Base;
using ClientApp.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Main;

public class MainViewModel : ViewModelBase
{
    #region Fields

    private readonly IMessenger _messenger;


    private bool isAuthenticated;

    public bool IsAuthenticated
    {
        get => isAuthenticated;
        set => base.PropertyChangeMethod(out isAuthenticated, value);
    }


    private ViewModelBase? activeViewModel;
    public ViewModelBase? ActiveViewModel
    {
        get => activeViewModel;
        set => base.PropertyChangeMethod(out activeViewModel, value);
    }

    #endregion


    #region Constructor
    public MainViewModel(IMessenger messenger)
    {
        _messenger = messenger;

        _messenger.Subscribe<NavigationMessage>((message) =>
        {
            if (message is NavigationMessage navigationMessage)
            {
                this.ActiveViewModel = navigationMessage.DestinationViewModel;
                this.ActiveViewModel.RefreshViewModel();
            }
        });
    }
    #endregion

    #region Commands

    private CommandBase? homeCommand;
    public CommandBase HomeCommand => this.homeCommand ??= new CommandBase(
            execute: () => {
                _messenger.Send(new NavigationMessage(App.Container.GetInstance<HomeViewModel>()));
            },
            canExecute: () => true);


    private CommandBase? addProductCommand;
    public CommandBase AddProductCommand => this.addProductCommand ??= new CommandBase(
            execute: () => {
                _messenger.Send(new NavigationMessage(App.Container.GetInstance<AddProductViewModel>()));
            },
            canExecute: () => true);

    #endregion

}
