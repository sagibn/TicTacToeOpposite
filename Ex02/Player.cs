using System;

namespace Ex02
{
    enum ePlayerType
    {
        Human,
        Computer
    }

    struct Player
    {
        private readonly ePlayerNumber? m_PlayerNum;
        private ePlayerType m_Type;

        public Player(ePlayerNumber i_PlayerNum, ePlayerType i_Type)
        {
            m_PlayerNum = i_PlayerNum;
            m_Type = i_Type;
        }

        public ePlayerNumber Symbol
        {
            get
            {
                if (m_PlayerNum.HasValue)
                {
                    return m_PlayerNum.Value;
                }
                else
                {
                    throw new InvalidOperationException("Symbol has not been initialized.");
                }
            }
        }

        public ePlayerType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
    }
}
