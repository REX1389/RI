using System.Windows.Controls;

namespace Gogogo.Pages;

public partial class Menu2 : Page
{
    private int n;

    public Menu2(int n)
    {
        InitializeComponent();

        this.n = n;
    }

    private void ButtonHH(object sender, System.Windows.RoutedEventArgs e)
    {
        this.NavigationService.Navigate(new Game(this.n, Player.Human, Player.Human));
    }

    private void ButtonHA(object sender, System.Windows.RoutedEventArgs e)
    {
        this.NavigationService.Navigate(new Game(this.n, Player.Human, Player.AI));
    }

    private void ButtonAH(object sender, System.Windows.RoutedEventArgs e)
    {
        this.NavigationService.Navigate(new Game(this.n, Player.AI, Player.Human));
    }

    private void ButtonAA(object sender, System.Windows.RoutedEventArgs e)
    {
        this.NavigationService.Navigate(new Game(this.n, Player.AI, Player.AI));
    }
}
