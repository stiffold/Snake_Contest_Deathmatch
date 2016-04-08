using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Jardos.Components
{
    public static class ComputeHelper
    {
        private static int _maxSize;
        private static int _myId;

        public static int MaxSize {
            get { return _maxSize; }
        }

        public static int MyId {
            get { return _myId; }
        }
        
        public static void SetDefaults(int maxSize, int myId)
        {
            _maxSize = maxSize;
            _myId = myId;
        }

        public static void Move(ref Direction dir, ref Point point, Move move)
        {
            dir = ApplyMove(dir, move);
            point.Update(dir);
        }

        public static IEnumerable<Move> OtherMoves(Move move)
        {
            foreach (Move m in Enum.GetValues(typeof(Move)))
            {
                if (m != move)
                {
                    yield return m;
                }
            }
        }

        public static Snake.SnakePoint Move(Snake.SnakePoint lastSnakePoint, Move move)
        {
            Direction fakeDirection = lastSnakePoint.Direction;
            Point fakePoint = lastSnakePoint.Point;
            Move(ref fakeDirection, ref fakePoint, move);
            return new Snake.SnakePoint(lastSnakePoint.Round + 1, fakePoint, move, fakeDirection, true);
        }

        private static Direction ApplyMove(Direction dir, Move move)
        {
            var directionInt = (int)dir;
            switch (move)
            {
                case Interface.Move.Right:
                    if (directionInt == 8)
                    {
                        return (Direction)1;
                    }
                    else
                    {
                        directionInt++;
                        return (Direction) directionInt;
                    }
                case Interface.Move.Left:
                    if (directionInt == 1)
                    {
                        return (Direction)8;
                    }
                    else
                    {
                        directionInt--;
                        return (Direction)directionInt;
                    }
            }

            return dir;
        }

        public static int Distance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;

            double distance = Math.Sqrt(dx * dx + dy * dy);

            return (int)Math.Round(distance, MidpointRounding.AwayFromZero);
        }

        public static Move CalculateMove(Direction from, Direction to)
        {
            if ((int)from == 1 && (int)to == 8) return Interface.Move.Right;
            if ((int)from == 8 && (int)to == 1) return Interface.Move.Left;
            if ((int)to < (int)from) return Interface.Move.Left; 
            if ((int)to > (int)from) return Interface.Move.Right;
            return Interface.Move.Straight;
        }

        public static int FreePointsCount(int[,] battleground, Point p)
        {
            int result = 0;
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                Point px = p;
                px.Update(dir);
                if (px.IsValid() && battleground[px.X, px.Y] == 0)
                {
                    result++;
                }
            }
            return result;
        }
    }
}
