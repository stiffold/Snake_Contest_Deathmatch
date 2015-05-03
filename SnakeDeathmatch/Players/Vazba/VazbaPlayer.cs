using System;
using System.Collections.Generic;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public class VazbaPlayer : IPlayerBehaviour2
    {
        public string Name { get { return "Vazba"; } }

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _id = playerId;
            _p = new Point(x, y);
            _dir = direction;
            _size = playgroundSize;
        }

        private int _id;
        private Point _p = new Point(-1, -1);
        private Direction _dir;
        private Snakes _snakes = new Snakes();
        private int _size;
        private int[,] _playground;
        private int wtf = 18;

        public Move GetNextMove(int[,] playground)
        {
            _playground = playground;
            _snakes.Update(playground);
            
            Move move = DoNextMove();

            _dir = _dir.Apply(move);
            _p = _p.Move(_dir);

            return move;
        }

        private Move DoNextMove()
        {
            Next next = _p.GetNext(_dir, _playground);

            int depthLeft = next.LeftPoint.HasValue ? GetDepth(next.LeftPoint.Value, next.LeftDirection, 0) : 0;
            int depthStraight = (depthLeft != wtf) && next.StraightPoint.HasValue ? GetDepth(next.StraightPoint.Value, next.StraightDirection, 0) : 0;
            int depthRight = (depthLeft != wtf && depthStraight != wtf) && next.RightPoint.HasValue ? GetDepth(next.RightPoint.Value, next.RightDirection, 0) : 0;

            if (depthLeft >= depthStraight && depthLeft >= depthRight) return Move.Left;
            if (depthStraight >= depthLeft && depthStraight >= depthRight) return Move.Straight;
            return Move.Right;
        }

        private int GetDepth(Point p, Direction direction, int level)
        {
            _playground[p.X, p.Y] = _id;

            if (level >= wtf)
                return wtf;

            int result = level;

            Next next = p.GetNext(direction, _playground);

            if (next.LeftPoint.HasValue)
                result = Math.Max(result, GetDepth(next.LeftPoint.Value, next.LeftDirection, level + 1));

            if (next.StraightPoint.HasValue)
                result = Math.Max(result, GetDepth(next.StraightPoint.Value, next.StraightDirection, level + 1));

            if (next.RightPoint.HasValue)
                result = Math.Max(result, GetDepth(next.RightPoint.Value, next.RightDirection, level + 1));

            _playground[p.X, p.Y] = 0;

            return result;
        }
    }
}
