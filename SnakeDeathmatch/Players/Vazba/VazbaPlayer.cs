using System;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public class VazbaPlayer : IPlayerBehaviour2
    {
        public string Name { get { return "Vazba"; } }

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            if (playerId != (int)PlayerId.Vazba)
                throw new ArgumentException(string.Format("Expected playerId {0} ({1}) but got {2}.", (int)PlayerId.Vazba, PlayerId.Vazba, playerId), "playerId");

            Snake me = new Snake((int)PlayerId.Vazba, x, y, direction);
            _snakes = new Snakes(me);
        }

        private Snakes _snakes;
        private IStrategy _strategy = new Strategy1();

        public Move GetNextMove(int[,] playground)
        {
            _snakes.Update(playground);

            var snakes = _snakes.IsInitialized ? _snakes : new Snakes(_snakes.Me);

            return _strategy.GetNextMove(playground, snakes);
        }
    }
}
