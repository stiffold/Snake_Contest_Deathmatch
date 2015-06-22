using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public struct Vector
    {
        public int X;
        public int Y;
        public Direction Direction;
        public Vector(int x, int y, Direction direction) { X = x; Y = y; Direction = direction; }
        public override string ToString() { return string.Format("[{0},{1},{2}]", X, Y, Direction); }
    }
}
