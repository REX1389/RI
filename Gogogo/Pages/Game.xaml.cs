using System.Windows;
using System.Windows.Controls;

namespace Gogogo.Pages;

public enum Player
{AI, Human}

public partial class Game : Page
{
    public Game(int n, Player player1, Player player2)
    {
        InitializeComponent();
    }

    private void ButtonClick(object sender, RoutedEventArgs e)
    {

    }
}
