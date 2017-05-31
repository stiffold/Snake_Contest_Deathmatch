using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Jardos.Components.Strategies
{
    public class RoterrorStrategy : IStrategy
    {
        private int _alocation;
        private List<SnakePoint> _snakePoints = new List<SnakePoint>();
        private Fact _fact;

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
            //return !_snakePoints.Find(x => x.Round == _fact.Round + 1).Collission(_fact.LastUnmaskedBattleGround);
            return ComputeHelper.SnakePointsIsValid(_snakePoints.Where(x=>x.Round > _fact.Round + 1),_fact.LastUnmaskedBattleGround);
        }

        public int EvaluateScore()
        {
            if (_fact.Round > 50) return 0;

            _snakePoints = new List<SnakePoint>();
            _alocation = 0;

            var targetPosition = new Point(0, 0);
            double min = 500;
            foreach (var s in _fact.Snakes.Where(x=>x.Id != ComputeHelper.MyId))
            {
                if (_fact.LastPoint.Distance(s.HeadPoint) < min)
                {
                    min = _fact.LastPoint.Distance(s.HeadPoint);
                    targetPosition = s.HeadPoint;
                }
            }

            var distance = targetPosition.Distance(_fact.LastPoint);
            var targets = new List<Tuple<int, Point>>();
            var number = 1;
            foreach (var dir in Enum.GetValues(typeof(Direction)))
            {
                Direction d = (Direction)dir;

                var p = targetPosition;
                for (var i = 0; i < (distance / 6 + 6); i++)
                {
                    p.Update(d);
                    if (!p.IsValid())
                    {
                        return 0;
                    }
                }
                targets.Add(new Tuple<int, Point>(number, p));
                //označení v poli
                //if (!p.IsInCollission(_ch.Max) && _simulateGameSurround[p.X, p.Y] == 0)
                //{
                //    _simulateGameSurround[p.X, p.Y] = 200;
                //}
                number++;
            }

            double minDistance = 200;
            var tartetPoint = targets.First();
            foreach (var p in targets)
            {
                if (p.Item2.Distance(_fact.LastPoint) < minDistance)
                {
                    tartetPoint = p;
                    minDistance = p.Item2.Distance(_fact.LastPoint);
                }
            }

            bool succes = ComputeHelper.SnakePointsToTarget(new SnakePoint(_fact.Round -1, _fact.LastPoint, _fact.LastMove ,_fact.LastDirection, false), tartetPoint.Item2,ref _snakePoints, _fact.LastUnmaskedBattleGround);
            
            if (succes)
            {
                foreach (var t in targets.Where(x => x.Item1 > tartetPoint.Item1).OrderBy(x => x.Item1))
                {
                    if (succes)
                    {
                        if (_snakePoints.Count == 0) return 0;
                        succes = ComputeHelper.SnakePointsToTarget(_snakePoints.OrderByDescending(x=>x.Round).First(), t.Item2,ref _snakePoints, _fact.LastUnmaskedBattleGround);
                    }
                }
            }

            if (succes)
            {
                foreach (var t in targets.Where(x => x.Item1 < tartetPoint.Item1).OrderBy(x => x.Item1))
                {
                    if (succes)
                    {
                        succes = ComputeHelper.SnakePointsToTarget(_snakePoints.OrderByDescending(x => x.Round).First(), t.Item2, ref _snakePoints, _fact.LastUnmaskedBattleGround);
                    }
                }
            }

            if (succes)
            {
                succes = ComputeHelper.SnakePointsToTarget(_snakePoints.OrderByDescending(x => x.Round).First(), tartetPoint.Item2, ref _snakePoints, _fact.LastUnmaskedBattleGround);
                ComputeHelper.Cut(2, _snakePoints);
                _alocation = _snakePoints.Count();
            }

            if (succes)
            {
                return 1000;
            }
            else
            {
                return 0;
            }
        }

        public Move GetMove()
        {
            return _snakePoints.Find(x => x.Round == _fact.Round).Move;
        }

        public void Update(Fact fact)
        {
            _fact = fact;
        }
    }
}
