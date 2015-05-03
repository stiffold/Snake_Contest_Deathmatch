using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public struct Me
    {
        public static int Id { get { return (int)PlayerId.Vazba; } }
        public Point P;
        public Direction Direction;

        public Me(Point point, Direction direction)
        {
            P = point;
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
                Left = P.CanMove(leftDirection, playground) ? new Me(P.Move(leftDirection), leftDirection) : (Me?)null,
                Straight = P.CanMove(straightDirection, playground) ? new Me(P.Move(straightDirection), straightDirection) : (Me?)null,
                Right = P.CanMove(rightDirection, playground) ? new Me(P.Move(rightDirection), rightDirection) : (Me?)null,
            };
        }
    }
}
