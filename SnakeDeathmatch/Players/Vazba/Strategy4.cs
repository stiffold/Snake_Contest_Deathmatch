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
        private struct Point
        {
            public int X;
            public int Y;
            public Point(int x, int y) { X = x; Y = y; }
            public override string ToString() { return string.Format("[{0},{1}]", X, Y); }
        }

        private struct Vector
        {
            public int X;
            public int Y;
            public Direction Direction;
            public Vector(int x, int y, Direction direction) { X = x; Y = y; Direction = direction; }
            public override string ToString() { return string.Format("[{0},{1},{2}]", X, Y, Direction); }
            public Point? GetNextPoint(IntPlayground playground)
            {
                return CanMove(Direction, playground) ? Move(Direction) : (Point?)null;
            }

            public bool CanMove(Direction direction, IntPlayground playground)
            {
                Point s = Move(direction);

                // detekce kolize s okrajem pole
                if (s.X < 0 || s.Y < 0 || s.X >= playground.Size || s.Y >= playground.Size)
                {
                    return false;
                }

                // detekce kolize do těla jiného hada
                if (playground[s.X, s.Y] == 0)
                {
                    return false;
                }

                // detekce snahy projet diagonálně skrz tělo jiného hada
                if ((direction == Direction.TopRight && (playground[s.X, s.Y + 1] == 0) && (playground[s.X - 1, s.Y] == 0)) ||
                    (direction == Direction.BottomRight && (playground[s.X, s.Y - 1] == 0) && (playground[s.X - 1, s.Y] == 0)) ||
                    (direction == Direction.BottomLeft && (playground[s.X, s.Y - 1] == 0) && (playground[s.X + 1, s.Y] == 0)) ||
                    (direction == Direction.TopLeft && (playground[s.X, s.Y + 1] == 0) && (playground[s.X + 1, s.Y] == 0)))
                {
                    return false;
                }

                return true;
            }

            public Point Move(Direction direction)
            {
                switch (direction)
                {
                    case Direction.Top: return new Point(X, Y - 1);
                    case Direction.TopRight: return new Point(X + 1, Y - 1);
                    case Direction.Right: return new Point(X + 1, Y);
                    case Direction.BottomRight: return new Point(X + 1, Y + 1);
                    case Direction.Bottom: return new Point(X, Y + 1);
                    case Direction.BottomLeft: return new Point(X - 1, Y + 1);
                    case Direction.Left: return new Point(X - 1, Y);
                    case Direction.TopLeft: return new Point(X - 1, Y - 1);
                }
                throw new Exception(string.Format("Unknown value {0}.{1}.", typeof(Direction).Name, direction));
            }
        }

        private const int WTF = 14;
        private int _size;
        private bool _deathPlaygroundsCreated;
        private Snakes _snakes;
        private Queue<Vector> _queue;
        private IEnumerable<Direction> _allDirections;
        private IDictionary<Direction, IntPlayground> _deathPlaygroundByDirection;
        private IDictionary<Direction, IntPlayground> _queuePlaygroundByDirection;
        private int InfinityId { get { return DeathIntArrayVisualizer.InfinityId; } }
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
            Snake me = liveSnakes.Me;

            if (!_deathPlaygroundsCreated)
                CreateDeathPlaygrounds();

            RecalculateDeathPlaygrounds();
            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4DeathPlaygroundsRecalculated));


            if (true)
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

                Move move = GetNextStepForPasticka();
                _step++;
                return move;
            }
        }

        private Move GetNextStepForPasticka()
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

        private void RecalculateDeathPlaygrounds()
        {
            _queue = new Queue<Vector>();
            CreateQueuePlaygrounds();

            var allSnakesIncludingMe = new List<Snake>();
            allSnakesIncludingMe.AddRange(_snakes);
            allSnakesIncludingMe.Add(_snakes.Me);
            
            foreach (Snake snake in allSnakesIncludingMe)
            {
                foreach (Direction direction in _allDirections)
                {
                    _queue.Enqueue(new Vector(snake.X, snake.Y, direction));
                    _queuePlaygroundByDirection[direction][snake.X, snake.Y] = 1;
                    InvokeBreakpointsForEnqueue(direction);
                }
            }
            ProcessQueue();
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
                IntPlayground deathPlaygroundForDirection = _deathPlaygroundByDirection[vector.Direction];

                if (_Playground[vector.X, vector.Y] != 0 && _snakes.Union(new[] {_snakes.Me}).Any(snake => snake.X == vector.X && snake.Y == vector.Y))
                {
                    // stojím na překážce
                    // poznačím si, že smrt je okamžitá (0 kroků do smrti)
                    deathPlaygroundForDirection[vector.X, vector.Y] = 0;
                    EnqueueVectors(GetNeighborhoodVectors(vector.X, vector.Y));
                }
                else
                {
                    // nestojím na překážce
                    // zjistím si, kolik kroků do smrti mi zbývá na pozici, kam bych chtěl jet (ve všech třech směrech)
                    // pokud najdu jistou smrt, poznačím si, na kolik kroků ta smrt bude
                    Point? nextPoint = vector.GetNextPoint(deathPlaygroundForDirection);

                    if (nextPoint == null)
                    {
                        if (deathPlaygroundForDirection[vector.X, vector.Y] > 1)
                        {
                            deathPlaygroundForDirection[vector.X, vector.Y] = 1;
                            EnqueueVectors(GetAffectedVectorsAsRectangle(vector.X, vector.Y, vector.Direction));
                        }
                    }
                    else
                    {
                        IntPlayground deathPlaygroundLeft = _deathPlaygroundByDirection[vector.Direction.TurnLeft()];
                        IntPlayground deathPlaygroundStraight = deathPlaygroundForDirection;
                        IntPlayground deathPlaygroundRight = _deathPlaygroundByDirection[vector.Direction.TurnRight()];

                        int valueLeft = deathPlaygroundLeft[nextPoint.Value.X, nextPoint.Value.Y];
                        int valueStraight = deathPlaygroundStraight[nextPoint.Value.X, nextPoint.Value.Y];
                        int valueRight = deathPlaygroundRight[nextPoint.Value.X, nextPoint.Value.Y];

                        int maxValue = Math.Max(Math.Max(valueLeft, valueStraight), valueRight);
                        int minValue = Math.Min(Math.Min(valueLeft, valueStraight), valueRight);
                        if (maxValue < InfinityId && deathPlaygroundForDirection[vector.X, vector.Y] > minValue + 1)
                        {
                            deathPlaygroundForDirection[vector.X, vector.Y] = minValue + 1;
                            EnqueueVectors(GetAffectedVectorsAsRectangle(vector.X, vector.Y, vector.Direction));
                        }
                    }
                }
            }
        }

        private void EnqueueVectors(IEnumerable<Vector> vectors)
        {
            foreach (Vector vector in vectors)
            {
                if (_queuePlaygroundByDirection[vector.Direction][vector.X, vector.Y] == 0/*
                    _deathPlaygroundByDirection[vector.Direction][vector.X, vector.Y] == InfinityId*/)
                {
                    _queue.Enqueue(vector);
                    _queuePlaygroundByDirection[vector.Direction][vector.X, vector.Y] = 1;
                    InvokeBreakpointsForEnqueue(vector.Direction);
                }
            }
        }

        //private IEnumerable<Vector> GetNeighborhoodVectors(int x, int y)
        //{
        //    if (y < _size - 1) yield return new Vector(x, y + 1, Direction.Top);
        //    if (x > 0 && y < _size - 1) yield return new Vector(x - 1, y + 1, Direction.TopRight);
        //    if (x > 0) yield return new Vector(x - 1, y, Direction.Right);
        //    if (x > 0 && y > 0) yield return new Vector(x - 1, y - 1, Direction.BottomRight);
        //    if (y > 0) yield return new Vector(x, y - 1, Direction.Bottom);
        //    if (x < _size - 1 && y > 0) yield return new Vector(x + 1, y - 1, Direction.BottomLeft);
        //    if (x < _size - 1) yield return new Vector(x + 1, y, Direction.Left);
        //    if (x < _size - 1 && y < _size - 1) yield return new Vector(x + 1, y + 1, Direction.TopLeft);
        //}

        private IEnumerable<Vector> GetAffectedVectorsForDirection(int x, int y, Direction direction)
        {
            if (direction == Direction.Top)
            {
                if (y < _size - 1)
                    yield return new Vector(x, y + 1, Direction.Top);
            }
            else if (direction == Direction.TopRight)
            {
                if (y < _size - 1)
                    yield return new Vector(x, y + 1, Direction.TopRight);
                if (x > 0)
                    yield return new Vector(x - 1, y, Direction.TopRight);
                if (x > 0 && y < _size - 1)
                    yield return new Vector(x - 1, y + 1, Direction.TopRight);
            }
            else if (direction == Direction.Right)
            {
                if (x > 0)
                    yield return new Vector(x - 1, y, Direction.Right);
            }
            else if (direction == Direction.BottomRight)
            {
                if (x > 0)
                    yield return new Vector(x - 1, y, Direction.BottomRight);
                if (x > 0 && y > 0)
                    yield return new Vector(x - 1, y - 1, Direction.BottomRight);
                if (y > 0)
                    yield return new Vector(x, y - 1, Direction.BottomRight);
            }
            else if (direction == Direction.Bottom)
            {
                if (y > 0)
                    yield return new Vector(x, y - 1, Direction.Bottom);
            }
            else if (direction == Direction.BottomLeft)
            {
                if (y > 0)
                    yield return new Vector(x, y - 1, Direction.BottomLeft);
                if (x < _size - 1 && y > 0)
                    yield return new Vector(x + 1, y - 1, Direction.BottomLeft);
                if (x < _size - 1)
                    yield return new Vector(x + 1, y, Direction.BottomLeft);
            }
            else if (direction == Direction.Left)
            {
                if (x < _size - 1)
                    yield return new Vector(x + 1, y, Direction.Left);
            }
            else if (direction == Direction.TopLeft)
            {
                if (y < _size - 1)
                    yield return new Vector(x, y + 1, Direction.TopLeft);
                if (x < _size - 1)
                    yield return new Vector(x + 1, y, Direction.TopLeft);
                if (x < _size - 1 && y < _size - 1)
                    yield return new Vector(x + 1, y + 1, Direction.TopLeft);
            }
        }

        private bool IsInRange(int x, int y)
        {
            return (x > 0 && y > 0 && x < _size && y < _size);
        }

        private IEnumerable<Vector> GetAffectedVectorsAsRectangle(int x, int y, Direction direction)
        {
            for (int newX = x - 4; newX <= x + 4; newX++)
                for (int newY = y - 4; newY <= y + 4; newY++)
                    if (IsInRange(newX, newY))
                        yield return new Vector(newX, newY, direction);
        }

        private IEnumerable<Vector> GetNeighborhoodVectors(int x, int y)
        {
            if (y < _size - 1)
            {
                yield return new Vector(x, y + 1, Direction.TopLeft);
                yield return new Vector(x, y + 1, Direction.Top);
                yield return new Vector(x, y + 1, Direction.TopRight);
            }
            if (x > 0 && y < _size - 1)
                yield return new Vector(x - 1, y + 1, Direction.TopRight);

            if (x > 0)
            {
                yield return new Vector(x - 1, y, Direction.TopRight);
                yield return new Vector(x - 1, y, Direction.Right);
                yield return new Vector(x - 1, y, Direction.BottomRight);
            }
            if (x > 0 && y > 0)
                yield return new Vector(x - 1, y - 1, Direction.BottomRight);

            if (y > 0)
            {
                yield return new Vector(x, y - 1, Direction.BottomLeft);
                yield return new Vector(x, y - 1, Direction.Bottom);
                yield return new Vector(x, y - 1, Direction.BottomRight);
            }

            if (x < _size - 1 && y > 0)
                yield return new Vector(x + 1, y - 1, Direction.BottomLeft);

            if (x < _size - 1)
            {
                yield return new Vector(x + 1, y, Direction.TopLeft);
                yield return new Vector(x + 1, y, Direction.Left);
                yield return new Vector(x + 1, y, Direction.BottomLeft);
            }
            if (x < _size - 1 && y < _size - 1)
                yield return new Vector(x + 1, y + 1, Direction.TopLeft);
        }
    }
}
