using System.Collections.Generic;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;
using System;
using System.Linq;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Strategy4 : IStrategy, IDebuggable
    {
        #region Support

        private const int WTF = 14;
        private int _size;
        private bool _deathPlaygroundsCreated;
        private Snakes _snakes;
        private Queue<Vector> _queue;
        private IEnumerable<Direction> _allDirections;
        private IEnumerable<Direction> _diagonalDirections;
        private IDictionary<Direction, IntPlayground> _deathPlaygroundByDirection;
        private IDictionary<Direction, IntPlayground> _queuePlaygroundByDirection;
        private int Infinity { get { return DeathIntArrayVisualizer.InfinityId; } }
        private int _step;

        public event BreakpointEventHandler Breakpoint;

        public Move GetNextMove(IntPlayground playground, Snakes liveSnakes)
        {
            _Playground = playground;
            _Track = playground.Clone();
            _size = playground.Size;
            _snakes = liveSnakes;
            _allDirections = new List<Direction>()
            {
                Direction.Top,
                Direction.TopRight,
                Direction.Right,
                Direction.BottomRight,
                Direction.Bottom,
                Direction.BottomLeft,
                Direction.Left,
                Direction.TopLeft,
            };
            _diagonalDirections = new List<Direction>()
            {
                Direction.TopRight,
                Direction.BottomRight,
                Direction.BottomLeft,
                Direction.TopLeft,
            };

            Snake me = liveSnakes.Me;

            if (!_deathPlaygroundsCreated)
                CreateDeathPlaygrounds();

            RecalculateDeathPlaygrounds();
            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4DeathPlaygroundsRecalculated));


            if (false)
            {
                Next next = me.GetNext(playground);

                int depthLeft = next.Left.HasValue ? GetDepth(next.Left.Value, 0) : 0;
                int depthStraight = (depthLeft != WTF) && next.Straight.HasValue ? GetDepth(next.Straight.Value, 0) : 0;
                int depthRight = (depthLeft != WTF && depthStraight != WTF) && next.Right.HasValue ? GetDepth(next.Right.Value, 0) : 0;

                _Track = null;
                _Playground = null;

                if (depthLeft >= depthStraight && depthLeft >= depthRight) return Move.Left;
                if (depthStraight >= depthLeft && depthStraight >= depthRight) return Move.Straight;
                return Move.Right;
            }
            else
            {
                _Track = null;
                _Playground = null;

                Move move = GetNextStepForPasticka2();
                _step++;
                return move;
            }
        }

        private Move GetNextStepForPasticka2()
        {
            if (_step == 0) return Move.Straight;
            else if (_step == 1) return Move.Right;
            else if (_step == 2) return Move.Straight;
            else if (_step == 3) return Move.Straight;
            else if (_step == 4) return Move.Straight;
            else if (_step == 5) return Move.Left;
            else if (_step == 6) return Move.Left;
            else if (_step == 7) return Move.Straight;
            else if (_step == 8) return Move.Left;
            else if (_step == 9) return Move.Left;
            else if (_step == 10) return Move.Straight;
            else
                return Move.Straight;
        }

        private Move GetNextStepForPasticka1()
        {
            if (_step == 0) return Move.Straight;
            else if (_step == 1) return Move.Straight;
            else if (_step == 2) return Move.Right;
            else if (_step == 3) return Move.Right;
            else if (_step == 4) return Move.Straight;
            else if (_step == 5) return Move.Straight;
            else if (_step == 6) return Move.Straight;
            else if (_step == 7) return Move.Straight;
            else if (_step == 8) return Move.Straight;
            else if (_step == 9) return Move.Left;
            else if (_step == 10) return Move.Left;
            else if (_step == 11) return Move.Straight;
            else if (_step == 12) return Move.Left;
            else if (_step == 13) return Move.Left;
            else if (_step == 14) return Move.Straight;
            else if (_step == 15) return Move.Straight;
            else if (_step == 16) return Move.Straight;
            else if (_step == 17) return Move.Straight;
            else if (_step == 18) return Move.Straight;
            else if (_step == 19) return Move.Right;
            else if (_step == 20) return Move.Right;
            else if (_step == 21) return Move.Straight;
            else if (_step == 22) return Move.Straight;
            else
                return Move.Straight;
        }

        private int GetDepth(Snake me, int level)
        {
            if (level >= WTF)
                return WTF;

            _Track[me.X, me.Y] = PlayersIntArrayVisualizer.TrackId;
            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4TrackChanged));

            int result = level;

            Next next = me.GetNext(_Track);

            if (next.Left.HasValue)
                result = Math.Max(result, GetDepth(next.Left.Value, level + 1));

            if (result < WTF && next.Straight.HasValue)
                result = Math.Max(result, GetDepth(next.Straight.Value, level + 1));

            if (result < WTF && next.Right.HasValue)
                result = Math.Max(result, GetDepth(next.Right.Value, level + 1));

            _Track[me.X, me.Y] = 0;

            return result;
        }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground _Playground { get; private set; }

        //[ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground _Track { get; private set; }

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

        #region QueuePlaygrounds

        [ToDebug(typeof(BlackAndWhiteIntArrayVisualizer))]
        public IntPlayground Queue_1_Top { get; private set; }

        [ToDebug(typeof(BlackAndWhiteIntArrayVisualizer))]
        public IntPlayground Queue_2_TopRight { get; private set; }

        [ToDebug(typeof(BlackAndWhiteIntArrayVisualizer))]
        public IntPlayground Queue_3_Right { get; private set; }

        [ToDebug(typeof(BlackAndWhiteIntArrayVisualizer))]
        public IntPlayground Queue_4_BottomRight { get; private set; }

        [ToDebug(typeof(BlackAndWhiteIntArrayVisualizer))]
        public IntPlayground Queue_5_Bottom { get; private set; }

        [ToDebug(typeof(BlackAndWhiteIntArrayVisualizer))]
        public IntPlayground Queue_6_BottomLeft { get; private set; }

        [ToDebug(typeof(BlackAndWhiteIntArrayVisualizer))]
        public IntPlayground Queue_7_Left { get; private set; }

        [ToDebug(typeof(BlackAndWhiteIntArrayVisualizer))]
        public IntPlayground Queue_8_TopLeft { get; private set; }

        #endregion

        private void CreateDeathPlaygrounds()
        {
            var array = new int[_size, _size];
            for (int x = 0; x < _size; x++)
                for (int y = 0; y < _size; y++)
                    array[x, y] = PlayersIntArrayVisualizer.InfinityId;

            var intPlayground = new IntPlayground(array);

            Death_1_Top = intPlayground;
            Death_2_TopRight = intPlayground.Clone();
            Death_3_Right = intPlayground.Clone();
            Death_4_BottomRight = intPlayground.Clone();
            Death_5_Bottom = intPlayground.Clone();
            Death_6_BottomLeft = intPlayground.Clone();
            Death_7_Left = intPlayground.Clone();
            Death_8_TopLeft = intPlayground.Clone();

            _deathPlaygroundByDirection = new Dictionary<Direction, IntPlayground>();

            _deathPlaygroundByDirection[Direction.Top] = Death_1_Top;
            _deathPlaygroundByDirection[Direction.TopRight] = Death_2_TopRight;
            _deathPlaygroundByDirection[Direction.Right] = Death_3_Right;
            _deathPlaygroundByDirection[Direction.BottomRight] = Death_4_BottomRight;
            _deathPlaygroundByDirection[Direction.Bottom] = Death_5_Bottom;
            _deathPlaygroundByDirection[Direction.BottomLeft] = Death_6_BottomLeft;
            _deathPlaygroundByDirection[Direction.Left] = Death_7_Left;
            _deathPlaygroundByDirection[Direction.TopLeft] = Death_8_TopLeft;

            _deathPlaygroundsCreated = true;
        }

        private void CreateQueuePlaygrounds()
        {
            Queue_1_Top = new IntPlayground(new int[_size, _size]);
            Queue_2_TopRight = new IntPlayground(new int[_size, _size]);
            Queue_3_Right = new IntPlayground(new int[_size, _size]);
            Queue_4_BottomRight = new IntPlayground(new int[_size, _size]);
            Queue_5_Bottom = new IntPlayground(new int[_size, _size]);
            Queue_6_BottomLeft = new IntPlayground(new int[_size, _size]);
            Queue_7_Left = new IntPlayground(new int[_size, _size]);
            Queue_8_TopLeft = new IntPlayground(new int[_size, _size]);

            _queuePlaygroundByDirection = new Dictionary<Direction, IntPlayground>();

            _queuePlaygroundByDirection[Direction.Top] = Queue_1_Top;
            _queuePlaygroundByDirection[Direction.TopRight] = Queue_2_TopRight;
            _queuePlaygroundByDirection[Direction.Right] = Queue_3_Right;
            _queuePlaygroundByDirection[Direction.BottomRight] = Queue_4_BottomRight;
            _queuePlaygroundByDirection[Direction.Bottom] = Queue_5_Bottom;
            _queuePlaygroundByDirection[Direction.BottomLeft] = Queue_6_BottomLeft;
            _queuePlaygroundByDirection[Direction.Left] = Queue_7_Left;
            _queuePlaygroundByDirection[Direction.TopLeft] = Queue_8_TopLeft;
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

        #endregion

        private void RecalculateDeathPlaygrounds()
        {
            _queue = new Queue<Vector>();
            CreateQueuePlaygrounds();

            var allSnakesIncludingMe = new List<Snake>();
            allSnakesIncludingMe.AddRange(_snakes);
            allSnakesIncludingMe.Add(_snakes.Me);

            // snake-heads
            foreach (Snake snake in allSnakesIncludingMe)
            {
                foreach (Direction direction in _allDirections)
                {
                    UpdateValue(snake.X, snake.Y, direction, 0);
                    InvokeBreakpointsForEnqueue(direction);
                }
            }

            // cross-collisions
            foreach (Snake snake in allSnakesIncludingMe)
            {
                foreach (Direction direction in _diagonalDirections)
                {
                    Vector vector1 = CreateVectorOneStepForward(snake.X, snake.Y, direction.TurnLeft().TurnLeft());
                    if (IsValid(vector1.X, vector1.Y) && _deathPlaygroundByDirection[direction][vector1.X, vector1.Y] == 0)
                    {
                        Vector tmpVector = CreateVectorOneStepForward(snake.X, snake.Y, vector1.Direction.TurnLeft());
                        UpdateValue(tmpVector.X, tmpVector.Y, direction, 1);
                    }

                    Vector vector2 = CreateVectorOneStepForward(snake.X, snake.Y, direction.TurnRight().TurnRight());
                    if (IsValid(vector2.X, vector2.Y) && _deathPlaygroundByDirection[direction][vector2.X, vector2.Y] == 0)
                    {
                        Vector tmpVector = CreateVectorOneStepForward(snake.X, snake.Y, vector2.Direction.TurnRight());
                        UpdateValue(tmpVector.X, tmpVector.Y, direction, 1);
                    }
                }
            }
            ProcessQueue();
        }

        private void UpdateValue(int x, int y, Direction direction, int value)
        {
            if (value < _deathPlaygroundByDirection[direction][x, y])
            {
                _deathPlaygroundByDirection[direction][x, y] = value;
                _queuePlaygroundByDirection[direction][x, y] = 1;

                Vector vector1 = CreateVectorOneStepBackward(x, y, direction.TurnRight());
                Vector vector2 = CreateVectorOneStepBackward(x, y, direction);
                Vector vector3 = CreateVectorOneStepBackward(x, y, direction.TurnLeft());

                if (IsValid(vector1.X, vector1.Y)) _queue.Enqueue(vector1);
                if (IsValid(vector2.X, vector2.Y)) _queue.Enqueue(vector2);
                if (IsValid(vector3.X, vector3.Y)) _queue.Enqueue(vector3);
            }
        }

        private void InvokeBreakpointsForEnqueue(Direction direction)
        {
            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4Enqueue));
            if (Breakpoint != null && direction == Direction.Top)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4Enqueue1Top));
            if (Breakpoint != null && direction == Direction.TopRight)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4Enqueue2TopRight));
            if (Breakpoint != null && direction == Direction.Right)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4Enqueue3Right));
            if (Breakpoint != null && direction == Direction.BottomRight)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4Enqueue4BottomRight));
            if (Breakpoint != null && direction == Direction.Bottom)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4Enqueue5Bottom));
            if (Breakpoint != null && direction == Direction.BottomLeft)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4Enqueue6BottomLeft));
            if (Breakpoint != null && direction == Direction.Left)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4Enqueue7Left));
            if (Breakpoint != null && direction == Direction.TopLeft)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4Enqueue8TopLeft));
        }

        private void ProcessQueue()
        {
            while (_queue.Count > 0)
            {
                Vector vector = _queue.Dequeue();
                Vector forwardVector = CreateVectorOneStepForward(vector.X, vector.Y, vector.Direction);
                if (IsValid(forwardVector.X, forwardVector.Y))
                {
                    int value1 = _deathPlaygroundByDirection[forwardVector.Direction.TurnLeft()][forwardVector.X, forwardVector.Y];
                    int value2 = _deathPlaygroundByDirection[forwardVector.Direction][forwardVector.X, forwardVector.Y];
                    int value3 = _deathPlaygroundByDirection[forwardVector.Direction.TurnRight()][forwardVector.X, forwardVector.Y];

                    int maxValue = Math.Max(Math.Max(value1, value2), value3);
                    if (maxValue < Infinity)
                    {
                        UpdateValue(vector.X, vector.Y, vector.Direction, maxValue + 1);
                    }
                }
            }
        }
    }
}
