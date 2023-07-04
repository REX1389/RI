using Gogogo.Pages;
using System.Windows;
using System.Windows.Navigation;

namespace Gogogo;
public partial class MainWindow : Window
{
    public static NavigationService NavigationService =>
            ((MainWindow)Application.Current.MainWindow).MainFrame.NavigationService;

    public MainWindow()
    {
        InitializeComponent();
        MainFrame.NavigationService.Navigate(new Menu());
    }
}
