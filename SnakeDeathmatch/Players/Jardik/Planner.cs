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


        }

        public List<Walk> GetBestWalksToMe(int round, Position position, Direction direction, int[,] gameSurround)
        {

            foreach (var walkSet in _walkSetsPro)
            {
                walkSet.Evaluate(round, position, direction, gameSurround);
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

                planedWalks.AddRange(GetBestWalksToMe(round, lastWalk.Position, lastWalk.Direction, gameSurround));
            }

        }
    }
}
