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

        public Game(ePlayerType i_Type, ushort i_Size)
        {
            m_Player[0] = new Player('X', ePlayerType.Human);
            m_Player[1] = new Player('O', i_Type);
            m_Board = new Board(i_Size);
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
                //TO DO!
            }
        }
    }
}
