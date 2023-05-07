using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ex02
{
    class Game
    {
        Board m_Board;
        Player[] m_Player = new Player[2];
        ushort[] m_PlayerScore = new ushort[2];

        public Game(ePlayerType i_Type, ushort i_Size)
        {
            m_Player[0] = new Player('X', ePlayerType.Human);
            m_Player[1] = new Player('O', i_Type);
            m_Board = new Board(i_Size);
            m_PlayerScore[0] = 0;
            m_PlayerScore[1] = 0;
        }

        public Board Board
        {
            get
            {
                return m_Board;
            }
        }
        public ushort[] PlayerScore
        {
            get
            {
                return PlayerScore;
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
            }
        }

        private void ComputerMove(ushort i_PlayerNum)
        {
            List<Tuple<ushort, ushort>> availableCells = new List<Tuple<ushort, ushort>>();

            for(ushort row = 0; row < m_Board.Size; row++)
            {
                for(ushort col = 0; col < m_Board.Size; col++)
                {
                    if(!m_Board.GetCell(row, col).HasValue)
                    {
                        availableCells.Add(new Tuple<ushort, ushort>(row, col));
                    }
                }
            }

            if(availableCells.Count == 1)
            {
                m_Board.SetCell(availableCells[0].Item1, availableCells[0].Item2, m_Player[i_PlayerNum].Symbol);
                return;
            }

            //Step 2: Remove cells that would cause a win for the current player
            List<Tuple<ushort, ushort>> availableCellsCopy = new List<Tuple<ushort, ushort>>(availableCells);

            foreach(var cell in availableCells)
            {
                if(IsWinningMove(cell.Item1, cell.Item2, m_Player[i_PlayerNum].Symbol))
                {
                    availableCellsCopy.Remove(new Tuple<ushort, ushort>(cell.Item1, cell.Item2));
                }
            }

            if(availableCellsCopy.Count == 1)
            {
                m_Board.SetCell(availableCellsCopy[0].Item1, availableCellsCopy[0].Item2, m_Player[i_PlayerNum].Symbol);
                return;
            }

            if(availableCellsCopy.Count > 0)
            {
                availableCells = availableCellsCopy;
            }

            //Step 3: Remove cells that would cause a win for the other player
            availableCellsCopy = new List<Tuple<ushort, ushort>>(availableCells);
            foreach(var cell in availableCells)
            {
                if(IsWinningMove(cell.Item1, cell.Item2, m_Player[1 - i_PlayerNum].Symbol))
                {
                    availableCellsCopy.Remove(new Tuple<ushort, ushort>(cell.Item1, cell.Item2));
                }
            }

            if(availableCellsCopy.Count == 1)
            {
                m_Board.SetCell(availableCellsCopy[0].Item1, availableCellsCopy[0].Item2, m_Player[i_PlayerNum].Symbol);
                return;
            }

            if(availableCellsCopy.Count > 0)
            {
                availableCells = availableCellsCopy;
            }

            Random random = new Random();
            Tuple<ushort, ushort> chosenCell = availableCells[random.Next(availableCells.Count)];

            m_Board.SetCell(chosenCell.Item1, chosenCell.Item2, m_Player[i_PlayerNum].Symbol);
        }

        private bool IsWinningMove(ushort i_Row, ushort i_Col, char i_Symbol)
        {
            return IsWinningRow(i_Row, i_Symbol) || IsWinningCol(i_Col, i_Symbol)
                    || IsWinningDiagonal(i_Symbol, i_Row, i_Col);
        }

        private bool IsWinningDiagonal(char i_Symbol, ushort i_Row, ushort i_Col)
        {
            bool isWinningDiagonal = false;

            if(i_Row == i_Col)
            {
                isWinningDiagonal = true;

                for(ushort row = 0; row < m_Board.Size; row++)
                {
                    if(m_Board.GetCell(row, row) != i_Symbol && m_Board.GetCell(row, row) != null)
                    {
                        isWinningDiagonal = false;
                        break;
                    }
                }
            }

            if(i_Row + i_Col == m_Board.Size - 1)
            {
                if(isWinningDiagonal)
                {
                    return true;
                }

                isWinningDiagonal = true;
                for (ushort row = 0; row < m_Board.Size; row++)
                {
                    if (m_Board.GetCell(row, (ushort)(m_Board.Size - 1 - row)) != i_Symbol 
                        && m_Board.GetCell(row, (ushort)(m_Board.Size - 1 - row)) != null)
                    {
                        isWinningDiagonal = false;
                        break;
                    }
                }
            }

            return isWinningDiagonal;
        }

        private bool IsWinningRow(ushort i_Row, char i_Symbol)
        {
            for (ushort col = 0; col < m_Board.Size; col++)
            {
                if (m_Board.GetCell(i_Row, col) != i_Symbol && m_Board.GetCell(i_Row, col) != null)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsWinningCol(ushort i_Col, char i_Symbol)
        {
            for(ushort row = 0; row < m_Board.Size; row++)
            {
                if(m_Board.GetCell(row, i_Col) != i_Symbol && m_Board.GetCell(row, i_Col) != null)
                {
                    return false;
                }
            }
            return true;
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
                                return true;
                            }
                        }
                    }
                }

                for(ushort row = 0; row < m_Board.Size; row++)
                {
                    int mainDiagonal = 0;
                    int subDiagonal = 0;

                    if(m_Board.GetCell(row, row) == m_Player[i].Symbol)
                    {
                        mainDiagonal++;
                        if(mainDiagonal == m_Board.Size)
                        {
                            m_PlayerScore[1 - i]++;
                            return true;
                        }
                    }

                    if(m_Board.GetCell(row, (ushort)(m_Board.Size-1-row)) == m_Player[i].Symbol)
                    {
                        subDiagonal++;
                        if (subDiagonal == m_Board.Size)
                        {
                            m_PlayerScore[1 - i]++;
                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
