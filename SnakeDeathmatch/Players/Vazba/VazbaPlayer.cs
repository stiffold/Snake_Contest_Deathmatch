using System;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;

namespace SnakeDeathmatch.Players.Vazba
{
    public class VazbaPlayer : IPlayerBehaviour2, IDebuggable
    {
        public string Name { get { return "Vazba"; } }

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            Strategy = new Strategy2();

            if (playerId != (int)PlayerId.Vazba)
                throw new ArgumentException(string.Format("Expected playerId {0} ({1}) but got {2}.", (int)PlayerId.Vazba, PlayerId.Vazba, playerId), "playerId");

            Snake me = new Snake((int)PlayerId.Vazba, x, y, direction);
            _snakes = new Snakes(me);
        }

        private Snakes _snakes;

        [ToDebug]
        public Snake Me { get { return _snakes.Me; } }

        [ToDebug]
        public IStrategy Strategy { get; private set; }

        public event BreakpointEventHandler Breakpoint;

        public Move GetNextMove(int[,] playground)
        {
            var intPlayground = new IntPlayground(playground);

            _snakes.Update(intPlayground);

            var snakes = _snakes.IsInitialized ? _snakes : new Snakes(_snakes.Me);
            if (Breakpoint != null)
                Breakpoint(snakes, new BreakpointEventArgs(VazbaBreakpointNames.SnakesUpdated));

            Move move = Strategy.GetNextMove(intPlayground, snakes);

            if (Breakpoint != null)
                Breakpoint(snakes, new BreakpointEventArgs(VazbaBreakpointNames.MoveEnd));

            return move;
        }
    }
}
