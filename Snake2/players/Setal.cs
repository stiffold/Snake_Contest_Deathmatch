using System;
using System.Threading;
using System.Windows;
using Snake2.game;

namespace Snake2.players
{
    public class Setal : IPlayerBehavior
    {
        private byte[,] _possibilities;
        private int _identificator;
        private int _size;
        private Direction _direction;
        private Point _actualPosition;
        private bool _firstRun;

        public Setal()
        {
            //TODO get field size
            _size = 100;
            _firstRun = true;
            _possibilities = new byte[_size, _size];
        }

        public void Init(int direction, int identificator)
        {
            _direction = (Direction)direction;
            _identificator = identificator;

            //throw new NotImplementedException();
        }

        public int NextMove(int[,] gameSurrond)
        {
            if (_firstRun)
            {
                _actualPosition = FindMySelf(gameSurrond);
                _firstRun = true;
            }

            EvaluateGame(gameSurrond);
            return 2;
            //throw new NotImplementedException();
        }

        public string MyName()
        {
            return "Setal(Šimík)";
        }

        private void EvaluateGame(int[,] gameSurrond)
        {
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                {
                    if (gameSurrond[i, j] != 0)
                    {
                        _possibilities[i, j] = 1;
                    }
                }
        }

        private Point FindMySelf(int[,] gameSurrond)
        {
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                {
                    if (gameSurrond[i, j] == _identificator)
                    {
                        return new Point(i, j);
                    }
                }
            throw new InvalidOperationException("Nejsem na herní ploše!");
        }
    }

}