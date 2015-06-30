using System;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba.Helper
{
    public struct Snake
    {
        public int Id { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public Direction? Direction { get; private set; }

        public Snake(int id, int x, int y, Direction? direction) : this()
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
            if (Direction == null)
                throw new InvalidOperationException(string.Format("Snake direction is not initialized yet (snake with Id {0} and position [{1},{2}]).", Id, X, Y));

            Direction leftDirection = Direction.Value.TurnLeft();
            Direction straightDirection = Direction.Value;
            Direction rightDirection = Direction.Value.TurnRight();

            return new Next()
            {
                Left = CanMove(leftDirection, playground) ? Move(leftDirection) : (Snake?)null,
                Straight = CanMove(straightDirection, playground) ? Move(straightDirection) : (Snake?)null,
                Right = CanMove(rightDirection, playground) ? Move(rightDirection) : (Snake?)null,
            };
        }

        public bool CanMove(Direction direction, IntPlayground playground)
        {
            if (Direction == null)
                throw new InvalidOperationException(string.Format("Snake direction is not initialized yet (snake with Id {0} and position [{1},{2}]).", Id, X, Y));

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
            if ((direction == Interface.Direction.TopRight && (playground[s.X, s.Y + 1] != 0) && (playground[s.X - 1, s.Y] != 0)) ||
                (direction == Interface.Direction.BottomRight && (playground[s.X, s.Y - 1] != 0) && (playground[s.X - 1, s.Y] != 0)) ||
                (direction == Interface.Direction.BottomLeft && (playground[s.X, s.Y - 1] != 0) && (playground[s.X + 1, s.Y] != 0)) ||
                (direction == Interface.Direction.TopLeft && (playground[s.X, s.Y + 1] != 0) && (playground[s.X + 1, s.Y] != 0)))
            {
                return false;
            }

            return true;
        }

        public Snake Move(Direction direction)
        {
            switch (direction)
            {
                case Interface.Direction.Top: return new Snake(Id, X, Y - 1, direction);
                case Interface.Direction.TopRight: return new Snake(Id, X + 1, Y - 1, direction);
                case Interface.Direction.Right: return new Snake(Id, X + 1, Y, direction);
                case Interface.Direction.BottomRight: return new Snake(Id, X + 1, Y + 1, direction);
                case Interface.Direction.Bottom: return new Snake(Id, X, Y + 1, direction);
                case Interface.Direction.BottomLeft: return new Snake(Id, X - 1, Y + 1, direction);
                case Interface.Direction.Left: return new Snake(Id, X - 1, Y, direction);
                case Interface.Direction.TopLeft: return new Snake(Id, X - 1, Y - 1, direction);
            }
            throw new Exception(string.Format("Unknown value {0}.{1}.", typeof(Direction).Name, direction));
        }
    }
}
