using System;
using SnakeGame.Game;
using System.Threading;
using SnakeGame.Interface;

namespace SnakeGame.Players.Fools
{
    using Direction = SnakeGame.Interface.Direction;
    using Move = SnakeGame.Interface.Move;

    public class RandomPlayer : IPlayerBehavior
    {
        private Random _random;

        public void Init(int direction, int identificator)
        {
            Thread.Sleep(10);
            _random = new Random(Environment.TickCount);           
        }

        public int NextMove(int[,] gameSurrond)
        {
            return _random.Next(1, 4);
        }

        public string MyName()
        {
            return "Random Fool";
        }
    }
}
