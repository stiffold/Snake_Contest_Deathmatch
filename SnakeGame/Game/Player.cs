using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeGame.Interface;

namespace SnakeGame.Game
{
    public class Player
    {
        private PlayerBehaviourWrapper _playerBehaviour;

        public string Name { get; private set; }
        public int Identifier { get; private set; }
        public Position Position { get; private set; }
        public Direction Direction { get; private set; }
        public object Color { get; private set; }
        public int Score { get; private set; }

        //public Player(Position position, Direction direction, Color color, object playerBehaviour, int identificator, int playgroundSize)
        //{
        //    _playerBehaviour = new PlayerBehaviourWrapper(playerBehaviour);
        //    Name = _playerBehaviour.Name;
        //    Position = position;
        //    Direction = direction;
        //    Color = color;
        //    Identifier = identificator;
        //    State = PlayerState.Playing;

        //    _playerBehaviour.Init(Identifier, playgroundSize, position.X, position.Y, Direction);
        //}

        public Player(Position position, Direction direction, object color, object playerBehaviour, int identificator, int playgroundSize)
        {
            _playerBehaviour = new PlayerBehaviourWrapper(playerBehaviour);
            Name = _playerBehaviour.Name;
            Position = position;
            Direction = direction;
            Color = color;
            Identifier = identificator;
            State = PlayerState.Playing;

            _playerBehaviour.Init(Identifier, playgroundSize, position.X, position.Y, Direction);
        }

        public PlayerState State { get; set; }

        public Move NextMove(int[,] playground)
        {
            Move move = _playerBehaviour.GetNextMove(playground);
            Direction newDirection = GetNewDirection(Direction, move);
            Direction = newDirection;
            Position.Update(newDirection);
            Score++;
            return move;
        }

        private Direction GetNewDirection(Direction oldDirection, Move move)
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
