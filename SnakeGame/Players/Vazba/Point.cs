using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeGame.Players.Vazba
{
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;

        public override string ToString()
        {
            return string.Format("[{0},{1}]", X, Y);
        }
    }
}
