using System;
using Snake2.game;
using System.Threading;

namespace Snake2.players
{
    public class Randomer : IPlayerBehavior
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
            return "Randomer";
        }
    }
}
