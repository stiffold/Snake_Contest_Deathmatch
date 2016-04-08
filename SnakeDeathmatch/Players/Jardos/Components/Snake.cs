using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Jardos.Components
{
    /// <summary>
    /// reprezentuje hada na hrací ploše
    /// </summary>
    public class Snake
    {
        private bool _rotationLeaved = false;
        private float _bingoCount = 0;
        private float _missCount = 0;
        private float _round;
        private Move _primariMove = Move.Straight;  

        public Point HeadPoint { get; set; }

        public Point StartPoint { get; set; }

        public Direction LastDirection { get; set; }

        public Move LastMove { get; set; }

        public int Id { get; set; }
        [ToDebug]
        public bool Live { get; set; }
        [ToDebug]
        public bool IsRotator { get; set; }

        [ToDebug]
        public float FutureSucces { get; set; }

        [ToDebug]
        public int Future { get; set; }

        public List<SnakePoint> WayPoints { get; set; }        

        public void Evaluate(int round, Point point, Direction direction, int[,] playGround)
        {
            _round = round;
            LastMove = ComputeHelper.CalculateMove(LastDirection, direction);
            HeadPoint = point;
            LastDirection = direction;
            var realPoint = new SnakePoint(round, point, LastMove, direction, false);
            var sameFuturePoint = WayPoints.FirstOrDefault(x => x.IsFuturePoint && x.Point.Equals(realPoint.Point));
            WayPoints.Add(realPoint);
            EvaluateRotator(playGround);
            
            if (sameFuturePoint!= null)
            {
                _bingoCount++;
                if (sameFuturePoint.Round > round)
                {
                    int diff =  round - sameFuturePoint.Round;
                    WayPoints.RemoveAll(x => x.Round <= round - diff && x.IsFuturePoint);
                    foreach (var p in WayPoints.Where(x=>x.IsFuturePoint))
                    {
                        p.Round += diff;
                    }
                }
            }
            else
            {
                _missCount++;
                if (_missCount > 3)
                {
                    _primariMove = GetNextPrimaryMove();
                    WayPoints.RemoveAll(x=>x.IsFuturePoint);
                    _missCount = 0;
                }
            }

            if (WayPoints.Count(x=>x.IsFuturePoint) < 200)
            {
                AddFuturePoints(200 - WayPoints.Count(x => x.IsFuturePoint), playGround);
            }

            WayPoints.RemoveAll(x => x.Round <= round && x.IsFuturePoint);

            FutureSucces =(_bingoCount / _round)*100;
        }

        private Move GetNextPrimaryMove()
        {
            int primaryMoveNumber = (int)_primariMove;
            if (primaryMoveNumber < 3)
            {
                primaryMoveNumber++;
            }
            else
            {
                primaryMoveNumber = 1;
            }
            return (Move)primaryMoveNumber;
        }

        private void AddFuturePoints(int max, int[,] playGround)
        {
            for (int i = 0; i < max; i++)
            {
                NextFuturePoint(playGround);
            }
        }

        private void EvaluateRotator(int [,] playGround)
        {
            if (WayPoints.Count(x => !x.IsFuturePoint) > 8 && !_rotationLeaved)
            {
                var minY = WayPoints.Where(x => !x.IsFuturePoint).Min(x => x.Point.Y);
                var maxY = WayPoints.Where(x => !x.IsFuturePoint).Max(x => x.Point.Y);
                var minX = WayPoints.Where(x => !x.IsFuturePoint).Min(x => x.Point.X);
                var maxX = WayPoints.Where(x => !x.IsFuturePoint).Max(x => x.Point.X);

                if (Math.Abs((maxX - minX) - (maxY -minY)) < 3 && Math.Abs(maxX - minX) < 70)
                {
                    if (IsRotator == false)
                    {
                        WayPoints.RemoveAll(x => x.IsFuturePoint);
                        AddFuturePoints(100, playGround);
                    }
                    IsRotator = true;                   
                }
                else
                {
                    if (IsRotator == true)
                    {
                        _rotationLeaved = true;
                    }
                    IsRotator = false;
                }
            }
        }

        public void NextFuturePoint(int[,] playGround)
        {
            var lastSnakePoint = WayPoints.OrderByDescending(x => x.Round).First();
            if (IsRotator)
            {
                var lastChangeMove = WayPoints.Where(x => x.Move != Move.Straight).OrderByDescending(x => x.Round).First();
                _primariMove = lastChangeMove.Move;
                int straightCount = WayPoints.OrderByDescending(x => x.Round).TakeWhile(x => x.Round < lastChangeMove.Round && x.Move == Move.Straight).Count();
                int nowStraightCount = WayPoints.Count(x=>x.Round > lastChangeMove.Round && x.Move == Move.Straight);
                if (nowStraightCount < straightCount)
                {
                    WayPoints.Add(ComputeHelper.Move(lastSnakePoint, Move.Straight));
                }
                else
                {
                    var changeMoveSnakePoint = ComputeHelper.Move(lastSnakePoint, lastChangeMove.Move);
                    if (WayPoints.Any(x=>x.Point.Equals(changeMoveSnakePoint.Point)))
                    {
                        WayPoints.Add(ComputeHelper.Move(lastSnakePoint, Move.Straight));
                    }
                    else
                    {
                        WayPoints.Add(changeMoveSnakePoint);
                    }                    
                }
            }
            else
            {
                Snake.SnakePoint nextPoint = ComputeHelper.Move(lastSnakePoint, _primariMove);
                if (!Collider.Collission(nextPoint.Direction, nextPoint.Point, playGround) && !WayPoints.Any(x=>x.IsFuturePoint && x.Point.Equals(nextPoint.Point)))
                {
                    WayPoints.Add(nextPoint);
                }
                else
                {
                    foreach (Move m in ComputeHelper.OtherMoves(_primariMove))
                    {
                        nextPoint = ComputeHelper.Move(lastSnakePoint, m);
                        if (!Collider.Collission(nextPoint.Direction, nextPoint.Point, playGround) && !WayPoints.Any(x => x.IsFuturePoint && x.Point.Equals(nextPoint.Point))) break;
                    }
                    WayPoints.Add(nextPoint);
                }                
            }
        }

        public class SnakePoint
        {
            public SnakePoint(int round, Point point, Move move, Direction direction, bool isFuturePoint)
            {
                Round = round;
                Point = point;
                Move = move;
                Direction = direction;
                IsFuturePoint = isFuturePoint;
            }

            public int Round { get; set; }

            public Point Point { get; set; }

            public Move Move { get; set; }

            public Direction Direction { get; set; }

            public bool IsFuturePoint { get; set; }
        }
    }
}
