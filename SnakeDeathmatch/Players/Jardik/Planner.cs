using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Jardik.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardik
{
    public class Planner : IDebuggable
    {
        CollissionHelper _ch;
        int _myId;
        int _max;
        List<WalkSetBase> _walkSets = new List<WalkSetBase>();
        private Safer _safer;
        static Random rnd = new Random();
        public Planner(int max, int myId)
        {
            _max = max;
            _myId = myId;
            _ch = new CollissionHelper(max, _myId);
            _safer = new Safer(_ch,_myId);
            _walkSets.Add(new Straight(_ch, _myId));
            _walkSets.Add(new Right45(_ch, _myId));
            _walkSets.Add(new Right90(_ch, _myId));
            _walkSets.Add(new Right125(_ch, _myId));
            _walkSets.Add(new Right180(_ch, _myId));
            _walkSets.Add(new Left45(_ch, _myId));
            _walkSets.Add(new Left90(_ch, _myId));
            _walkSets.Add(new Left125(_ch, _myId));
            _walkSets.Add(new Left180(_ch, _myId));
            _walkSets.Add(new LeftGap(_ch, _myId));
            _walkSets.Add(new RightGap(_ch, _myId));

            _walkSets.Add(new RollLeft(_ch, _myId));
            _walkSets.Add(new RollRight(_ch, _myId));
            _walkSets.Add(new SquareLeft(_ch, _myId));
            _walkSets.Add(new SquareRight(_ch, _myId));
            _walkSets.Add(new RollRight(_ch, _myId));
            _walkSets.Add(new Snaker(_ch, _myId));
            _walkSets.Add(new Quaker(_ch, _myId));
            _walkSets.Add(new FunkyTerror(_ch, _myId));
            _walkSets.Add(new Roll100(_ch, _myId));
            _walkSets.Add(new Raper(_ch, _myId));
            _walkSets.Add(new Bingo(_ch, _myId));
            _walkSets.Add(new Randomer(_ch, _myId));


        }

        [ToDebug(typeof(WalkSetVizualizer))]
        public List<DebugablePlayground> Options { get; private set; }

        public event BreakpointEventHandler Breakpoint;

        public List<Walk> GetVariant(int round, Position position, Direction direction, int[,] gameSurround, WalkSetType type)
        {
            var best = _walkSets.Where(x => x.Type() == type).First();
            best.Evaluate(round, position, direction, gameSurround);
            return best.Walks;
        }

        public List<Walk> GetBestWalksToMe(int round, Position position, Direction direction, int[,] gameSurround, List<Other> others)
        {
            Mask(gameSurround, position, others);
            
            List<WalkSetResult> results = new List<WalkSetResult>();
            
            foreach (var walkSet in _walkSets)
            {
                walkSet.Evaluate(round, position, direction, gameSurround);
                results.Add(new WalkSetResult { FirstScore = walkSet.Score, FirstType = walkSet.Type(), FirstWalks = walkSet.Walks.ToList(),SimulateSurroundBefore = walkSet.SimulateGameSurround });
            }

            foreach (var r in results)
            {
                var lastWalk = r.LastWalk;
                if (lastWalk != null)
                {
                    foreach (var walkSet in _walkSets)
                    {
                        walkSet.Evaluate(lastWalk.Round+1, lastWalk.Position, lastWalk.Direction, r.SimulateSurroundBefore);
                        if (r.SecondScore < walkSet.Score)
                        {
                            r.SecondScore = walkSet.Score;
                            r.TotalScore = r.FirstScore + r.SecondScore;
                            r.SecondType = walkSet.Type();
                            r.SecondWalks = walkSet.Walks.ToList();
                            r.SimulateSurroundAfter = walkSet.SimulateGameSurround;
                        }                        
                    } 
                }
            }


            foreach (var r in results)
            {
                var lastWalk = r.LastWalk;
                r.SimulateSurroundBefore = r.SimulateSurroundAfter;
                if (lastWalk != null)
                {
                    foreach (var walkSet in _walkSets)
                    {

                        walkSet.Evaluate(lastWalk.Round + 1, lastWalk.Position, lastWalk.Direction, r.SimulateSurroundBefore);
                        if (r.ThirdScore < walkSet.Score)
                        {
                            r.ThirdScore = walkSet.Score;
                            r.TotalScore = r.FirstScore + r.SecondScore + r.ThirdScore;
                            r.ThirdType = walkSet.Type();
                            r.ThirdWalks = walkSet.Walks.ToList();
                            r.SimulateSurroundAfter = walkSet.SimulateGameSurround;
                            if (r.LastWalk != null)
                            {
                                if (ShitScore(r.LastWalk.Position, walkSet.SimulateGameSurround) < 15)
                                {
                                    r.TotalScore += -100;
                                }
                            }
                        }
                    }
                }
            }

            Options = results.Where(x=>x.SimulateSurroundAfter!= null).Select(x => new DebugablePlayground(x.SimulateSurroundAfter, x.ToString())).ToList();

            var bestResult = results.OrderByDescending(x => x.TotalScore).First();
            
            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(JardikBreakpointNames.Planner));

            if (bestResult.TotalScore < 100 && ShitScore(position, gameSurround) < 15)
            {
                _safer.Evaluate(round, position, direction, gameSurround);
                return _safer.Walks;
            }

            if (bestResult != null) return bestResult.TotalWalks;
            return new List<Walk>();
        }

        private void Mask(int[,] gameSurround, Position position, List<Other> others)
        {
            List<Position> aura = new List<Position>();
            int size = gameSurround.GetUpperBound(0) + 1;
            aura.Add(position);

            foreach (var dir in Enum.GetValues(typeof(Direction)))
            {
                var p = position.Copy();
                p.Update((Direction)dir);
                if (!p.IsInCollission(size))
                {
                    aura.Add(p);
                    foreach (var dir2 in Enum.GetValues(typeof(Direction)))
                    {
                        var p2 = p.Copy();
                        p2.Update((Direction)dir2);
                        if (!p2.IsInCollission(size))
                        {
                            aura.Add(p2);
                        }
                    }
                }
            }

            foreach (var o in others)
            {
                MaskFromPosition(gameSurround, o.Position, 100, aura, size,3);
            }
            
                                        
        }

        private void MaskFromPosition(int [,] gameSurround, Position position, int maskId, List<Position> aura, int size, int count)
        {
            count--;
            foreach (var dir in Enum.GetValues(typeof(Direction)))
            {
                var p = position.Copy();
                p.Update((Direction)dir);
                if (!p.IsInCollission(size))
                {
                    if (!aura.Any(a => a.X == p.X && a.Y == p.Y))
                    {
                        if (gameSurround[p.X, p.Y] ==0)
                        {
                            gameSurround[p.X, p.Y] = maskId; 
                        } 

                    }
                }
                if (count > 0)
                {
                    MaskFromPosition(gameSurround, p, maskId, aura, size, count);
                }
            }
            

        }

        private double ShitScore (Position p, int[,] gameSurround)
        {
            List<Direction> dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
            List<int>collissions = new List<int>();
            foreach (var d in dirs)
            {
                collissions.Add(ColideAt(p.Copy(), d, gameSurround));
            }
            double result = collissions.Average();
            return result;

        }

        private int ColideAt(Position p, Direction direction, int[,] gameSurround)
        {
            int i = 1;
            p.Update(direction);
            while (!p.IsInCollission(_max)&&gameSurround[p.X, p.Y] == 0)
            {
                p.Update(direction);
                i++;
            }
            return i;
        }

        internal void RepairSteps(int round, List<Walk> planedWalks, int[,] gameSurround, Position position, List<Other> others)
        {
            Mask(gameSurround, position, others);
            
            int roundIndCollission = 0;
            foreach (Walk walk in planedWalks.Where(x => x.Round > round).OrderBy(y=>y.Round))
            {
                if (_ch.Collission(walk.Direction, gameSurround, walk.Position))
                {
                    roundIndCollission = walk.Round;
                    break;
                }
            }

            var last = planedWalks.OrderByDescending(x => x.Round).FirstOrDefault();
            if (last != null)
            {
                if (_ch.Collission(last.Direction, gameSurround, last.Position))
                {
                    roundIndCollission = last.Round;
                }
            }

            if (roundIndCollission != 0)
            {
                RemovePosibbleWalks(round, planedWalks.Where(x=>x.Round >= roundIndCollission).ToList().Count + 3, planedWalks);
                if (Breakpoint != null)
                    Breakpoint(this, new BreakpointEventArgs(JardikBreakpointNames.Collission));
            }

            if (roundIndCollission == 0)
            {
                var lastWalk = planedWalks.OrderByDescending(x => x.Round).First();

                foreach (var walkSet in _walkSets)
                {
                    walkSet.Evaluate(lastWalk.Round, lastWalk.Position, lastWalk.Direction, gameSurround);
                }
                var best = _walkSets.OrderByDescending(x => x.Score).FirstOrDefault();

                if (best.Score < 50)
                {
                    int bestScore = 0;
                    int bestCountToRemove = 20;

                    foreach (var w in planedWalks.Where(x => x.Round > round))
                    {
                        foreach (var walkSet in _walkSets)
                        {
                            walkSet.Evaluate(w.Round, w.Position, w.Direction, gameSurround);
                        }

                        int myPositionScore = _walkSets.OrderByDescending(x => x.Score).FirstOrDefault().Score;
                        if (myPositionScore > bestScore)
                        {
                            bestScore = myPositionScore;
                            bestCountToRemove = planedWalks.Max(x => x.Round) - w.Round;
                        }
                    }

                    RemovePosibbleWalks(round, bestCountToRemove, planedWalks);
                    
                    if (Breakpoint != null)
                        Breakpoint(this, new BreakpointEventArgs(JardikBreakpointNames.Repair));
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

    class WalkSetResult
    {
        public int FirstScore { get; set; }
        public int SecondScore { get; set; }
        public int ThirdScore { get; set; }
        public WalkSetType FirstType { get; set; }
        public WalkSetType SecondType { get; set; }
        public WalkSetType ThirdType { get; set; }
        public Walk LastWalk { get 
                                    {
                                        if (SecondWalks == null) return FirstWalks.OrderByDescending(w => w.Round).FirstOrDefault();
                                        if (ThirdWalks == null) return SecondWalks.OrderByDescending(w => w.Round).FirstOrDefault();
                                        return null;
                                    }
                                }
        public List<Walk> FirstWalks { get; set; }
        public List<Walk> SecondWalks { get; set; }
        public List<Walk> ThirdWalks { get; set; }
        public List<Walk> TotalWalks {
                                        get 
                                        { 
                                            var result = FirstWalks;
                                            if (SecondWalks != null) { 
                                                result.AddRange(SecondWalks);
                                                if (ThirdWalks != null)
                                                {
                                                    result.AddRange(ThirdWalks);
                                                }
                                            }
                                            return result;
                                        }
                                    }
        public int TotalScore { get; set; }
        public int[,] SimulateSurroundBefore { get; set; }
        public int[,] SimulateSurroundAfter { get; set; }

        public override string ToString()
        {
            return String.Format("FirstStrategy {0}:{1} SecondStrategy {2}:{3} ThirdStrategy {4}:{5} Total: {6}", FirstType, FirstScore, SecondType, SecondScore,ThirdType,ThirdScore, TotalScore);
        }


    }
}
