using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Snake2.enums;

namespace Snake2.game
{
    public class Player
    {
        private string _name;
        private Position _position;
        private Direction _direction;
        private Color _color;
        private IPlayerBehavior _playerBehavior;
        private int _identificator;
        private int _score;

        public Player(Position position, Direction direction, Color color, IPlayerBehavior playerBehavior, int identificator)
        {
            _name = playerBehavior.MyName();
            _position = position;
            _direction = direction;
            _color = color;
            _playerBehavior = playerBehavior;
            _identificator = identificator;
            State = PlayerState.Playing;

            playerBehavior.Init((int)_direction, _identificator);
        }

        public string Name
        {
            get { return _name; }
        }
        public int Identificator
        {
            get { return _identificator; }
        }
        public Position Position
        {
            get { return _position; }
        }
        public Direction MyDirection
        {
            get { return _direction; }
        }
        public Color Color
        {
            get { return _color; }
        }
        public IPlayerBehavior Behavior
        {
            get { return _playerBehavior; }
        }
        public int Score
        {
            get { return _score; }
        }

        public PlayerState State { get; set; }

        public Move NextMove (int [,] gameSurrond)
        {
            Move myMove;
            int move = Behavior.NextMove(gameSurrond);
            myMove = move > 0 && move < 4 ? (Move) move : Move.Straight;
            Direction newDirection = GetNewDirection(_direction, myMove);
            _direction = newDirection;
            Position.Update(newDirection);
            _score++;
            return myMove;
        }

        private Direction GetNewDirection(Direction oldDirection , Move move)
        {
            if (move == Move.Left)
            {
                switch (oldDirection)
                {
                    case Direction.Top: return Direction.TopLeft;
                    case Direction.TopRight: return Direction.Top;
                    case Direction.Right: return Direction.TopRight;
                    case Direction.BottomRight: return Direction.Right;
                    case Direction.Bottom: return Direction.BottomRight;
                    case Direction.BottomLeft: return Direction.Bottom;
                    case Direction.Left: return Direction.BottomLeft;
                    case Direction.TopLeft: return Direction.Left;
                }
            }

            if (move == Move.Right)
            {
                switch (oldDirection)
                {
                    case Direction.Top: return Direction.TopRight;
                    case Direction.TopRight: return Direction.Right;
                    case Direction.Right: return Direction.BottomRight;
                    case Direction.BottomRight: return Direction.Bottom;
                    case Direction.Bottom: return Direction.BottomLeft;
                    case Direction.BottomLeft: return Direction.Left;
                    case Direction.Left: return Direction.TopLeft;
                    case Direction.TopLeft: return Direction.Top;
                }
            }

            return oldDirection;
        }

    }
}
