using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardik
{
    public class Walk
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

        public Walk Copy()
        {
            return new Walk(_round,_move,_direction,_position);
        }

        public override string ToString()
        {
            return String.Format("{0},{1},X:{2},Y:{3},{4}",_round,_move,_position.X, _position.Y, _direction);
        }
    }

}
