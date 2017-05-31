using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Jardos.Components.Strategies
{
    class RecurseStrategy : IStrategy
    {
        private Fact _fact;
        private int _score;
        private int _count = 0;
        private int _nextCount = 0;
        private bool _wrapped;
        private bool _changeMind;
        private int[,] _wrappedBattleGround;
        private const int _DEEP = 8;
        private List<MovePriority> _moves;

        private int[,] GetBattleGround()
        {
            if (_wrapped && _wrappedBattleGround != null)
            {
                return _wrappedBattleGround;
            }
            return _fact.ActualBattleGround;
        }

        public RecurseStrategy(bool isWraped = false, bool changeMind = true)
        {
            _moves = new List<MovePriority> {new MovePriority{Move = Move.Left, Priority = 0}, new MovePriority { Move = Move.Straight, Priority = 0 }, new MovePriority { Move = Move.Right, Priority = 0 } };
            _wrapped = isWraped;
            _changeMind = changeMind;
        }

        public int EvaluateScore()
        {
            _score = 0;
            if (_changeMind)
            {
                if (_count > _nextCount)
                {
                    if (_fact.Snakes.Any(x=>x.Live && x.HeadPoint.Distance(_fact.LastPoint) < 20))
                    {
                        var moveToChange = _moves.OrderBy(x=>x.Priority).First();
                        moveToChange.Priority += _count;
                    }
                    _nextCount += 50;
                }
            }

            foreach (var m in _moves.OrderByDescending(x=>x.Priority))
            {
                int forMoveScore = Simulate(_DEEP, 0, _fact.LastPoint, _fact.LastDirection, GetBattleGround(), m.Move, new List<Point>());

                if (forMoveScore > _score)
                {
                    _score = forMoveScore;                    
                }
            }

            
            return _score;
        }

        public int Alocation
        {
            get { return _DEEP; }
        }

        public void Update(Fact fact)
        {
            _fact = fact;
        }

        public void Update(int [,] wrappedBattleGround)
        {
            _wrappedBattleGround = wrappedBattleGround;
        }

        public bool CanContinue()
        {
            return _score >= (_DEEP - 1);
        }

        public Move GetMove()
        {
            _count++;
            Move nextMove = Move.Straight;
            _score = 0;
            foreach (var m in _moves.OrderByDescending(x => x.Priority))
            {
                int forMoveScore = Simulate(_DEEP, 0, _fact.LastPoint, _fact.LastDirection, GetBattleGround(), m.Move, new List<Point>());
                
                if (forMoveScore > _score)
                {
                    _score = forMoveScore;
                    nextMove = m.Move;
                }
            }
            return nextMove;
        }

        private int Simulate(int deep, int score ,Point myPoint, Direction myDirection, int[,] playground, Move move, List<Point> beforePoints)
        {
            Point fakePoint = myPoint;
            Direction fakeDirection = myDirection;            

            ComputeHelper.Move(ref fakeDirection, ref fakePoint, move);
            if (Collider.Collission(fakeDirection, fakePoint, playground) || beforePoints.Any(x=>x.Equals(fakePoint)))
            {
                return score;
            }
            beforePoints.Add(fakePoint);
            score++;
            deep--;
            if (deep == 0) return score;
            int subScore = 0;
            foreach (var m in _moves.OrderByDescending(x => x.Priority))
            {
                int subResultScore = Simulate(deep, 0 ,fakePoint, fakeDirection, playground, m.Move, beforePoints.ToList());
                if (subResultScore > subScore)
                {
                    subScore = subResultScore;
                }
            }
            return score + subScore;
        }

        public event BreakpointEventHandler Breakpoint;
    }

    class MovePriority
    {
        public int Priority { get; set; }
        public Move Move { get; set; }
    }
}
