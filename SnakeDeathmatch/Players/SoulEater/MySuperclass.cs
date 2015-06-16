using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater
{
    public static class MySuperClass
    {
        public static IEnumerable<Point> GetBorderPoints(Point point)
        {
            yield return GetNextPoint(point, Direction.Top);
            yield return GetNextPoint(point, Direction.TopRight);
            yield return GetNextPoint(point, Direction.Right);
            yield return GetNextPoint(point, Direction.BottomRight);
            yield return GetNextPoint(point, Direction.Bottom);
            yield return GetNextPoint(point, Direction.BottomLeft);
            yield return GetNextPoint(point, Direction.Left);
            yield return GetNextPoint(point, Direction.TopLeft);

        }

        public static Direction GetDirection(Point oldPoint, Point newPoint)
        {
            int xDiff = newPoint.X - oldPoint.X;
            int yDiff = newPoint.Y - oldPoint.Y;

            if (xDiff == 0 && yDiff == 1)
                return Direction.Bottom;

            if (xDiff == -1 && yDiff == 1)
                return Direction.BottomLeft;

            if (xDiff == 1 && yDiff == 1)
                return Direction.BottomRight;

            if (xDiff == -1 && yDiff == 0)
                return Direction.Left;

            if (xDiff == 1 && yDiff == 0)
                return Direction.Right;

            if (xDiff == 0 && yDiff == -1)
                return Direction.Top;

            if (xDiff == -1 && yDiff == -1)
                return Direction.TopLeft;

            if (xDiff == 1 && yDiff == -1)
                return Direction.TopRight;

            // sem by to nemelo jit :)
            return Direction.BottomRight;
        }

        public static Point GetNextPoint(Point position, Direction absoluteDirection)
        {
            switch (absoluteDirection)
            {
                case Direction.Top:
                    return new Point(position.X, position.Y - 1);
                case Direction.TopRight:
                    return new Point(position.X + 1, position.Y - 1);
                case Direction.Right:
                    return new Point(position.X + 1, position.Y);
                case Direction.BottomRight:
                    return new Point(position.X + 1, position.Y + 1);
                case Direction.Bottom:
                    return new Point(position.X, position.Y + 1);
                case Direction.BottomLeft:
                    return new Point(position.X - 1, position.Y + 1);
                case Direction.Left:
                    return new Point(position.X - 1, position.Y);
                case Direction.TopLeft:
                    return new Point(position.X - 1, position.Y - 1);
            }
            throw new InvalidProgramException("uuu");
        }

        public static Direction GetAbsoluteDirection(Direction direction, Move move)
        {
            if (move == Move.Left)
            {
                if (direction == Direction.Top)
                    return Direction.TopLeft;
                return direction - 1;
            }

            if (move == Move.Right)
            {
                if (direction == Direction.TopLeft)
                    return Direction.Top;
                return direction + 1;
            }

            return direction;
        }

        public static Direction GetOpositeDirection(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return Direction.Bottom;
                case Direction.TopRight:
                    return Direction.BottomLeft;
                case Direction.Right:
                    return Direction.Left;
                case Direction.BottomRight:
                    return Direction.TopLeft;
                case Direction.Bottom:
                    return Direction.Top;
                case Direction.BottomLeft:
                    return Direction.TopRight;
                case Direction.Left:
                    return Direction.Right;
                case Direction.TopLeft:
                    return Direction.BottomRight;
            }
            throw new InvalidProgramException("uuu");
        }

        public static Move GetMove(Direction direction1, Direction direction2)
        {
            if (GetAbsoluteDirection(direction1,Move.Left) == direction2)
                return Move.Left;

            if (GetAbsoluteDirection(direction1, Move.Straight) == direction2)
                return Move.Straight;

            if (GetAbsoluteDirection(direction1, Move.Right) == direction2)
                return Move.Right;

            throw new InvalidOperationException();
        }
    }
}
