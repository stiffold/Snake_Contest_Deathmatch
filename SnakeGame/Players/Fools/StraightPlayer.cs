using SnakeGame.Interface;

namespace SnakeGame.Players.Fools
{
    public class StraightPlayer : IPlayerBehaviour2
    {
        private FoolHelper _foolHelper;
        private Direction _direction;
        private Direction _desiredDirection;

        public StraightPlayer(Direction desiredDirection)
        {
            _desiredDirection = desiredDirection;
        }

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _foolHelper = new FoolHelper();
            _direction = direction;
        }

        public Move GetNextMove(int[,] playground)
        {
            if (_direction == _desiredDirection)
            {
                return Move.Straight;
            }
            else
            {
                _direction = _foolHelper.GetNextDirection(_direction, Move.Left);
                return Move.Left;
            }
        }

        public string Name { get { return "Straight Fool"; } }
    }
}
