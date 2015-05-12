using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public interface IStrategy
    {
        Move GetNextMove(IntPlayground playground, Snakes liveSnakes);
    }
}
