using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Players.Vazba.Debug;
using SnakeDeathmatch.Players.Vazba.Helper;
using SnakeDeathmatch.Players.Vazba.PlaygroundAnalysis;

namespace SnakeDeathmatch.Players.Vazba.Strategies
{
    public class StrategyBarricade : IStrategy
    {
        private enum State { Begin, TurnLeftFirst, TurnRightFirst, End }
        private int _x1;
        private int _y1;
        private int _x2;
        private int _y2;

        private const int EnoughtSpace = 1200;

        private State _state = State.Begin;
        private IStrategy _strategy;

        public Analysis Analysis { get; private set; }

        public StrategyBarricade(Analysis analysis)
        {
            Analysis = analysis;
        }

        [ToDebug(typeof(PlayersIntArrayVisualizer))]
        public IntPlayground BarricadePlayground { get; private set; }

        public Move GetNextMove(IntPlayground playground, Snakes liveSnakes)
        {
            Snake me = liveSnakes.Me;

            if (_state == State.Begin)
            {
                int topRightQuadrantSpace = liveSnakes.Others.All(snake => snake.X < me.X || snake.Y > me.Y) ? (playground.Size - me.X) * me.Y : 0;
                int bottomRightQuadrantSpace = liveSnakes.Others.All(snake => snake.X < me.X || snake.Y < me.Y) ? (playground.Size - me.X) * (playground.Size - me.Y) : 0;
                int bottomLeftQuadrantSpace = liveSnakes.Others.All(snake => snake.X > me.X || snake.Y < me.Y) ? me.X * (playground.Size - me.Y) : 0;
                int topLeftQuadrantSpace = liveSnakes.Others.All(snake => snake.X > me.X || snake.Y > me.Y) ? me.X * me.Y : 0;

                var quadrantSpaces = new [] {topRightQuadrantSpace, bottomRightQuadrantSpace, bottomLeftQuadrantSpace, topLeftQuadrantSpace};

                if (topRightQuadrantSpace == quadrantSpaces.Max() && topRightQuadrantSpace >= EnoughtSpace) { _x1 = me.X - 1; _y1 = 0; _x2 = playground.Size - 1; _y2 = me.Y + 1; _state = State.TurnLeftFirst; };
                if (bottomRightQuadrantSpace == quadrantSpaces.Max() && bottomRightQuadrantSpace >= EnoughtSpace) { _x1 = me.X - 1; _y1 = me.Y - 1; _x2 = playground.Size - 1; _y2 = playground.Size - 1; _state = State.TurnLeftFirst; };
                if (bottomLeftQuadrantSpace == quadrantSpaces.Max() && bottomLeftQuadrantSpace >= EnoughtSpace) { _x1 = 0; _y1 = me.Y - 1; _x2 = me.X + 1; _y2 = playground.Size - 1; _state = State.TurnLeftFirst; };
                if (topLeftQuadrantSpace == quadrantSpaces.Max() && topLeftQuadrantSpace >= EnoughtSpace) { _x1 = 0; _y1 = 0; _x2 = me.X + 1; _y2 = me.Y + 1; _state = State.TurnLeftFirst; };

                if (_state == State.Begin)
                    _state = State.End;
            }
            
            if (_state == State.TurnLeftFirst)
            {
                if (_strategy == null)
                    _strategy = new Strategy5(Analysis, Move.Left);

                for (int y = 0; y < playground.Size; y++)
                {
                    for (int x = 0; x < playground.Size; x++)
                    {
                        if (!(_x1 <= x && x <= _x2 && _y1 <= y && y <= _y2))
                            playground[x, y] = -1;
                    }
                }

                if (me.X == 0 || me.X == playground.Size - 1 || me.Y == 0 || me.Y == playground.Size - 1)
                {
                    _state = State.TurnRightFirst;
                    _strategy = null;
                }

                if (liveSnakes.Others.Any(snake => _x1 <= snake.X && snake.X <= _x2 && _y1 <= snake.Y && snake.Y <= _y2))
                {
                    _state = State.End;
                    _strategy = null;
                }
            }
            
            if (_state == State.TurnRightFirst)
            {
                if (_strategy == null)
                    _strategy = new Strategy5(Analysis, Move.Right);

                for (int y = 0; y < playground.Size; y++)
                {
                    for (int x = 0; x < playground.Size; x++)
                    {
                        if (!(_x1 <= x && x <= _x2 && _y1 <= y && y <= _y2))
                            playground[x, y] = -1;
                    }
                }

                if (liveSnakes.Others.Any(snake => _x1 <= snake.X && snake.X <= _x2 && _y1 <= snake.Y && snake.Y <= _y2))
                {
                    _state = State.End;
                    _strategy = null;
                }
            }

            if (_strategy == null)
                _strategy = new Strategy5(Analysis, Move.Left);

            BarricadePlayground = playground;

            return _strategy.GetNextMove(playground, liveSnakes);
        }
    }
}
