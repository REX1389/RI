using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gogogo.Pages;

public enum Player
{AI, Human}

public class GameState
{
    public int turn, n, pass;
    public List<List<int>> board = new List<List<int>>();

    public GameState(int n)
    {
        this.turn = 0;
        this.n = n;
        this.pass = 0;

        for (int i = 1; i <= n; i++)
        {
            board.Add(new List<int>());
            for (int j = 1; j <= n; j++)
            {
                board[i - 1].Add(-1);
            }
        }
    }
}

public partial class Game : Page
{
    private Player player1, player2;
    private GameState state;

    public Game(int n, Player player1, Player player2)
    {
        InitializeComponent();
        this.player1 = player1;
        this.player2 = player2;
        this.state = new GameState(n);

        MakeBoard(n);
    }

    private void MakeBoard(int n)
    {
        for (int i = 1; i <= n; i++)
        {
            Grid row = new Grid();
            for (int j = 1; j <= n; j++)
            {
                Image img = new Image();
                img.Source = new BitmapImage(new Uri("/Pictures/Empty.png", UriKind.Relative));
                Button button = new Button();
                button.BorderThickness = new Thickness(0);
                button.Padding = new Thickness(0);
                button.Content = img;
                button.Click += PlaceStone;
                
                ColumnDefinition gridColumn1 = new ColumnDefinition();
                gridColumn1.Width = new GridLength(50);
                row.ColumnDefinitions.Add(gridColumn1);
                Grid.SetColumn(button, j - 1);
                row.Children.Add(button);
            }
            RowDefinition gridRow1 = new RowDefinition();
            gridRow1.Height = new GridLength(50);
            Board.RowDefinitions.Add(gridRow1);
            Grid.SetRow(row, i - 1);
            Board.Children.Add(row);
        }
    }

    private void PlaceStone(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        Image img = button.Content as Image;
        img.Source = new BitmapImage(new Uri("/Pictures/Black.png", UriKind.Relative));
    }

    private void Pass(object sender, RoutedEventArgs e)
    {

    }

    private void Resign(object sender, RoutedEventArgs e)
    {

    }
}
