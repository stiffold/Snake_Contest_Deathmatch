using System;
using System.Collections.Generic;
using SnakeDeathmatch.Game;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public class VazbaPlayer : IPlayerBehavior
    {
        #region Init

        public enum Direction
        {
            Unknown = 0,
            Top = 1,
            TopRight = 2,
            Right = 3,
            BottomRight = 4,
            Bottom = 5,
            BottomLeft = 6,
            Left = 7,
            TopLeft = 8,
        }

        public enum Move
        {
            Left = 1,
            Straight = 2,
            Right = 3,
        }

        public struct Point
        {
            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X;
            public int Y;

            public override string ToString()
            {
                return string.Format("[{0},{1}]", X, Y);
            }
        }

        public class Next
        {
            public Direction LeftDirection;
            public Direction StraightDirection;
            public Direction RightDirection;

            public Point LeftPoint;
            public Point StraightPoint;
            public Point RightPoint;

            public override string ToString()
            {
                return string.Format("Left: {0}, Straight: {1}, Right: {2}", LeftPoint, StraightPoint, RightPoint);
            }
        }

        public string MyName() { return "Vazba"; }

        public void Init(int direction, int id)
        {
            _direction = (Direction)direction;
            _myId = id;
        }

        #endregion

        private int _myId;
        private Direction _direction = Direction.Unknown;
        private int _step = 0;
        private Point _p = new Point(-1, -1);
        private int _width = -1;
        private int _height = -1;
        private int[,] _playground;

        private void InitIfTheFirstMove()
        {
            if (_step == 0)
            {
                _width = _playground.GetUpperBound(0) + 1;
                _height = _playground.GetUpperBound(1) + 1;

                // init _p
                bool found = false;
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        if (_playground[x, y] == _myId)
                        {
                            _p = new Point(x, y);
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }
                if (!found)
                    throw new Exception("Na Vazbíka jste zapomněli, není v hracím poli!");
            }
        }

        private int GetValue(Point p)
        {
            return _playground[p.X, p.Y];
        }

        private bool IsEmpty(Point p)
        {
            return IsInPlayground(p) && (_playground[p.X, p.Y] == 0);
        }

        private bool IsFull(Point p)
        {
            return !IsEmpty(p);
        }

        private bool IsInPlayground(Point p)
        {
            return (p.X >= 0 && p.X < _width && p.Y >= 0 && p.Y < _height);
        }

        private bool IsNotInPlayground(Point p)
        {
            return !IsInPlayground(p);
        }

        private bool IsCrossCollision(Point p, Direction direction)
        {
            switch (direction)
            {
                case Direction.TopRight: return (_playground[p.X, p.Y + 1] != 0) && (_playground[p.X - 1, p.Y] != 0);
                case Direction.BottomRight: return (_playground[p.X, p.Y - 1] != 0) && (_playground[p.X - 1, p.Y] != 0);
                case Direction.BottomLeft: return (_playground[p.X, p.Y - 1] != 0) && (_playground[p.X + 1, p.Y] != 0);
                case Direction.TopLeft: return (_playground[p.X, p.Y + 1] != 0) && (_playground[p.X + 1, p.Y] != 0);
            }
            return false;
        }

        private Next GetNextMove(Point p, Direction direction)
        {
            Direction leftDirection = GetNextDirection(direction, Move.Left);
            Direction straightDirection = GetNextDirection(direction, Move.Straight);
            Direction rightDirection = GetNextDirection(direction, Move.Right);

            return new Next()
            {
                LeftDirection = leftDirection,
                StraightDirection = straightDirection,
                RightDirection = rightDirection,
                LeftPoint = GetNextPoint(p, leftDirection),
                StraightPoint = GetNextPoint(p, straightDirection),
                RightPoint = GetNextPoint(p, rightDirection),
            };
        }

        private Direction GetNextDirection(Direction direction, Move move)
        {
            if (move == Move.Straight)
            {
                return direction;
            }
            else if (move == Move.Left)
            {
                switch (direction)
                {
                    case Direction.Top: return Direction.TopLeft;
                    case Direction.TopRight: return Direction.Top;
                    case Direction.Right: return Direction.TopRight;
                    case Direction.BottomRight: return Direction.Right;
                    case Direction.Bottom: return Direction.BottomRight;
                    case Direction.BottomLeft: return Direction.Bottom;
                    case Direction.Left: return Direction.BottomLeft;
                    case Direction.TopLeft: return Direction.Left;
                }
            }
            else if (move == Move.Right)
            {
                switch (direction)
                {
                    case Direction.Top: return Direction.TopRight;
                    case Direction.TopRight: return Direction.Right;
                    case Direction.Right: return Direction.BottomRight;
                    case Direction.BottomRight: return Direction.Bottom;
                    case Direction.Bottom: return Direction.BottomLeft;
                    case Direction.BottomLeft: return Direction.Left;
                    case Direction.Left: return Direction.TopLeft;
                    case Direction.TopLeft: return Direction.Top;
                }
            }
            throw new Exception();
        }

        private Point GetNextPoint(Point p, Direction nextDirection)
        {
            switch (nextDirection)
            {
                case Direction.Top: return new Point(p.X, p.Y - 1);
                case Direction.TopRight: return new Point(p.X + 1, p.Y - 1);
                case Direction.Right: return new Point(p.X + 1, p.Y);
                case Direction.BottomRight: return new Point(p.X + 1, p.Y + 1);
                case Direction.Bottom: return new Point(p.X, p.Y + 1);
                case Direction.BottomLeft: return new Point(p.X - 1, p.Y + 1);
                case Direction.Left: return new Point(p.X - 1, p.Y);
                case Direction.TopLeft: return new Point(p.X - 1, p.Y - 1);
            }
            throw new Exception();
        }

        public int NextMove(int[,] playground)
        {
            _playground = playground;
            InitIfTheFirstMove();
            _step++;

            Move move = DoNextMove();

            _direction = GetNextDirection(_direction, move);
            _p = GetNextPoint(_p, _direction);

            return (int)move;
        }

        private int wtf = 18;

        private Move DoNextMove()
        {
            Next next = GetNextMove(_p, _direction);

            int depthLeft = IsEmpty(next.LeftPoint) && !IsCrossCollision(next.LeftPoint, next.LeftDirection) ? GetEmptyDepth(next.LeftPoint, next.LeftDirection, 0) : 0;
            int depthStraight = (depthLeft != wtf) && IsEmpty(next.StraightPoint) && !IsCrossCollision(next.StraightPoint, next.StraightDirection) ? GetEmptyDepth(next.StraightPoint, next.StraightDirection, 0) : 0;
            int depthRight = (depthLeft != wtf && depthStraight != wtf) && IsEmpty(next.RightPoint) && !IsCrossCollision(next.RightPoint, next.RightDirection) ? GetEmptyDepth(next.RightPoint, next.RightDirection, 0) : 0;

            if (depthLeft >= depthStraight && depthLeft >= depthRight) return Move.Left;
            if (depthStraight >= depthLeft && depthStraight >= depthRight) return Move.Straight;
            return Move.Right;
        }

        private int GetEmptyDepth(Point p, Direction direction, int level)
        {
            _playground[p.X, p.Y] = _myId;

            if (level >= wtf)
                return wtf;

            int result = level;

            Next next = GetNextMove(p, direction);

            if (IsEmpty(next.LeftPoint) && !IsCrossCollision(next.LeftPoint, next.LeftDirection))
                result = Math.Max(result, GetEmptyDepth(next.LeftPoint, next.LeftDirection, level + 1));

            if (IsEmpty(next.StraightPoint) && !IsCrossCollision(next.StraightPoint, next.StraightDirection))
                result = Math.Max(result, GetEmptyDepth(next.StraightPoint, next.StraightDirection, level + 1));

            if (IsEmpty(next.RightPoint) && !IsCrossCollision(next.RightPoint, next.RightDirection))
                result = Math.Max(result, GetEmptyDepth(next.RightPoint, next.RightDirection, level + 1));

            _playground[p.X, p.Y] = 0;

            return result;
        }
    }
}
