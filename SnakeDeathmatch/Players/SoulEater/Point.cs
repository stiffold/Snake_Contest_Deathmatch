namespace SnakeDeathmatch.Players.SoulEater
{
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;

        public bool Equals(Point point)
        {
            return X == point.X && Y == point.Y;
        }

        public override string ToString()
        {
            return string.Format("[{0},{1}]", X, Y);
        }
    }

    public struct PointWithValue
    {
        public PointWithValue(int x, int y, int value)
        {
            X = x;
            Y = y;
            Value = value;
        }

        public int X;
        public int Y;
        public int Value;
    }
}