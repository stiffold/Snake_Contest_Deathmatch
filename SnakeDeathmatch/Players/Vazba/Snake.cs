using System;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Snake
    {
        public int Id { get; private set; }
        public Point P { get; private set; }       // position
        public Direction? Dir { get; private set; }  // direction
        public bool IsDead { get; private set; }

        public Snake(int id, Point initialPosition)
        {
            Id = id;
            P = initialPosition;
            Dir = null;
            IsDead = false;
        }

        public void UpdatePosition(Point newP)
        {
            Dir = GetDirection(P, newP);
            P = newP;
        }

        private Direction GetDirection(Point oldP, Point newP)
        {
            if (oldP.X == newP.X && oldP.Y - 1 == newP.Y) return Direction.Top;
            if (oldP.X == newP.X && oldP.Y + 1 == newP.Y) return Direction.Bottom;
            if (oldP.X + 1 == newP.X && oldP.Y == newP.Y) return Direction.Right;
            if (oldP.X - 1 == newP.X && oldP.Y == newP.Y) return Direction.Left;
            if (oldP.X + 1 == newP.X && oldP.Y - 1 == newP.Y) return Direction.TopRight;
            if (oldP.X - 1 == newP.X && oldP.Y - 1 == newP.Y) return Direction.TopLeft;
            if (oldP.X + 1 == newP.X && oldP.Y + 1 == newP.Y) return Direction.BottomRight;
            if (oldP.X - 1 == newP.X && oldP.Y + 1 == newP.Y) return Direction.BottomLeft;

            throw new Exception(string.Format("Old point {0} and new point {1} are not next to each other.", oldP, newP));
        }
    }
}
