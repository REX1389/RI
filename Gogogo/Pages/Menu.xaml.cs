using System.Windows.Controls;

namespace Gogogo.Pages;

public partial class Menu : Page
{
    public Menu()
    {
        InitializeComponent();
    }

    private void Button7(object sender, System.Windows.RoutedEventArgs e)
    {
        this.NavigationService.Navigate(new Menu2(7));
    }

    private void Button9(object sender, System.Windows.RoutedEventArgs e)
    {
        this.NavigationService.Navigate(new Menu2(9));
    }

    private void Button13(object sender, System.Windows.RoutedEventArgs e)
    {
        this.NavigationService.Navigate(new Menu2(13));
    }

    private void Button19(object sender, System.Windows.RoutedEventArgs e)
    {
        this.NavigationService.Navigate(new Menu2(19));
    }
}
