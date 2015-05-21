using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardik
{
    class CollissionHelper
    {
        private int _max;
        private int _myId;

        public CollissionHelper(int max, int myId)
        {
            _max = max;
            _myId = myId;
        }

        public int Max { get { return _max; } }

        public bool Collission(Direction direction, int[,] gameSurround, Position position, bool repair = false)
        {
            if (CrossColision(direction, gameSurround, position)) return true;
            if (position.IsInCollission(_max)) return true;
            if (CollissionWithPlayer(position,gameSurround)) return true;

            return false;
        }

        private bool CollissionWithPlayer(Position position, int[,] gameSurround)
        {
            if (gameSurround[position.X, position.Y] != 0) return true;
            return false;
        }

        private bool CollissionWithPlayerMore(Position position, int[,] gameSurround)
        {
            if (!(position.X <= 0 || position.Y <= 0 || position.X >= _max - 1 || position.Y >= _max - 1)) 
            {
                if (gameSurround[position.X - 1, position.Y + 1] != 0 && gameSurround[position.X - 1, position.Y + 1] != _myId) return true;
                if (gameSurround[position.X - 1, position.Y] != 0 && gameSurround[position.X - 1, position.Y] != _myId) return true;
                if (gameSurround[position.X - 1, position.Y - 1] != 0 && gameSurround[position.X - 1, position.Y - 1] != _myId) return true;
                if (gameSurround[position.X, position.Y - 1] != 0 && gameSurround[position.X, position.Y - 1] != _myId) return true;
                if (gameSurround[position.X, position.Y + 1] != 0 && gameSurround[position.X, position.Y + 1] != _myId) return true;
                if (gameSurround[position.X + 1, position.Y + 1] != 0 && gameSurround[position.X + 1, position.Y + 1] != _myId) return true;
                if (gameSurround[position.X + 1, position.Y] != 0 && gameSurround[position.X + 1, position.Y] != _myId) return true;
                if (gameSurround[position.X + 1, position.Y - 1] != 0 && gameSurround[position.X + 1, position.Y - 1] != _myId) return true;
            }

            if (gameSurround[position.X, position.Y] != 0) return true;
            return false;
        }

        private bool CollissionWithPlayerRepair(Position position, Direction direction, int[,] gameSurround)
        {
            if (!(position.X <= 1 || position.Y <= 1 || position.X >= _max - 2 || position.Y >= _max - 2))
            {
                if (direction == Direction.BottomLeft && (gameSurround[position.X - 1, position.Y + 1] != 0 )) return true;
                if (direction == Direction.Left && (gameSurround[position.X - 1, position.Y] != 0)) return true;
                if (direction == Direction.TopLeft && (gameSurround[position.X - 1, position.Y - 1] != 0)) return true;
                if (direction == Direction.Top && (gameSurround[position.X, position.Y - 1] != 0)) return true;
                if (direction == Direction.Bottom && (gameSurround[position.X, position.Y + 1] != 0)) return true;
                if (direction == Direction.BottomRight && (gameSurround[position.X + 1, position.Y + 1] != 0)) return true;
                if (direction == Direction.Right && (gameSurround[position.X + 1, position.Y] != 0)) return true;
                if (direction == Direction.TopRight && (gameSurround[position.X + 1, position.Y - 1] != 0)) return true;

                if (direction == Direction.BottomLeft && (gameSurround[position.X - 2, position.Y + 1] != 0)) return true;
                if (direction == Direction.BottomLeft && (gameSurround[position.X - 1, position.Y + 2] != 0)) return true;

                if (direction == Direction.TopLeft && (gameSurround[position.X - 2, position.Y - 1] != 0)) return true;
                if (direction == Direction.TopLeft && (gameSurround[position.X - 1, position.Y - 2] != 0)) return true;

                if (direction == Direction.BottomRight && (gameSurround[position.X + 1, position.Y + 2] != 0)) return true;
                if (direction == Direction.BottomRight && (gameSurround[position.X + 2, position.Y + 1] != 0)) return true;

                if (direction == Direction.TopRight && (gameSurround[position.X + 1, position.Y - 2] != 0)) return true;
                if (direction == Direction.TopRight && (gameSurround[position.X + 2, position.Y - 1] != 0)) return true;
            }

            if (gameSurround[position.X, position.Y] != 0) return true;
            return false;
        }

        bool CrossColision(Direction direction, int[,] gameSurround, Position position)
        {
            if (position.X <= 0 || position.Y <= 0 || position.X >= _max - 1 || position.Y >= _max - 1) return false;

            return (direction == Direction.TopRight && (gameSurround[position.X, position.Y + 1] != 0) && (gameSurround[position.X - 1, position.Y] != 0)) ||
                    (direction == Direction.BottomRight && (gameSurround[position.X, position.Y - 1] != 0) && (gameSurround[position.X - 1, position.Y] != 0)) ||
                    (direction == Direction.BottomLeft && (gameSurround[position.X, position.Y - 1] != 0) && (gameSurround[position.X + 1, position.Y] != 0)) ||
                    (direction == Direction.TopLeft && (gameSurround[position.X, position.Y + 1] != 0) && (gameSurround[position.X + 1, position.Y] != 0));
        }
    }
}
