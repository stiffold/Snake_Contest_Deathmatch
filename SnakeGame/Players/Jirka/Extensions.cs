using SnakeGame.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeGame.Players.Jirka
{
    static class Extensions
    {
        public static Direction TurnLeft(this Direction direction)
        {
            if (direction == Direction.Top)
                return Direction.TopLeft;
            return direction--;
        }

        public static Direction TurnRight(this Direction direction)
        {
            if (direction == Direction.TopLeft)
                return Direction.Top;
            return direction++;
        }
    }
}
