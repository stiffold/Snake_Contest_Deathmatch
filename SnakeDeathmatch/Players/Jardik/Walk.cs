using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardik
{
    class Walk
    {
        private int _round;
        private Move _move;
        private Direction _direction;
        private Position _position;

        public Walk(int round, Move move, Direction direction, Position position)
        {
            _round = round;
            _move = move;
            _direction = direction;
            _position = position;
        }

        public int Round { get { return _round; } }
        public Move Move { get { return _move; } }
        public Direction Direction { get { return _direction; } }
        public Position Position { get { return _position; } }
    }

}
