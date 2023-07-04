﻿using System;
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

    public int CountThroughBoard(int x, int y)
    {
        int result = 1; //1 jer je pocetno polje vec poseceno

        int s = board[x][y], i, j;

        List<int> VisitedX = new List<int>();
        List<int> VisitedY = new List<int>();

        VisitedX.Add(x);
        VisitedY.Add(y);

        for (int k = 0; k < VisitedX.Count; k++)
        {
            i = VisitedX[k];
            j = VisitedY[k];

            if (i > 0)
            {
                if (board[i - 1][j] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i - 1 == VisitedX[l] && j == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i - 1);
                        VisitedY.Add(j);
                        result++;
                    }
                }
            }
            if (i < n - 1)
            {
                if (board[i + 1][j] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i + 1 == VisitedX[l] && j == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i + 1);
                        VisitedY.Add(j);
                        result++;
                    }
                }
            }
            if (j > 0)
            {
                if (board[i][j - 1] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i == VisitedX[l] && j - 1 == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i);
                        VisitedY.Add(j - 1);
                        result++;
                    }
                }
            }
            if (j < n - 1)
            {
                if (board[i][j + 1] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i == VisitedX[l] && j + 1 == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i);
                        VisitedY.Add(j + 1);
                        result++;
                    }
                }
            }
        }

        return result;
    }

    public void ReplaceThroughBoard(int x, int y, int v, List<List<int>> b)
    {
        int s = b[x][y], i, j;

        List<int> VisitedX = new List<int>();
        List<int> VisitedY = new List<int>();

        VisitedX.Add(x);
        VisitedY.Add(y);

        for (int k = 0; k < VisitedX.Count; k++)
        {
            i = VisitedX[k];
            j = VisitedY[k];

            b[i][j] = v;

            if (i > 0)
            {
                if (b[i - 1][j] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i - 1 == VisitedX[l] && j == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i - 1);
                        VisitedY.Add(j);
                    }
                }
            }
            if (i < n - 1)
            {
                if (b[i + 1][j] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i + 1 == VisitedX[l] && j == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i + 1);
                        VisitedY.Add(j);
                    }
                }
            }
            if (j > 0)
            {
                if (b[i][j - 1] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i == VisitedX[l] && j - 1 == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i);
                        VisitedY.Add(j - 1);
                    }
                }
            }
            if (j < n - 1)
            {
                if (b[i][j + 1] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i == VisitedX[l] && j + 1 == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i);
                        VisitedY.Add(j + 1);
                    }
                }
            }
        }
    }

    public bool SearchThroughBoard(int x, int y, int v, List<List<int>> b)
    {
        int s = b[x][y], i, j;

        List<int> VisitedX = new List<int>();
        List<int> VisitedY = new List<int>();

        VisitedX.Add(x);
        VisitedY.Add(y);

        for(int k = 0; k < VisitedX.Count; k++)
        {
            i = VisitedX[k];
            j = VisitedY[k];

            if(i > 0)
            {
                if (b[i - 1][j] == v)
                    return true;
                if (b[i - 1][j] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i - 1 == VisitedX[l] && j == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i - 1);
                        VisitedY.Add(j);
                    }
                }
            }
            if(i < n - 1)
            {
                if (b[i + 1][j] == v)
                    return true;
                if (b[i + 1][j] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i + 1 == VisitedX[l] && j == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i + 1);
                        VisitedY.Add(j);
                    }
                }
            }
            if(j > 0)
            {
                if (b[i][j - 1] == v)
                    return true;
                if (b[i][j - 1] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i == VisitedX[l] && j - 1 == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i);
                        VisitedY.Add(j - 1);
                    }
                }
            }
            if(j < n - 1)
            {
                if (b[i][j + 1] == v)
                    return true;
                if (b[i][j + 1] == s)
                {
                    bool put = true;
                    for (int l = 0; l < VisitedX.Count; l++)
                        if (i == VisitedX[l] && j + 1 == VisitedY[l])
                            put = false;
                    if (put)
                    {
                        VisitedX.Add(i);
                        VisitedY.Add(j + 1);
                    }
                }
            }
        }

        return false;
    }

    public void PlaceStoneOnBoard(int i, int j, List<List<int>> b)
    {
        b[i][j] = turn;
        if (i > 0 && b[i - 1][j] == 1 - turn && !SearchThroughBoard(i - 1, j, -1, b))
            ReplaceThroughBoard(i - 1, j, -1, b);
        if (j > 0 && b[i][j - 1] == 1 - turn && !SearchThroughBoard(i, j - 1, -1, b))
            ReplaceThroughBoard(i, j - 1, -1, b);
        if (i < n - 1 && b[i + 1][j] == 1 - turn && !SearchThroughBoard(i + 1, j, -1, b))
            ReplaceThroughBoard(i + 1, j, -1, b);
        if (j < n - 1 && b[i][j + 1] == 1 - turn && !SearchThroughBoard(i, j + 1, -1, b))
            ReplaceThroughBoard(i, j + 1, -1, b);
    }

    public bool CanPlaceStone(int x, int y)
    {
        if (board[x][y] != -1)
            return false;

        List<List<int>> b = new List<List<int>>();
        for (int i = 1; i <= n; i++)
        {
            b.Add(new List<int>());
            for (int j = 1; j <= n; j++)
            {
                b[i - 1].Add(board[i - 1][j - 1]);
            }
        }

        PlaceStoneOnBoard(x, y, b);
        if (EqualsToPrevious(b))
            return false;
        if (!SearchThroughBoard(x, y, -1, b))
            return false;

        return true;
    }

    public void PlaceStone(int x, int y)
    {
        previousBoard = new List<List<int>>();
        for (int i = 1; i <= n; i++)
        {
            previousBoard.Add(new List<int>());
            for (int j = 1; j <= n; j++)
            {
                previousBoard[i - 1].Add(board[i - 1][j - 1]);
            }
        }
        PlaceStoneOnBoard(x, y, board);
        turn = 1 - turn;
        pass = 0;
    }

    public void Pass()
    {
        pass++;
        turn = 1 - turn;
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
        if (state.turn == 0)
            TurnTxt.Text = "Turn: Black";
        if (state.turn == 1)
            TurnTxt.Text = "Turn: White";
        DrawBoard();
    }

    private void ButtonPlaceStone(object sender, RoutedEventArgs e)
    {
        if (state.turn == 0 && player1 == Player.AI)
            return;
        if (state.turn == 1 && player2 == Player.AI)
            return;

        Button button = sender as Button;
        int i, j;
        int.TryParse(button.Name.Split("_")[1], out i);
        int.TryParse(button.Name.Split("_")[2], out j);
        if(state.CanPlaceStone(i - 1, j - 1))
        {
            state.PlaceStone(i - 1, j - 1);
            EndTurn();
        }
    }

    private void ButtonPass(object sender, RoutedEventArgs e)
    {
        if (state.turn == 0 && player1 == Player.AI)
            return;
        if (state.turn == 1 && player2 == Player.AI)
            return;

        state.Pass();
        if (state.pass == 2)
            EndGame();
        else
            EndTurn();
    }

    private void EndGame()
    {
        if (state.turn == 0)
            WinnerTxt.Text = "Winner: White";
        else
            WinnerTxt.Text = "Winner: Black";
        state.turn = -1;
        TurnTxt.Text = "Turn:";
    }

    private void Resign(object sender, RoutedEventArgs e)
    {
        if (state.turn == 0)
            WinnerTxt.Text = "Winner: White";
        else
            WinnerTxt.Text = "Winner: Black";
        state.turn = -1;
        TurnTxt.Text = "Turn:";
    }
}
