using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Jardos.Components
{
    public class SnakePoint
    {
        public SnakePoint(int round, Point point, Move move, Direction direction, bool isFuturePoint)
        {
            Round = round;
            Point = point;
            Move = move;
            Direction = direction;
            IsFuturePoint = isFuturePoint;
        }

        public int Round { get; set; }

        public Point Point { get; set; }

        public Move Move { get; set; }

        public Direction Direction { get; set; }

        public bool IsFuturePoint { get; set; }
    }
}
