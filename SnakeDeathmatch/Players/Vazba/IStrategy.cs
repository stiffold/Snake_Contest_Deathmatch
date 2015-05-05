using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public interface IStrategy
    {
        Move GetNextMove(int[,] playground, Snakes liveSnakes);
    }
}
