using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Jardos.Components;

namespace SnakeDeathmatch.Players.Jardos
{
    public class Jardos : IPlayerBehaviour2
    {
        private Point _myPoint;
        private Direction _myDirection;
        private Move _myMove;
        private StrategyProcessor _strategyProcessor;
        
        public string Name {
            get { return "Jardos"; }
        }

        public Move GetNextMove(int[,] playground)
        {
            _myMove = _strategyProcessor.GetMove(playground, _myPoint, _myDirection, _myMove);
            ComputeHelper.Move(ref _myDirection, ref _myPoint, _myMove);
            return _myMove;
        }

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            ComputeHelper.SetDefaults(playgroundSize,playerId);

            _myPoint = new Point(x,y);
            _myDirection = direction;
            _strategyProcessor = new StrategyProcessor();
        }

        [ToDebug]
        public StrategyProcessor StrategyProcessor { get { return _strategyProcessor; } }

    }
}
