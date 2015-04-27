using System;
using SnakeDeathmatch.Game;
using System.Threading;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Randomer
{
    public class RandomerPlayer : IPlayerBehavior
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
