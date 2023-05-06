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
        private readonly char? m_Symbol;
        private ePlayerType m_Type;

        public Player(char i_Symbol, ePlayerType i_Type)
        {
            m_Symbol = i_Symbol;
            m_Type = i_Type;
        }

        public char Symbol
        {
            get
            {
                if (m_Symbol.HasValue)
                {
                    return m_Symbol.Value;
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
