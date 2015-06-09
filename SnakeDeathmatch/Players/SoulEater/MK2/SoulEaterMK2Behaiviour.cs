using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater.MK2
{
    public class SoulEaterMK2Behaiviour : IPlayerBehaviour2, IDebuggable
    {
        private int playerId;
        [ToDebug]
        private GameGroundMK2 _gameGround;
        private PointClass _myPositionPointClass;
        private Direction _myDirection;

        private BasicRecursiveStrategy _strategy = new BasicRecursiveStrategy(350, true);

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _gameGround = new GameGroundMK2(playgroundSize, playerId);

            _gameGround.Init();
            _myPositionPointClass = _gameGround[x, y];
            _myDirection = direction;
        }

        public Move GetNextMove(int[,] playground)
        {
            _gameGround.Update(playground);

            if (_gameGround.Players.All(x => x.IsDown))
            {
                _strategy.RecurseDepth = 10;
            }

            return _strategy.GetNextMoveAndUpdateMyNextPositionAndDirection(_gameGround, ref _myPositionPointClass, ref _myDirection);
        }

        

        public string Name
        {
            get { return "SoulEater"; }
        }

        public event BreakpointEventHandler Breakpoint;
    }
}
