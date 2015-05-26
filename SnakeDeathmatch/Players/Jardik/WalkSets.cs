using System;
using System.Collections.Generic;
using System.Linq;

namespace SnakeDeathmatch.Players.Jardik
{

    #region normal

    internal class Straight : WalkSetBase
    {
        public Straight(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

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

    internal class Left45 : WalkSetBase
    {
        public Left45(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

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

    internal class Left90 : WalkSetBase
    {
        public Left90(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

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

    internal class Left125 : WalkSetBase
    {
        public Left125(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

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

    internal class Left180 : WalkSetBase
    {
        public Left180(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

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

    internal class Right45 : WalkSetBase
    {
        public Right45(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

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

    internal class Right90 : WalkSetBase
    {
        public Right90(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

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

    internal class Right125 : WalkSetBase
    {
        public Right125(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

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

    internal class Right180 : WalkSetBase
    {
        public Right180(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

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

    internal class RightGap : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public RightGap(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            m.Add(new Tuple<int, Move>(1, Move.Right));
            m.Add(new Tuple<int, Move>(2, Move.Right));
            m.Add(new Tuple<int, Move>(3, Move.Straight));
            m.Add(new Tuple<int, Move>(4, Move.Left));
            m.Add(new Tuple<int, Move>(5, Move.Left));
        }

        public override WalkSetType Type()
        {
            return WalkSetType.RightGap;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var nextMoves = 1;
            _nextMove = Move.Right;
            do
            {
                if (nextMoves > 5)
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

    internal class Right180Bigger : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public Right180Bigger(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            m.Add(new Tuple<int, Move>(1, Move.Right));
            m.Add(new Tuple<int, Move>(2, Move.Right));
            m.Add(new Tuple<int, Move>(3, Move.Straight));
            m.Add(new Tuple<int, Move>(4, Move.Right));
            m.Add(new Tuple<int, Move>(5, Move.Right));
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Right180Bigger;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var nextMoves = 1;
            _nextMove = Move.Right;
            do
            {
                if (nextMoves > 5)
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

    internal class Left180Bigger : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public Left180Bigger(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            m.Add(new Tuple<int, Move>(1, Move.Left));
            m.Add(new Tuple<int, Move>(2, Move.Left));
            m.Add(new Tuple<int, Move>(3, Move.Straight));
            m.Add(new Tuple<int, Move>(4, Move.Left));
            m.Add(new Tuple<int, Move>(5, Move.Left));
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Left180Bigger;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var nextMoves = 1;
            _nextMove = Move.Right;
            do
            {
                if (nextMoves > 5)
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

    internal class LeftGap : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public LeftGap(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            m.Add(new Tuple<int, Move>(1, Move.Left));
            m.Add(new Tuple<int, Move>(2, Move.Left));
            m.Add(new Tuple<int, Move>(3, Move.Straight));
            m.Add(new Tuple<int, Move>(4, Move.Right));
            m.Add(new Tuple<int, Move>(5, Move.Right));
        }

        public override WalkSetType Type()
        {
            return WalkSetType.LeftGap;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var nextMoves = 1;
            _nextMove = Move.Left;
            do
            {
                if (nextMoves > 5)
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

    internal class RightLightGap : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public RightLightGap(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            m.Add(new Tuple<int, Move>(1, Move.Right));
            m.Add(new Tuple<int, Move>(2, Move.Right));
            m.Add(new Tuple<int, Move>(3, Move.Left));
            m.Add(new Tuple<int, Move>(4, Move.Left));
        }

        public override WalkSetType Type()
        {
            return WalkSetType.RightLigntGap;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var nextMoves = 1;
            _nextMove = Move.Right;
            do
            {
                if (nextMoves > 4)
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

    internal class LeftLightGap : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public LeftLightGap(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            m.Add(new Tuple<int, Move>(1, Move.Left));
            m.Add(new Tuple<int, Move>(2, Move.Left));
            m.Add(new Tuple<int, Move>(3, Move.Right));
            m.Add(new Tuple<int, Move>(4, Move.Right));
        }

        public override WalkSetType Type()
        {
            return WalkSetType.LeftLightGap;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var nextMoves = 1;
            _nextMove = Move.Left;
            do
            {
                if (nextMoves > 4)
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

    internal class RightLightLightGap : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public RightLightLightGap(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            m.Add(new Tuple<int, Move>(1, Move.Right));
            m.Add(new Tuple<int, Move>(2, Move.Left));
        }

        public override WalkSetType Type()
        {
            return WalkSetType.RightLigntLightGap;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var nextMoves = 1;
            _nextMove = Move.Right;
            do
            {
                if (nextMoves > 2)
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

    internal class LeftLightLightGap : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public LeftLightLightGap(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            m.Add(new Tuple<int, Move>(1, Move.Left));
            m.Add(new Tuple<int, Move>(2, Move.Right));
        }

        public override WalkSetType Type()
        {
            return WalkSetType.LeftLightLightGap;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var nextMoves = 1;
            _nextMove = Move.Left;
            do
            {
                if (nextMoves > 2)
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

    internal class Love : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> m = new List<Tuple<int, Move>>();

        public Love(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {
            m.Add(new Tuple<int, Move>(1, Move.Straight));
            m.Add(new Tuple<int, Move>(2, Move.Right));
            m.Add(new Tuple<int, Move>(3, Move.Straight));
            m.Add(new Tuple<int, Move>(4, Move.Right));
            m.Add(new Tuple<int, Move>(5, Move.Straight));
            m.Add(new Tuple<int, Move>(6, Move.Straight));
            m.Add(new Tuple<int, Move>(7, Move.Straight));
            m.Add(new Tuple<int, Move>(8, Move.Right));
            m.Add(new Tuple<int, Move>(9, Move.Straight));
            m.Add(new Tuple<int, Move>(10, Move.Straight));
            m.Add(new Tuple<int, Move>(11, Move.Left));
            m.Add(new Tuple<int, Move>(12, Move.Left));
            m.Add(new Tuple<int, Move>(13, Move.Straight));
            m.Add(new Tuple<int, Move>(14, Move.Straight));
            m.Add(new Tuple<int, Move>(15, Move.Right));
            m.Add(new Tuple<int, Move>(16, Move.Straight));
            m.Add(new Tuple<int, Move>(17, Move.Straight));
            m.Add(new Tuple<int, Move>(18, Move.Straight));
            m.Add(new Tuple<int, Move>(19, Move.Right));
            m.Add(new Tuple<int, Move>(20, Move.Straight));
            m.Add(new Tuple<int, Move>(21, Move.Right));
            m.Add(new Tuple<int, Move>(22, Move.Straight));
            m.Add(new Tuple<int, Move>(23, Move.Straight));
            m.Add(new Tuple<int, Move>(24, Move.Right));
            m.Add(new Tuple<int, Move>(25, Move.Straight));
            m.Add(new Tuple<int, Move>(26, Move.Straight));
            m.Add(new Tuple<int, Move>(27, Move.Straight));
            m.Add(new Tuple<int, Move>(28, Move.Straight));
            m.Add(new Tuple<int, Move>(29, Move.Straight));
            m.Add(new Tuple<int, Move>(30, Move.Straight));
            m.Add(new Tuple<int, Move>(31, Move.Straight));
            m.Add(new Tuple<int, Move>(32, Move.Straight));
            m.Add(new Tuple<int, Move>(33, Move.Right));
            m.Add(new Tuple<int, Move>(34, Move.Right));
            m.Add(new Tuple<int, Move>(35, Move.Straight));
            m.Add(new Tuple<int, Move>(36, Move.Straight));
            m.Add(new Tuple<int, Move>(37, Move.Straight));
            m.Add(new Tuple<int, Move>(38, Move.Straight));
            m.Add(new Tuple<int, Move>(39, Move.Straight));
            m.Add(new Tuple<int, Move>(40, Move.Straight));
            m.Add(new Tuple<int, Move>(41, Move.Straight));
            m.Add(new Tuple<int, Move>(42, Move.Straight));
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Love;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var nextMoves = 1;
            _nextMove = Move.Left;
            do
            {
                if (nextMoves > m.Count)
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

    internal class GetToWall : WalkSetBase
    {
        public GetToWall(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

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

    internal class Roll100 : WalkSetBase
    {
        public Roll100(CollissionHelper ch, int myId) : base(ch, myId)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Roll100;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var iteration = 1;
            var limit = 1;
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

    internal class RollLeft : WalkSetBase
    {
        public RollLeft(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.RollLeft;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var count = 1;
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

    internal class Safer : WalkSetBase
    {
        private readonly List<Move> _moveList = new List<Move> {Move.Left, Move.Straight, Move.Right};

        public Safer(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Safer;
        }

        protected override int CutCount()
        {
            return 2;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            _walks = GetWalks(_round, position, direction, 10, gameSurrond, new List<Walk>());
        }


        private List<Walk> GetWalks(int round, Position position, Direction currentDirection, int moves,
            int[,] gameSurrond, List<Walk> walks)
        {
            moves--;
            if (moves <= 0) return walks;
            var simulateGameSurround = (int[,]) gameSurrond.Clone();
            Walk myWalk = null;
            var bestCount = 0;
            var nextWalks = new List<Walk>();

            foreach (var m in _moveList)
            {
                var simulateDirection = currentDirection;
                var simulatePosition = position.Copy();
                simulateDirection = simulateDirection.GetNewDirection(m);
                simulatePosition.Update(simulateDirection);

                if (!_ch.Collission(simulateDirection, simulateGameSurround, simulatePosition))
                {
                    simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                    myWalk = new Walk(round + walks.Count, m, simulateDirection, simulatePosition);
                    walks.Add(myWalk);
                    var submoves = GetWalks(round, simulatePosition, simulateDirection, moves, simulateGameSurround,
                        walks.ToList());
                    if (submoves.Count > bestCount)
                    {
                        nextWalks = submoves;
                        bestCount = submoves.Count;
                    }
                    walks.Remove(myWalk);
                    simulateGameSurround[simulatePosition.Y, simulatePosition.Y] = 0;
                }
            }

            if (myWalk == null)
            {
                return walks;
            }

            return nextWalks;
        }
    }

    internal class RollRight : WalkSetBase
    {
        public RollRight(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.RollRight;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var count = 1;
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

    internal class Raper : WalkSetBase
    {
        public Raper(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Raper;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var count = 2;
            var straight = 3;
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

    internal class Bingo : WalkSetBase
    {
        public Bingo(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Bingo;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var count = 1;
            var cycle = 1;
            var straight = 2;
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
                    if (cycle > 4)
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

    internal class Randomer : WalkSetBase
    {
        public Randomer(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Randomer;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var r = new Random();
            _nextMove = (Move) r.Next(1, 4);
            do
            {
                Update();
                _nextMove = (Move) r.Next(1, 4);
            } while (!_ch.Collission(_simulateDirection, _simulateGameSurround, _simulatePosition));
        }

        protected override int CutCount()
        {
            return 3;
        }
    }

    internal class Snaker : WalkSetBase
    {
        public Snaker(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Snaker;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var count = 2;
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

    internal class Quaker : WalkSetBase
    {
        public Quaker(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Quaker;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var count = 1;
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

    internal class SquareRight : WalkSetBase
    {
        public SquareRight(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.SquareRight;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var count = 1;
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

    internal class SquareLeft : WalkSetBase
    {
        public SquareLeft(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.SquareLeft;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var count = 1;
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

    internal class FunkyTerror : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> left_nextMoves = new List<Tuple<int, Move>>();
        private readonly List<Tuple<int, Move>> right_nextMoves = new List<Tuple<int, Move>>();

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
            var straightMoves = 5;
            var nextMoves = 1;
            var left = true;
            var doStraightMoves = false;
            _nextMove = direction.IsDiagonal() ? Move.Straight : Move.Right;


            do
            {
                if (doStraightMoves)
                {
                    _nextMove = Move.Straight;
                }
                else
                {
                    _nextMove = left
                        ? left_nextMoves.First(l => l.Item1 == nextMoves).Item2
                        : right_nextMoves.First(l => l.Item1 == nextMoves).Item2;
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

    internal class Killer : WalkSetBase
    {
        public Killer(CollissionHelper ch, int myId) : base(ch, myId, true)
        {
        }

        public override WalkSetType Type()
        {
            return WalkSetType.Killer;
        }

        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            var targetPosition = new Position(0, 0);
            double min = 500;
            foreach (var o in _others.Where(x => x.Id != 4))
            {
                if (position.Distance(o.Position) < min)
                {
                    min = position.Distance(o.Position);
                    targetPosition = o.Position;
                }
            }

            var distance = targetPosition.Distance(position);
            var targets = new List<Tuple<int, Position>>();
            var number = 1;
            foreach (var dir in Enum.GetValues(typeof (Direction)))
            {
                var p = targetPosition.Copy();
                for (var i = 0; i < ((distance/6) + 5); i++)
                {
                    p.Update((Direction) dir);
                    if (p.IsInCollission(100))
                    {
                        return;
                    }
                }
                targets.Add(new Tuple<int, Position>(number, p));
                if (!p.IsInCollission(100) && _simulateGameSurround[p.X, p.Y] == 0)
                {
                    _simulateGameSurround[p.X, p.Y] = 200;
                }
                number++;
            }

            double minDistance = 200;
            var tartetPoint = targets.First();
            foreach (var p in targets)
            {
                if (p.Item2.Distance(_position) < minDistance)
                {
                    tartetPoint = p;
                    minDistance = p.Item2.Distance(_position);
                }
            }

            var succes = MovesToGetPosition(_position, _direction, tartetPoint.Item2);


            if (succes)
            {
                foreach (var t in targets.Where(x => x.Item1 > tartetPoint.Item1).OrderBy(x => x.Item1))
                {
                    if (succes)
                    {
                        succes = MovesToGetPosition(_simulatePosition, _simulateDirection, t.Item2);
                    }
                }
            }

            if (succes)
            {
                foreach (var t in targets.Where(x => x.Item1 < tartetPoint.Item1).OrderBy(x => x.Item1))
                {
                    if (succes)
                    {
                        succes = MovesToGetPosition(_simulatePosition, _simulateDirection, t.Item2);
                    }
                }
            }

            if (succes)
            {
                MovesToGetPosition(_simulatePosition, _simulateDirection, tartetPoint.Item2);
            }
        }

        private bool MovesToGetPosition(Position position, Direction direction, Position targetPosition)
        {
            var pos = position.Copy();
            var dir = direction;
            var myMove = Move.Right;
            var i = 0;

            var myDirection = direction;
            var myPosition = position.Copy();
            while (!targetPosition.IsSame(_simulatePosition) && i < 150)
            {
                i++;
                double minDistance = 200;
                var chosen = false;
                foreach (var m in Enum.GetValues(typeof (Move)))
                {
                    var p = pos.Copy();
                    var d = dir.GetNewDirection((Move) m);

                    p.Update(d);

                    if (!p.IsInCollission(100) && p.Distance(targetPosition) < minDistance &&
                        (_simulateGameSurround[p.X, p.Y] == 0 || _simulateGameSurround[p.X, p.Y] == 200))
                    {
                        myMove = (Move) m;
                        myDirection = d;
                        myPosition = p;
                        minDistance = p.Distance(targetPosition);
                        chosen = true;
                    }
                }
                if (chosen)
                {
                    pos = myPosition;
                    dir = myDirection;
                    _nextMove = myMove;
                    Update();
                }
                else
                {
                    var notChoosenPosition = pos.Copy();
                    dir = dir.GetNewDirection(Move.Right);
                    notChoosenPosition.Update(myDirection);
                    pos = notChoosenPosition;
                    _nextMove = Move.Right;
                    Update();
                }
            }

            if (i < 150)
            {
                return true;
            }
            return false;
        }

        protected override int CutCount()
        {
            return 2;
        }
    }

    internal class StrikeLeft : WalkSetBase
    {
        private readonly List<Tuple<int, Move>> _nextMoveList = new List<Tuple<int, Move>>();


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
            var count = 1;
            do
            {
                _nextMove = count <= _nextMoveList.Count
                    ? _nextMoveList.First(l => l.Item1 == count).Item2
                    : Move.Straight;
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
        private readonly List<Tuple<int, Move>> _nextMoveList = new List<Tuple<int, Move>>();

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
            var count = 1;
            _nextMove = _nextMoveList.First(x => x.Item1 == 1).Item2;
            do
            {
                _nextMove = count <= _nextMoveList.Count
                    ? _nextMoveList.First(l => l.Item1 == count).Item2
                    : Move.Straight;
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
