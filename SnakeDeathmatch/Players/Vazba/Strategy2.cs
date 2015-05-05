using System;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Strategy2 : IStrategy
    {
        public Move GetNextMove(int[,] playground, Snakes liveSnakes)
        {
            return Move.Straight;
        }
    }
}
