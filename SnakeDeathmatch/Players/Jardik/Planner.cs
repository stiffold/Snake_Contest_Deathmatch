using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardik
{
    class Planner
    {
        CollissionHelper _ch;
        int _myId;
        List<WalkSetBase> _walkSets = new List<WalkSetBase>();
        List<WalkSetBase> _walkSetsPro = new List<WalkSetBase>();
        static Random rnd = new Random();
        public Planner(int max, int myId)
        {
            _ch = new CollissionHelper(max);
            _myId = myId;
            _walkSets.Add(new Straight(_ch, _myId));
            _walkSets.Add(new Right45(_ch, _myId));
            _walkSets.Add(new Right90(_ch, _myId));
            _walkSets.Add(new Right125(_ch, _myId));
            _walkSets.Add(new Right180(_ch, _myId));
            _walkSets.Add(new Left45(_ch, _myId));
            _walkSets.Add(new Left90(_ch, _myId));
            _walkSets.Add(new Left125(_ch, _myId));
            _walkSets.Add(new Left180(_ch, _myId));

            _walkSetsPro.Add(new RollLeft(_ch, _myId));
            _walkSetsPro.Add(new RollRight(_ch, _myId));
            _walkSetsPro.Add(new SquareRight(_ch, _myId));
            _walkSetsPro.Add(new SquareLeft(_ch, _myId));
            _walkSetsPro.Add(new Snaker(_ch, _myId));
            _walkSetsPro.Add(new Quaker(_ch, _myId));


        }

        public List<Walk> GetVariant(int round, Position position, Direction direction, int[,] gameSurround, WalkSetType type)
        {
            foreach (var walkSet in _walkSetsPro)
            {
                walkSet.Evaluate(round, position, direction, gameSurround);
            }

            var best = _walkSetsPro.Where(x => x.Type() == type).First();

            if (best != null) return best.Walks;
            return new List<Walk>();
        }

        public List<Walk> GetBestWalksToMe(int round, Position position, Direction direction, int[,] gameSurround)
        {

            foreach (var walkSet in _walkSetsPro)
            {
                walkSet.Evaluate(round, position, direction, gameSurround);
            }
            var randomWalkSet = _walkSetsPro[rnd.Next(_walkSetsPro.Count)];
            if (randomWalkSet.Score > 30)
            {
                return randomWalkSet.Walks;
            }
            var bestPro = _walkSetsPro.OrderByDescending(x => x.Score).FirstOrDefault();
            if (bestPro.Score > 30)
            {
                return bestPro.Walks;
            }

            foreach (var walkSet in _walkSets)
            {
                walkSet.Evaluate(round, position, direction, gameSurround);
            }
            var best = _walkSets.OrderByDescending(x => x.Score).FirstOrDefault();

            if (best != null) return best.Walks;
            return new List<Walk>();
        }

        internal void RepairSteps(int round, List<Walk> planedWalks, int[,] gameSurround)
        {
            int roundIndCollission = 0;
            foreach (Walk walk in planedWalks.Where(x => x.Round > round))
            {
                if (_ch.Collission(walk.Direction, gameSurround, walk.Position))
                {
                    roundIndCollission = walk.Round;
                    continue;
                }
            }

            if (roundIndCollission != 0)
            {
                var lastWalk = planedWalks.Where(x => x.Round == round - 1).First();
                planedWalks.Clear();
            }

            if (roundIndCollission == 0)
            {
                var lastWalk = planedWalks.OrderByDescending(x => x.Round).First();

                foreach (var walkSet in _walkSets)
                {
                    walkSet.Evaluate(lastWalk.Round, lastWalk.Position, lastWalk.Direction, gameSurround);
                }
                var best = _walkSets.OrderByDescending(x => x.Score).FirstOrDefault();
                if (best.Score < 20)
                {
                    RemovePosibbleWalks(round, 20 ,planedWalks);
                }                
            }

        }

        private void RemovePosibbleWalks(int round, int toRemoveCount ,List<Walk> planedWalks)
        {
            int lastRound = planedWalks.Max(x => x.Round);
            int canDeleteCount = lastRound - round;
            if (canDeleteCount >= toRemoveCount)
            {
                planedWalks.RemoveAll(x => x.Round >= (lastRound - toRemoveCount));
            }
            else
            {
                planedWalks.RemoveAll(x => x.Round >= (lastRound - canDeleteCount));
            }
        }
    }
}
