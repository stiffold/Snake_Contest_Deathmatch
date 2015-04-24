using System;
using Snake2.game;

namespace Snake2.players
{
    public class TestPlayer : IPlayerBehavior
    {
        Random r = new Random(Environment.TickCount);
        public void Init(int direction, int identificator)
        {
           
        }

        public int NextMove(int[,] gameSurrond)
        {
            return r.Next(1,5);
        }

        public string MyName()
        {
            return "Random";
        }
    }
}
