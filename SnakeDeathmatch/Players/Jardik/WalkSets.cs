using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeDeathmatch.Players.Jardik
{   
    #region normal
    class Straight : WalkSetBase
    {
        public Straight(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Straight;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            TryGetCollission(Move.Straight, position, direction, gameSurrond);
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    class Left45 : WalkSetBase
    {
        public Left45(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Left45;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            TryGetCollissionTurn(Move.Left, position, direction, gameSurrond, 1);
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    class Left90 : WalkSetBase
    {
        public Left90(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Left90;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            TryGetCollissionTurn(Move.Left, position, direction, gameSurrond, 2);
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    class Left125 : WalkSetBase
    {
        public Left125(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Left125;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            TryGetCollissionTurn(Move.Left, position, direction, gameSurrond, 3);
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    class Left180 : WalkSetBase
    {
        public Left180(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Left180;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            TryGetCollissionTurn(Move.Left, position, direction, gameSurrond, 4);
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    class Right45 : WalkSetBase
    {
        public Right45(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Right45;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            TryGetCollissionTurn(Move.Right, position, direction, gameSurrond, 1);
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    class Right90 : WalkSetBase
    {
        public Right90(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Right90;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            TryGetCollissionTurn(Move.Right, position, direction, gameSurrond, 2);
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    class Right125 : WalkSetBase
    {
        public Right125(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Right125;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            TryGetCollissionTurn(Move.Right, position, direction, gameSurrond, 3);
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    class Right180 : WalkSetBase
    {
        public Right180(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Right180;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            TryGetCollissionTurn(Move.Right, position, direction, gameSurrond, 4);
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    #endregion

    #region Pro
    
    class RightGap : WalkSetBase
    {
        List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public RightGap(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {

            m.Add(new Tuple<int, Move>(1, Move.Right));
            m.Add(new Tuple<int, Move>(2, Move.Left));
            m.Add(new Tuple<int, Move>(3, Move.Left));
            
        }
        public override WalkSetType Type()
        {
            return WalkSetType.RightGap;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int nextMoves = 1;
            _nextMove = Move.Right;
            do
            {
                if (nextMoves > 3)
                {
                    _nextMove = Move.Straight;
                }
                else
                {
                    _nextMove = m.First(l => l.Item1 == nextMoves).Item2;
                }
                Update();
                nextMoves++;

            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    class LeftGap : WalkSetBase
    {
        List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public LeftGap(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {

            m.Add(new Tuple<int, Move>(1, Move.Left));
            m.Add(new Tuple<int, Move>(2, Move.Right));
            m.Add(new Tuple<int, Move>(3, Move.Right));

        }
        public override WalkSetType Type()
        {
            return WalkSetType.LeftGap;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int nextMoves = 1;
            _nextMove = Move.Left;
            do
            {
                if (nextMoves > 3)
                {
                    _nextMove = Move.Straight;
                }
                else
                {
                    _nextMove = m.First(l => l.Item1 == nextMoves).Item2;
                }
                Update();
                nextMoves++;

            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    class GetToWall : WalkSetBase
    {
        public GetToWall(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.GetToWall;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            throw new NotImplementedException();
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    class Roll100 : WalkSetBase
    {
        public Roll100(CollissionHelper ch, int myId) : base(ch, myId) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Roll100;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int iteration = 1;
            int limit = 1;
            _nextMove = direction.IsDiagonal() ? Move.Straight : Move.Right;
            do
            {
                if (iteration < limit)
                {
                    _nextMove = Move.Straight;
                }
                else if (iteration < limit + 1)
                {
                    _nextMove = Move.Right;
                }
                else
                {
                    iteration = 1;
                    limit++;
                }
                Update();
                iteration++;

            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    class RollLeft : WalkSetBase
    {
        public RollLeft(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.RollLeft;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int count = 1;
            _nextMove = Move.Left;
            do
            {
                Update();
                count++;
                if (count <= 4) _nextMove = Move.Left;
                if (count > 4) _nextMove = Move.Right;
                if (count == 8) count = 1;
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    class Safer : WalkSetBase
    {
        List<Move> _moveList = new List<Move> { Move.Straight, Move.Left, Move.Right };
        public Safer(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Safer;
        }

        protected override int CutCount()
        {
            return 0;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            _walks = MoveIt(_round, 9, position, direction, gameSurrond);
        }

        private List<Walk> MoveIt(int round, int moves, Position position, Direction direction, int[,] gameSurrond)
        {
            foreach (Move move in _moveList)
            {
                List<Walk> walks = GetWalks(round, position.Copy(), direction, move, moves, gameSurrond, new List<Walk>());
                if (walks.Count >= moves - 1)
                    return walks;
            }

            return MoveIt(round, --moves, position.Copy(), direction, gameSurrond);
        }

        private List<Walk> GetWalks(int round, Position position, Direction currentDirection, Move move, int moves, int[,] gameSurrond, List<Walk> walks)
        {
            moves--;
            if (moves <= 0) return walks;
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();


            Direction simulateDirection = currentDirection;
            Position simulatePosition = position.Copy();
            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);

            if (_ch.Collission(simulateDirection, simulateGameSurround, simulatePosition))
            {
                return new List<Walk>();
            }
            simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
            var myWalk = new Walk(round + walks.Count, move, simulateDirection, simulatePosition);
            walks.Add(myWalk);


            List<Walk> straightWalks = GetWalks(round, simulatePosition, simulateDirection, Move.Straight, moves, simulateGameSurround, walks.ToList());
            List<Walk> leftWalks = GetWalks(round, simulatePosition, simulateDirection, Move.Left, moves, simulateGameSurround, walks.ToList());
            List<Walk> rightWalks = GetWalks(round, simulatePosition, simulateDirection, Move.Right, moves, simulateGameSurround, walks.ToList());


            int max = new[] { leftWalks.Count, rightWalks.Count, straightWalks.Count }.Max();

            if (straightWalks.Count == max)
            {
                straightWalks.Add(myWalk);
                return straightWalks;
            }
            if (leftWalks.Count == max)
            {
                leftWalks.Add(myWalk);
                return leftWalks;
            }
            if (rightWalks.Count == max)
            {
                rightWalks.Add(myWalk);
                return rightWalks;
            }


            return new List<Walk>();
        }
    }

    class RollRight : WalkSetBase
    {
        public RollRight(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.RollRight;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int count = 1;
            _nextMove = Move.Right;

            do
            {
                Update();
                count++;
                if (count <= 4) _nextMove = Move.Right;
                if (count > 4) _nextMove = Move.Left;
                if (count == 8) count = 1;
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }

    }

    class Raper : WalkSetBase
    {
        public Raper(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Raper;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int count = 2;
            int straight = 3;
            _nextMove = Move.Right;
            do
            {
                Update();
                count++;
                if (count <= 5) _nextMove = Move.Right;
                if (count > 5 && count < 11) _nextMove = Move.Left;
                if (count >= 11 && count < 11 + straight) _nextMove = Move.Straight;
                if (count == 11 + straight)
                {
                    count = 1;
                    straight++;
                }

            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    class Bingo : WalkSetBase
    {
        public Bingo(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Bingo;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int count = 1;
            int cycle = 1;
            int straight = 2;
            _nextMove = Move.Right;
           do
            {
                Update();
                count++;
                if (count <= 2) _nextMove = Move.Right;
                if (count > 2 && count < 2 + straight) _nextMove = Move.Straight;
                if (count == 2 + straight)
                {
                    cycle++;
                    count = 1;
                    if (cycle> 4)
                    {
                        straight++;
                        cycle = 1;
                    }                    
                }

            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    class Randomer : WalkSetBase
    {
        public Randomer(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Randomer;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            Random r = new Random();
            _nextMove = (Move)r.Next(1, 4);
            do
            {
                Update();
                _nextMove = (Move)r.Next(1, 4);
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
        
    }

    class Snaker : WalkSetBase
    {
        public Snaker(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Snaker;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int count = 2;
            _nextMove = Move.Right;
            do
            {
                Update();
                count++;
                if (count <= 6) _nextMove = Move.Right;
                if (count > 6) _nextMove = Move.Left;
                if (count == 11) count = 1;
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    class Quaker : WalkSetBase
    {
        public Quaker(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Quaker;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int count = 1;
            _nextMove = Move.Straight;
            do
            {
                Update();
                count++;
                if (count <= 5) _nextMove = Move.Straight;
                if (count > 5 && count <= 7) _nextMove = Move.Right;
                if (count > 7 && count <= 11) _nextMove = Move.Straight;
                if (count > 11 && count <= 13) _nextMove = Move.Left;
                if (count > 13 && count <= 15) _nextMove = Move.Left;
                if (count > 15 && count <= 19) _nextMove = Move.Straight;
                if (count > 19 && count <= 21) _nextMove = Move.Right;
                if (count > 21) count = 1;
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    class SquareRight : WalkSetBase
    {
        public SquareRight(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.SquareRight;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int count = 1;
            _nextMove = Move.Straight;
            do
            {
                Update();
                count++;
                if (count <= 8) _nextMove = Move.Straight;
                if (count > 8 && count <= 10) _nextMove = Move.Right;
                if (count > 10 && count <= 18) _nextMove = Move.Straight;
                if (count > 18 && count <= 20) _nextMove = Move.Right;
                if (count > 20 && count <= 28) _nextMove = Move.Straight;
                if (count > 28 && count <= 30) _nextMove = Move.Right;
                if (count > 30 && count <= 38) _nextMove = Move.Straight;
                if (count > 38 && count <= 40) _nextMove = Move.Left;
                if (count > 40) _nextMove = Move.Straight;
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    class SquareLeft : WalkSetBase
    {
        public SquareLeft(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.SquareLeft;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int count = 1;
            _nextMove = Move.Straight;
            do
            {
                Update();
                count++;
                if (count <= 8) _nextMove = Move.Straight;
                if (count > 8 && count <= 10) _nextMove = Move.Left;
                if (count > 10 && count <= 18) _nextMove = Move.Straight;
                if (count > 18 && count <= 20) _nextMove = Move.Left;
                if (count > 20 && count <= 28) _nextMove = Move.Straight;
                if (count > 28 && count <= 30) _nextMove = Move.Left;
                if (count > 30 && count <= 38) _nextMove = Move.Straight;
                if (count > 38 && count <= 40) _nextMove = Move.Right;
                if (count > 40) _nextMove = Move.Straight;
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    class FunkyTerror : WalkSetBase
    {
        List<Tuple<int, Move>> left_nextMoves = new List<Tuple<int, Move>>();
        List<Tuple<int, Move>> right_nextMoves = new List<Tuple<int, Move>>();

        public FunkyTerror(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {

            left_nextMoves.Add(new Tuple<int, Move>(1, Move.Left));
            left_nextMoves.Add(new Tuple<int, Move>(2, Move.Left));
            left_nextMoves.Add(new Tuple<int, Move>(3, Move.Straight));
            left_nextMoves.Add(new Tuple<int, Move>(4, Move.Right));
            left_nextMoves.Add(new Tuple<int, Move>(5, Move.Right));
            left_nextMoves.Add(new Tuple<int, Move>(6, Move.Right));
            left_nextMoves.Add(new Tuple<int, Move>(7, Move.Right));
            left_nextMoves.Add(new Tuple<int, Move>(8, Move.Straight));
            left_nextMoves.Add(new Tuple<int, Move>(9, Move.Straight));
            left_nextMoves.Add(new Tuple<int, Move>(10, Move.Straight));
            left_nextMoves.Add(new Tuple<int, Move>(11, Move.Right));
            left_nextMoves.Add(new Tuple<int, Move>(12, Move.Right));

            right_nextMoves.Add(new Tuple<int, Move>(1, Move.Right));
            right_nextMoves.Add(new Tuple<int, Move>(2, Move.Right));                      
            right_nextMoves.Add(new Tuple<int, Move>(3, Move.Straight));
            right_nextMoves.Add(new Tuple<int, Move>(4, Move.Left));
            right_nextMoves.Add(new Tuple<int, Move>(5, Move.Left));
            right_nextMoves.Add(new Tuple<int, Move>(6, Move.Left));
            right_nextMoves.Add(new Tuple<int, Move>(7, Move.Left));
            right_nextMoves.Add(new Tuple<int, Move>(8, Move.Straight));
            right_nextMoves.Add(new Tuple<int, Move>(9, Move.Straight));
            right_nextMoves.Add(new Tuple<int, Move>(10, Move.Straight));
            right_nextMoves.Add(new Tuple<int, Move>(11, Move.Left));
            right_nextMoves.Add(new Tuple<int, Move>(12, Move.Left));
        }
        public override WalkSetType Type()
        {
            return WalkSetType.FunkyTerror;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int straightMoves = 5;
            int nextMoves = 1;
            bool left = true;
            bool doStraightMoves = false;
            _nextMove = direction.IsDiagonal() ? Move.Straight : Move.Right;


            do
            {
                if (doStraightMoves)
                {
                    _nextMove = Move.Straight;
                }
                else
                {
                    _nextMove = left ? left_nextMoves.First(l => l.Item1 == nextMoves).Item2 : right_nextMoves.First(l => l.Item1 == nextMoves).Item2;
                }
                Update();
                nextMoves++;
                if (nextMoves > 12 && doStraightMoves == false)
                {
                    doStraightMoves = true;
                    nextMoves = 1;
                }

                if (nextMoves > straightMoves && doStraightMoves)
                {
                    doStraightMoves = false;
                    straightMoves += 6;
                    left = !left;
                    nextMoves = 1;
                }
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }


    }

    class VazbaKiller : WalkSetBase
    {
        List<Tuple<int, Move>> _nextMoves = new List<Tuple<int, Move>>();
        public VazbaKiller(CollissionHelper ch, int myId) : base(ch, myId, true) { }
        public override WalkSetType Type()
        {
            return WalkSetType.Killer;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int step = 1;
            Position vazbaPosition = new Position(0, 0);
            for (int x = 0; x < _ch.Max; x++)
            {
                for (int y = 0; y < _ch.Max; y++)
                {
                    if (gameSurrond[x, y] == 2)
                    {
                        vazbaPosition = new Position(x, y);
                    }
                }
            }
            double distance = vazbaPosition.Distance(position);

            if (distance > 75)
            {
                return;
            }

            Direction targetDirection = position.PositionDiagonalDirection(vazbaPosition);
            var afterPosition = position.Copy();
            foreach (var m in direction.MovesToGetDirection(targetDirection))
            {
                _nextMoves.Add(new Tuple<int,Move>(step,m));
                step++;
            }
            double minDistance = 300;
            while (vazbaPosition.Distance(afterPosition) < minDistance)
            {
                minDistance = vazbaPosition.Distance(afterPosition);
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Straight));
                afterPosition.Update(targetDirection);                
                step++;
            }

            for (int s = 0; s < distance/2.5; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //right
            for (int s = 0; s < 2; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Right));
                step++;
            }

            //straight
            for (int s = 0; s < distance / 3 /2; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //left
            for (int s = 0; s < 2; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Left));
                step++;
            }

            //straight
            for (int s = 0; s < distance / 3; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //left
            for (int s = 0; s < 2; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Left));
                step++;
            }

            //straight
            for (int s = 0; s < distance / 3; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //left
            for (int s = 0; s < 2; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Left));
                step++;
            }

            //straight
            for (int s = 0; s < distance / 3; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //left
            for (int s = 0; s < 2; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Left));
                step++;
            }

            //straight
            for (int s = 0; s < (distance / 3 / 2) -4; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //right
            for (int s = 0; s < 2; s++)
            {
                _nextMoves.Add(new Tuple<int, Move>(step, Move.Right));
                step++;
            }

            int count = 1;
            _nextMove = _nextMoves.First(x => x.Item1 == count).Item2;

            do
            {
                Update();
                count++;
                _nextMove = count <= _nextMoves.Count ? _nextMoves.First(x => x.Item1 == count).Item2 : Move.Straight;
            }while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
            
        }

        protected override int CutCount()
        {
            return 3;
        }


    }

    class StrikeLeft : WalkSetBase
    {
        List<Tuple<int, Move>> _nextMoveList = new List<Tuple<int, Move>>();
        

        public StrikeLeft(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            _nextMoveList.Add(new Tuple<int, Move>(1, Move.Left));
            _nextMoveList.Add(new Tuple<int, Move>(2, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(3, Move.Left));
            _nextMoveList.Add(new Tuple<int, Move>(4, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(5, Move.Left));
            _nextMoveList.Add(new Tuple<int, Move>(6, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(7, Move.Left));
            _nextMoveList.Add(new Tuple<int, Move>(8, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(9, Move.Left));
            _nextMoveList.Add(new Tuple<int, Move>(10, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(11, Move.Left));
            _nextMoveList.Add(new Tuple<int, Move>(12, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(13, Move.Right));
            _nextMoveList.Add(new Tuple<int, Move>(14, Move.Right));
        }
        public override WalkSetType Type()
        {
            return WalkSetType.StrikeLeft;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            _nextMove = _nextMoveList.First(x => x.Item1 == 1).Item2;
            int count = 1;
            do
            {
                _nextMove = count <= _nextMoveList.Count ? _nextMoveList.First(l => l.Item1 == count).Item2 : Move.Straight;
                Update();
                count++;
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    internal class StrikeRight : WalkSetBase
    {
        private List<Tuple<int, Move>> _nextMoveList = new List<Tuple<int, Move>>();

        public StrikeRight(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            _nextMoveList.Add(new Tuple<int, Move>(1, Move.Right));
            _nextMoveList.Add(new Tuple<int, Move>(2, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(3, Move.Right));
            _nextMoveList.Add(new Tuple<int, Move>(4, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(5, Move.Right));
            _nextMoveList.Add(new Tuple<int, Move>(6, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(7, Move.Right));
            _nextMoveList.Add(new Tuple<int, Move>(8, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(9, Move.Right));
            _nextMoveList.Add(new Tuple<int, Move>(10, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(11, Move.Right));
            _nextMoveList.Add(new Tuple<int, Move>(12, Move.Straight));
            _nextMoveList.Add(new Tuple<int, Move>(13, Move.Left));
            _nextMoveList.Add(new Tuple<int, Move>(14, Move.Left));
        }

        public override WalkSetType Type()
        {
            return WalkSetType.StrikeRight;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            int count = 1;
            _nextMove = _nextMoveList.First(x => x.Item1 == 1).Item2;
            do
            {
                _nextMove = count <= _nextMoveList.Count ? _nextMoveList.First(l => l.Item1 == count).Item2 : Move.Straight;
                Update();
                count++;
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
        }

        protected override int CutCount()
        {
            return 3;
        }
    }
    #endregion

}
