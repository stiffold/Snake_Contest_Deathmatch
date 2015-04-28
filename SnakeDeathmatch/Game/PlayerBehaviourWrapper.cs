using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Game
{
    public class PlayerBehaviourWrapper
    {
        private IPlayerBehavior _playerBehaviour1;
        private IPlayerBehaviour2 _playerBehaviour2;

        public PlayerBehaviourWrapper(object playerBehaviour)
        {
            var knownInterfaces = new[] { typeof(IPlayerBehaviour2), typeof(IPlayerBehavior) };

            _playerBehaviour2 = (playerBehaviour as IPlayerBehaviour2);
            _playerBehaviour1 = (playerBehaviour as IPlayerBehavior);

            if (_playerBehaviour2 == null && _playerBehaviour1 == null)
            {
                throw new Exception(string.Format("Object {0} must implement at least one of these interfaces: {1}.",
                    playerBehaviour.GetType().Name, string.Join(", ", knownInterfaces.Select(x => x.Name))));
            }
        }

        public void Init(int identifier, int playgroundSize, int x, int y, Direction direction)
        {
            if (_playerBehaviour2 != null)
                _playerBehaviour2.Init(identifier, playgroundSize, x, y, direction);
            else if (_playerBehaviour1 != null)
                _playerBehaviour1.Init((int)direction, identifier);
        }

        public Move GetNextMove(int[,] playground)
        {
            if (_playerBehaviour2 != null)
                return _playerBehaviour2.GetNextMove(playground);
            else if (_playerBehaviour1 != null)
                return (Move)_playerBehaviour1.NextMove(playground);
            throw new Exception();
        }

        public string Name
        {
            get
            {
                if (_playerBehaviour2 != null)
                    return _playerBehaviour2.Name;
                else if (_playerBehaviour1 != null)
                    return _playerBehaviour1.MyName();
                throw new Exception();
            }
        }
    }
}
