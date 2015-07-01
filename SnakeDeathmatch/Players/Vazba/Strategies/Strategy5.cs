using System.Collections.Generic;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;
using SnakeDeathmatch.Players.Vazba.Helper;
using SnakeDeathmatch.Players.Vazba.PlaygroundAnalysis;
using System;

namespace SnakeDeathmatch.Players.Vazba.Strategies
{
    // Strategy5 = Recursion(18) with DangerZone and DeathField
    public class Strategy5 : IStrategy, IDebuggable
    {
        #region private class ExplorationResult
        private class ExplorationResult
        {
            public int Depth;
            public decimal AliveProbability;

            public ExplorationResult(int depth, decimal aliveProbability)
            {
                Depth = depth;
                AliveProbability = aliveProbability;
            }

            public override string ToString()
            {
                return string.Format("Depth: {0}, AliveProbability: {1:0.0000000000}", Depth, AliveProbability);
            }

            public int CompareTo(ExplorationResult other)
            {
                if (Depth > other.Depth)
                    return 1;

                if (Depth == other.Depth && AliveProbability > other.AliveProbability)
                    return 1;

                if (Depth == other.Depth && AliveProbability == other.AliveProbability)
                    return 0;

                return -1;
            }

            public static bool operator <(ExplorationResult result1, ExplorationResult result2)
            {
                return result1.CompareTo(result2) < 0;
            }

            public static bool operator >(ExplorationResult result1, ExplorationResult result2)
            {
                return result1.CompareTo(result2) > 0;
            }

            public static bool operator >=(ExplorationResult result1, ExplorationResult result2)
            {
                return result1.CompareTo(result2) >= 0;
            }

            public static bool operator <=(ExplorationResult result1, ExplorationResult result2)
            {
                return result1.CompareTo(result2) <= 0;
            }

            public static bool operator ==(ExplorationResult result1, ExplorationResult result2)
            {
                return result1.Equals(result2);
            }

            public static bool operator !=(ExplorationResult result1, ExplorationResult result2)
            {
                return !result1.Equals(result2);
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                {
                    return false;
                }

                return CompareTo((ExplorationResult)obj) == 0;
            }

            public override int GetHashCode()
            {
                return Depth.GetHashCode() * 17 + AliveProbability.GetHashCode();
            }

            public static ExplorationResult BestPossibleResult { get { return new ExplorationResult(DeathField.Infinity, 1); } }
            public static ExplorationResult WorstPossibleResult { get { return new ExplorationResult(0, 0); } }
        }
        #endregion

        public const int MyWTF = 15;
        public const int OthersWTF = 5;

        public event BreakpointEventHandler Breakpoint;

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground Playground { get; private set; }

        public Analysis Analysis { get; private set; }

        private int _size;
        private Move _priorityMove;
        private Snakes _snakes;
        private int _absoluteStep;

        public Strategy5(Analysis analysis, Move priorityMove)
        {
            _size = analysis.PlaygroundSize;
            _priorityMove = priorityMove;
            Analysis = analysis;
        }

        public Move GetNextMove(IntPlayground playground, Snakes liveSnakes)
        {
            Playground = playground;
            _snakes = liveSnakes;
            _absoluteStep++;

            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy5Initialized));

            if (Breakpoint != null && _absoluteStep % 10 == 0)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy5StopEvery10Steps));

            Snake me = liveSnakes.Me;

            Next next = me.GetNext(Playground);

            if (_priorityMove == Move.Left)
            {
                // try find BestPossibleResult
                var resultForLeft = next.Left.HasValue ? ExploreTrack(next.Left.Value, 1) : ExplorationResult.WorstPossibleResult;
                if (resultForLeft == ExplorationResult.BestPossibleResult)
                    return Move.Left;

                var resultForStraight = next.Straight.HasValue ? ExploreTrack(next.Straight.Value, 1) : ExplorationResult.WorstPossibleResult;
                if (resultForStraight == ExplorationResult.BestPossibleResult)
                    return Move.Straight;

                var resultForRight = next.Right.HasValue ? ExploreTrack(next.Right.Value, 1) : ExplorationResult.WorstPossibleResult;
                if (resultForRight == ExplorationResult.BestPossibleResult)
                    return Move.Right;

                // no direction has BestPossibleResult, so select best from these not perfect results
                if (resultForLeft >= resultForStraight && resultForLeft >= resultForRight)
                    return Move.Left;

                if (resultForStraight >= resultForLeft && resultForStraight >= resultForRight)
                    return Move.Straight;

                return Move.Right;
            }
            else if (_priorityMove == Move.Right)
            {
                // try find BestPossibleResult

                var resultForRight = next.Right.HasValue ? ExploreTrack(next.Right.Value, 1) : ExplorationResult.WorstPossibleResult;
                if (resultForRight == ExplorationResult.BestPossibleResult)
                    return Move.Right;

                var resultForStraight = next.Straight.HasValue ? ExploreTrack(next.Straight.Value, 1) : ExplorationResult.WorstPossibleResult;
                if (resultForStraight == ExplorationResult.BestPossibleResult)
                    return Move.Straight;

                var resultForLeft = next.Left.HasValue ? ExploreTrack(next.Left.Value, 1) : ExplorationResult.WorstPossibleResult;
                if (resultForLeft == ExplorationResult.BestPossibleResult)
                    return Move.Left;

                // no direction has BestPossibleResult, so select best from these not perfect results
                if (resultForRight >= resultForStraight && resultForRight >= resultForLeft)
                    return Move.Right;

                if (resultForStraight >= resultForLeft && resultForStraight >= resultForRight)
                    return Move.Straight;

                return Move.Left;
            }
            throw new NotImplementedException(string.Format("PriorityMove {0} is not implemented yet.", _priorityMove));
        }

        private ExplorationResult ExploreTrack(Snake me, int step)
        {
            if (step == MyWTF)
                return ExplorationResult.BestPossibleResult;

            if (step > 0)
                Playground[me.X, me.Y] = PlayersIntArrayVisualizer.TrackId;

            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy5ExploreTrackRecursionStep));

            Next next = me.GetNext(Playground);

            ExplorationResult currentBestResult = new ExplorationResult(step, 0);

            if (_priorityMove == Move.Left)
            {
                currentBestResult = ExploreMove(Move.Left, me, step, next, currentBestResult);
                currentBestResult = ExploreMove(Move.Straight, me, step, next, currentBestResult);
                currentBestResult = ExploreMove(Move.Right, me, step, next, currentBestResult);
            }
            else if (_priorityMove == Move.Straight)
            {
                currentBestResult = ExploreMove(Move.Straight, me, step, next, currentBestResult);
                currentBestResult = ExploreMove(Move.Left, me, step, next, currentBestResult);
                currentBestResult = ExploreMove(Move.Right, me, step, next, currentBestResult);
            }
            else if (_priorityMove == Move.Right)
            {
                currentBestResult = ExploreMove(Move.Right, me, step, next, currentBestResult);
                currentBestResult = ExploreMove(Move.Straight, me, step, next, currentBestResult);
                currentBestResult = ExploreMove(Move.Left, me, step, next, currentBestResult);
            }

            if (step > 0)
                Playground[me.X, me.Y] = 0;

            //decimal aliveProbabilityForCurrentStep = (step > OthersWTF) ? 1 : 1 - PlaygroundForStep[step][me.X, me.Y];
            decimal aliveProbabilityForCurrentStep = (step > OthersWTF) ? 1 : 1 - Analysis.DangerZone.PlaygroundForStep[OthersWTF][me.X, me.Y];

            return new ExplorationResult(currentBestResult.Depth, currentBestResult.AliveProbability * aliveProbabilityForCurrentStep);
        }

        private ExplorationResult ExploreMove(Move move, Snake me, int step, Next next, ExplorationResult currentBestResult)
        {
            Snake? nextSnake;
            if (move == Move.Left)
                nextSnake = next.Left;
            else if (move == Move.Straight)
                nextSnake = next.Straight;
            else
                nextSnake = next.Right;

            if (nextSnake.HasValue && currentBestResult < ExplorationResult.BestPossibleResult)
            {
                ExplorationResult justExploredResult;

                int stepsToDeath = Analysis.DeathField.DeathPlaygroundByDirection[nextSnake.Value.Direction.Value][me.X, me.Y];
                if (stepsToDeath != DeathField.Infinity)
                    justExploredResult = new ExplorationResult(step + stepsToDeath, aliveProbability: 1);
                else
                    justExploredResult = ExploreTrack(nextSnake.Value, step + 1);

                if (justExploredResult > currentBestResult)
                    currentBestResult = justExploredResult;
            }
            return currentBestResult;
        }
    }
}
