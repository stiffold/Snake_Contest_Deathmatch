using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater
{
    public class PlayerInfo
    {
        public PlayerInfo(int x, int y, int identificator)
        {
            CurrentPosition = new Point(x, y);
            IsDown = false;
            Direction = null;
            Identificator = identificator;
        }

        public PlayerInfo(Point currentPosition, bool isDown, Direction? direction, int identificator)
        {
            CurrentPosition = currentPosition;
            IsDown = isDown;
            Direction = direction;
            Identificator = identificator;
        }

        public PlayerInfo MakeACopy()
        {
            return new PlayerInfo(CurrentPosition, IsDown, Direction, Identificator);
        }

        public Direction? Direction { get; set; }

        public Point CurrentPosition { get; set; }

        public int Identificator { get; set; }

        public bool IsDown { get; set; }
    }
}