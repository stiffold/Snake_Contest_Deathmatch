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
        private const int _DEEP = 7;
        
        public int EvaluateScore()
        {
            int _score = 0;
            foreach (Move formove in Enum.GetValues(typeof(Move)))
            {
                int forMoveScore = Simulate(_DEEP, 0, _fact.LastPoint, _fact.LastDirection, _fact.ActualBattleGround, formove);
                if (forMoveScore == _DEEP - 1) return forMoveScore;
                if (forMoveScore > _score)
                {
                    _score = forMoveScore;
                }
            }
            return _score;
        }

        public int Alocation
        {
            get { return _DEEP * 2; }
        }

        public void Update(Fact fact)
        {
            _fact = fact;
        }

        public bool CanContinue()
        {
            return _score >= ((_DEEP*2) - 1);
        }

        public Move GetMove()
        {
            Move nextMove = Move.Straight;

            _score = 0;
            foreach (Move formove in Enum.GetValues(typeof(Move)))
            {
                int forMoveScore = Simulate(_DEEP, 0, _fact.LastPoint, _fact.LastDirection, _fact.ActualBattleGround, formove);
                //if (forMoveScore == _DEEP-1) return formove;
                if (forMoveScore > _score)
                {
                    _score = forMoveScore;
                    nextMove = formove;
                }
            }
            return nextMove;
        }

        private int Simulate(int deep, int score ,Point myPoint, Direction myDirection, int[,] playground, Move move)
        {
            Point fakePoint = myPoint;
            Direction fakeDirection = myDirection;
            int[,] fakePlayGround = (int[,])playground.Clone();
            bool wasInCollission = false;

            for (int i = 0; i < 2; i++)
            {
                ComputeHelper.Move(ref fakeDirection, ref fakePoint, move);
                if (Collider.Collission(fakeDirection, fakePoint, fakePlayGround))
                {
                    wasInCollission = true;
                    break;
                }
                fakePlayGround[fakePoint.X, fakePoint.Y] = ComputeHelper.MyId;
                score++;
            }            

            deep--;
            if (deep == 0 || wasInCollission) return score;            

            int subScore = 0;
            foreach (Move formove in Enum.GetValues(typeof(Move)))
            {
                int subResultScore = Simulate(deep, 0 ,fakePoint, fakeDirection, fakePlayGround, formove);
                if (subResultScore > subScore)
                {
                    subScore = subResultScore;
                }
            }
            return score + subScore;
        }

        public event BreakpointEventHandler Breakpoint;
    }
}
