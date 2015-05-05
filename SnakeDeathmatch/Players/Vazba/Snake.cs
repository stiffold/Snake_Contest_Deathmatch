using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public struct Snake
    {
        public int Id { get; private set; }
        public Point P { get; private set; }
        public Direction Direction { get; private set; }
        public int X { get { return P.X; } }
        public int Y { get { return P.Y; } }

        public Snake(int id, Point p, Direction direction) : this()
        {
            Id = id;
            P = p;
            Direction = direction;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", P, Direction);
        }

        public Next GetNext(int[,] playground)
        {
            Direction leftDirection = Direction.TurnLeft();
            Direction straightDirection = Direction;
            Direction rightDirection = Direction.TurnRight();

            return new Next()
            {
                Left = P.CanMove(leftDirection, playground) ? new Snake(Id, P.Move(leftDirection), leftDirection) : (Snake?)null,
                Straight = P.CanMove(straightDirection, playground) ? new Snake(Id, P.Move(straightDirection), straightDirection) : (Snake?)null,
                Right = P.CanMove(rightDirection, playground) ? new Snake(Id, P.Move(rightDirection), rightDirection) : (Snake?)null,
            };
        }
    }
}
