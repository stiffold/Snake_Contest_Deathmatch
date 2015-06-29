using System;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba.Helper
{
    public static class DirectionExt
    {
        public static Direction Apply(this Direction direction, Move move)
        {
            switch (move)
            {
                case Move.Left: return direction.TurnLeft();
                case Move.Straight: return direction;
                case Move.Right: return direction.TurnRight();
            }
            throw new Exception(string.Format("Unknown value {0}.{1}.", typeof(Move).Name, move));
        }

        public static Direction TurnLeft(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Top: return Direction.TopLeft;
                case Direction.TopRight: return Direction.Top;
                case Direction.Right: return Direction.TopRight;
                case Direction.BottomRight: return Direction.Right;
                case Direction.Bottom: return Direction.BottomRight;
                case Direction.BottomLeft: return Direction.Bottom;
                case Direction.Left: return Direction.BottomLeft;
                case Direction.TopLeft: return Direction.Left;
            }
            throw new Exception(string.Format("Unknown value {0}.{1}.", typeof(Direction).Name, direction));
        }

        public static Direction TurnRight(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Top: return Direction.TopRight;
                case Direction.TopRight: return Direction.Right;
                case Direction.Right: return Direction.BottomRight;
                case Direction.BottomRight: return Direction.Bottom;
                case Direction.Bottom: return Direction.BottomLeft;
                case Direction.BottomLeft: return Direction.Left;
                case Direction.Left: return Direction.TopLeft;
                case Direction.TopLeft: return Direction.Top;
            }
            throw new Exception(string.Format("Unknown value {0}.{1}.", typeof(Direction).Name, direction));
        }

        public static Direction Opposite(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Top: return Direction.Bottom;
                case Direction.TopRight: return Direction.BottomLeft;
                case Direction.Right: return Direction.Left;
                case Direction.BottomRight: return Direction.TopLeft;
                case Direction.Bottom: return Direction.Top;
                case Direction.BottomLeft: return Direction.TopRight;
                case Direction.Left: return Direction.Right;
                case Direction.TopLeft: return Direction.BottomRight;
            }
            throw new Exception(string.Format("Unknown value {0}.{1}.", typeof(Direction).Name, direction));
        }
    }
}
