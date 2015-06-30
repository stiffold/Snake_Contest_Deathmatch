using System.Collections.Generic;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;
using SnakeDeathmatch.Players.Vazba.Helper;

namespace SnakeDeathmatch.Players.Vazba.PlaygroundAnalysis
{
    public class DangerZone
    {
        private int _size;
        private int _maxDepth;

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground Playground { get; private set; }

        /// <summary>
        /// Herní hřiště (pro každý krok jedno) s pravděpodobnostmi obsazenosti jednotlivých políček v daném kroku.
        /// </summary>
        [ToDebug(typeof(ZeroToOneDecimalArrayVisualizer))]
        public List<DecimalPlayground> PlaygroundForStep { get; private set; }

        public DangerZone(int size, int maxDepth)
        {
            _size = size;
            _maxDepth = maxDepth;
            Playground = new IntPlayground(new int[size, size]);

            PlaygroundForStep = new List<DecimalPlayground>();
            for (int step = 0; step <= _maxDepth; step++)
            {
                PlaygroundForStep.Add(new DecimalPlayground(size));
            }
        }

        public void Update(Snakes snakes)
        {
            // aktualizace herního pole podle aktuálních pozic hadů
            foreach (Snake snake in snakes.OthersAndMe)
            {
                Playground[snake.X, snake.Y] = snake.Id;
            }            

            // inicializace obsazenosti polí podle aktuálního herního hřiště
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    decimal value = (Playground[x, y] != 0) ? 1 : 0;

                    foreach (DecimalPlayground playground in PlaygroundForStep)
                    {
                        playground[x, y] = value;
                    }
                }
            }

            // inicializace pravděpodobnosti obsazenosti polí v budoucnu
            if (snakes.IsInitialized)
            {
                foreach (Snake snake in snakes.Others)
                {
                    FillPlaygroundsWithDeathProbabilityForSnake(snake, step: 0, deathProbability: 1);
                }
            }
        }

        private void FillPlaygroundsWithDeathProbabilityForSnake(Snake snake, int step, decimal deathProbability)
        {
            if (step == _maxDepth)
                return;

            if (step > 0) Playground[snake.X, snake.Y] = snake.Id;

            AddDeathProbabilityForTheStepAndUpwards(deathProbability, snake.X, snake.Y, step);

            Next next = snake.GetNext(Playground);

            decimal freeWays = (next.Left.HasValue ? 1 : 0) + (next.Straight.HasValue ? 1 : 0) + (next.Right.HasValue ? 1 : 0);

            if (next.Left.HasValue) FillPlaygroundsWithDeathProbabilityForSnake(next.Left.Value, step + 1, deathProbability / freeWays);
            if (next.Straight.HasValue) FillPlaygroundsWithDeathProbabilityForSnake(next.Straight.Value, step + 1, deathProbability / freeWays);
            if (next.Right.HasValue) FillPlaygroundsWithDeathProbabilityForSnake(next.Right.Value, step + 1, deathProbability / freeWays);

            if (step > 0) Playground[snake.X, snake.Y] = 0;
        }

        private void AddDeathProbabilityForTheStepAndUpwards(decimal deathProbability, int x, int y, int targetStep)
        {
            for (int currentStep = targetStep; currentStep < PlaygroundForStep.Count; currentStep++)
            {
                PlaygroundForStep[currentStep][x, y] += deathProbability;
            }
        }
    }
}
