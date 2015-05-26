using System;
using System.Collections.Generic;
using System.Linq;
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
        private Other _victim = new Other();
        private int _numberOfWBM;
        private Move _victimDirection = Move.Straight;
        private int[,] _victimArray;
        private int numberOfKillSteps;

        #region counstructor

        public Planner(int max, int myId)
        {
            BoomKillingMode = false;
            _max = max;
            _myId = myId;
            _victimArray = new int[max, max];
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

            _walkSets.Add(new Love(_ch, _myId));

            _walkSets.Add(new Right180Bigger(_ch, _myId));
            _walkSets.Add(new Left180Bigger(_ch, _myId));

            _walkSets.Add(new LeftLightGap(_ch, _myId));
            _walkSets.Add(new LeftLightLightGap(_ch, _myId));
            _walkSets.Add(new RightLightGap(_ch, _myId));
            _walkSets.Add(new LeftLightLightGap(_ch, _myId));

            //_walkSets.Add(new RollLeft(_ch, _myId));
            //_walkSets.Add(new RollRight(_ch, _myId));
            //_walkSets.Add(new SquareLeft(_ch, _myId));
            //_walkSets.Add(new SquareRight(_ch, _myId));
            //_walkSets.Add(new RollRight(_ch, _myId));
            //_walkSets.Add(new Snaker(_ch, _myId));
            //_walkSets.Add(new Quaker(_ch, _myId));
            //_walkSets.Add(new FunkyTerror(_ch, _myId));
            //_walkSets.Add(new Roll100(_ch, _myId));
            //_walkSets.Add(new Raper(_ch, _myId));
            //_walkSets.Add(new Bingo(_ch, _myId));
            //_walkSets.Add(new Randomer(_ch, _myId));
            //_walkSets.Add(new StrikeLeft(_ch, _myId));
            //_walkSets.Add(new StrikeRight(_ch, _myId));

            _leftSets.Add(new Left90(_ch, _myId));
            _leftSets.Add(new Left125(_ch, _myId));
            _leftSets.Add(new LeftGap(_ch, _myId));
            _leftSets.Add(new LeftLightGap(_ch, _myId));
            _leftSets.Add(new LeftLightLightGap(_ch, _myId));

            _rightSets.Add(new Right90(_ch, _myId));
            _rightSets.Add(new Right125(_ch, _myId));
            _rightSets.Add(new RightGap(_ch, _myId));
            _rightSets.Add(new RightLightGap(_ch, _myId));
            _rightSets.Add(new LeftLightLightGap(_ch, _myId));
        }

        #endregion

        #region Main Func

        internal List<Walk> UpdateMoves(List<Walk> myPlanedMoves, int round, Position position, Direction direction,
            int[,] gameSurround, List<Other> others)
        {
            if (round == 1)
            {
                myPlanedMoves.AddRange(Killer(round, position, direction, gameSurround, others));
            }

            if (!BoomKillingMode)
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

            if (_numberOfWBM > 3)
            {
                BoomKillingMode = true;
                if (Breakpoint != null)
                    Breakpoint(this, new BreakpointEventArgs(JardikBreakpointNames.KillMode));
            }

            if (BoomKillingMode)
            {
                numberOfKillSteps++;
                if (numberOfKillSteps > 5)
                {
                    numberOfKillSteps = 0;
                    BoomKillingMode = false;
                }
                _victimDirection = FindVictim(others.Find(x => x.Id == _victim.Id), round, position, direction,
                    gameSurround, others, myPlanedMoves);
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
                                RemovePosibbleWalks(round, myPlanedMoves.Count, myPlanedMoves);
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
                            if (best != null && best.Score > 7)
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

            Mask(gameSurround, position, others, 3);

            if (myPlanedMoves.All(s => s.Round != round))
            {
                myPlanedMoves.AddRange(GetBestWalksToMe(round, position, direction, gameSurround, others));
            }
            else if (!BoomKillingMode)
            {
                RepairSteps(round + 1, myPlanedMoves, gameSurround, position, direction, others);
            }

            //myPlanedMoves.AddRange(GetVariant(round, position, direction, gameSurround, WalkSetType.Bingo,others));

            return myPlanedMoves;
        }

        private IEnumerable<Walk> Killer(int round, Position position, Direction direction, int[,] gameSurround,
            List<Other> others)
        {
            var killer = new Killer(_ch, _myId);
            killer.Evaluate(round, position, direction, gameSurround, others);

            if (killer.SimulateGameSurround != null)
            {
                KillerOption = new DebugablePlayground(killer.SimulateGameSurround, "killer");
            }
            return killer.Walks;
        }

        private Move FindVictim(Other victim, int round, Position position, Direction direction, int[,] gameSurround,
            List<Other> others, List<Walk> myPlanedMoves)
        {
            if (victim == null) return Move.Straight;
            Mask(gameSurround, position, others, 2);
            _victimArray = (int[,]) gameSurround.Clone();
            var right = direction;
            right = right.GetNewDirection(Move.Right);
            right = right.GetNewDirection(Move.Right);

            var left = direction;
            left = left.GetNewDirection(Move.Left);
            left = left.GetNewDirection(Move.Left);

            for (var w = 0; w < 20; w++)
            {
                var walk = myPlanedMoves.Find(x => x.Round == round - w);
                if (walk == null)
                {
                    return Move.Straight;
                }
                var simulatePosition = walk.Position.Copy();
                for (var r = 0; r < 10; r++)
                {
                    simulatePosition.Update(right);
                    if (!simulatePosition.IsInCollission(100))
                    {
                        if (victim.Position.AuraContains(simulatePosition))
                        {
                            return Move.Right;
                        }
                        if (gameSurround[simulatePosition.X, simulatePosition.Y] != 0)
                        {
                            break;
                        }
                        _victimArray[simulatePosition.X, simulatePosition.Y] = 101;
                    }
                }

                simulatePosition = walk.Position.Copy();
                for (var r = 0; r < 10; r++)
                {
                    simulatePosition.Update(left);
                    if (!simulatePosition.IsInCollission(100))
                    {
                        if (victim.Position.AuraContains(simulatePosition))
                        {
                            return Move.Left;
                        }
                        if (gameSurround[simulatePosition.X, simulatePosition.Y] != 0)
                        {
                            break;
                        }
                        _victimArray[simulatePosition.X, simulatePosition.Y] = 101;
                    }
                }
            }

            return Move.Straight;
        }

        private void Unmask(int[,] gameSurround)
        {
            var size = gameSurround.GetUpperBound(0) + 1;
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    if (gameSurround[x, y] == 100)
                    {
                        gameSurround[x, y] = 0;
                    }
                }
            }
        }

        internal void RepairSteps(int round, List<Walk> planedWalks, int[,] gameSurround, Position position,
            Direction direction, List<Other> others)
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

        public List<Walk> GetBestWalksToMe(int round, Position position, Direction direction, int[,] gameSurround,
            List<Other> others)
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
                        if (r.FourScore < walkSet.Score)
                        {
                            r.FourScore = walkSet.Score;
                            r.TotalScore = r.FirstScore + r.SecondScore + r.ThirdScore + r.FourScore;
                            r.FourType = walkSet.Type();
                            r.FourWalks = walkSet.Walks.ToList();
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
                        if (r.FiveScore < walkSet.Score)
                        {
                            r.FiveScore = walkSet.Score;
                            r.TotalScore = r.FirstScore + r.SecondScore + r.ThirdScore + r.FourScore + r.FiveScore;
                            r.FiveType = walkSet.Type();
                            r.FiveWalks = walkSet.Walks.ToList();
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

        public List<Walk> GetVariant(int round, Position position, Direction direction, int[,] gameSurround,
            WalkSetType type, List<Other> others)
        {
            var best = _walkSets.First(x => x.Type() == type);
            best.Evaluate(round, position, direction, gameSurround, others);
            return best.Walks;
        }

        #endregion

        #region Debug

        [ToDebug(typeof (WalkSetVizualizer))]
        public List<DebugablePlayground> Options { get; private set; }

        [ToDebug(typeof (WalkSetVizualizer))]
        public DebugablePlayground BestOption { get; private set; }

        [ToDebug(typeof (WalkSetVizualizer))]
        public DebugablePlayground KillerOption { get; private set; }

        [ToDebug(typeof (WalkSetVizualizer))]
        public DebugablePlayground BoomzVictimArray
        {
            get { return new DebugablePlayground(_victimArray, "Victims"); }
        }

        public event BreakpointEventHandler Breakpoint;

        [ToDebug]
        public double BoomWBM { get; set; }

        [ToDebug]
        public bool BoomKillingMode { get; private set; }

        [ToDebug]
        public Move BoomVictimDirection
        {
            get { return _victimDirection; }
        }

        #endregion

        #region Mask

        private void Mask(int[,] gameSurround, Position position, List<Other> others, int deep)
        {
            var aura = new List<Position>();
            var size = gameSurround.GetUpperBound(0) + 1;
            aura.Add(position);

            foreach (var dir in Enum.GetValues(typeof (Direction)))
            {
                var p = position.Copy();
                p.Update((Direction) dir);
                if (!p.IsInCollission(size))
                {
                    aura.Add(p);
                    foreach (var dir2 in Enum.GetValues(typeof (Direction)))
                    {
                        var p2 = p.Copy();
                        p2.Update((Direction) dir2);
                        if (!p2.IsInCollission(size))
                        {
                            aura.Add(p2);
                        }
                    }
                }
            }

            foreach (var o in others)
            {
                MaskFromPosition(gameSurround, o.Position, o.Direction, 100, aura, size, deep, o.Id);
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
            var dirs = Enum.GetValues(typeof (Direction)).Cast<Direction>().ToList();
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

        private Other VictimBehindMe(Position position, Direction direction, List<Other> others)
        {
            if (
                others.Any(
                    o =>
                        o.Direction == direction && (o.Position.Distance(position) < 20) &&
                        o.Position.IsOpposite(position, direction)))
            {
                var other = others.First(
                    o =>
                        o.Direction == direction && (o.Position.Distance(position) < 20) &&
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
        public int FourScore { get; set; }
        public int FiveScore { get; set; }
        public WalkSetType FirstType { get; set; }
        public WalkSetType SecondType { get; set; }
        public WalkSetType ThirdType { get; set; }
        public WalkSetType FourType { get; set; }
        public WalkSetType FiveType { get; set; }

        public Walk LastWalk
        {
            get
            {
                if (SecondWalks == null) return FirstWalks.OrderByDescending(w => w.Round).FirstOrDefault();
                if (ThirdWalks == null) return SecondWalks.OrderByDescending(w => w.Round).FirstOrDefault();
                if (FourWalks == null) return ThirdWalks.OrderByDescending(w => w.Round).FirstOrDefault();
                if (FiveWalks == null) return FourWalks.OrderByDescending(w => w.Round).FirstOrDefault();
                return null;
            }
        }

        public List<Walk> FirstWalks { get; set; }
        public List<Walk> SecondWalks { get; set; }
        public List<Walk> ThirdWalks { get; set; }
        public List<Walk> FourWalks { get; set; }
        public List<Walk> FiveWalks { get; set; }

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
                        if (FourWalks != null)
                        {
                            result.AddRange(FourWalks);
                            if (FiveWalks != null)
                            {
                                result.AddRange(FiveWalks);
                            }
                        }
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
            return String.Format("{0}:{1}||{2}:{3}||{4}:{5}||{6}:{7}||{8}:{9}||Total: {10}",
                FirstType, FirstScore, SecondType, SecondScore, ThirdType, ThirdScore,FourType, FourScore, FiveType, FiveScore ,TotalScore);
        }
    }
}
