using System;

namespace dottictactoe
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Play Tic Tac Toe ===");
            Game game = new Game();
            while (true)
            {
                game.DisplayBoard();
                while (true)
                {
                    Console.Write($"Play for {game.NextTurn} (input format 'INT,INT') > ");
                    string playPosition = Console.ReadLine();
                    try
                    {
                        game.PlayTurn(playPosition);
                        break;
                    }
                    catch (ArgumentException ex) {
                        Console.WriteLine(ex.Message);
                    }
                }
                if (game.HasWinner)
                {
                    game.DisplayBoard();
                    Console.WriteLine($"'{game.NextTurn}' won the game!");
                    Console.Write("Play again? ");
                    string ans = Console.ReadLine();
                    if (ans.ToLower()[0] == 'y')
                    {
                        game.ResetGame();
                    }
                    else
                    {
                        Console.WriteLine("Goodbye!");
                        break;
                    }
                }
            }
        }
    }

    class Game
    {
        private char[,] board = {
            {' ', ' ', ' '},
            {' ', ' ', ' '},
            {' ', ' ', ' '},
        };
        public char NextTurn { get; private set; } = 'X';
        public bool HasWinner { get; private set; } = false;

        public void PlayTurn(string inputPosition)
        {
            if (HasWinner)
                throw new ArgumentException("Game already has a winner");

            (int row, int col) = parsePosition(inputPosition);

            if (board[row, col] != ' ')
                throw new ArgumentException($"'{inputPosition}' is already taken.");
            board[row, col] = NextTurn;

            if (checkForWinner())
                HasWinner = true;
            else
                alternateTurns();
        }

        private static (int row, int column) parsePosition(string position)
        {
            string[] args = position.Split(',');
            int row;
            int column;

            if (args.Length != 2 || !int.TryParse(args[0], out row) || !int.TryParse(args[1], out column))
                throw new ArgumentException($"'{position}' is of invalid format. Expected format is 'INT,INT'");

            row--;
            column--;
            if (!(row >= 0 && row < 3) || !(column >= 0 && column < 3))
                throw new ArgumentException($"'{position}' is out of legal bounds.");

            return (row, column);
        }

        private void alternateTurns()
        {
            if (NextTurn == 'X')
                NextTurn = 'O';
            else
                NextTurn = 'X';

        }

        private bool checkForWinner()
        {
            // check all rows and columns.
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != ' ')
                    return true;
                if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != ' ')
                    return true;
            }
            // check the diagonals.
            if (board[1, 1] != ' ')
            {
                if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                    return true;
                if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                    return true;
            }

            return false;
        }

        public void ResetGame()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = ' ';
                }
            }
            NextTurn = 'X';
            HasWinner = false;
        }

        public void DisplayBoard()
        {
            Console.WriteLine("+---+---+---+");
            for (int row = 0; row < 3; row++)
            {
                Console.Write("|");
                for (int col = 0; col < 3; col++)
                {
                    if (board[row, col] == ' ')
                        Console.Write($"{row + 1},{col + 1}|");
                    else
                        Console.Write($" {board[row, col]} |");
                }
                Console.WriteLine("\n+---+---+---+");
            }
        }
    }
}
