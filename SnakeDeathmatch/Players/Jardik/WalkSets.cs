using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardik
{
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
            Cut(2);
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
            Cut(2);
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
            Cut(2);
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
            Cut(2);
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
            Cut(2);
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
            Cut(2);
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
            Cut(2);
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
            Cut(2);
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
            Cut(2);
        }
    }

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
            _score = 1;
            int _moves = 1;
            Move move = Move.Right;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                if (_moves > 3)
                {
                    move = Move.Straight;
                }
                else
                {
                    move =  m.Where(l => l.Item1 == _moves).First().Item2;
                }
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _moves++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));               
            }
            Cut(2);
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
            _score = 1;
            int _moves = 1;
            Move move = Move.Left;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                if (_moves > 3)
                {
                    move = Move.Straight;
                }
                else
                {
                    move = m.Where(l => l.Item1 == _moves).First().Item2;
                }
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _moves++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
            }
            Cut(2);
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
            Cut(2);
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
            _score = 1;
            int iteration = 1;
            int limit = 1;
            Move move = direction.IsDiagonal() ? Move.Straight : Move.Right;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                if (iteration < limit)
                {
                    move = Move.Straight;
                }
                else if (iteration < limit + 1)
                {
                    move = Move.Right; ;
                }
                else
                {
                    iteration = 1;
                    limit++;
                }

                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                iteration++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
            }
            Cut(3);
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
            _score = 1;
            int _count = 1;
            Move move = Move.Left;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _count++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                if (_count <= 4) move = Move.Left;
                if (_count > 4) move = Move.Right;
                if (_count == 8) _count = 1;
            }
            Cut(3);
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
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            _walks = MoveIt(_round, 9, position, direction, gameSurrond);
        }

        private List<Walk> MoveIt(int round,int moves, Position position, Direction direction, int[,] gameSurrond)
        {
            foreach (Move move in _moveList)
            {
                List<Walk> walks = GetWalks(round,position.Copy(), direction, move, moves, gameSurrond, new List<Walk>());
                if (walks.Count >= moves-1 )
                    return walks;
            }

            return MoveIt(round,--moves,position.Copy(),direction, gameSurrond);
        }

        private List<Walk> GetWalks(int round,Position position, Direction currentDirection, Move move, int moves, int[,] gameSurrond, List<Walk> walks)
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
                moves = 0;
                return new List<Walk>();
            }
            simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
            var myWalk = new Walk(round + walks.Count, move, simulateDirection, simulatePosition);
            walks.Add(myWalk);

            
            List<Walk> straightWalks = GetWalks(round, simulatePosition, simulateDirection, Move.Straight, moves, simulateGameSurround, walks.ToList());
            List<Walk> leftWalks = GetWalks(round, simulatePosition, simulateDirection, Move.Left, moves, simulateGameSurround, walks.ToList());
            List<Walk> rightWalks = GetWalks(round, simulatePosition, simulateDirection, Move.Right, moves, simulateGameSurround, walks.ToList());            
            

            int max = new[]{leftWalks.Count, rightWalks.Count, straightWalks.Count}.Max();

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
            _score = 1;
            int _count = 1;
            Move move = Move.Right;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _count++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                if (_count <= 4) move = Move.Right;
                if (_count > 4) move = Move.Left;
                if (_count == 8) _count = 1;
            }
            Cut(3);
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
            _score = 1;
            int _count = 2;
            int straight = 3;
            Move move = Move.Right;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _count++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                if (_count <= 5) move = Move.Right;
                if (_count > 5 && _count < 11) move = Move.Left;
                if (_count >= 11 && _count < 11 + straight) move = Move.Straight;
                if (_count == 11 + straight)
                {
                    _count = 1;
                    straight++;
                }

            }
            Cut(3);
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
            _score = 1;
            int _count = 1;
            int cycle = 1;
            int straight = 2;
            Move move = Move.Right;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _count++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                if (_count <= 2) move = Move.Right;
                if (_count > 2 && _count < 2 + straight) move = Move.Straight;
                if (_count == 2 + straight)
                {
                    cycle++;
                    _count = 1;
                    if (cycle> 4)
                    {
                        straight++;
                        cycle = 1;
                    }                    
                }

            }
            Cut(3);
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
            _score = 1;
            Move move = (Move)r.Next(1, 4);
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                move = (Move)r.Next(1, 4);
            }
            Cut(3);
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
            _score = 1;
            int _count = 2;
            Move move = Move.Right;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _count++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                if (_count <= 6) move = Move.Right;
                if (_count > 6) move = Move.Left;
                if (_count == 11) _count = 1;
            }
            Cut(3);
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
            _score = 1;
            int _count = 1;
            Move move = Move.Straight;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _count++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                if (_count <= 5) move = Move.Straight;
                if (_count > 5 && _count <= 7) move = Move.Right;
                if (_count > 7 && _count <= 11) move = Move.Straight;
                if (_count > 11 && _count <= 13) move = Move.Left;
                //if (_count > 13 && _count <= 15) move = Move.Straight;
                if (_count > 13 && _count <= 15) move = Move.Left;
                if (_count > 15 && _count <= 19) move = Move.Straight;
                if (_count > 19 && _count <= 21) move = Move.Right;
                if (_count > 21) _count = 1;
            }
            Cut(3);
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
            _score = 1;
            int _count = 1;
            Move move = Move.Straight;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _count++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                if (_count <= 8) move = Move.Straight;
                if (_count > 8 && _count <= 10) move = Move.Right;
                if (_count > 10 && _count <= 18) move = Move.Straight;
                if (_count > 18 && _count <= 20) move = Move.Right;
                if (_count > 20 && _count <= 28) move = Move.Straight;
                if (_count > 28 && _count <= 30) move = Move.Right;
                if (_count > 30 && _count <= 38) move = Move.Straight;
                if (_count > 38 && _count <= 40) move = Move.Left;
                if (_count > 40) move = Move.Straight;
            }
            Cut(3);
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
            _score = 1;
            int _count = 1;
            Move move = Move.Straight;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _count++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                if (_count <= 8) move = Move.Straight;
                if (_count > 8 && _count <= 10) move = Move.Left;
                if (_count > 10 && _count <= 18) move = Move.Straight;
                if (_count > 18 && _count <= 20) move = Move.Left;
                if (_count > 20 && _count <= 28) move = Move.Straight;
                if (_count > 28 && _count <= 30) move = Move.Left;
                if (_count > 30 && _count <= 38) move = Move.Straight;
                if (_count > 38 && _count <= 40) move = Move.Right;
                if (_count > 40) move = Move.Straight;
            }
            Cut(3);
        }
    }

    class FunkyTerror : WalkSetBase
    {
        List<Tuple<int, Move>> leftMoves = new List<Tuple<int, Move>>();
        List<Tuple<int, Move>> rightMoves = new List<Tuple<int, Move>>();

        public FunkyTerror(CollissionHelper ch, int myId)
            : base(ch, myId, true)
        {

            leftMoves.Add(new Tuple<int, Move>(1, Move.Left));
            leftMoves.Add(new Tuple<int, Move>(2, Move.Left));
            leftMoves.Add(new Tuple<int, Move>(3, Move.Straight));
            leftMoves.Add(new Tuple<int, Move>(4, Move.Right));
            leftMoves.Add(new Tuple<int, Move>(5, Move.Right));
            leftMoves.Add(new Tuple<int, Move>(6, Move.Right));
            leftMoves.Add(new Tuple<int, Move>(7, Move.Right));
            leftMoves.Add(new Tuple<int, Move>(8, Move.Straight));
            leftMoves.Add(new Tuple<int, Move>(9, Move.Straight));
            leftMoves.Add(new Tuple<int, Move>(10, Move.Straight));
            leftMoves.Add(new Tuple<int, Move>(11, Move.Right));
            leftMoves.Add(new Tuple<int, Move>(12, Move.Right));

            rightMoves.Add(new Tuple<int, Move>(1, Move.Right));
            rightMoves.Add(new Tuple<int, Move>(2, Move.Right));                      
            rightMoves.Add(new Tuple<int, Move>(3, Move.Straight));
            rightMoves.Add(new Tuple<int, Move>(4, Move.Left));
            rightMoves.Add(new Tuple<int, Move>(5, Move.Left));
            rightMoves.Add(new Tuple<int, Move>(6, Move.Left));
            rightMoves.Add(new Tuple<int, Move>(7, Move.Left));
            rightMoves.Add(new Tuple<int, Move>(8, Move.Straight));
            rightMoves.Add(new Tuple<int, Move>(9, Move.Straight));
            rightMoves.Add(new Tuple<int, Move>(10, Move.Straight));
            rightMoves.Add(new Tuple<int, Move>(11, Move.Left));
            rightMoves.Add(new Tuple<int, Move>(12, Move.Left));
        }
        public override WalkSetType Type()
        {
            return WalkSetType.FunkyTerror;
        }
        protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
        {
            _score = 1;
            int _straightMoves = 5;
            int _moves = 1;
            bool left = true;
            bool doStraightmoves = false;
            Move move = direction.IsDiagonal() ? Move.Straight : Move.Right;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                if (doStraightmoves)
                {
                    move = Move.Straight;
                }
                else
                {
                    move = left ? leftMoves.Where(l => l.Item1 == _moves).First().Item2 : rightMoves.Where(l => l.Item1 == _moves).First().Item2;
                }
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _moves++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                if (_moves > 12 && doStraightmoves == false)
                {
                    doStraightmoves = true;
                    _moves = 1;
                }

                if (_moves > _straightMoves && doStraightmoves == true)
                {
                    doStraightmoves = false;
                    _straightMoves += 6;
                    left = !left;
                    _moves = 1;
                }
            }
            Cut(3);
        }


    }

    class VazbaKiller : WalkSetBase
    {
        List<Tuple<int, Move>> moves = new List<Tuple<int, Move>>();
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
                moves.Add(new Tuple<int,Move>(step,m));
                step++;
            }
            double minDistance = 300;
            while (vazbaPosition.Distance(afterPosition) < minDistance)
            {
                minDistance = vazbaPosition.Distance(afterPosition);
                moves.Add(new Tuple<int, Move>(step, Move.Straight));
                afterPosition.Update(targetDirection);                
                step++;
            }

            for (int s = 0; s < distance/2.5; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //right
            for (int s = 0; s < 2; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Right));
                step++;
            }

            //straight
            for (int s = 0; s < distance / 3 /2; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //left
            for (int s = 0; s < 2; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Left));
                step++;
            }

            //straight
            for (int s = 0; s < distance / 3; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //left
            for (int s = 0; s < 2; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Left));
                step++;
            }

            //straight
            for (int s = 0; s < distance / 3; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //left
            for (int s = 0; s < 2; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Left));
                step++;
            }

            //straight
            for (int s = 0; s < distance / 3; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //left
            for (int s = 0; s < 2; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Left));
                step++;
            }

            //straight
            for (int s = 0; s < (distance / 3 / 2) -4; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Straight));
                step++;
            }

            //right
            for (int s = 0; s < 2; s++)
            {
                moves.Add(new Tuple<int, Move>(step, Move.Right));
                step++;
            }
            
            _score = 1;
            int _count = 1;
            Move move = moves.Where(x => x.Item1 == _count).First().Item2;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _count++;
                if (_count <= moves.Count)
                {
                    move = moves.Where(x => x.Item1 == _count).First().Item2;
                    _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));                    
                }
                else
                {
                    _walks.Add(new Walk(_round, Move.Straight, simulateDirection, simulatePosition.Copy()));               
                }
                
            }
            Cut(3);


        }


    }
}
