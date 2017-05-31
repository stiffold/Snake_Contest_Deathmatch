using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Jardos.Debug;

namespace SnakeDeathmatch.Players.Jardos.Components.Strategies
{
    class SurviveStrategy : IStrategy
    {
        private IEnumerable<SnakePoint> _snakePoints;
        private Fact _fact;
        private int _alocation;
        private int[,] debugBattleGround;        
            
        public int Alocation
        {
            get
            {
                return _alocation;
            }
        }

        public event BreakpointEventHandler Breakpoint;

        public bool CanContinue()
        {
            return ComputeHelper.SnakePointsIsValid(_snakePoints.Where(x => x.Round > _fact.Round + 1), _fact.LastUnmaskedBattleGround);
        }

        public int EvaluateScore()
        {
            _snakePoints = new List<SnakePoint>();
            _alocation = 0;

            int score = 0;
            foreach (Move m in Enum.GetValues(typeof(Move)))
            {
                int[,] battleGround = (int[,])_fact.ActualBattleGround.Clone();
                var snakePoints = new List<SnakePoint>();
                snakePoints.Add(new SnakePoint(_fact.Round - 1, _fact.LastPoint, _fact.LastMove, _fact.LastDirection, false));
                while (AddNextPoint(m,battleGround,snakePoints));
                ComputeHelper.Cut(2, snakePoints);
                if (snakePoints.Count > score)
                {                    
                    score = snakePoints.Count();
                    _snakePoints = snakePoints.OrderBy(X=>X.Round).Take(20);
                    debugBattleGround = battleGround;
                    if (Breakpoint != null)
                        Breakpoint(this, new BreakpointEventArgs(JardosBreakpointNames.Jardos_SurviveStrategyChangedScope));
                }
            }
            _alocation = _snakePoints.Count() - 1;
            return _alocation;
        }

        public Move GetMove()
        {
            if (_snakePoints.Any(x => x.Round == _fact.Round))
            {
                return _snakePoints.First(x => x.Round == _fact.Round).Move;
            }
            else
            {
                return Move.Straight;
            }
            
        }

        [ToDebug(typeof(DebugVizualizer))]
        public DebugablePlayground Choosen { get { return new DebugablePlayground(debugBattleGround, "battleGround"); } set { } }

        public void Update(Fact fact)
        {
            _fact = fact;
        }


        private bool AddNextPoint(Move primaryMove, int[,] battleGround, List<SnakePoint> snakePoints)
        {
            bool choosen = false;
            var lastSnakePoint = snakePoints.OrderByDescending(x => x.Round).FirstOrDefault();       
                        
            SnakePoint nextPoint = ComputeHelper.Move(lastSnakePoint, primaryMove);
            SnakePoint nextPoint2 = ComputeHelper.Move(nextPoint, primaryMove);
            if (!nextPoint.Collission(battleGround) &&
                !nextPoint2.Collission(battleGround))
            {
                choosen = true;                                
            }
            else
            {
                foreach (Move m in ComputeHelper.OtherMoves(primaryMove))
                {
                    nextPoint = ComputeHelper.Move(lastSnakePoint, m);
                    nextPoint2 = ComputeHelper.Move(nextPoint, m);
                    if (!nextPoint.Collission(battleGround) &&
                        !nextPoint2.Collission(battleGround))
                    {
                        choosen = true;          
                        break;
                    } 
                }                
            }
            if (choosen)
            {
                battleGround[nextPoint.Point.X, nextPoint.Point.Y] = 201;
                snakePoints.Add(nextPoint);
            } 

            return choosen;
        }
        
    }
}
