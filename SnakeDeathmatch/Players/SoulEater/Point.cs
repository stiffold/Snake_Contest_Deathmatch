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
            Point = new Point(x,y);
            Value = value;
        }

        public PointWithValue(Point point, int value)
        {
            Point = point;
            Value = value;
        }

        public Point Point;
        public int X {
            get { return Point.X; }
        }
        public int Y {
            get { return Point.Y; }
        }
        public int Value;
    }
}