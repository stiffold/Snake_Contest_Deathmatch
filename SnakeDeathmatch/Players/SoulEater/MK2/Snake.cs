using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater.MK2
{
    public class Snake
    {
        public Snake(PointClass point, int identificator, int x, int y)
        {
            Point = point;
            Identificator = identificator;
            IsDown = false;
            PreviousPoints =new List<PointClass>();
            Direction = Direction.Left;  // todo tp
            X = x;
            Y = y;
        }

        public void UpdatePosition(PointClass newPoint, int x, int y)
        {
            IsDown = false;

            Direction = MySuperClass.GetDirection(new Point(X, Y), new Point(x, y));

            Point = newPoint;

            X = x;
            Y = y;
        }

        public void UpdatePosition(Direction direction)
        {
            IsDown = false;

            var point = MySuperClass.GetNextPoint(new Point(X, Y), direction);

            X = point.X;
            Y = point.Y;

            Direction = direction;
            Point = Point.GetLinkedPoint(direction);        
        }

        public void UpdatePosition(Move move)
        {
            var direction = MySuperClass.GetAbsoluteDirection(Direction, move);

            UpdatePosition(direction);
        }

        public int X { get; set; }

        public int Y { get; set; }

        public PointClass Point { get; set; }

        public IList<PointClass> PreviousPoints { get; set; }

        public int Identificator { get; set; }

        public Direction Direction { get; set; }

        public bool IsDown { get; set; }
    }
}
