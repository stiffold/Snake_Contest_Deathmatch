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

        public void Update(Point newPosition, Direction newDirection)
        {
//            Dir = GetDirection(P, newP);
//            P = newP;

            P = newPosition;
            Dir = newDirection;
        }
    }
}
