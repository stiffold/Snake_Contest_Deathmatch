using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;

namespace SnakeDeathmatch.Players.Vazba
{
    [Visualizer(typeof(DecimalPlaygroundVisualizer))]
    public class DecimalPlayground
    {
        private decimal[,] _array;

        public int Size { get; private set; }

        public DecimalPlayground(int size)
        {
            _array = new decimal[size, size];
            Size = size;
        }

        public decimal this[int x, int y]
        {
            get { return _array[x, y]; }
            set { _array[x, y] = value; }
        }
    }
}
