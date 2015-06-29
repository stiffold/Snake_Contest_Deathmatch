using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Vazba.Helper;
using SnakeDeathmatch.Players.Vazba.PlaygroundAnalysis;

namespace SnakeDeathmatch.Players.Vazba.Strategies
{
    public interface IStrategy
    {
        Move GetNextMove(IntPlayground playground, Snakes liveSnakes);
    }
}
