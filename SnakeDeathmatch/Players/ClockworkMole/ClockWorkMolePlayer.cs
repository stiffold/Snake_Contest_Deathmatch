using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.ClockworkMole
{
    public class ClockWorkMolePlayer : IPlayerBehaviour2
    {
        private int _playerId;
        private int _playgroundsize;
        private int _initX;
        private int _initY;
        private Direction _initDirection;


        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _playerId = playerId;
            _playgroundsize = playgroundSize;
            _initX = x;
            _initY = y;
            _initDirection = direction;
        }

        public Move GetNextMove(int[,] playground)
        {
            return Move.Left;
        }

        public string Name
        {
            get { return "Mechanický krtek"; }
        }
    }
}
