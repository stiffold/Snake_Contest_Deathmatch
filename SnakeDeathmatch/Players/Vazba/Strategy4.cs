using System.Collections.Generic;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;
using System;
using System.Linq;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Strategy4 : IStrategy, IDebuggable
    {
        private const int WTF = 14;
        private int _size;
        private Snakes _snakes;
        private int _step;

        [ToDebug]
        public DeathField DeathField { get; private set; }

        public event BreakpointEventHandler Breakpoint;

        public Move GetNextMove(IntPlayground playground, Snakes liveSnakes)
        {
            _Playground = playground;
            _Track = playground.Clone();
            _size = playground.Size;
            _snakes = liveSnakes;

            Snake me = liveSnakes.Me;

            if (DeathField == null)
                DeathField = new DeathField(_size);

            IEnumerable<DeathField.Point> newPoints = liveSnakes.IncludingMe.Select(snake => new DeathField.Point(snake.X, snake.Y));
            DeathField.Update(newPoints);
            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4DeathPlaygroundsRecalculated));

            if (true)
            {
                Next next = me.GetNext(playground);

                int depthLeft = next.Left.HasValue ? GetDepth(next.Left.Value, 0) : 0;
                int depthStraight = (depthLeft != WTF) && next.Straight.HasValue ? GetDepth(next.Straight.Value, 0) : 0;
                int depthRight = (depthLeft != WTF && depthStraight != WTF) && next.Right.HasValue ? GetDepth(next.Right.Value, 0) : 0;

                _Track = null;
                _Playground = null;

                _step++;
                if (_step / 100 == 0)
                    if (Breakpoint != null)
                        Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4StopEvery100Steps));

                if (depthLeft >= depthStraight && depthLeft >= depthRight) return Move.Left;
                if (depthStraight >= depthLeft && depthStraight >= depthRight) return Move.Straight;
                return Move.Right;
            }
            else
            {
                _Track = null;
                _Playground = null;

                Move move = GetNextStepForPasticka();
                _step++;
                if (_step / 100 == 0)
                    if (Breakpoint != null)
                        Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4StopEvery100Steps));
                return move;
            }
        }

        private Move GetNextStepForPasticka()
        {
            if (_step == 0) return Move.Straight;
            else if (_step == 1) return Move.Right;
            else if (_step == 2) return Move.Straight;
            else if (_step == 3) return Move.Straight;
            else if (_step == 4) return Move.Straight;
            else if (_step == 5) return Move.Left;
            else if (_step == 6) return Move.Left;
            else if (_step == 7) return Move.Straight;
            else if (_step == 8) return Move.Left;
            else if (_step == 9) return Move.Left;
            else if (_step == 10) return Move.Straight;
            else
                return Move.Straight;
        }

        private int GetDepth(Snake me, int level)
        {
            if (level >= WTF)
                return WTF;

            _Track[me.X, me.Y] = PlayersIntArrayVisualizer.TrackId;
            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4TrackChanged));

            int result = level;

            Next next = me.GetNext(_Track);

            if (next.Left.HasValue)
                result = Math.Max(result, GetDepth(next.Left.Value, level + 1));

            if (result < WTF && next.Straight.HasValue)
                result = Math.Max(result, GetDepth(next.Straight.Value, level + 1));

            if (result < WTF && next.Right.HasValue)
                result = Math.Max(result, GetDepth(next.Right.Value, level + 1));

            _Track[me.X, me.Y] = 0;

            return result;
        }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground _Playground { get; private set; }

        //[ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground _Track { get; private set; }
    }
}
