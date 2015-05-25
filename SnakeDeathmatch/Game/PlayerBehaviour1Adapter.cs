using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Game
{
    public class PlayerBehaviour1Adapter : IPlayerBehaviour2
    {
        private IPlayerBehavior _playerBehaviour1;

        public PlayerBehaviour1Adapter(IPlayerBehavior playerBehaviour1)
        {
            _playerBehaviour1 = playerBehaviour1;
        }

        public void Init(int identifier, int playgroundSize, int x, int y, Direction direction)
        {
            _playerBehaviour1.Init((int)direction, identifier);
        }

        public Move GetNextMove(int[,] playground)
        {
            return (Move)_playerBehaviour1.NextMove(playground);
        }

        public string Name { get { return _playerBehaviour1.MyName(); } }
    }
}
