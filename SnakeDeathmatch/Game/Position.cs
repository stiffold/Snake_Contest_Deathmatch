using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Game
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

        public bool IsInCollission(int max)
        {
            if (X < 0) return true;
            if (Y < 0) return true;
            if (X >= max) return true;
            if (Y >= max) return true;
            return false;
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

        internal bool IsInTrap(int[,] gameSurround, int max)
        {
            if (Y < max -2 && Y > 2)
            {
                if (gameSurround[X, Y - 2] != 0 && gameSurround[X, Y + 2] != 0)
                {
                    return true;
                }
            }

            if (X < max - 2 && X > 2)
            {
                if (gameSurround[X + 2, Y] != 0 && gameSurround[X - 2, Y] != 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
