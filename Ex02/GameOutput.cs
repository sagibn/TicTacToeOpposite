using System;
using Ex02.ConsoleUtils;
using System.Threading;

namespace Ex02
{
    enum eGameMode
    {
        Human,
        Computer,
        Quit,
        RestartMatchHuman,
        RestartMatchComputer
    }
    public class GameOutput
    {
        private Game m_Game;
        private eGameMode m_GameMode;
        private ushort m_BoardSize;

        public void MainMenuPlayer2Option()
        {
            string opponet = "";
            int startGameLoop = 0;

            while(startGameLoop == 0)
            {
                Console.WriteLine("Welcome to the Tic Tac Toe Opposite game, we are happy to have you here!");
                Console.WriteLine("First to start the game please select 1 if you want to play againts another player or 2 if you want to play againts the computer than press enter.");
                Console.WriteLine("If you wish to exit the game at any point please press Q.");
                opponet = Console.ReadLine();
                switch(opponet)
                {
                    case "1":
                        m_GameMode = eGameMode.Human;
                        startGameLoop = 1;
                        break;

                    case "2":
                        m_GameMode = eGameMode.Computer;
                        startGameLoop = 1;
                        break;

                    case "Q":
                        m_GameMode = eGameMode.Quit;
                        startGameLoop = 1;
                        break;

                    case "q":
                        m_GameMode = eGameMode.Quit;
                        startGameLoop = 1;
                        break;

                    default:
                        Screen.Clear();
                        Console.WriteLine("Please enter a valid input!");
                        break;
                }
            }
        }
        public void MainMenuBoardSize()
        {
            bool optionLoop = true;
            string i_PlayerChose = "";

            while(optionLoop)
            {
                Console.WriteLine("Please select the Board size, the bord will be YxY cubed size where Y is your option.");
                Console.WriteLine("Your option needed to be between 3 to 9.");
                i_PlayerChose = Console.ReadLine();
                if(ushort.TryParse(i_PlayerChose, out m_BoardSize))
                {
                    try
                    {
                        if(m_GameMode == eGameMode.Human)
                        {
                            m_Game = new Game(ePlayerType.Human, m_BoardSize);
                            break;
                        }
                        else
                        {
                            m_Game = new Game(ePlayerType.Computer, m_BoardSize);
                            break;
                        }
                    }
                    catch(InvalidOperationException ex)
                    {
                        Screen.Clear();
                        Console.WriteLine(ex.Message);
                    }
                    catch(ArgumentOutOfRangeException ex)
                    {
                        Screen.Clear();
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Screen.Clear();
                    Console.WriteLine("Invalid input please try again!");
                }
            }
        }

        private void PrintBoard(Board i_Board)
        {

            int size = i_Board.Size;
            ushort[] playersScores = new ushort[2];
            char printOut;

            Console.Write("  ");
            for(ushort col = 0; col < size; col++)
            {
                Console.Write((col + 1) + "   ");
            }

            Console.WriteLine();

            for(ushort row = 0; row < size; row++)
            {
                Console.Write((row + 1) + "|");
                for(ushort col = 0; col < size; col++)
                {
                    if (i_Board.GetCell(row, col) == ePlayerNumber.Player1) 
                    {
                        printOut = 'X';
                    }
                    else if(i_Board.GetCell(row, col) == ePlayerNumber.Player2)
                    {
                        printOut = 'O';
                    }
                    else
                    {
                        printOut = ' ';
                    }
                    
                    Console.Write(" " + (printOut.ToString()) + " |");
                }

                Console.WriteLine();
                Console.Write(" ");
                for(int col = 0; col < size; col++)
                {
                    Console.Write("====");
                }

                Console.WriteLine("=");
            }
        }

        public void GetMoveFromPlayer(ushort i_PlayerNum)
        {
            string playerInput;
            string[] splitedInput;
            ushort row = 0;
            ushort col = 0;

            while(true)
            {
                Console.WriteLine("Player number " + (i_PlayerNum + 1) + " please select row and col for your move:");
                Console.WriteLine("Note that your input should be 'x y' than enter. for example: 3 1");
                Console.WriteLine("If you wish to exit/restart the match please press q than enter.");
                playerInput = Console.ReadLine();
                if(playerInput == "q" || playerInput == "Q")
                {
                    if(m_GameMode == eGameMode.Human)
                    {
                        m_GameMode = eGameMode.RestartMatchHuman;
                    }
                    else
                    {
                        m_GameMode = eGameMode.RestartMatchComputer;
                    }
                    break;
                }

                splitedInput = playerInput.Split(' ');
                if(splitedInput.Length == 2)
                {
                    if(!ushort.TryParse(splitedInput[0], out row) || !ushort.TryParse(splitedInput[1], out col))
                    {
                        Console.WriteLine("Wrong input, please try again!");
                    }
                    else
                    {
                        try
                        {
                            m_Game.Move(i_PlayerNum, (ushort)(row - 1), (ushort)(col - 1));
                            Screen.Clear();
                            this.PrintBoard(m_Game.Board);
                            break;
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
        }

        private void showScore()
        {
            string input;

            Screen.Clear();
            Console.WriteLine("The winner is: " + m_Game.WinnerOfTheMatch);
            Console.WriteLine("The score is:");
            Console.WriteLine("Player1 score: " + m_Game.PlayerScore[0]);
            Console.WriteLine("Player2 score: " + m_Game.PlayerScore[1]);
            Console.WriteLine("If you wish to continue playing please press any key than enter, if you wish to exit press q.");
            input = Console.ReadLine();
            if(input == "q" || input == "Q")
            {
                m_GameMode = eGameMode.Quit;
            }
            else
            {
                Screen.Clear();
            }
        }

        private void PlayerOptionRestartOrExit()
        {
            string userInput;

            Console.WriteLine("You pressed q while in game.");
            Console.WriteLine("If you wish to restart the game press any key.");
            Console.WriteLine("If you wish to exit the game press q.");
            userInput = Console.ReadLine();
            if(userInput == "q" || userInput == "Q")
            {
                m_GameMode = eGameMode.Quit;
            }
            else
            {
                Screen.Clear();
                if(m_GameMode == eGameMode.RestartMatchComputer)
                {
                    m_GameMode = eGameMode.Computer;
                    m_Game = new Game(ePlayerType.Computer, m_BoardSize);
                }
                else
                {
                    m_GameMode = eGameMode.Human;
                    m_Game = new Game(ePlayerType.Human, m_BoardSize);
                }

                PrintBoard(m_Game.Board);
            }
        }

        private void RestartGameIfOver(bool i_GameOver)
        {
            if (i_GameOver)
            {
                showScore();

                if (m_GameMode == eGameMode.Human)
                {
                    m_Game = new Game(ePlayerType.Human, m_BoardSize);
                }
                else
                {
                    m_Game = new Game(ePlayerType.Computer, m_BoardSize);
                }

                PrintBoard(m_Game.Board);
            }
        }
        private void TheGameItself()
        {
            bool gameOver = false;

            this.PrintBoard(m_Game.Board);
            while(m_GameMode != eGameMode.Quit)
            {
                GetMoveFromPlayer(0);
                gameOver = m_Game.IsGameOver();
                RestartGameIfOver(gameOver);
                if (!gameOver)
                {
                    if (m_GameMode == eGameMode.Human)
                    {
                        GetMoveFromPlayer(1);
                    }
                    else if (m_GameMode == eGameMode.Computer)
                    {
                        Console.WriteLine("It's the computer's turn");
                        Thread.Sleep(1000);
                        m_Game.Move(1, null, null);
                    }
                    if (m_GameMode == eGameMode.RestartMatchHuman || m_GameMode == eGameMode.RestartMatchComputer)
                    {
                        PlayerOptionRestartOrExit();
                        if (m_GameMode == eGameMode.Quit)
                        {
                            return;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    Screen.Clear();
                    this.PrintBoard(m_Game.Board);
                    gameOver = m_Game.IsGameOver();
                    RestartGameIfOver(gameOver);
                }
            }
        }

        public void InitializeGame()
        {
            this.MainMenuPlayer2Option();
            if(m_GameMode == eGameMode.Quit)
            {
                return;
            }

            this.MainMenuBoardSize();
            Screen.Clear();
            TheGameItself();
        }
    }
}