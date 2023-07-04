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
    public List<List<int>> board, previousBoard;

    public GameState(int n)
    {
        this.turn = 0;
        this.n = n;
        this.pass = 0;

        board = new List<List<int>>();
        previousBoard = new List<List<int>>();
        for (int i = 1; i <= n; i++)
        {
            board.Add(new List<int>());
            previousBoard.Add(new List<int>());
            for (int j = 1; j <= n; j++)
            {
                board[i - 1].Add(-1);
                previousBoard[i - 1].Add(-1);
            }
        }
    }

    public bool EqualsToPrevious(List<List<int>> board2)
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                if (previousBoard[i][j] != board2[i][j])
                    return false;
        return true;
    }


    public bool CanPlaceStone(int i, int j)
    {
        return true;
    }

    public void PlaceStone(int i, int j)
    {
        board[i - 1][j - 1] = turn;
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
                button.Name = "Button_" + i + "_" + j;
                RegisterName(button.Name, button);
                button.BorderThickness = new Thickness(0);
                button.Padding = new Thickness(0);
                button.Content = img;
                button.Click += ButtonPlaceStone;
                
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

    public void DrawBoard()
    {
        int n = state.n;
        for(int i = 1; i <= n; i++)
            for(int j = 1; j <= n; j++)
            {
                Button button = FindName("Button_" + i + "_" + j) as Button;
                Image img = button.Content as Image;
                if (state.board[i - 1][j - 1] == -1)
                    img.Source = new BitmapImage(new Uri("/Pictures/Empty.png", UriKind.Relative));
                if (state.board[i - 1][j - 1] == 0)
                    img.Source = new BitmapImage(new Uri("/Pictures/Black.png", UriKind.Relative));
                if (state.board[i - 1][j - 1] == 1)
                    img.Source = new BitmapImage(new Uri("/Pictures/White.png", UriKind.Relative));
            }
    }

    public void EndTurn()
    {
        state.turn = 1 - state.turn;
        if (state.turn == 0)
            TurnTxt.Text = "Turn: Black";
        else
            TurnTxt.Text = "Turn: White";
        DrawBoard();
    }

    private void ButtonPlaceStone(object sender, RoutedEventArgs e)
    {
        Button button = sender as Button;
        int i, j;
        int.TryParse(button.Name.Split("_")[1], out i);
        int.TryParse(button.Name.Split("_")[2], out j);
        if(state.CanPlaceStone(i, j))
        {
            state.PlaceStone(i, j);
            EndTurn();
        }
    }

    private void Pass(object sender, RoutedEventArgs e)
    {

    }

    private void Resign(object sender, RoutedEventArgs e)
    {

    }
}
