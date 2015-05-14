using System.Collections.Generic;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Strategy2 : IStrategy
    {
        public const int MyWTF = 18;
        public const int OthersWTF = 5;

        [ToDebug]
        public IntPlayground PlaygroundForTrack { get; private set; }

        /// <summary>Herní hřiště (pro každý krok jedno) s pravděpodobnostmi obsazenosti jednotlivých políček v daném kroku.</summary>
        [ToDebug]
        public List<DecimalPlayground> PlaygroundForStep { get; private set; }

        private int _size;
        private Snakes _snakes;

        public Move GetNextMove(IntPlayground playground, Snakes liveSnakes)
        {
            PlaygroundForTrack = playground;
            _size = playground.Size;
            _snakes = liveSnakes;

            CreateAndInitPlaygroundsForAllSteps();

            Snake me = liveSnakes.Me;

            Next next = me.GetNext(PlaygroundForTrack);

            var resultForLeft = next.Left.HasValue ? ExploreTrack(next.Left.Value, 1) : TrackExplorationResult.WorstPossibleResult;
            var resultForStraight = (resultForLeft != TrackExplorationResult.BestPossibleResult && next.Straight.HasValue) ? ExploreTrack(next.Straight.Value, 1) : TrackExplorationResult.WorstPossibleResult;
            var resultForRight = (resultForLeft != TrackExplorationResult.BestPossibleResult && resultForStraight != TrackExplorationResult.BestPossibleResult && next.Right.HasValue) ? ExploreTrack(next.Right.Value, 1) : TrackExplorationResult.WorstPossibleResult;

            //var resultForStraight = next.Straight.HasValue ? ExploreTrack(next.Straight.Value, 1) : TrackExplorationResult.WorstPossibleResult;
            //var resultForLeft = (resultForStraight != TrackExplorationResult.BestPossibleResult && next.Left.HasValue) ? ExploreTrack(next.Left.Value, 1) : TrackExplorationResult.WorstPossibleResult;
            //var resultForRight = (resultForLeft != TrackExplorationResult.BestPossibleResult && resultForStraight != TrackExplorationResult.BestPossibleResult && next.Right.HasValue) ? ExploreTrack(next.Right.Value, 1) : TrackExplorationResult.WorstPossibleResult;

            if (resultForLeft >= resultForStraight && resultForLeft >= resultForRight) return Move.Left;
            if (resultForStraight >= resultForLeft && resultForStraight >= resultForRight) return Move.Straight;
            return Move.Right;
        }

        private void CreateAndInitPlaygroundsForAllSteps()
        {
            // vytvoření polí
            PlaygroundForStep = new List<DecimalPlayground>();
            for (int step = 0; step <= OthersWTF; step++)
            {
                PlaygroundForStep.Add(new DecimalPlayground(_size));
            }

            // inicializace obsazenosti polí podle aktuálního herního hřiště
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    decimal value = (PlaygroundForTrack[x, y] != 0) ? 1 : 0;

                    foreach (DecimalPlayground playground in PlaygroundForStep)
                    {
                        playground[x, y] = value;
                    }
                }
            }

            // inicializace pravděpodobnosti obsazenosti polí v budoucnu
            foreach (Snake snake in _snakes)
            {
                FillPlaygroundsWithDeathProbabilityForSnake(snake, step: 0, deathProbability: 1);
            }
        }

        private void FillPlaygroundsWithDeathProbabilityForSnake(Snake snake, int step, decimal deathProbability)
        {
            if (step == OthersWTF)
                return;

            if (step > 0) PlaygroundForTrack[snake.X, snake.Y] = snake.Id;

            AddDeathProbabilityForTheStepAndUpwards(deathProbability, snake.X, snake.Y, step);

            Next next = snake.GetNext(PlaygroundForTrack);

            decimal freeWays = (next.Left.HasValue ? 1 : 0) + (next.Straight.HasValue ? 1 : 0) + (next.Right.HasValue ? 1 : 0);

            if (next.Left.HasValue) FillPlaygroundsWithDeathProbabilityForSnake(next.Left.Value, step + 1, deathProbability / freeWays);
            if (next.Straight.HasValue) FillPlaygroundsWithDeathProbabilityForSnake(next.Straight.Value, step + 1, deathProbability / freeWays);
            if (next.Right.HasValue) FillPlaygroundsWithDeathProbabilityForSnake(next.Right.Value, step + 1, deathProbability / freeWays);

            if (step > 0) PlaygroundForTrack[snake.X, snake.Y] = 0;
        }

        private void AddDeathProbabilityForTheStepAndUpwards(decimal deathProbability, int x, int y, int targetStep)
        {
            for (int currentStep = targetStep; currentStep < PlaygroundForStep.Count; currentStep++)
            {
                PlaygroundForStep[currentStep][x, y] += deathProbability;
            }
        }

        private TrackExplorationResult ExploreTrack(Snake me, int step)
        {
            if (step == MyWTF)
                return new TrackExplorationResult(step, aliveProbability: 1);

            if (step > 0) PlaygroundForTrack[me.X, me.Y] = me.Id;

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
            decimal aliveProbabilityForCurrentStep = (step > OthersWTF) ? 1 : 1 - PlaygroundForStep[OthersWTF][me.X, me.Y];

            return new TrackExplorationResult(currentBestResult.Depth, currentBestResult.AliveProbability * aliveProbabilityForCurrentStep);
        }
    }

    public class TrackExplorationResult
    {
        public int Depth;
        public decimal AliveProbability;

        public TrackExplorationResult(int depth, decimal aliveProbability)
        {
            Depth = depth;
            AliveProbability = aliveProbability;
        }

        public override string ToString()
        {
            return string.Format("Depth: {0}, AliveProbability: {1:0.0000000000}", Depth, AliveProbability);
        }

        public int CompareTo(TrackExplorationResult other)
        {
            if (Depth > other.Depth)
                return 1;

            if (Depth == other.Depth && AliveProbability > other.AliveProbability)
                return 1;

            if (Depth == other.Depth && AliveProbability == other.AliveProbability)
                return 0;

            return -1;
        }

        public static bool operator <(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return result1.CompareTo(result2) < 0;
        }

        public static bool operator >(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return result1.CompareTo(result2) > 0;
        }

        public static bool operator >=(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return result1.CompareTo(result2) >= 0;
        }

        public static bool operator <=(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return result1.CompareTo(result2) <= 0;
        }

        public static bool operator ==(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return result1.Equals(result2);
        }

        public static bool operator !=(TrackExplorationResult result1, TrackExplorationResult result2)
        {
            return !result1.Equals(result2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            
            return CompareTo((TrackExplorationResult)obj) == 0;
        }

        public override int GetHashCode()
        {
            return Depth.GetHashCode() * 17 + AliveProbability.GetHashCode();
        }

        public static TrackExplorationResult BestPossibleResult { get { return new TrackExplorationResult(Strategy2.MyWTF, 1); } }
        public static TrackExplorationResult WorstPossibleResult { get { return new TrackExplorationResult(0, 0); } }
    }
}
