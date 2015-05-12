using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeGame.Players.Fools
{
    using Direction = SnakeGame.Interface.Direction;
    using Move = SnakeGame.Interface.Move;

    public class FoolHelper
    {
        public Direction GetNextDirection(Direction direction, Move move)
        {
            if (move == Move.Straight)
            {
                return direction;
            }
            else if (move == Move.Left)
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
            }
            else if (move == Move.Right)
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
            }
            throw new Exception();
        }
    }
}
