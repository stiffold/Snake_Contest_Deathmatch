using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardik
{
    public class Position
    {
        private int _x;
        private int _y;
        public Position(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X
        {
            get { return _x; }
        }
        public int Y
        {
            get { return _y; }
        }

        public double Distance(Position p)
        {
            int x1 = _x;
            int x2 = p.X;
            int y1 = _y;
            int y2 = p.Y;
            return Math.Sqrt((x1-x2)*(x1-x2)+(y1-y2)*(y1-y2));
        }

        public bool IsSame(Position p)
        {
            if (p.X == X && p.Y == Y)
            {
                return true;
            }
            return false;
        }

        public bool IsInCollission(int max)
        {
            if (X < 0) return true;
            if (Y < 0) return true;
            if (X >= max) return true;
            if (Y >= max) return true;
            return false;
        }

        public Position Copy()
        {
            return new Position(_x, _y);
        }

        public Direction PositionDiagonalDirection(Position p)
        {
            if (p.X < _x)
            {
                if (p.Y < _y)
                {
                    return Direction.TopLeft;
                }
                else
                {
                    return Direction.BottomLeft;
                }
            }
            else
            {
                if (p.Y < _y)
                {
                    return Direction.TopRight;
                }
                else
                {
                    return Direction.BottomRight;
                }
            }
        }

        public void Update(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    _y--;
                    break;
                case Direction.TopRight:
                    _y--;
                    _x++;
                    break;
                case Direction.Right:
                    _x++;
                    break;
                case Direction.BottomRight:
                    _x++;
                    _y++;
                    break;
                case Direction.Bottom:
                    _y++;
                    break;
                case Direction.BottomLeft:
                    _y++;
                    _x--;
                    break;
                case Direction.Left:
                    _x--;
                    break;
                case Direction.TopLeft:
                    _y--;
                    _x--;
                    break;
            }
        }

        internal bool IsOpposite(Position position, Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    if (Y > position.Y) return true;
                    break;
                case Direction.TopLeft:
                    if (Y > position.Y && X > position.X) return true;
                    break;
                case Direction.Left:
                    if (X > position.X) return true;
                    break;
                case Direction.BottomLeft:
                    if (Y < position.Y && X > position.X) return true;
                    break;
                case Direction.Bottom:
                    if (Y < position.Y) return true;
                    break;
                case Direction.BottomRight:
                    if (Y < position.Y && X < position.X) return true;
                    break;
                case Direction.Right:
                    if (X < position.X) return true;
                    break;
                case Direction.TopRight:
                    if (Y > position.Y && X < position.X) return true;
                    break;
            }
            return false;
        }

        public bool AuraContains(Position position)
        {
            if (this.IsSame(position))
            {
                return true;
            }
            foreach (var dir in Enum.GetValues(typeof(Direction)))
            {
                Position newPos = this.Copy();
                newPos.Update((Direction)dir);
                if (newPos.IsSame(position))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
