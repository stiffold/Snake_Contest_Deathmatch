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
        [ToDebug]
        private GameGroundMK2 _gameGround;

        private Snake _soulEater;

        private BasicRecursiveStrategy _mainStrategy = new BasicRecursiveStrategy(350, true);

        private IStrategy _currentStrategy = null; 

        private IList<IStrategyTrigger> _strategyTriggers = new List<IStrategyTrigger>{new TurnAroundBeforeClash()};


        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _gameGround = new GameGroundMK2(playgroundSize, playerId);

            _gameGround.Init();

            _soulEater = new Snake(_gameGround[x, y], playerId, x, y);
            _soulEater.Direction = direction;
        }

        public Move GetNextMove(int[,] playground)
        {
            //if (_gameGround.OtherSnakes.All(x => x.IsDown))
            //{
            //    _mainStrategy.RecurseDepth = 10;
            //}

            _gameGround.Update(playground);

            List<IStrategy> minorStrategies = GetMinorStrategies(_gameGround, _soulEater);

            RemoveUnvalidStragies(minorStrategies,_gameGround, _soulEater);

            var strategy = SelectStrategy(minorStrategies);

            //if (strategy != null)
            //{
            //    _plannedMovesList = new Queue<Move>(strategy.GetPlanList());
            //}

            Move nextMove  = Move.Straight;
            //if (_plannedMovesList.Any())
            //{
            //    nextMove = _plannedMovesList.Dequeue();
            //}
            //else
            //{
            //    nextMove = _mainStrategy.GetNextMove(_gameGround, _soulEater);
            //}

            //_soulEater.UpdatePosition(nextMove);

            return nextMove;
        }

        private IStrategy SelectStrategy(List<IStrategy> minorStrategies)
        {
            return minorStrategies.FirstOrDefault();
        }

        private void RemoveUnvalidStragies(List<IStrategy> minorStrategies, GameGroundMK2 gameGround, Snake soulEater)
        {
            var strategiesToRemove = new List<IStrategy>();
            foreach (IStrategy strategy in minorStrategies)
            {
                IList<Move> strategyMoves = strategy.GetPlanList();

                bool result = ValidateMoves(strategyMoves, gameGround, soulEater);

                if (result == false)
                    strategiesToRemove.Add(strategy);
            }

            foreach (var strategyToRemove in strategiesToRemove)
            {
                minorStrategies.Remove(strategyToRemove);
            }
        }

        private bool ValidateMoves(IList<Move> strategyMoves, GameGroundMK2 gameGround, Snake soulEater)
        {
            Direction direction = soulEater.Direction;
            PointClass point = soulEater.Point;
            foreach (var move in strategyMoves)
            {
                direction = MySuperClass.GetAbsoluteDirection(direction, move);
                var path = point.GetPath(direction);

                if (path.PathState != PathState.Ok)
                    return false;

                point = path.PointTo;
            }

            return true;
        }

        private List<IStrategy> GetMinorStrategies(GameGroundMK2 gameGround, Snake soulEater)
        {
            List<IStrategy> strategies = new List<IStrategy>();

            foreach (var triger in _strategyTriggers)
            {
                var strategy = triger.GetStrategy(gameGround, soulEater);
                strategies.AddRange(strategy);
            }

            return strategies;
        }


        public string Name
        {
            get { return "SoulEater"; }
        }

        public event BreakpointEventHandler Breakpoint;
    }
}
