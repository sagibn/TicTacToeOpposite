using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02
{
    enum eWinnerOfTheMatch
    {
        Player1,
        Player2,
        Tie,
        None
    }

    class Game
    {
        private Board m_Board;
        private Player[] m_Player = new Player[2];
        private static ushort[] m_PlayerScore = new ushort[2] {0, 0};
        private eWinnerOfTheMatch m_WinnerOfTheMatch;
        private int m_NumOfAvailableCells;

        public Game(ePlayerType i_Type, ushort i_Size)
        {
            m_Player[0] = new Player(ePlayerNumber.Player1, ePlayerType.Human);
            m_Player[1] = new Player(ePlayerNumber.Player2, i_Type);
            m_Board = new Board(i_Size);
            m_WinnerOfTheMatch = eWinnerOfTheMatch.None;
            m_NumOfAvailableCells = i_Size * i_Size;
        }

        public Board Board
        {
            get
            {
                return m_Board;
            }
        }
        public eWinnerOfTheMatch WinnerOfTheMatch
        {
            get
            {
                return m_WinnerOfTheMatch;
            }
        }
        public ushort[] PlayerScore
        {
            get
            {
                return m_PlayerScore;
            }
        }

        public void Move(ushort i_PlayerNum, ushort? i_X, ushort? i_Y)
        {
            if(m_Player[i_PlayerNum].Type == ePlayerType.Human)
            {
                if(i_X.HasValue && i_Y.HasValue)
                {
                    try
                    {
                        m_Board.SetCell(i_X.Value, i_Y.Value, m_Player[i_PlayerNum].Symbol);
                        m_NumOfAvailableCells--;
                    }
                    catch(InvalidOperationException ex)
                    {
                        throw new InvalidOperationException(ex.Message);
                    }
                    catch(ArgumentOutOfRangeException ex)
                    {
                        throw new ArgumentOutOfRangeException(ex.Message);
                    }
                }
                else
                {
                    throw new ArgumentException("Both i_X and i_Y must have values.");
                }
            }
            else
            {
                ComputerMove(i_PlayerNum);
                m_NumOfAvailableCells--;
            }
        }

        private void ComputerMove(ushort i_PlayerNum)
        {
            ePlayerNumber playerNum = m_Player[i_PlayerNum].Symbol;
            int size = m_Board.Size;
            int maxScore = -size * size; // initialize to a very low value
            ushort bestRow = 0, bestCol = 0;

            for(ushort row = 0; row < size; row++)
            {
                for(ushort col = 0; col < size; col++)
                {
                    if(!m_Board.GetCell(row, col).HasValue)
                    {
                        m_Board.SetCell((ushort)row, (ushort)col, playerNum);
                        int score = EvaluateBoard(playerNum);
                        m_Board.SetCell((ushort)row, (ushort)col, null);

                        if (score > maxScore)
                        {
                            maxScore = score;
                            bestRow = (ushort)row;
                            bestCol = (ushort)col;
                        }
                    }
                }
            }

            m_Board.SetCell(bestRow, bestCol, playerNum);
        }

        private int EvaluateBoard(ePlayerNumber playerNum)
        {
            //Evaluate the score of the current board position for the given symbol
            int size = m_Board.Size;
            int score = 0;

            for(ushort row = 0; row < size; row++)
            {
                int count = 0;

                for(ushort col = 0; col < size; col++)
                {
                    if(m_Board.GetCell(row, col) == playerNum)
                    {
                        count++;
                    }
                    else if(m_Board.GetCell(row, col).HasValue)
                    {
                        count--;
                    }
                }

                score += ScoreCount(count);
            }

            for(ushort col = 0; col < size; col++)
            {
                int count = 0;

                for(ushort row = 0; row < size; row++)
                {
                    if(m_Board.GetCell(row, col) == playerNum)
                    {
                        count++;
                    }
                    else if(m_Board.GetCell(row, col).HasValue)
                    {
                        count--;
                    }
                }

                score += ScoreCount(count);
            }

            ushort count1 = 0, count2 = 0;

            for(ushort i = 0; i < size; i++)
            {
                if(m_Board.GetCell(i, i) == playerNum)
                {
                    count1++;
                }
                else if(m_Board.GetCell(i, i).HasValue)
                {
                    count1--;
                }
                if(m_Board.GetCell(i, (ushort)(size - i - 1)) == playerNum)
                {
                    count2++;
                }
                else if(m_Board.GetCell(i, (ushort)(size - i - 1)).HasValue)
                {
                    count2--;
                }
            }

            score += ScoreCount(count1) + ScoreCount(count2);

            return score;
        }

        private int ScoreCount(int i_Count)
        {
            int retValue;

            if(i_Count == m_Board.Size - 1)
            {
                retValue = 100;
            }
            else if(i_Count == m_Board.Size - 2)
            {
                retValue = 10;
            }
            else if(i_Count == m_Board.Size - 3)
            {
                retValue = 1;
            }
            else
            {
                retValue = 0;
            }

            return retValue;
        }

        public bool IsGameOver()
        {
            for(int i = 0; i < 2; i++) 
            {
                for(ushort row = 0; row < m_Board.Size; row++)
                {
                    int sequence = 0;

                    for (ushort col = 0; col < m_Board.Size; col++)
                    {
                        if (m_Board.GetCell(row, col) == m_Player[i].Symbol)
                        {
                            sequence++;
                            if(sequence == m_Board.Size)
                            {
                                m_PlayerScore[1 - i]++;
                                m_WinnerOfTheMatch = (eWinnerOfTheMatch)(1 - i);
                                return true;
                            }
                        }
                    }
                }

                for(ushort col = 0; col < m_Board.Size; col++)
                {
                    int sequence = 0;

                    for(ushort row = 0; row < m_Board.Size; row++)
                    {
                        if(m_Board.GetCell(row, col) == m_Player[i].Symbol)
                        {
                            sequence++;
                            if(sequence == m_Board.Size)
                            {
                                m_PlayerScore[1 - i]++;
                                m_WinnerOfTheMatch = (eWinnerOfTheMatch)(1 - i);
                                return true;
                            }
                        }
                    }
                }

                int mainDiagonal = 0;
                int subDiagonal = 0;
                for (ushort row = 0; row < m_Board.Size; row++)
                {
                    if(m_Board.GetCell(row, row) == m_Player[i].Symbol)
                    {
                        mainDiagonal++;
                        if(mainDiagonal == m_Board.Size)
                        {
                            m_PlayerScore[1 - i]++;
                            m_WinnerOfTheMatch = (eWinnerOfTheMatch)(1 - i);
                            return true;
                        }
                    }

                    if(m_Board.GetCell(row, (ushort)(m_Board.Size-1-row)) == m_Player[i].Symbol)
                    {
                        subDiagonal++;
                        if (subDiagonal == m_Board.Size)
                        {
                            m_PlayerScore[1 - i]++;
                            m_WinnerOfTheMatch = (eWinnerOfTheMatch)(1 - i);
                            return true;
                        }
                    }
                }
            }

            bool isTie = m_NumOfAvailableCells == 0;

            m_WinnerOfTheMatch = isTie ? eWinnerOfTheMatch.Tie : eWinnerOfTheMatch.None;

            return isTie;
        }
    }
}
