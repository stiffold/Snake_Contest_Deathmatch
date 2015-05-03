using System;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
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

        public bool CanMove(Direction direction, int[,] playground)
        {
            Point p = Move(direction);

            // detekce kolize s okrajem pole
            int size = playground.GetUpperBound(0) + 1;
            if (p.X < 0 || p.Y < 0 || p.X >= size || p.Y >= size)
            {
                return false;
            }

            // detekce kolize do těla jiného hada
            if (playground[p.X, p.Y] != 0)
            {
                return false;
            }

            // detekce snahy projet diagonálně skrz tělo jiného hada
            if ((direction == Direction.TopRight && (playground[p.X, p.Y + 1] != 0) && (playground[p.X - 1, p.Y] != 0)) ||
                (direction == Direction.BottomRight && (playground[p.X, p.Y - 1] != 0) && (playground[p.X - 1, p.Y] != 0)) ||
                (direction == Direction.BottomLeft && (playground[p.X, p.Y - 1] != 0) && (playground[p.X + 1, p.Y] != 0)) ||
                (direction == Direction.TopLeft && (playground[p.X, p.Y + 1] != 0) && (playground[p.X + 1, p.Y] != 0)))
            {
                return false;
            }

            return true;
        }

        public Point Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top: return new Point(X, Y - 1);
                case Direction.TopRight: return new Point(X + 1, Y - 1);
                case Direction.Right: return new Point(X + 1, Y);
                case Direction.BottomRight: return new Point(X + 1, Y + 1);
                case Direction.Bottom: return new Point(X, Y + 1);
                case Direction.BottomLeft: return new Point(X - 1, Y + 1);
                case Direction.Left: return new Point(X - 1, Y);
                case Direction.TopLeft: return new Point(X - 1, Y - 1);
            }
            throw new Exception(string.Format("Unknown value {0}.{1}.", typeof(Direction).Name, direction));
        }
    }
}
