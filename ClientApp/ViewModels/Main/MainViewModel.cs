using ClientApp.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.ViewModels.Main;

public class MainViewModel : ViewModelBase
{
    #region Fields

    private ViewModelBase? activeViewModel;
    public ViewModelBase? ActiveViewModel
    {
        get => activeViewModel;
        set => base.PropertyChangeMethod(out activeViewModel, value);
    }

    #endregion


    public MainViewModel()
    {

    }
}
