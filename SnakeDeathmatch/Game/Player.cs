using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;
using System.Diagnostics;

namespace SnakeDeathmatch.Game
{
    public class Player
    {
        IPlayerBehaviour2 _playerBehaviour;
        private long _miliseconds;

        public string Name { get; private set; }
        public int Identifier { get; private set; }
        public Position Position { get; private set; }
        public Direction Direction { get; private set; }
        public object Color { get; private set; }
        public int Score { get; private set; }
        public long TotalMiliseconds { get { return _miliseconds; } }


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

        public Player(Position position, Direction direction, object color, IPlayerBehaviour2 playerBehaviour, int identificator, int playgroundSize)
        {
            _playerBehaviour = playerBehaviour;
            Name = _playerBehaviour.Name;
            Position = position;
            Direction = direction;
            Color = color;
            Identifier = identificator;
            State = PlayerState.Playing;
            _miliseconds =0;

            _playerBehaviour.Init(Identifier, playgroundSize, position.X, position.Y, Direction);
        }

        public PlayerState State { get; set; }

        public Move NextMove(int[,] playground)
        {
            var sw = Stopwatch.StartNew();
            Move move = _playerBehaviour.GetNextMove(playground);
            sw.Stop();
            _miliseconds += sw.ElapsedMilliseconds;
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

        public override string ToString()
        {
            return Name;
        }

        public string PositionAndDirection { get { return string.Format("[{0},{1}], {2}", Position.X, Position.Y, Direction); } }

        [ToDebug]
        public IPlayerBehaviour2 Details { get { return _playerBehaviour; } }
    }
}
