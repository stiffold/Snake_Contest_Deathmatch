using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Snakes : List<Snake>
    {
        private int[,] _oldPlayground;
        private int[,] _newPlayground;
        private int _size;

        public Snakes(Snake me)
        {
            this.Me = me;
        }

        public Snake Me { get; private set; }
        public bool IsInitialized { get; private set; }

        public void Update(int[,] playground)
        {
            playground = (int[,])playground.Clone();

            if (_oldPlayground == null && _newPlayground == null)
            {
                _oldPlayground = playground;
                _size = playground.GetUpperBound(0) + 1;

                for (int y = 0; y < _size; y++)
                {
                    for (int x = 0; x < _size; x++)
                    {
                        int playerId = playground[x, y];
                        if (playerId != 0 && playerId != Me.Id)
                            this.Add(new Snake(playerId, new Point(x, y), Direction.Top));
                    }
                }
                return;
            }

            if (_oldPlayground != null && _newPlayground == null)
            {
                _newPlayground = playground;
                IsInitialized = true;
            }
            else
            {
                _oldPlayground = _newPlayground;
                _newPlayground = playground;
            }

            var liveSnakes = new List<Snake>();

            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    if (_oldPlayground[x, y] == 0 && _newPlayground[x, y] != 0)
                    {
                        int playerId = _newPlayground[x, y];
                        Point newPoint = new Point(x, y);
                        if (playerId == Me.Id)
                            this.Me = new Snake((int)PlayerId.Vazba, newPoint, GetDirection(this.Me.P, newPoint));
                        else
                        {
                            Snake? snake = this.SingleOrDefault(s => s.Id == playerId);
                            if (snake.HasValue)
                            {
                                liveSnakes.Add(new Snake(snake.Value.Id, newPoint, GetDirection(snake.Value.P, newPoint)));
                            }
                        }
                    }
                }
            }

            Clear();
            AddRange(liveSnakes);
        }

        private Direction GetDirection(Point oldP, Point newP)
        {
            if (oldP.X == newP.X && oldP.Y - 1 == newP.Y) return Direction.Top;
            if (oldP.X == newP.X && oldP.Y + 1 == newP.Y) return Direction.Bottom;
            if (oldP.X + 1 == newP.X && oldP.Y == newP.Y) return Direction.Right;
            if (oldP.X - 1 == newP.X && oldP.Y == newP.Y) return Direction.Left;
            if (oldP.X + 1 == newP.X && oldP.Y - 1 == newP.Y) return Direction.TopRight;
            if (oldP.X - 1 == newP.X && oldP.Y - 1 == newP.Y) return Direction.TopLeft;
            if (oldP.X + 1 == newP.X && oldP.Y + 1 == newP.Y) return Direction.BottomRight;
            if (oldP.X - 1 == newP.X && oldP.Y + 1 == newP.Y) return Direction.BottomLeft;

            throw new Exception(string.Format("Old point {0} and new point {1} are not next to each other.", oldP, newP));
        }
    }
}
