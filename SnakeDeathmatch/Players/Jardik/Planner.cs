using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Jardik.Debug;

namespace SnakeDeathmatch.Players.Jardik
{
    public class Planner : IDebuggable
    {
        private readonly CollissionHelper _ch;
        private readonly int _myId;
        private readonly int _max;
        private readonly List<WalkSetBase> _walkSets = new List<WalkSetBase>();

        private readonly List<WalkSetBase> _rightSets = new List<WalkSetBase>();

        private readonly List<WalkSetBase> _leftSets = new List<WalkSetBase>();
        private readonly Safer _safer;
        private bool _killOrDie = false;
        private Other _victim = new Other();
        private int _numberOfWBM = 0;
        private Move _victimDirection = Move.Straight;
        private int[,] _victimArray;
        private int numberOfKillSteps = 0;

        #region counstructor

        public Planner(int max, int myId)
        {
            _max = max;
            _myId = myId;
            _victimArray = new int[max,max];
            _ch = new CollissionHelper(max, _myId);
            _safer = new Safer(_ch, _myId);
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
            _walkSets.Add(new StrikeLeft(_ch, _myId));
            _walkSets.Add(new StrikeRight(_ch, _myId));

            _leftSets.Add(new Left90(_ch, _myId));
            _leftSets.Add(new Left125(_ch, _myId));
            _leftSets.Add(new StrikeLeft(_ch, _myId));

            _rightSets.Add(new Right90(_ch, _myId));
            _rightSets.Add(new Right125(_ch, _myId));
            _rightSets.Add(new StrikeRight(_ch, _myId));
        }

        #endregion

        #region Main Func
        internal List<Walk> UpdateMoves(List<Walk> myPlanedMoves, int round, Position position, Direction direction, int[,] gameSurround, List<Other> others)
        {
            Mask(gameSurround, position, others);

            if (!_killOrDie)
            {
                _victim = VictimBehindMe(position, direction, others);
            }
            
            if (_victim != null)
            {
                BoomWBM = _victim.Position.Distance(position);
                _numberOfWBM++;
                if (Breakpoint != null)
                    Breakpoint(this, new BreakpointEventArgs(JardikBreakpointNames.Behind));
            }
            else
            {
                _numberOfWBM = 0;
            }

            if (_numberOfWBM > 5)
            {
                _killOrDie = true;
                if (Breakpoint != null)
                    Breakpoint(this, new BreakpointEventArgs(JardikBreakpointNames.KillMode));
            }

            if (_killOrDie)
            {
                numberOfKillSteps++;
                if (numberOfKillSteps > 5)
                {
                    numberOfKillSteps = 0;
                    _killOrDie = false;
                }
                _victimDirection = FindVictim(others.Find(x => x.Id == _victim.Id), round, position, direction, gameSurround, others, myPlanedMoves);
                if (_victimDirection != Move.Straight)
                {
                   
                    if (numberOfKillSteps == 1)
                    {

                        numberOfKillSteps++;
                        if (_victimDirection == Move.Right)
                        {
                            foreach (var walkSet in _rightSets)
                            {
                                walkSet.Evaluate(round, position, direction, gameSurround, others);
                            }
                            var best = _rightSets.OrderByDescending(x => x.Score).First();
                            if (best != null && best.Score > 7)
                            {
                                RemovePosibbleWalks(round,myPlanedMoves.Count,myPlanedMoves);
                                myPlanedMoves.AddRange(best.Walks.ToList());
                                foreach (var b in best.Walks)
                                {
                                    _victimArray[b.Position.X, b.Position.Y] = 100;
                                }
                            }
                        }
                        if (_victimDirection == Move.Left)
                        {
                            foreach (var walkSet in _leftSets)
                            {
                                walkSet.Evaluate(round, position, direction, gameSurround, others);
                            }
                            var best = _leftSets.OrderByDescending(x => x.Score).First();
                            if (best != null && best.Score > 5)
                            {
                                RemovePosibbleWalks(round, myPlanedMoves.Count, myPlanedMoves);
                                myPlanedMoves.AddRange(best.Walks.ToList());
                                foreach (var b in best.Walks)
                                {
                                    _victimArray[b.Position.X, b.Position.Y] = 100;
                                }
                            }
                        }

 
                        if (Breakpoint != null)
                            Breakpoint(this, new BreakpointEventArgs(JardikBreakpointNames.VictimFinded));
                    }

                }


            }

            if (myPlanedMoves.All(s => s.Round != round))
            {
                myPlanedMoves.AddRange(GetBestWalksToMe(round, position, direction, gameSurround, others));
            }
            else if (!_killOrDie)
            {
                RepairSteps(round + 1, myPlanedMoves, gameSurround, position, direction, others);
            }

            //myPlanedMoves.AddRange(GetVariant(round, position, direction, gameSurround, WalkSetType.Bingo,others));

            return myPlanedMoves;
        }

        private Move FindVictim(Other victim, int round, Position position, Direction direction, int[,] gameSurround, List<Other> others, List<Walk> myPlanedMoves)
        {
            _victimArray = (int [,])gameSurround.Clone();
            Direction right = direction;
            right= right.GetNewDirection(Move.Right);
            right=right.GetNewDirection(Move.Right);

            Direction left = direction;
            left =left.GetNewDirection(Move.Left);
            left = left.GetNewDirection(Move.Left);

            for (int w = 0; w < 15; w++)
            {
                var walk = myPlanedMoves.Find(x => x.Round == round - w);
                if (walk == null)
                {
                    return Move.Straight;
                }
                Position simulatePosition = walk.Position.Copy();
                for (int r = 0; r < 10; r++)
                {
                    simulatePosition.Update(right);
                    if (!simulatePosition.IsInCollission(100))
                    {
                        if (victim.Positions.Any(p=> p.Item1 > round -20 && p.Item2.IsSame(simulatePosition)))
                        {
                            return Move.Right;
                        }
                        else
                        {
                            _victimArray[simulatePosition.X, simulatePosition.Y] = 101;
                        }
                    }
                }

                simulatePosition = walk.Position.Copy();
                for (int r = 0; r < 10; r++)
                {
                    simulatePosition.Update(left);
                    if (!simulatePosition.IsInCollission(100))
                    {
                        if (victim.Positions.Any(p => p.Item1 > round - 20 && p.Item2.IsSame(simulatePosition)))
                        {
                            return Move.Left;
                        }
                        else
                        {
                            _victimArray[simulatePosition.X, simulatePosition.Y] = 101;
                        }
                    }
                }
            }

            return Move.Straight;

        }

        internal void RepairSteps(int round, List<Walk> planedWalks, int[,] gameSurround, Position position, Direction direction, List<Other> others)
        {
            var roundIndCollission = 0;
            foreach (var walk in planedWalks.Where(x => x.Round > round).OrderBy(y => y.Round))
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
                RemovePosibbleWalks(round, planedWalks.Where(x => x.Round >= roundIndCollission).ToList().Count + 3,
                    planedWalks);
                if (Breakpoint != null)
                    Breakpoint(this, new BreakpointEventArgs(JardikBreakpointNames.Collission));
            }

            if (roundIndCollission == 0)
            {
                var lastWalk = planedWalks.OrderByDescending(x => x.Round).First();

                foreach (var walkSet in _walkSets)
                {
                    walkSet.Evaluate(lastWalk.Round, lastWalk.Position, lastWalk.Direction, gameSurround, others);
                }
                var best = _walkSets.OrderByDescending(x => x.Score).FirstOrDefault();

                if (best.Score < 50)
                {
                    var bestScore = 0;
                    var bestCountToRemove = 20;

                    foreach (var w in planedWalks.Where(x => x.Round > round))
                    {
                        foreach (var walkSet in _walkSets)
                        {
                            walkSet.Evaluate(w.Round, w.Position, w.Direction, gameSurround, others);
                        }

                        var myPositionScore = _walkSets.OrderByDescending(x => x.Score).FirstOrDefault().Score;
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

        public List<Walk> GetBestWalksToMe(int round, Position position, Direction direction, int[,] gameSurround, List<Other> others)
        {
            var results = new List<WalkSetResult>();

            foreach (var walkSet in _walkSets)
            {
                walkSet.Evaluate(round, position, direction, gameSurround, others);
                results.Add(new WalkSetResult
                {
                    FirstScore = walkSet.Score,
                    FirstType = walkSet.Type(),
                    FirstWalks = walkSet.Walks.ToList(),
                    SimulateSurroundBefore = walkSet.SimulateGameSurround
                });
            }

            foreach (var r in results)
            {
                var lastWalk = r.LastWalk;
                if (lastWalk != null)
                {
                    foreach (var walkSet in _walkSets)
                    {
                        walkSet.Evaluate(lastWalk.Round + 1, lastWalk.Position, lastWalk.Direction,
                            r.SimulateSurroundBefore, others);
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
                        walkSet.Evaluate(lastWalk.Round + 1, lastWalk.Position, lastWalk.Direction,
                            r.SimulateSurroundBefore, others);
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

            Options =
                results.Where(x => x.SimulateSurroundAfter != null)
                    .Select(x => new DebugablePlayground(x.SimulateSurroundAfter, x.ToString()))
                    .ToList();

            var bestResult = results.OrderByDescending(x => x.TotalScore).First();

            if (bestResult.SimulateSurroundAfter != null)
            {
                BestOption = new DebugablePlayground(bestResult.SimulateSurroundAfter, bestResult.ToString());
            }
            

            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(JardikBreakpointNames.Planner));

            if (bestResult.TotalScore < 100 && ShitScore(position, gameSurround) < 15)
            {
                _safer.Evaluate(round, position, direction, gameSurround, others);
                return _safer.Walks;
            }

            if (bestResult != null) return bestResult.TotalWalks;
            return new List<Walk>();
        }

        public List<Walk> GetVariant(int round, Position position, Direction direction, int[,] gameSurround,WalkSetType type, List<Other> others)
        {
            var best = _walkSets.First(x => x.Type() == type);
            best.Evaluate(round, position, direction, gameSurround, others);
            return best.Walks;
        }
        #endregion

        #region Debug
        [ToDebug(typeof(WalkSetVizualizer))]
        public List<DebugablePlayground> Options { get; private set; }

        [ToDebug(typeof(WalkSetVizualizer))]
        public DebugablePlayground BestOption { get; private set; }

        [ToDebug(typeof(WalkSetVizualizer))]
        public DebugablePlayground BoomzVictimArray {
            get {return new DebugablePlayground(_victimArray,"Victims"); }
        }

        public event BreakpointEventHandler Breakpoint;

        [ToDebug]
        public double BoomWBM { get; set; }

        [ToDebug]
        public bool BoomKillingMode {get {return _killOrDie; } }

        [ToDebug]
        public Move BoomVictimDirection
        {
            get { return _victimDirection; }
        }

        #endregion

        #region Mask
        private void Mask(int[,] gameSurround, Position position, List<Other> others)
        {
            var aura = new List<Position>();
            var size = gameSurround.GetUpperBound(0) + 1;
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
                MaskFromPosition(gameSurround, o.Position, o.Direction, 100, aura, size, 4, o.Id);
            }
        }

        private void MaskFromPosition(int[,] gameSurround, Position position, Direction direction, int maskId,
            List<Position> aura, int size, int count, int id)
        {
            count--;
            if (!position.IsInCollission(size) &&
                (gameSurround[position.X, position.Y] == maskId || gameSurround[position.X, position.Y] == id))
            {
                foreach (var dir in direction.GetPossibleDirections())
                {
                    var p = position.Copy();
                    p.Update(dir);
                    if (!p.IsInCollission(size))
                    {
                        if (!aura.Any(a => a.X == p.X && a.Y == p.Y))
                        {
                            if (gameSurround[p.X, p.Y] == 0)
                            {
                                gameSurround[p.X, p.Y] = maskId;
                                if (count > 0)
                                {
                                    MaskFromPosition(gameSurround, p, dir, maskId, aura, size, count, id);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Tools
        private double ShitScore(Position p, int[,] gameSurround)
        {
            var dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
            var collissions = new List<int>();
            foreach (var d in dirs)
            {
                collissions.Add(ColideAt(p.Copy(), d, gameSurround));
            }
            var result = collissions.Average();
            return result;
        }

        private int ColideAt(Position p, Direction direction, int[,] gameSurround)
        {
            var i = 1;
            p.Update(direction);
            while (!p.IsInCollission(_max) && gameSurround[p.X, p.Y] == 0)
            {
                p.Update(direction);
                i++;
            }
            return i;
        }

        private Other VictimBehindMe (Position position, Direction direction, List<Other> others)
        {
            if (
                others.Any(
                    o =>
                        o.Direction == direction && (o.Position.Distance(position) < 10) &&
                        o.Position.IsOpposite(position, direction)))
            {

                var other = others.First(
                    o =>
                        o.Direction == direction && (o.Position.Distance(position) < 10) &&
                        o.Position.IsOpposite(position, direction));
                return other;
            }
            return null;
        }

        private void RemovePosibbleWalks(int round, int toRemoveCount, List<Walk> planedWalks)
        {
            var lastRound = planedWalks.Max(x => x.Round);
            var canDeleteCount = lastRound - round;
            if (canDeleteCount >= toRemoveCount)
            {
                planedWalks.RemoveAll(x => x.Round >= (lastRound - toRemoveCount));
            }
            else
            {
                planedWalks.RemoveAll(x => x.Round >= (lastRound - canDeleteCount));
            }
        }
        #endregion
    }

    internal class WalkSetResult
    {
        public int FirstScore { get; set; }
        public int SecondScore { get; set; }
        public int ThirdScore { get; set; }
        public WalkSetType FirstType { get; set; }
        public WalkSetType SecondType { get; set; }
        public WalkSetType ThirdType { get; set; }

        public Walk LastWalk
        {
            get
            {
                if (SecondWalks == null) return FirstWalks.OrderByDescending(w => w.Round).FirstOrDefault();
                if (ThirdWalks == null) return SecondWalks.OrderByDescending(w => w.Round).FirstOrDefault();
                return null;
            }
        }

        public List<Walk> FirstWalks { get; set; }
        public List<Walk> SecondWalks { get; set; }
        public List<Walk> ThirdWalks { get; set; }

        public List<Walk> TotalWalks
        {
            get
            {
                var result = FirstWalks;
                if (SecondWalks != null)
                {
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
            return String.Format("FirstStrategy {0}:{1} SecondStrategy {2}:{3} ThirdStrategy {4}:{5} Total: {6}",
                FirstType, FirstScore, SecondType, SecondScore, ThirdType, ThirdScore, TotalScore);
        }
    }
}
