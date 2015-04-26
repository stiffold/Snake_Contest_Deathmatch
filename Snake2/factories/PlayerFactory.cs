using System;
using System.Windows.Media;
using Snake2.enums;
using Snake2.game;
using Position = Snake2.game.Position;

namespace Snake2.factories
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

        public Player Create<TPlayerBehvaiour>(Color color) where TPlayerBehvaiour : IPlayerBehavior, new()
        {
            var behavior = new TPlayerBehvaiour();
            var position = RandomPosition();

            _game[position.X, position.Y] = _nextIdentifikator;

            var player = new Player(position, RandomDirection(), color, behavior, _nextIdentifikator);

            _nextIdentifikator++;

            return player;
        }

        public Player Create<TPlayerBehvaiour>(Color color, Position position) where TPlayerBehvaiour : IPlayerBehavior, new()
        {
            var behavior = new TPlayerBehvaiour();
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