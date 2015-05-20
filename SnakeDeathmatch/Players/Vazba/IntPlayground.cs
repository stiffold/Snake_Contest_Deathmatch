using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;

namespace SnakeDeathmatch.Players.Vazba
{
    public class IntPlayground : IDebuggable, IIntArray
    {
        public int[,] InnerArray { get; private set; }

        public int Size { get; private set; }

        public event BreakpointEventHandler Breakpoint;

        public IntPlayground(int[,] array)
        {
            InnerArray = (int[,])array.Clone();
            Size = array.GetUpperBound(0) + 1;
        }

        public int this[int x, int y]
        {
            get { return InnerArray[x, y]; }
            set
            {
                InnerArray[x, y] = value;
                if (Breakpoint != null)
                    Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.IntPlaygroundChanged));
            }
        }

        public IntPlayground Clone()
        {
            return new IntPlayground(InnerArray);
        }
    }
}
