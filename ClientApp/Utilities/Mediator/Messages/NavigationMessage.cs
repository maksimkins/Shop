using ClientApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Utilities.Mediator.Messages;

public class NavigationMessage
{
    public readonly ViewModelBase DestinationViewModel;

    public NavigationMessage(ViewModelBase destinationViewModel)
    {
        this.DestinationViewModel = destinationViewModel;
    }
}
