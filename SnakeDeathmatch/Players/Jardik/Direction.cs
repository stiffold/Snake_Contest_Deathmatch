using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardik
{
    public enum Direction
    {
        Top = 1,
        TopRight = 2,
        Right = 3,
        BottomRight = 4,
        Bottom = 5,
        BottomLeft = 6,
        Left = 7,
        TopLeft = 8,   
    }

    static class DirectionExtensions
    {
        public static Direction GetNewDirection(this Direction oldDirection, Move move)
        {
            if (move == Move.Left)
            {
                switch (oldDirection)
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
            }

            if (move == Move.Right)
            {
                switch (oldDirection)
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
            }

            return oldDirection;
        }

        public static bool IsDiagonal(this Direction direction)
        {
                switch (direction)
                {
                    case Direction.TopRight: return true;
                    case Direction.BottomRight: return true;
                    case Direction.BottomLeft: return true;
                    case Direction.TopLeft: return true;
                }
                return false;          
        }

        public static List<Move> MovesToGetDirection(this Direction direction, Direction targetDirection)
        {
            List<Move> rightMoves = new List<Move>();
            List<Move> leftMoves = new List<Move>();
            Direction simulateDirection = direction;
            while(simulateDirection != targetDirection)
            {
                simulateDirection = simulateDirection.GetNewDirection(Move.Right);
                rightMoves.Add(Move.Right);
            }

            simulateDirection = direction;
            while (simulateDirection != targetDirection)
            {
                simulateDirection = simulateDirection.GetNewDirection(Move.Left);
                leftMoves.Add(Move.Left);
            }

            if (leftMoves.Count < rightMoves.Count)
            {
                return leftMoves;
            }
            else
            {
                return rightMoves;
            }
        }

        public static List<Direction> GetPossibleDirections(this Direction direction)
        { 
            List<Direction> dirs = new List<Direction>();

            dirs.Add(GetNewDirection(direction,Move.Left));
            dirs.Add(GetNewDirection(direction, Move.Straight));
            dirs.Add(GetNewDirection(direction, Move.Right));

            return dirs;
        }
    }
}
