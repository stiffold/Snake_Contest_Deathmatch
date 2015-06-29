using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;

namespace SnakeDeathmatch.Players.Vazba.Helper
{
    public class IntPlayground : IIntArray
    {
        public int[,] InnerArray { get; private set; }

        public int Size { get; private set; }

        public IntPlayground(int[,] array)
        {
            InnerArray = (int[,])array.Clone();
            Size = array.GetUpperBound(0) + 1;
        }

        public int this[int x, int y]
        {
            get { return InnerArray[x, y]; }
            set { InnerArray[x, y] = value; }
        }

        public IntPlayground Clone()
        {
            return new IntPlayground(InnerArray);
        }
    }
}
