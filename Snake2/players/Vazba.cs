using System;
using Snake2.game;

namespace Deathmatch
{
    public class Vazba : IPlayerBehavior
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
        }

        public class Next
        {
            public Point Left;
            public Point Straight;
            public Point Right;
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

        private Next GetNextMove(Point p, Direction direction)
        {
            return new Next
            {
                Left = GetNextPoint(p, direction, Move.Left),
                Straight = GetNextPoint(p, direction, Move.Straight),
                Right = GetNextPoint(p, direction, Move.Right),
            };
        }

        private Point GetNextPoint(Point p, Direction direction, Move move)
        {
            if (move == Move.Straight)
            {
                switch (direction)
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
            }
            else if (move == Move.Left)
            {
                switch (direction)
                {
                    case Direction.Top: return new Point(p.X - 1, p.Y - 1);
                    case Direction.TopRight: return new Point(p.X, p.Y - 1);
                    case Direction.Right: return new Point(p.X + 1, p.Y - 1);
                    case Direction.BottomRight: return new Point(p.X + 1, p.Y);
                    case Direction.Bottom: return new Point(p.X + 1, p.Y + 1);
                    case Direction.BottomLeft: return new Point(p.X, p.Y + 1);
                    case Direction.Left: return new Point(p.X - 1, p.Y + 1);
                    case Direction.TopLeft: return new Point(p.X - 1, p.Y);
                }
            }
            else if (move == Move.Right)
            {
                switch (direction)
                {
                    case Direction.Top: return new Point(p.X + 1, p.Y - 1);
                    case Direction.TopRight: return new Point(p.X + 1, p.Y);
                    case Direction.Right: return new Point(p.X + 1, p.Y + 1);
                    case Direction.BottomRight: return new Point(p.X, p.Y + 1);
                    case Direction.Bottom: return new Point(p.X - 1, p.Y + 1);
                    case Direction.BottomLeft: return new Point(p.X - 1, p.Y);
                    case Direction.Left: return new Point(p.X - 1, p.Y - 1);
                    case Direction.TopLeft: return new Point(p.X, p.Y - 1);
                }
            }
            throw new Exception();
        }

        public int NextMove(int[,] playground)
        {
            _playground = playground;
            InitIfTheFirstMove();
            _step++;
            return (int)DoNextMove();
        }

        private Move DoNextMove()
        {
            Move move;

            Next next = GetNextMove(_p, _direction);

            if (IsEmpty(next.Left))
                move = Move.Left;
            else if (IsEmpty(next.Straight))
                move = Move.Straight;
            else
                move = Move.Right;

            return move;
        }
    }
}
