using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Gogogo.Pages;

public class con
{
    public const int C = 2, TIME = 3;
}

public enum Player
{AI, Human}

public class Turn
{
    public int x, y;

    public Turn(int i, int j)
    {
        this.x = i;
        this.y = j;
    }
}

public class Node
{
    public int visited = 0;
    public double value = 0;
    public GameState state;
    public Turn Turn;
    public List<Node> children = new List<Node>();

    public Node(GameState gs)
    {
        state = new GameState(gs);
    }

    public Node(GameState gs, int i, int j)
    {
        Turn = new Turn(i, j);
        state = new GameState(gs, i, j);
    }

    public double Explore()
    {
        double v;

        if(visited == 0)
        {
            v = state.Simulate();
            foreach (Turn t in state.FindAllMoves())
                children.Add(new Node(state, t.x, t.y));
        }
        else
        {
            double maxV = -1;
            int turn = 0;

            foreach(Node child in children)
            {
                double tempV;
                if (child.visited == 0)
                    tempV = 1000;
                else
                    tempV = child.value / child.visited + con.C * Math.Sqrt(visited / child.visited);

                if(tempV > maxV)
                {
                    maxV = tempV;
                    turn = children.IndexOf(child);
                }
            }

            v = children[turn].Explore();
        }

        value += v;
        visited++;

        return v;
    }
}

public class GameState
{
    public int turn, n, pass, winner = -1;
    public double point1 = 0, point2 = 0;
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

    public GameState(GameState gs)
    {
        turn = gs.turn;
        n = gs.n;
        pass = gs.pass;

        board = new List<List<int>>();
        previousBoard = new List<List<int>>();
        for (int i = 1; i <= n; i++)
        {
            board.Add(new List<int>());
            previousBoard.Add(new List<int>());
            for (int j = 1; j <= n; j++)
            {
                board[i - 1].Add(gs.board[i - 1][j - 1]);
                previousBoard[i - 1].Add(gs.previousBoard[i - 1][j - 1]);
            }
        }
    }

    public GameState(GameState gs, int x, int y)
    {
        turn = gs.turn;
        n = gs.n;
        pass = gs.pass;

        board = new List<List<int>>();
        previousBoard = new List<List<int>>();
        for (int i = 1; i <= n; i++)
        {
            board.Add(new List<int>());
            previousBoard.Add(new List<int>());
            for (int j = 1; j <= n; j++)
            {
                board[i - 1].Add(gs.board[i - 1][j - 1]);
                previousBoard[i - 1].Add(gs.previousBoard[i - 1][j - 1]);
            }
        }

        if (x == -1 && y == -1)
            Pass();
        else
            PlaceStone(x, y);
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

    public void PlaceStoneOnBoard(int i, int j, List<List<int>> b, bool count)
    {
        b[i][j] = turn;
        if (i > 0 && b[i - 1][j] == 1 - turn && !SearchThroughBoard(i - 1, j, -1, b))
        {
            if (turn == 0 && count)
                point1 += CountThroughBoard(i - 1, j);
            if (turn == 1 && count)
                point2 += CountThroughBoard(i - 1, j);
            ReplaceThroughBoard(i - 1, j, -1, b);
        }
        if (j > 0 && b[i][j - 1] == 1 - turn && !SearchThroughBoard(i, j - 1, -1, b))
        {
            if (turn == 0 && count)
                point1 += CountThroughBoard(i, j - 1);
            if (turn == 1 && count)
                point2 += CountThroughBoard(i, j - 1);
            ReplaceThroughBoard(i, j - 1, -1, b);

        }
        if (i < n - 1 && b[i + 1][j] == 1 - turn && !SearchThroughBoard(i + 1, j, -1, b))
        {
            if (turn == 0 && count)
                point1 += CountThroughBoard(i + 1, j);
            if (turn == 1 && count)
                point2 += CountThroughBoard(i + 1, j);
            ReplaceThroughBoard(i + 1, j, -1, b);
        }
        if (j < n - 1 && b[i][j + 1] == 1 - turn && !SearchThroughBoard(i, j + 1, -1, b))
        {
            if (turn == 0 && count)
                point1 += CountThroughBoard(i, j + 1);
            if (turn == 1 && count)
                point2 += CountThroughBoard(i, j + 1);
            ReplaceThroughBoard(i, j + 1, -1, b);
        }
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

        PlaceStoneOnBoard(x, y, b, false);
        if (EqualsToPrevious(b))
            return false;
        if (!SearchThroughBoard(x, y, -1, b))
            return false;

        return true;
    }

    public List<Turn> FindAllMoves()
    {
        List<Turn> list = new List<Turn>();

        for(int i = 0; i < n; i++)
            for(int j = 0; j < n; j++)
                if(CanPlaceStone(i, j))
                    list.Add(new Turn(i, j));
        list.Add(new Turn(-1, -1)); //Za pass jer je i to potez

        return list;
    }

    public double Simulate()
    {
        return 1;
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
        PlaceStoneOnBoard(x, y, board, true);
        turn = 1 - turn;
        pass = 0;
    }

    public void Pass()
    {
        pass++;
        turn = 1 - turn;

        if(pass == 2)
        {
            ChooseWinner();
            turn = -1;
        }
    }

    public void ChooseWinner()
    {
        turn = -1;
        for(int i = 0; i < n; i++)
            for(int j = 0; j < n; j++)
            {
                if (board[i][j] == -1)
                {
                    if (SearchThroughBoard(i, j, 0, board) && !SearchThroughBoard(i, j, 1, board))
                        point1++;
                    else if (!SearchThroughBoard(i, j, 0, board) && SearchThroughBoard(i, j, 1, board))
                        point2++;
                }
            }
        if (n == 7 || n == 19)
            point2 += 0.5;
        else
            point2 += 6.5;

        if (point1 > point2)
            winner = 0;
        else
            winner = 1;

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
        {
            TurnTxt.Text = "Turn: Black";
            if (player1 == Player.AI)
                AITurn();
        }
        if (state.turn == 1)
        {
            TurnTxt.Text = "Turn: White";
            if (player2 == Player.AI)
                AITurn();
        }
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
        if (state.winner == 0)
            WinnerTxt.Text = "Winner: Black";
        if (state.winner == 1)
            WinnerTxt.Text = "Winner: White";
        TurnTxt.Text = "Turn:";

        PointsTxt.Text = state.point1 + " - " + state.point2;
    }

    private void Resign(object sender, RoutedEventArgs e)
    {
        if (state.turn == 0 && player1 == Player.AI)
            return;
        if (state.turn == 1 && player2 == Player.AI)
            return;

        if (state.turn == 0)
            WinnerTxt.Text = "Winner: White";
        if (state.turn == -1)
            WinnerTxt.Text = "Winner: Black";
        state.turn = -1;
        TurnTxt.Text = "Turn:";
    }

    private void AITurn()
    {
        Node root = new Node(state);

        DateTime start = new DateTime();
        TimeSpan time = new TimeSpan(3);

        while (DateTime.Now < start + time)
        {
            root.Explore();
        }

        double tempV = -1;
        Turn turn = new Turn(-1, -1);

        foreach(Node child in root.children)
            if(child.value > tempV)
            {
                tempV = child.value;
                turn = new Turn(child.Turn.x, child.Turn.y);
            }

        if (turn.x == -1 && turn.y == -1)
        {
            state.Pass();
            if (state.pass == 2)
                EndGame();
            else
                EndTurn();
        }
        else
        {
            state.PlaceStone(turn.x, turn.y);
            EndTurn();
        }
    }
}
