using System;
using System.Windows.Media;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Game;
using Position = SnakeDeathmatch.Game.Position;

namespace SnakeDeathmatch.Game
{
    public class PlayerFactory
    {
        private int[,] _game;
        private int _length;
        private Random _rnd;

        //Pøíští použitý identifikátor
        private int _nextIdentifikator;

        public PlayerFactory(int[,] game, int length)
        {
            _game = game;
            _length = length;
            _rnd = new Random();
            _nextIdentifikator = 1;
        }

        public Player Create<TPlayerBehaviour>(Color color, int? x = null, int? y = null, Direction? direction = null)
            where TPlayerBehaviour : IPlayerBehavior, new()
        {
            var behavior = new TPlayerBehaviour();
            var position = RandomPosition();
            direction = direction ?? RandomDirection();

            if (x.HasValue) position = new Position(x.Value, position.Y);
            if (y.HasValue) position = new Position(position.X, y.Value);

            _game[position.X, position.Y] = _nextIdentifikator;

            var player = new Player(position, direction.Value, color, behavior, _nextIdentifikator);

            _nextIdentifikator++;

            return player;
        }

        public Player Create<TPlayerBehaviour>(Color color, Position position) where TPlayerBehaviour : IPlayerBehavior, new()
        {
            var behavior = new TPlayerBehaviour();
            var player = new Player(position, RandomDirection(), color, behavior, _nextIdentifikator);

            _game[position.X, position.Y] = _nextIdentifikator;

            _nextIdentifikator++;

            return player;
        }

        private Direction RandomDirection()
        {
            return (Direction)(_rnd.Next(1, 8));
        }

        private Position RandomPosition()
        {
            var position = new Position(_rnd.Next(4, _length - 4), _rnd.Next(4, _length - 4));
            return position;
        }
    }
}