using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SnakeGame.Players.Jardik
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
            throw new NotImplementedException();
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
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, simulateGameSurround, simulatePosition))
            {
                simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
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
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, simulateGameSurround, simulatePosition))
            {
                simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
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
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, simulateGameSurround, simulatePosition))
            {
                simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
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
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, simulateGameSurround, simulatePosition))
            {
                simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
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
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, simulateGameSurround, simulatePosition))
            {
                simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
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
                if (_count > 36 && _count <= 37) move = Move.Left;
                if (_count > 37) move = Move.Straight;
            }
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
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, simulateGameSurround, simulatePosition))
            {
                simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
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
                if (_count > 36 && _count <= 37) move = Move.Right;
                if (_count > 37) move = Move.Straight;
            }
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
            Move move = Move.Straight;
            Direction simulateDirection = direction;
            Position simulatePosition = position.Copy();
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));

            while (!_ch.Collission(simulateDirection, simulateGameSurround, simulatePosition))
            {
                if (doStraightmoves)
                {
                    move = Move.Straight;
                }
                else
                {
                    move = left ? leftMoves.Where(l => l.Item1 == _moves).First().Item2 : rightMoves.Where(l => l.Item1 == _moves).First().Item2;
                }
                simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
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
        }
    }
}
