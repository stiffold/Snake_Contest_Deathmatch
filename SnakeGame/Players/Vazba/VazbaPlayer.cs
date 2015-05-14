using System;
using System.Collections.Generic;
using SnakeGame.Interface;

namespace SnakeGame.Players.Vazba
{
    public class VazbaPlayer : IPlayerBehaviour2
    {
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

        public string Name { get { return "Vazba"; } }

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _id = playerId;
            _p = new Point(x, y);
            _dir = direction;
            _size = playgroundSize;
        }

        private int _id;
        private Point _p = new Point(-1, -1);
        private Direction _dir;
        private Snakes _snakes = new Snakes();
        private int _size;
        private int[,] _playground;
        private int wtf = 18;
        private int step = 0;

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
            return (p.X >= 0 && p.X < _size && p.Y >= 0 && p.Y < _size);
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

        public Move GetNextMove(int[,] playground)
        {
            _playground = playground;
            _snakes.Update(playground);
            
            Move move = DoNextMove();

            _dir = GetNextDirection(_dir, move);
            _p = GetNextPoint(_p, _dir);

            return move;
        }

        private Move DoNextMove()
        {
            Next next = GetNextMove(_p, _dir);

            int depthLeft = IsEmpty(next.LeftPoint) && !IsCrossCollision(next.LeftPoint, next.LeftDirection) ? GetEmptyDepth(next.LeftPoint, next.LeftDirection, 0) : 0;
            int depthStraight = (depthLeft != wtf) && IsEmpty(next.StraightPoint) && !IsCrossCollision(next.StraightPoint, next.StraightDirection) ? GetEmptyDepth(next.StraightPoint, next.StraightDirection, 0) : 0;
            int depthRight = (depthLeft != wtf && depthStraight != wtf) && IsEmpty(next.RightPoint) && !IsCrossCollision(next.RightPoint, next.RightDirection) ? GetEmptyDepth(next.RightPoint, next.RightDirection, 0) : 0;

            if (depthLeft >= depthStraight && depthLeft >= depthRight) return Move.Left;
            if (depthStraight >= depthLeft && depthStraight >= depthRight) return Move.Straight;
            return Move.Right;
        }

        private int GetEmptyDepth(Point p, Direction direction, int level)
        {
            _playground[p.X, p.Y] = _id;

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
