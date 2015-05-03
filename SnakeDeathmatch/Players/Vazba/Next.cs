using System;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Next
    {
        public Direction LeftDirection;
        public Direction StraightDirection;
        public Direction RightDirection;

        public Point? LeftPoint;
        public Point? StraightPoint;
        public Point? RightPoint;

        public override string ToString()
        {
            return string.Format("Left: {0}, Straight: {1}, Right: {2}", LeftPoint, StraightPoint, RightPoint);
        }
    }
}
