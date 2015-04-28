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
                if (_count > 13 && _count <= 15) move = Move.Straight;
                if (_count > 15 && _count <= 17) move = Move.Left;
                if (_count > 17 && _count <= 21) move = Move.Straight;
                if (_count > 21 && _count <= 23) move = Move.Right;
                if (_count > 23) _count = 1;
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
}
