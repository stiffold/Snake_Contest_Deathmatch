using System.Collections.Generic;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;

namespace SnakeDeathmatch.Players.Vazba
{
    // Strategy5 = Strategy2, ze které je vyseparovaná DangerZone a ona ji pouze využívá
    public class Strategy5 : IStrategy, IDebuggable
    {
        public const int MyWTF = 18;
        public const int OthersWTF = 5;

        public event BreakpointEventHandler Breakpoint;

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground PlaygroundForTrack { get; private set; }

        [ToDebug]
        public DangerZone DangerZone { get; private set; }

        private int _size;
        private Snakes _snakes;

        public Strategy5(int size)
        {
            _size = size;
            DangerZone = new DangerZone(size, maxDepth: OthersWTF);
        }

        public Move GetNextMove(IntPlayground playground, Snakes liveSnakes)
        {
            PlaygroundForTrack = playground;
            _snakes = liveSnakes;

            DangerZone.Update(liveSnakes);

            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy2Initialized));

            Snake me = liveSnakes.Me;

            Next next = me.GetNext(PlaygroundForTrack);

            var resultForLeft = next.Left.HasValue ? ExploreTrack(next.Left.Value, 1) : TrackExplorationResult.WorstPossibleResult;
            var resultForStraight = (resultForLeft != TrackExplorationResult.BestPossibleResult && next.Straight.HasValue) ? ExploreTrack(next.Straight.Value, 1) : TrackExplorationResult.WorstPossibleResult;
            var resultForRight = (resultForLeft != TrackExplorationResult.BestPossibleResult && resultForStraight != TrackExplorationResult.BestPossibleResult && next.Right.HasValue) ? ExploreTrack(next.Right.Value, 1) : TrackExplorationResult.WorstPossibleResult;

            //var resultForStraight = next.Straight.HasValue ? ExploreTrack(next.Straight.Value, 1) : TrackExplorationResult.WorstPossibleResult;
            //var resultForLeft = (resultForStraight != TrackExplorationResult.BestPossibleResult && next.Left.HasValue) ? ExploreTrack(next.Left.Value, 1) : TrackExplorationResult.WorstPossibleResult;
            //var resultForRight = (resultForLeft != TrackExplorationResult.BestPossibleResult && resultForStraight != TrackExplorationResult.BestPossibleResult && next.Right.HasValue) ? ExploreTrack(next.Right.Value, 1) : TrackExplorationResult.WorstPossibleResult;

            //var resultForRight = next.Right.HasValue ? ExploreTrack(next.Right.Value, 1) : TrackExplorationResult.WorstPossibleResult;
            //var resultForStraight = (resultForRight != TrackExplorationResult.BestPossibleResult && next.Straight.HasValue) ? ExploreTrack(next.Straight.Value, 1) : TrackExplorationResult.WorstPossibleResult;
            //var resultForLeft = (resultForRight != TrackExplorationResult.BestPossibleResult && resultForStraight != TrackExplorationResult.BestPossibleResult && next.Left.HasValue) ? ExploreTrack(next.Left.Value, 1) : TrackExplorationResult.WorstPossibleResult;

            if (resultForLeft >= resultForStraight && resultForLeft >= resultForRight) return Move.Left;
            if (resultForStraight >= resultForLeft && resultForStraight >= resultForRight) return Move.Straight;
            return Move.Right;
        }

        private TrackExplorationResult ExploreTrack(Snake me, int step)
        {
            if (step == MyWTF)
                return new TrackExplorationResult(step, aliveProbability: 1);

            if (step > 0) PlaygroundForTrack[me.X, me.Y] = PlayersIntArrayVisualizer.TrackId;

            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy2TrackChanged));

            Next next = me.GetNext(PlaygroundForTrack);

            TrackExplorationResult currentBestResult = new TrackExplorationResult(step, 0);

            if (next.Left.HasValue && currentBestResult < TrackExplorationResult.BestPossibleResult)
            {
                var justExploredResult = ExploreTrack(next.Left.Value, step + 1);
                if (justExploredResult > currentBestResult)
                    currentBestResult = justExploredResult;
            }

            if (next.Straight.HasValue && currentBestResult < TrackExplorationResult.BestPossibleResult)
            {
                var justExploredResult = ExploreTrack(next.Straight.Value, step + 1);
                if (justExploredResult > currentBestResult)
                    currentBestResult = justExploredResult;
            }

            if (next.Right.HasValue && currentBestResult < TrackExplorationResult.BestPossibleResult)
            {
                var justExploredResult = ExploreTrack(next.Right.Value, step + 1);
                if (justExploredResult > currentBestResult)
                    currentBestResult = justExploredResult;
            }

            if (step > 0) PlaygroundForTrack[me.X, me.Y] = 0;

            //decimal aliveProbabilityForCurrentStep = (step > OthersWTF) ? 1 : 1 - PlaygroundForStep[step][me.X, me.Y];
            decimal aliveProbabilityForCurrentStep = (step > OthersWTF) ? 1 : 1 - DangerZone.PlaygroundForStep[OthersWTF][me.X, me.Y];

            return new TrackExplorationResult(currentBestResult.Depth, currentBestResult.AliveProbability * aliveProbabilityForCurrentStep);
        }
    }
}
