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
        public Snake()
        {
            MoveStats = new MoveStat();
        }

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
         
        public MoveStat MoveStats { get; set; }

        public void Evaluate(int round, Point point, Direction direction, int[,] playGround)
        {
            _round = round;
            LastMove = ComputeHelper.CalculateMove(LastDirection, direction);
            HeadPoint = point;
            LastDirection = direction;
            MoveStats.Update(LastMove);
            EvaluateRotator(playGround);
            var realPoint = new SnakePoint(round, point, LastMove, direction, false);
            WayPoints.Add(realPoint);

            //ComputeFuture(round, playGround, realPoint);
        }

        private void ComputeFuture(int round, int[,] playGround, SnakePoint realPoint)
        {
            var sameFuturePoint = WayPoints.FirstOrDefault(x => x.IsFuturePoint && x.Point.Equals(realPoint.Point));

            if (sameFuturePoint != null)
            {
                _bingoCount++;
                if (sameFuturePoint.Round > round)
                {
                    int diff = round - sameFuturePoint.Round;
                    WayPoints.RemoveAll(x => x.Round <= round - diff && x.IsFuturePoint);
                    foreach (var p in WayPoints.Where(x => x.IsFuturePoint))
                    {
                        p.Round += diff;
                    }
                }
            }
            else
            {
                _missCount++;
                if (_missCount > 5)
                {
                    _primariMove = GetNextPrimaryMove();
                    WayPoints.RemoveAll(x => x.IsFuturePoint);
                    _missCount = 0;
                }
            }

            if (WayPoints.Count(x => x.IsFuturePoint) < 100)
            {
                AddFuturePoints(100 - WayPoints.Count(x => x.IsFuturePoint), playGround);
            }

            WayPoints.RemoveAll(x => x.Round <= round && x.IsFuturePoint);

            FutureSucces = (_bingoCount / _round) * 100;
        }

        private Move GetNextPrimaryMove()
        {
            int primaryMoveNumber = (int)_primariMove;
            if (primaryMoveNumber < 3)
            {
                primaryMoveNumber++;
            }
            else if (primaryMoveNumber == 2)
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
                SnakePoint nextPoint = ComputeHelper.Move(lastSnakePoint, _primariMove);
                SnakePoint nextPoint2 = ComputeHelper.Move(nextPoint, _primariMove);
                if (!Collider.Collission(nextPoint.Direction, nextPoint.Point, playGround) && 
                    !Collider.Collission(nextPoint2.Direction, nextPoint2.Point, playGround) && 
                    !WayPoints.Any(x=>x.IsFuturePoint && x.Point.Equals(nextPoint.Point)))
                {
                    WayPoints.Add(nextPoint);
                }
                else
                {
                    foreach (Move m in MoveStats.OtherMoves(_primariMove))
                    {
                        nextPoint = ComputeHelper.Move(lastSnakePoint, m);
                        nextPoint2 = ComputeHelper.Move(nextPoint, _primariMove);
                        if (!Collider.Collission(nextPoint.Direction, nextPoint.Point, playGround) &&
                            !Collider.Collission(nextPoint2.Direction, nextPoint2.Point, playGround) &&
                            !WayPoints.Any(x => x.IsFuturePoint && x.Point.Equals(nextPoint.Point))) break;
                    }
                    WayPoints.Add(nextPoint);
                }                
            }
        }

        public class MoveStat
        {
            private Dictionary<Move, int> _moves = new Dictionary<Move, int>();

            public MoveStat()
            {
                _moves.Add(Move.Straight, 0);
                _moves.Add(Move.Left, 0);
                _moves.Add(Move.Right, 0);
            }

            public void Update(Move move)
            {
                _moves[move] = _moves[move] + 1;

                var sorted  = _moves.OrderByDescending(x => x.Value).ToList();

                Primary = sorted[0].Key;
                Secondary = sorted[1].Key;
                Low = sorted[2].Key;
            }

            public IEnumerable<Move> OtherMoves(Move move)
            {
                if (Primary != move) yield return Primary;
                if (Secondary != move) yield return Secondary;
                if (Low != move) yield return Low;
            }

            public Move Primary { get; set; }
            public Move Secondary { get; set; }
            public Move Low { get; set; }
        }
    }
}
