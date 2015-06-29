using System;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba.Helper
{
    public struct Snake
    {
        public int Id { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction Direction { get; private set; }

        public Snake(int id, int x, int y, Direction direction) : this()
        {
            Id = id;
            X = x;
            Y = y;
            Direction = direction;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}] {2}", X, Y, Direction);
        }

        public Next GetNext(IntPlayground playground)
        {
            Direction leftDirection = Direction.TurnLeft();
            Direction straightDirection = Direction;
            Direction rightDirection = Direction.TurnRight();

            return new Next()
            {
                Left = CanMove(leftDirection, playground) ? Move(leftDirection) : (Snake?)null,
                Straight = CanMove(straightDirection, playground) ? Move(straightDirection) : (Snake?)null,
                Right = CanMove(rightDirection, playground) ? Move(rightDirection) : (Snake?)null,
            };
        }

        public bool CanMove(Direction direction, IntPlayground playground)
        {
            Snake s = Move(direction);

            // detekce kolize s okrajem pole
            if (s.X < 0 || s.Y < 0 || s.X >= playground.Size || s.Y >= playground.Size)
            {
                return false;
            }

            // detekce kolize do těla jiného hada
            if (playground[s.X, s.Y] != 0)
            {
                return false;
            }

            // detekce snahy projet diagonálně skrz tělo jiného hada
            if ((direction == Direction.TopRight && (playground[s.X, s.Y + 1] != 0) && (playground[s.X - 1, s.Y] != 0)) ||
                (direction == Direction.BottomRight && (playground[s.X, s.Y - 1] != 0) && (playground[s.X - 1, s.Y] != 0)) ||
                (direction == Direction.BottomLeft && (playground[s.X, s.Y - 1] != 0) && (playground[s.X + 1, s.Y] != 0)) ||
                (direction == Direction.TopLeft && (playground[s.X, s.Y + 1] != 0) && (playground[s.X + 1, s.Y] != 0)))
            {
                return false;
            }

            return true;
        }

        public Snake Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top: return new Snake(Id, X, Y - 1, direction);
                case Direction.TopRight: return new Snake(Id, X + 1, Y - 1, direction);
                case Direction.Right: return new Snake(Id, X + 1, Y, direction);
                case Direction.BottomRight: return new Snake(Id, X + 1, Y + 1, direction);
                case Direction.Bottom: return new Snake(Id, X, Y + 1, direction);
                case Direction.BottomLeft: return new Snake(Id, X - 1, Y + 1, direction);
                case Direction.Left: return new Snake(Id, X - 1, Y, direction);
                case Direction.TopLeft: return new Snake(Id, X - 1, Y - 1, direction);
            }
            throw new Exception(string.Format("Unknown value {0}.{1}.", typeof(Direction).Name, direction));
        }
    }
}
