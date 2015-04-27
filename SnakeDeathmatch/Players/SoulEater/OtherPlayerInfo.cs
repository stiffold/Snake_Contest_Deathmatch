using SnakeDeathmatch.Interface;

public class OtherPlayerInfo
{
    public OtherPlayerInfo(int x, int y, int identificator)
    {
        CurrentPosition = new Point(x, y);
        IsDown = false;
        Direction = null;
        Identificator = identificator;
    }

    public Direction? Direction { get; set; }

    public Point CurrentPosition { get; set; }

    public int Identificator { get; set; }

    public bool IsDown { get; set; }
}