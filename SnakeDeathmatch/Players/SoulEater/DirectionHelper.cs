using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater
{
    public class DirectionHelper
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

        public static Interface.Direction GetDirection(Point oldPoint, Point newPoint)
        {
            int xDiff = newPoint.X - oldPoint.X;
            int yDiff = newPoint.Y - oldPoint.Y;

            if (xDiff == 0 && yDiff == 1)
                return Interface.Direction.Bottom;

            if (xDiff == -1 && yDiff == 1)
                return Interface.Direction.BottomLeft;

            if (xDiff == 1 && yDiff == 1)
                return Interface.Direction.BottomRight;

            if (xDiff == -1 && yDiff == 0)
                return Interface.Direction.Left;

            if (xDiff == 1 && yDiff == 0)
                return Interface.Direction.Right;

            if (xDiff == 0 && yDiff == -1)
                return Interface.Direction.Top;

            if (xDiff == -1 && yDiff == -1)
                return Interface.Direction.TopLeft;

            if (xDiff == 1 && yDiff == -1)
                return Interface.Direction.TopRight;

            // sem by to nemelo jit :)
            return Interface.Direction.BottomRight;
        }

        public static Point GetNextPoint(Point position, Interface.Direction absoluteDirection)
        {
            switch (absoluteDirection)
            {
                case Interface.Direction.Top:
                    return new Point(position.X, position.Y - 1);
                case Interface.Direction.TopRight:
                    return new Point(position.X + 1, position.Y - 1);
                case Interface.Direction.Right:
                    return new Point(position.X + 1, position.Y);
                case Interface.Direction.BottomRight:
                    return new Point(position.X + 1, position.Y + 1);
                case Interface.Direction.Bottom:
                    return new Point(position.X, position.Y + 1);
                case Interface.Direction.BottomLeft:
                    return new Point(position.X - 1, position.Y + 1);
                case Interface.Direction.Left:
                    return new Point(position.X - 1, position.Y);
                case Interface.Direction.TopLeft:
                    return new Point(position.X - 1, position.Y - 1);
            }
            throw new InvalidProgramException("uuu");
        }

        public static Interface.Direction GetAbsoluteDirection(Interface.Direction direction, Interface.Move move)
        {
            if (move == Interface.Move.Left)
            {
                if (direction == Interface.Direction.Top)
                    return Interface.Direction.TopLeft;
                return direction - 1;
            }

            if (move == Interface.Move.Right)
            {
                if (direction == Interface.Direction.TopLeft)
                    return Interface.Direction.Top;
                return direction + 1;
            }

            return direction;
        }
    }
}
