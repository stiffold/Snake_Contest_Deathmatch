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

        public bool Equals(Point p)
        {
            return p.X == X && p.Y == Y;
        }

        public bool IsValid()
        {
            return X < ComputeHelper.MaxSize && X >= 0 && Y < ComputeHelper.MaxSize && Y >= 0;
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
    }
}
