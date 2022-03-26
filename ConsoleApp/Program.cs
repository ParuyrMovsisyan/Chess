// See https://aka.ms/new-console-template for more information
using GameLibrary;
Run();

/// <summary>
/// Runs a program
/// </summary>
void Run()
{
    var game = new Game();
    PrintChessboard(game.GetBoard());
    while (true)
    {
        Console.WriteLine("Please enter start position: acceptable value is[A-H][1-8]");
        string startPosition = Console.ReadLine();
        Console.WriteLine("Please enter finish position: acceptable value is[A-H][1-8]");
        string targetPosition = Console.ReadLine();
        game.Move(startPos: startPosition, targetPos: targetPosition);
        PrintChessboard(game.GetBoard());
        if (game.IsGameOver())
        {
            Console.Beep();
            Console.WriteLine("Game Over");
            Console.WriteLine("Do you want to start a new game? /Yes or No");
            string s=Console.ReadLine();
            if (s is not null && s.ToUpper() == "YES")
                Run();
            else
                break;
        }
        else if (game.IsUnderCheck())
        {
            Console.Beep();
            Console.WriteLine("Check");
        }
    }
}

/// <summary>
/// Prints chessboard with figures
/// </summary>
/// <param name="board">
/// two dimensinal chars array: 
/// where each item is chessboard's cell and it is emty or figure symbol
/// </param>
void PrintChessboard(char[,] board)
{
    Console.OutputEncoding = System.Text.Encoding.Unicode;
    string s = "    A B C D E F G H";
    Console.WriteLine(s);
    for (int i = 0; i < 8; i++)
    {
        Console.Write(8 - i + "  ");
        for (int j = 0; j < 8; j++)
        {
            if ((i + j) % 2 == 0)
            {
                Console.BackgroundColor = ConsoleColor.White;
            }
            if (board[i, j] != '\u265F')
                Console.Write(board[i, j].ToString().PadRight(2));
            else
                Console.Write(board[i, j].ToString());
            Console.ResetColor();
        }
        Console.WriteLine("  " + (8 - i));
    }
    Console.WriteLine(s);
}
