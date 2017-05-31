using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Jardos.Components
{
    /// <summary>
    /// bod hrací plochy
    /// </summary>
    public struct Point
    {
        private int _x;
        private int _y;

        public Point(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X {
            get { return _x; }
        }
        public int Y {
            get { return _y;}
        }

        public void Update(Direction dir)
        {
            switch (dir)
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
                    _x--;
                    _y--;
                    break;
            }
        }

        public void Update(Direction dir, int increment)
        {
            switch (dir)
            {
                case Direction.Top:
                    _y-= increment;
                    break;
                case Direction.TopRight:
                    _y-= increment;
                    _x+= increment;
                    break;
                case Direction.Right:
                    _x+= increment;
                    break;
                case Direction.BottomRight:
                    _x+= increment;
                    _y+= increment;
                    break;
                case Direction.Bottom:
                    _y+= increment;
                    break;
                case Direction.BottomLeft:
                    _y+= increment;
                    _x-= increment;
                    break;
                case Direction.Left:
                    _x-= increment;
                    break;
                case Direction.TopLeft:
                    _x-= increment;
                    _y-= increment;
                    break;
            }
            if (_y < 0) _y = 0;
            if (_x < 0) _x = 0;
            if (_x > ComputeHelper.MaxSize) _x = ComputeHelper.MaxSize;
            if (_y > ComputeHelper.MaxSize) _y = ComputeHelper.MaxSize;
        }

        public bool Equals(Point p)
        {
            return p.X == X && p.Y == Y;
        }

        public double Distance(Point p)
        {
            int x1 = _x;
            int x2 = p.X;
            int y1 = _y;
            int y2 = p.Y;
            return Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public bool IsValid()
        {
            return X < ComputeHelper.MaxSize && X >= 0 && Y < ComputeHelper.MaxSize && Y >= 0;
        }

        public bool IsValid(int [,] battleground)
        {
            if (!IsValid()) return false;
            return battleground[X, Y] == 0;
        }

        public IEnumerable<Point> GetNeighbours()
        {
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                Point px = this;
                px.Update(dir);
                if (px.IsValid())
                {
                    yield return px;
                }
            }
        }

        public bool IsOpposite(Point position, Direction direction)
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
    }
}
