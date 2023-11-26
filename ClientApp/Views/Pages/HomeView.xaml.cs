using ClientApp.Utilities.Mediator;
using ClientApp.Utilities.Mediator.Interfaces;
using ClientApp.Utilities.Mediator.Messages;
using ClientApp.ViewModels.Pages;
using SharedProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClientApp.Views.Pages;

/// <summary>
/// Interaction logic for HomeView.xaml
/// </summary>
public partial class HomeView : UserControl
{
    //private readonly IMessenger _messenger;

    public HomeView()
    {
        InitializeComponent();
        //_messenger = messenger;
    }

    private void Products_LV_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        int selectedIndex = Products_LV.SelectedIndex;
        var itemSource = Products_LV.ItemsSource;

        var selectedProduct = itemSource.Cast<Product>().ElementAt(selectedIndex);

        App.Container.GetInstance<ProductInfoViewModel>().ProductId = selectedProduct.Id;
        App.Container.GetInstance<IMessenger>().Send(new NavigationMessage(App.Container.GetInstance<ProductInfoViewModel>()));
    }
}
