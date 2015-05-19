using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;

namespace SnakeDeathmatch.Players.Vazba
{
    [Visualizer(typeof(IntPlaygroundVisualizer))]
    public class IntPlayground : IDebuggable
    {
        private int[,] _array;

        public int Size { get; private set; }

        public event BreakpointEventHandler Breakpoint;

        public IntPlayground(int[,] array)
        {
            _array = (int[,])array.Clone();
            Size = array.GetUpperBound(0) + 1;
        }

        public int this[int x, int y]
        { 
            get { return _array[x, y]; }
            set
            {
                _array[x, y] = value;
                if (Breakpoint != null)
                    Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.IntPlaygroundChanged));
            }
        }

        public IntPlayground Clone()
        {
            return new IntPlayground(_array);
        }
    }
}
