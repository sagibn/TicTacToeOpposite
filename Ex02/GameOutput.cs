using System;
using Ex02.ConsoleUtils;
namespace Ex02
{
    enum eGameMode
    {
        Human,
        Computer,
        Quit
    }
    public class GameOutput
    {
        private Game m_Game;
        private eGameMode m_GameMode;

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
                        m_GameMode =  eGameMode.Human;
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
            ushort i_BoardSize = 0;
            bool optionLoop = true;
            string i_PlayerChose = "";

            while(optionLoop) 
            {
                Console.WriteLine("Please select the Board size, the bord will be YxY cubed size where Y is your option.");
                Console.WriteLine("Your option needed to be between 3 to 9.");
                i_PlayerChose = Console.ReadLine();
                if(ushort.TryParse(i_PlayerChose,out i_BoardSize))
                {
                    try
                    {
                        if (m_GameMode == eGameMode.Human)
                        {
                            m_Game = new Game(ePlayerType.Human, i_BoardSize);
                            break;
                        }
                        else
                        {
                            m_Game = new Game(ePlayerType.Computer, i_BoardSize);
                            break;
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        Screen.Clear();
                        Console.WriteLine(ex.Message);
                    }
                    catch (ArgumentOutOfRangeException ex)
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

        public void RunTheGame()
        {

        }

        private void PrintBoard(Board i_Board)
        {
            
            int size = i_Board.Size;
            Console.Write("  ");
            for(ushort col = 0; col < size; col++)
            {
                Console.Write((col + 1) + "   ");
            }
            Console.WriteLine();

            for (ushort row = 0; row < size; row++)
            {
                Console.Write((row + 1) + "|");
                for(ushort col = 0; col < size; col++)
                {
                    char? c = i_Board.GetCell(row, col);
                    if(c == null)
                    {
                        c = ' ';
                    }
                    Console.Write(" " + (c.ToString()) + " |"); 
                }
                Console.WriteLine();

                Console.Write(" ");
                for (int col = 0; col < size; col++)
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
                playerInput = Console.ReadLine();
                if(playerInput == "q" || playerInput == "Q")
                {
                    m_GameMode = eGameMode.Quit;
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
                            m_Game.Move(i_PlayerNum,(ushort)(row - 1), (ushort)(col - 1));
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
        public void StartTheGame()
        {
            this.MainMenuPlayer2Option();
            if(m_GameMode == eGameMode.Quit) {
                return;
            }

            this.MainMenuBoardSize();
            Screen.Clear();
            this.PrintBoard(m_Game.Board);
            while(m_GameMode != eGameMode.Quit)
            {
               GetMoveFromPlayer(0);

               if(m_GameMode == eGameMode.Human)
                {
                    GetMoveFromPlayer(1);
                }

            }
            


        }
    }
}