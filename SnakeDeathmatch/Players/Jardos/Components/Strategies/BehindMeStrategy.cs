using System.Linq;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Jardos.Components.Strategies
{
    public class BehindMeStrategy : IStrategy
    {
        private bool _boolKillMode;
        private Fact _fact;
        private int _killSteps;
        private Move _vitimSide;
        private int _waitSteps;

        public BehindMeStrategy()
        {
            _boolKillMode = false;
            _waitSteps = 0;
            _killSteps = 0;
            _vitimSide = Move.Left;
        }

        public Snake SnakeBehindMe { get; set; }

        public event BreakpointEventHandler Breakpoint;

        public int EvaluateScore()
        {
            if (_boolKillMode)
                return 1000;
            return 0;
        }

        public int Alocation
        {
            get { return 5; }
        }

        public void Update(Fact fact)
        {
            _fact = fact;
            UpdateBehindMe();

            if (SnakeBehindMe != null)
                _waitSteps++;
            else
                _waitSteps = 0;

            if (_waitSteps > 3)
                _boolKillMode = true;
        }

        public bool CanContinue()
        {
            if (_killSteps > 5)
                return false;
            return true;
        }

        public Move GetMove()
        {
            _killSteps++;
            if (_killSteps > 5)
            {
                _killSteps = 0;
                _boolKillMode = false;
            }
            _vitimSide = VictimSide();
            _killSteps++;
            if (_vitimSide == Move.Right)
                if (_killSteps < 3)
                    return Move.Right;
                else if (_killSteps == 4)
                    return Move.Straight;
                else
                    return Move.Left;
            if (_vitimSide == Move.Left)
                if (_killSteps < 3)
                    return Move.Left;
                else if (_killSteps == 4)
                    return Move.Straight;
                else
                    return Move.Right;
            return Move.Straight;
        }

        private void UpdateBehindMe()
        {
            if (
                _fact.Snakes.Any(
                    s =>
                        (s.LastDirection == _fact.LastDirection) && (s.HeadPoint.Distance(_fact.LastPoint) < 20) &&
                        s.HeadPoint.IsOpposite(_fact.LastPoint, _fact.LastDirection)))
                SnakeBehindMe =
                    _fact.Snakes.First(
                        s =>
                            (s.LastDirection == _fact.LastDirection) && (s.HeadPoint.Distance(_fact.LastPoint) < 20) &&
                            s.HeadPoint.IsOpposite(_fact.LastPoint, _fact.LastDirection));
            else
                SnakeBehindMe = null;
        }

        private Move VictimSide()
        {
            Direction dir = _fact.LastDirection;
            Point p = new Point(_fact.LastPoint.X, _fact.LastPoint.Y);
            ComputeHelper.Move(ref dir, ref p, Move.Left);
            ComputeHelper.Move(ref dir, ref p, Move.Left);
            if (Collider.Collission(dir, p, _fact.ActualBattleGround))
            {
                return Move.Right;
            }
            else
            {
                return Move.Left;
            }
        }
    }
}