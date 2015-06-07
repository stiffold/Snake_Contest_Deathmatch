using System.Collections.Generic;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;
using System;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Strategy4 : IStrategy, IDebuggable
    {
        private const int WTF = 6;
        private int _size;
        private bool _deathPlaygroundsCreated;

        public event BreakpointEventHandler Breakpoint;

        public Move GetNextMove(IntPlayground playground, Snakes liveSnakes)
        {
            _Playground = playground;
            _Track = playground.Clone();
            _size = playground.Size;
            Snake me = liveSnakes.Me;

            if (!_deathPlaygroundsCreated)
                CreateDeathPlaygrounds();

            RecalculateDeathPlaygrounds();
            if (Breakpoint != null)
                Breakpoint(this, new BreakpointEventArgs(VazbaBreakpointNames.Strategy4DeathPlaygroundsRecalculated));


            Next next = me.GetNext(playground);

            int depthLeft = next.Left.HasValue ? GetDepth(next.Left.Value, 0) : 0;
            int depthStraight = (depthLeft != WTF) && next.Straight.HasValue ? GetDepth(next.Straight.Value, 0) : 0;
            int depthRight = (depthLeft != WTF && depthStraight != WTF) && next.Right.HasValue ? GetDepth(next.Right.Value, 0) : 0;

            _Track = null;
            _Playground = null;

            if (depthLeft >= depthStraight && depthLeft >= depthRight) return Move.Left;
            if (depthStraight >= depthLeft && depthStraight >= depthRight) return Move.Straight;
            return Move.Right;
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

        #region #DeathPlaygrounds

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground _Playground { get; private set; }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground _Track { get; private set; }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground Death_Top { get; private set; }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground Death_TopRight { get; private set; }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground Death_Right { get; private set; }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground Death_BottomRight { get; private set; }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground Death_Bottom { get; private set; }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground Death_BottomLeft { get; private set; }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground Death_Left { get; private set; }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground Death_TopLeft { get; private set; }

        #endregion

        private void CreateDeathPlaygrounds()
        {
            Death_Top = new IntPlayground(new int[_size, _size]);
            Death_TopRight = new IntPlayground(new int[_size, _size]);
            Death_Right = new IntPlayground(new int[_size, _size]);
            Death_BottomRight = new IntPlayground(new int[_size, _size]);
            Death_Bottom = new IntPlayground(new int[_size, _size]);
            Death_BottomLeft = new IntPlayground(new int[_size, _size]);
            Death_Left = new IntPlayground(new int[_size, _size]);
            Death_TopLeft = new IntPlayground(new int[_size, _size]);

            _deathPlaygroundsCreated = true;
        }

        private void RecalculateDeathPlaygrounds()
        {
            // ...
        }
    }
}
