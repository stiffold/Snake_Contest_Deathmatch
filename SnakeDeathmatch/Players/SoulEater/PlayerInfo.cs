using SnakeDeathmatch.Interface;

public class PlayerInfo
{
    public PlayerInfo(int x, int y, int identificator)
    {
        CurrentPosition = new Point(x, y);
        IsDown = false;
        Direction = null;
        Identificator = identificator;
    }

    public Direction? Direction;

    public Point CurrentPosition;

    public int Identificator;

    public bool IsDown;
}