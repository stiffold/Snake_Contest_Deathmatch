using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Players.Vazba.Debug;
using SnakeDeathmatch.Players.Vazba.Helper;

namespace SnakeDeathmatch.Players.Vazba.PlaygroundAnalysis
{
    public class DeathField
    {
        private struct Vector
        {
            public int X;
            public int Y;
            public Direction Direction;
            public Vector(int x, int y, Direction direction) { X = x; Y = y; Direction = direction; }
            public override string ToString() { return string.Format("[{0},{1},{2}]", X, Y, Direction); }
        }

        private Queue<Vector> _queue;
        private int _size;
        private IEnumerable<Direction> _diagonalDirections = new[] { Direction.TopRight, Direction.BottomRight, Direction.BottomLeft, Direction.TopLeft };
        private IEnumerable<Direction> _allDirections = new[] { Direction.Top, Direction.TopRight, Direction.Right, Direction.BottomRight,
                                                                Direction.Bottom, Direction.BottomLeft, Direction.Left, Direction.TopLeft };
        public static int Infinity { get { return DeathIntArrayVisualizer.InfinityId; } }

        public IDictionary<Direction, IntPlayground> DeathPlaygroundByDirection;

        #region DeathPlaygrounds

        [ToDebug(typeof(DeathIntArrayVisualizer))]
        public IntPlayground Death_1_Top { get; private set; }

        [ToDebug(typeof(DeathIntArrayVisualizer))]
        public IntPlayground Death_2_TopRight { get; private set; }

        [ToDebug(typeof(DeathIntArrayVisualizer))]
        public IntPlayground Death_3_Right { get; private set; }

        [ToDebug(typeof(DeathIntArrayVisualizer))]
        public IntPlayground Death_4_BottomRight { get; private set; }

        [ToDebug(typeof(DeathIntArrayVisualizer))]
        public IntPlayground Death_5_Bottom { get; private set; }

        [ToDebug(typeof(DeathIntArrayVisualizer))]
        public IntPlayground Death_6_BottomLeft { get; private set; }

        [ToDebug(typeof(DeathIntArrayVisualizer))]
        public IntPlayground Death_7_Left { get; private set; }

        [ToDebug(typeof(DeathIntArrayVisualizer))]
        public IntPlayground Death_8_TopLeft { get; private set; }

        #endregion

        public DeathField(int size)
        {
            _size = size;
            _queue = new Queue<Vector>();

            int infinity = Infinity;
            var array = new int[_size, _size];
            for (int x = 0; x < _size; x++)
                for (int y = 0; y < _size; y++)
                    array[x, y] = infinity;

            var intPlayground = new IntPlayground(array);

            Death_1_Top = intPlayground;
            Death_2_TopRight = intPlayground.Clone();
            Death_3_Right = intPlayground.Clone();
            Death_4_BottomRight = intPlayground.Clone();
            Death_5_Bottom = intPlayground.Clone();
            Death_6_BottomLeft = intPlayground.Clone();
            Death_7_Left = intPlayground.Clone();
            Death_8_TopLeft = intPlayground.Clone();

            DeathPlaygroundByDirection = new Dictionary<Direction, IntPlayground>();
            DeathPlaygroundByDirection[Direction.Top] = Death_1_Top;
            DeathPlaygroundByDirection[Direction.TopRight] = Death_2_TopRight;
            DeathPlaygroundByDirection[Direction.Right] = Death_3_Right;
            DeathPlaygroundByDirection[Direction.BottomRight] = Death_4_BottomRight;
            DeathPlaygroundByDirection[Direction.Bottom] = Death_5_Bottom;
            DeathPlaygroundByDirection[Direction.BottomLeft] = Death_6_BottomLeft;
            DeathPlaygroundByDirection[Direction.Left] = Death_7_Left;
            DeathPlaygroundByDirection[Direction.TopLeft] = Death_8_TopLeft;

            InitializeBorders();
        }

        private void InitializeBorders()
        {
            int N0 = _size - 1;
            int N1 = _size - 2;
            int N2 = _size - 3;

            for (int i = 0; i < _size; i++)
            {
                Death_1_Top[i, 0] = 1;
                Death_1_Top[i, 1] = 2;
                Death_2_TopRight[i, 0] = 1;
                Death_2_TopRight[N0, i] = 1;
                Death_3_Right[N0, i] = 1;
                Death_3_Right[N1, i] = 2;
                Death_4_BottomRight[N0, i] = 1;
                Death_4_BottomRight[i, N0] = 1;
                Death_5_Bottom[i, N0] = 1;
                Death_5_Bottom[i, N1] = 2;
                Death_6_BottomLeft[0, i] = 1;
                Death_6_BottomLeft[i, N0] = 1;
                Death_7_Left[0, i] = 1;
                Death_7_Left[1, i] = 2;
                Death_8_TopLeft[0, i] = 1;
                Death_8_TopLeft[i, 0] = 1;
            }
            Death_2_TopRight[N1, 1] = 2;
            Death_2_TopRight[N2, 1] = 3;
            Death_2_TopRight[N2, 2] = 3;
            Death_2_TopRight[N1, 2] = 3;
            Death_4_BottomRight[N1, N1] = 2;
            Death_4_BottomRight[N2, N1] = 3;
            Death_4_BottomRight[N2, N2] = 3;
            Death_4_BottomRight[N1, N2] = 3;
            Death_6_BottomLeft[1, N1] = 2;
            Death_6_BottomLeft[2, N1] = 3;
            Death_6_BottomLeft[2, N2] = 3;
            Death_6_BottomLeft[1, N2] = 3;
            Death_8_TopLeft[1, 1] = 2;
            Death_8_TopLeft[2, 1] = 3;
            Death_8_TopLeft[2, 2] = 3;
            Death_8_TopLeft[1, 2] = 3;
        }

        private Vector CreateVectorOneStepBackward(int x, int y, Direction direction)
        {
            switch (direction)
            {
                case Direction.Top: return new Vector(x, y + 1, direction);
                case Direction.TopRight: return new Vector(x - 1, y + 1, direction);
                case Direction.Right: return new Vector(x - 1, y, direction);
                case Direction.BottomRight: return new Vector(x - 1, y - 1, direction);
                case Direction.Bottom: return new Vector(x, y - 1, direction);
                case Direction.BottomLeft: return new Vector(x + 1, y - 1, direction);
                case Direction.Left: return new Vector(x + 1, y, direction);
                case Direction.TopLeft: return new Vector(x + 1, y + 1, direction);
            }
            throw new Exception(string.Format("Unknown value {0}.{1}.", typeof(Direction).Name, direction));
        }

        private Vector CreateVectorOneStepForward(int x, int y, Direction direction)
        {
            switch (direction)
            {
                case Direction.Top: return new Vector(x, y - 1, direction);
                case Direction.TopRight: return new Vector(x + 1, y - 1, direction);
                case Direction.Right: return new Vector(x + 1, y, direction);
                case Direction.BottomRight: return new Vector(x + 1, y + 1, direction);
                case Direction.Bottom: return new Vector(x, y + 1, direction);
                case Direction.BottomLeft: return new Vector(x - 1, y + 1, direction);
                case Direction.Left: return new Vector(x - 1, y, direction);
                case Direction.TopLeft: return new Vector(x - 1, y - 1, direction);
            }
            throw new Exception(string.Format("Unknown value {0}.{1}.", typeof(Direction).Name, direction));
        }

        private bool IsValid(int x, int y)
        {
            return x >= 0 && x < _size && y >= 0 && y < _size;
        }

        private void UpdateValue(int x, int y, Direction direction, int value)
        {
            if (value < DeathPlaygroundByDirection[direction][x, y])
            {
                DeathPlaygroundByDirection[direction][x, y] = value;

                Vector vector1 = CreateVectorOneStepBackward(x, y, direction.TurnRight());
                Vector vector2 = CreateVectorOneStepBackward(x, y, direction);
                Vector vector3 = CreateVectorOneStepBackward(x, y, direction.TurnLeft());

                if (IsValid(vector1.X, vector1.Y)) _queue.Enqueue(vector1);
                if (IsValid(vector2.X, vector2.Y)) _queue.Enqueue(vector2);
                if (IsValid(vector3.X, vector3.Y)) _queue.Enqueue(vector3);
            }
        }

        private void ProcessQueue()
        {
            while (_queue.Count > 0)
            {
                Vector vector = _queue.Dequeue();
                Vector forwardVector = CreateVectorOneStepForward(vector.X, vector.Y, vector.Direction);
                if (IsValid(forwardVector.X, forwardVector.Y))
                {
                    int value1 = DeathPlaygroundByDirection[forwardVector.Direction.TurnLeft()][forwardVector.X, forwardVector.Y];
                    int value2 = DeathPlaygroundByDirection[forwardVector.Direction][forwardVector.X, forwardVector.Y];
                    int value3 = DeathPlaygroundByDirection[forwardVector.Direction.TurnRight()][forwardVector.X, forwardVector.Y];

                    int maxValue = Math.Max(Math.Max(value1, value2), value3);
                    if (maxValue < Infinity)
                    {
                        UpdateValue(vector.X, vector.Y, vector.Direction, maxValue + 1);
                    }
                }
            }
        }

        public void Update(Snakes snakes)
        {
            // new points (new positions of snake heads)
            foreach (Snake snake in snakes.OthersAndMe)
            {
                foreach (Direction direction in _allDirections)
                {
                    UpdateValue(snake.X, snake.Y, direction, 0);
                }
            }

            // cross-collisions
            foreach (Snake snake in snakes.OthersAndMe)
            {
                foreach (Direction direction in _diagonalDirections)
                {
                    Vector vector1 = CreateVectorOneStepForward(snake.X, snake.Y, direction.TurnLeft().TurnLeft());
                    if (IsValid(vector1.X, vector1.Y) && DeathPlaygroundByDirection[direction][vector1.X, vector1.Y] == 0)
                    {
                        Vector tmpVector = CreateVectorOneStepForward(snake.X, snake.Y, vector1.Direction.TurnLeft());
                        UpdateValue(tmpVector.X, tmpVector.Y, direction, 1);
                    }

                    Vector vector2 = CreateVectorOneStepForward(snake.X, snake.Y, direction.TurnRight().TurnRight());
                    if (IsValid(vector2.X, vector2.Y) && DeathPlaygroundByDirection[direction][vector2.X, vector2.Y] == 0)
                    {
                        Vector tmpVector = CreateVectorOneStepForward(snake.X, snake.Y, vector2.Direction.TurnRight());
                        UpdateValue(tmpVector.X, tmpVector.Y, direction, 1);
                    }
                }
            }

            ProcessQueue();
        }
    }
}
