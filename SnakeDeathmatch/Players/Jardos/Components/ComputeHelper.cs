using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public static bool IsMoveValid(Move move, Point point, Direction direction, int[,] battleground)
        {
            Point fakePoint = point;
            Direction fakeDirection = direction;

            ComputeHelper.Move(ref fakeDirection, ref fakePoint, move);
            if (Collider.Collission(fakeDirection, fakePoint, battleground))
            {
                return false;
            }
            return true;
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

        internal static bool SnakePointsIsValid(IEnumerable<SnakePoint> snakePoints ,int[,] playGround)
        {
            foreach (var sp in snakePoints)
            {
                if (Collider.Collission(sp.Direction, sp.Point, playGround))
                {
                    return false;
                }
            }
            return true;
        }

        public static void Cut(int count, IList<SnakePoint> snakePoints)
        {
            if (count > snakePoints.Count)
            {
                snakePoints.Clear();
                return;
            }
            int maxRound = snakePoints.Max(x => x.Round);
            for (int i = 0; i < count; i++)
            {
                var w = snakePoints.FirstOrDefault(x => x.Round == maxRound - i);
                snakePoints.Remove(w);
            }
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


        public static bool SnakePointsToTarget(SnakePoint first, Point targetPoint ,ref List<SnakePoint> snakePoints, int[,] playGround)
        {
            SnakePoint head = first;
            int i = 0;
            while (!targetPoint.Equals(head.Point) && i < 150)
            {
                i++;
                double minDistance = 200;
                SnakePoint choosenSnakePoint = null;
                foreach (var m in Enum.GetValues(typeof(Move)))
                {
                    var nextSnakePoint = Move(head, (Move)m);
                    if (nextSnakePoint.Point.Distance(targetPoint) < minDistance
                        && !Collider.Collission(nextSnakePoint.Direction, nextSnakePoint.Point, playGround))
                    {
                        choosenSnakePoint = nextSnakePoint;
                        minDistance = nextSnakePoint.Point.Distance(targetPoint);
                    }
                }
                if (choosenSnakePoint != null)
                {
                    snakePoints.Add(choosenSnakePoint);
                    head = choosenSnakePoint;
                }
                else
                {
                    snakePoints.Add(Move(head, Interface.Move.Right));
                }
            }

            if (i < 150)
            {
                return true;
            }
            return false;
        }

        public static IEnumerable<Move> OtherLRMoves(Move move)
        {
            foreach (Move m in Enum.GetValues(typeof(Move)))
            {
                if (m != move && m != Interface.Move.Straight)
                {
                    yield return m;
                }
            }
        }

        public static SnakePoint Move(SnakePoint lastSnakePoint, Move move)
        {
            Direction fakeDirection = lastSnakePoint.Direction;
            Point fakePoint = lastSnakePoint.Point;
            Move(ref fakeDirection, ref fakePoint, move);
            return new SnakePoint(lastSnakePoint.Round + 1, fakePoint, move, fakeDirection, true);
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

        public static void MergeArrays(int[,] from, int[,] to)
        {
            for (int x = 0; x < ComputeHelper.MaxSize; x++)
            {
                for (int y = 0; y < ComputeHelper.MaxSize; y++)
                {
                    var fromValue = from[x, y];
                    var toValue = to[x, y];
                    if (fromValue != 0 && fromValue != toValue)
                    {
                        to[x, y] = from[x, y];
                    }
                }
            }
        }
    }
}
