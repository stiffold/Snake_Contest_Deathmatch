using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Jardos.Components
{
    static class Collider
    {

        public static bool Collission(Direction direction, Point point, int[,] playGround)
        {
            if (!point.IsValid()) return true;
            if (CrossColision(direction, playGround, point)) return true;
            if (CollissionWithPlayer(point, playGround)) return true;

            return false;
        }

        private static bool CollissionWithPlayer(Point point, int[,] playGround)
        {
            if (playGround[point.X, point.Y] != 0) return true;
            return false;
        }

        private static bool CrossColision(Direction direction, int[,] playGround, Point point)
        {
            if (point.X <= 0 || point.Y <= 0 || point.X >= ComputeHelper.MaxSize - 1 || point.Y >= ComputeHelper.MaxSize - 1) return false;

            return (direction == Direction.TopRight && (playGround[point.X, point.Y + 1] != 0) && (playGround[point.X - 1, point.Y] != 0)) ||
                    (direction == Direction.BottomRight && (playGround[point.X, point.Y - 1] != 0) && (playGround[point.X - 1, point.Y] != 0)) ||
                    (direction == Direction.BottomLeft && (playGround[point.X, point.Y - 1] != 0) && (playGround[point.X + 1, point.Y] != 0)) ||
                    (direction == Direction.TopLeft && (playGround[point.X, point.Y + 1] != 0) && (playGround[point.X + 1, point.Y] != 0));
        }
    }
}
