using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Snakes : List<Snake>
    {
        private int[,] _oldPlayground;
        private int[,] _newPlayground;
        private int _size;

        public bool IsInitialized { get; private set; }
        public void Update(int[,] playground)
        {
            playground = (int[,])playground.Clone();

            if (_oldPlayground == null && _newPlayground == null)
            {
                _oldPlayground = playground;
                _size = playground.GetUpperBound(0) + 1;

                for (int y = 0; y < _size; y++)
                    for (int x = 0; x < _size; x++)
                        if (playground[x, y] != 0)
                            this.Add(new Snake(id: playground[x, y], initialPosition: new Point(x, y)));
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
            
            for (int y = 0; y < _size; y++)
            {
                for (int x = 0; x < _size; x++)
                {
                    if (_oldPlayground[x, y] == 0 && _newPlayground[x, y] != 0)
                    {
                        int snakeId = _newPlayground[x, y];
                        Snake snake = this.Single(s => s.Id == snakeId);
                        snake.UpdatePosition(new Point(x, y));
                    }
                }
            }
        }
    }
}
