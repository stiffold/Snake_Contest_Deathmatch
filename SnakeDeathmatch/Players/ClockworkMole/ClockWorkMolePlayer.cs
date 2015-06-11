using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private FloodStrategy _newStrategy;

        private Playground _playground;

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _playerId = playerId;

            _initX = x;
            _initY = y;
            _initDirection = direction;
            _currentPosition = new Position(x, y, direction);

            _currentStrategy = new BlindMoleStrategy();
            _newStrategy = new FloodStrategy();

            _playground = new Playground(playgroundSize, playerId);

        }

        public Move GetNextMove(int[,] playgroundArray)
        {

            _playground.ApplyNextMove(playgroundArray);

            //var sw = new Stopwatch();
            //sw.Start();
            //var nextMove = _currentStrategy.GetNextMove(_playground, _currentPosition);
            //sw.Stop();
            //_currentPosition = _currentPosition.Move(nextMove);
            //return nextMove;

            //var sw = new Stopwatch();
            //sw.Start();
            var nextMove = _newStrategy.GetNextMove(_playground, _currentPosition);
            //sw.Stop();

            if (nextMove == null)
                nextMove = _currentStrategy.GetNextMove(_playground, _currentPosition);

            _currentPosition = _currentPosition.Move(nextMove.Value);
            return nextMove.Value;

        }

        public string Name
        {
            get { return "Mechanický krtek"; }
        }
    }
}
