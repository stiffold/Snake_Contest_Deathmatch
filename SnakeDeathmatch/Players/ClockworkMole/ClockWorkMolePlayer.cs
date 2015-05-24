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

        private int _initX;
        private int _initY;
        private Direction _initDirection;

        private Random _random = new Random();

        private Position _currentPosition;
        private BlindMoleStrategy _currentStrategy;
        private Playground _playground;

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _playerId = playerId;

            _initX = x;
            _initY = y;
            _initDirection = direction;
            _currentPosition = new Position(x, y, direction);

            _currentStrategy = new BlindMoleStrategy();
            _playground = new Playground(playgroundSize, playerId);

        }

        public Move GetNextMove(int[,] playgroundArray)
        {

            _playground.ApplyNextMove(playgroundArray);
            var nextMove = _currentStrategy.GetNextMove(_playground, _currentPosition);
            _currentPosition = _currentPosition.Move(nextMove);
            return nextMove;

        }

        public string Name
        {
            get { return "Mechanický krtek"; }
        }
    }
}
