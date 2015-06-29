using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;

namespace SnakeDeathmatch.Players.Vazba.Helper
{
    public class DecimalPlayground : IDecimalArray
    {
        public decimal[,] InnerArray { get; private set; }

        public int Size { get; private set; }

        public DecimalPlayground(int size)
        {
            InnerArray = new decimal[size, size];
            Size = size;
        }

        public decimal this[int x, int y]
        {
            get { return InnerArray[x, y]; }
            set { InnerArray[x, y] = value; }
        }
    }
}
