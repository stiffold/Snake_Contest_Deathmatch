using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Snake2.game;

namespace Snake2.players
{
    public class Jardik : IPlayerBehavior
    {
        private Direction _myFirstDirection;

        Direction _myDirection;
        Position _myPosition = new Position(1, 1);
        Planner _planner;

        private int _myID;
        private int _round;
        private int _max = 100;

        private List<Walk> _myPlanedMoves = new List<Walk>();
        public void Init(int direction, int identificator)
        {
            _myFirstDirection = (Direction)direction;
            _myID = identificator;
            _round = 0;            
        }

        public int NextMove(int[,] gameSurrond)
        {
            _round++;
            InitOrUpdateMyState(gameSurrond);

            if (!_myPlanedMoves.Any(s=>s.Round ==_round))
            {
                _myPlanedMoves.AddRange(_planner.GetBestWalksToMe(_round, _myPosition, _myDirection, gameSurrond));
            }
            
            var nextMove = _myPlanedMoves.Where(x => x.Round == _round).FirstOrDefault();
            if (nextMove!=null)
            {
                return (int)nextMove.Move;
            }
            else
            {
                return (int)Move.Straight;
            }
        }

        private void InitOrUpdateMyState(int[,] gameSurrond)
        {
            if (_round == 1)
            {
                _myDirection = _myFirstDirection;
                _max = (int)Math.Sqrt(gameSurrond.Length);
                _planner = new Planner(_max, _myID);

                for (int x = 0; x < _max; x++)
                {
                    for (int y = 0; y < _max; y++)
                    {
                        if (gameSurrond[x, y] == _myID)
                        {
                            _myPosition = new Position(x, y);
                        }
                    }
                }
            }
            else
            {
                var lastMove = _myPlanedMoves.OrderByDescending(m => m.Round).First();
                _myPosition = lastMove.Position;
                _myDirection = lastMove.Direction;
            }            
        }

        

       
        
        public string MyName()
        {
            return "Jardík";
        }

        

        
    }

    enum Direction
    {
        Top = 1,
        TopRight = 2,
        Right = 3,
        BottomRight = 4,
        Bottom = 5,
        BottomLeft = 6,
        Left = 7,
        TopLeft = 8,        
    }

    static class DirectionExtensions
    {
        public static Direction GetNewDirection(this Direction oldDirection, Move move)
        {
            if (move == Move.Left)
            {
                switch (oldDirection)
                {
                    case Direction.Top: return Direction.TopLeft;
                    case Direction.TopRight: return Direction.Top;
                    case Direction.Right: return Direction.TopRight;
                    case Direction.BottomRight: return Direction.Right;
                    case Direction.Bottom: return Direction.BottomRight;
                    case Direction.BottomLeft: return Direction.Bottom;
                    case Direction.Left: return Direction.BottomLeft;
                    case Direction.TopLeft: return Direction.Left;
                }
            }

            if (move == Move.Right)
            {
                switch (oldDirection)
                {
                    case Direction.Top: return Direction.TopRight;
                    case Direction.TopRight: return Direction.Right;
                    case Direction.Right: return Direction.BottomRight;
                    case Direction.BottomRight: return Direction.Bottom;
                    case Direction.Bottom: return Direction.BottomLeft;
                    case Direction.BottomLeft: return Direction.Left;
                    case Direction.Left: return Direction.TopLeft;
                    case Direction.TopLeft: return Direction.Top;
                }
            }

            return oldDirection;
        }
    }

    enum Move
    {
        Left = 1,
        Straight = 2,
        Right = 3
    }

    enum StepSetType
    { 
        Straight,
        Left45,
        Left90,
        Left125,
        left180,
        Right45,
        Right90,
        Right125,
        Right180,
        GetToWall,
        Roll100,
    }

    class Position
    {
        private int _x;
        private int _y;
        public Position(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X
        {
            get { return _x; }
        }
        public int Y
        {
            get { return _y; }
        }

        public bool IsInCollission(int max)
        {
            if (X < 0) return true;
            if (Y < 0) return true;
            if (X >= max) return true;
            if (Y >= max) return true;
            return false;
        }

        public Position Copy()
        {
            return new Position(_x,_y);
        }

        public void Update(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    _y--;
                    break;
                case Direction.TopRight:
                    _y--;
                    _x++;
                    break;
                case Direction.Right:
                    _x++;
                    break;
                case Direction.BottomRight:
                    _x++;
                    _y++;
                    break;
                case Direction.Bottom:
                    _y++;
                    break;
                case Direction.BottomLeft:
                    _y++;
                    _x--;
                    break;
                case Direction.Left:
                    _x--;
                    break;
                case Direction.TopLeft:
                    _y--;
                    _x--;
                    break;
            }
        }
    }

    internal class Planner 
    {
        CollissionHelper _ch;
        int _myId;
        List<WalkSetBase> _walkSets = new List<WalkSetBase>();
        public Planner(int max, int myId)
        {
           _ch = new CollissionHelper(max);
           _myId = myId;
           _walkSets.Add(new Straight(_ch, _myId));
           _walkSets.Add(new Right45(_ch, _myId));
           _walkSets.Add(new Right90(_ch, _myId));
           _walkSets.Add(new Right125(_ch, _myId));
           _walkSets.Add(new Right180(_ch, _myId));
           _walkSets.Add(new Left45(_ch, _myId));
           _walkSets.Add(new Left90(_ch, _myId));
           _walkSets.Add(new Left125(_ch, _myId));
           _walkSets.Add(new Left180(_ch, _myId));      
        }

        public List<Walk> GetBestWalksToMe(int round,Position position, Direction direction, int[,] gameSurround)
        {
            foreach (var walkSet in _walkSets)
            {
                walkSet.Evaluate(round,position, direction, gameSurround);
            }
            var best = _walkSets.OrderByDescending(x => x.Score).FirstOrDefault();

            if (best != null) return best.Walks;
            return new List<Walk>();
       }   
   }

    internal abstract class WalkSetBase
    {
        CollissionHelper _ch;
        int _score = 0;
        int _myId = 0;
        int _round = 0;
        List<Walk> _walks = new List<Walk>();

        protected WalkSetBase(CollissionHelper ch, int myId)
        {
            _myId = myId;
            _ch = ch;
        }

        public void Evaluate(int round,Position position, Direction direction, int[,] gameSurrond)
        {
            _round = round;
            _score = 0;
            _walks.Clear();
            DoEvaluate(position, direction,gameSurrond);
        }

        protected int TryGetCollissionTurn(Move move, Position _myPosition, Direction _myDirection, int[,] gameSurrond, int movestToStraight)
        {
            _score = 1;
            Direction simulateDirection = _myDirection;
            Position simulatePosition = _myPosition.Copy();
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round,move,simulateDirection,simulatePosition.Copy()));
            if (_score == movestToStraight) move = Move.Straight;            

            while (!_ch.Collission(simulateDirection, simulateGameSurround, simulatePosition))
            {
                simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _walks.Add(new Walk(_round,move,simulateDirection,simulatePosition.Copy()));
                if (_score == movestToStraight) move = Move.Straight;
            }
            return _score;
        }

        protected int TryGetCollission(Move move, Position _myPosition, Direction _myDirection, int[,] gameSurrond)
        {
            _score = 1;
            Direction simulateDirection = _myDirection;
            Position simulatePosition = _myPosition.Copy();
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
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
            }

            return _score;
        }

        protected abstract void DoEvaluate(Position position, Direction direction, int[,] gameSurrond);
        public int Score{get {return _score;}}
        public List<Walk> Walks { get { return _walks.OrderBy(x=>x.Round).Take(2).ToList(); } }
    }

    internal class CollissionHelper
    {
        private int _max;

        public CollissionHelper(int max)
        {
            _max = max;
        }

        public bool Collission(Direction direction, int[,] gameSurround, Position position)
        {
            if (CrossColision(direction, gameSurround, position)) return true;
            if (position.IsInCollission(_max)) return true;
            if (gameSurround[position.X, position.Y] != 0) return true;
            return false;
        }

        bool CrossColision(Direction direction, int[,] gameSurround, Position position)
        {
            if (position.X <= 0 || position.Y <= 0 || position.X >= _max - 1 || position.Y >= _max - 1)
            {
                return false;
            }

            switch (direction)
            {
                case Direction.TopRight: return (gameSurround[position.X, position.Y + 1] != 0) && (gameSurround[position.X - 1, position.Y] != 0);

                case Direction.BottomRight: return (gameSurround[position.X, position.Y - 1] != 0) && (gameSurround[position.X - 1, position.Y] != 0);

                case Direction.BottomLeft: return (gameSurround[position.X, position.Y - 1] != 0) && (gameSurround[position.X + 1, position.Y] != 0);

                case Direction.TopLeft: return (gameSurround[position.X, position.Y + 1] != 0) && (gameSurround[position.X + 1, position.Y] != 0);
            }

            return false;
        }        
    }

    internal class Walk
    {
        private int _round;
        private Move _move;
        private Direction _direction;
        private Position _position;

        public Walk(int round, Move move, Direction direction, Position position)
        {
            _round = round;
            _move = move;
            _direction = direction;
            _position = position;
        }

        public int Round { get { return _round; } }
        public Move Move { get { return _move; } }
        public Direction Direction { get { return _direction; } }
        public Position Position { get { return _position; } }
    }

        internal class Straight : WalkSetBase
        {
            public Straight(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                TryGetCollission(Move.Straight, position, direction, gameSurrond);
            }
        }
        internal class Left45 : WalkSetBase
        {
            public Left45(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                TryGetCollissionTurn(Move.Left, position, direction, gameSurrond, 1);
            }
        }
        internal class Left90 : WalkSetBase
        {
            public Left90(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                TryGetCollissionTurn(Move.Left, position, direction, gameSurrond, 2);
            }
        }
        internal class Left125 : WalkSetBase
        {
            public Left125(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                TryGetCollissionTurn(Move.Left, position, direction, gameSurrond, 3);
            }
        }
        internal class Left180 : WalkSetBase
        {
            public Left180(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                TryGetCollissionTurn(Move.Left, position, direction, gameSurrond, 4);
            }
        }
        internal class Right45 : WalkSetBase
        {
            public Right45(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                TryGetCollissionTurn(Move.Right, position, direction, gameSurrond, 1);
            }
        }
        internal class Right90 : WalkSetBase
        {
            public Right90(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                TryGetCollissionTurn(Move.Right, position, direction, gameSurrond, 2);
            }
        }
        internal class Right125 : WalkSetBase
        {
            public Right125(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                TryGetCollissionTurn(Move.Right, position, direction, gameSurrond, 3);
            }
        }
        internal class Right180 : WalkSetBase
        {
            public Right180(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                TryGetCollissionTurn(Move.Right, position, direction, gameSurrond, 4);
            }
        }
        internal class GetToWall : WalkSetBase
        {
            public GetToWall(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                throw new NotImplementedException();
            }
        }
        internal class Roll100 : WalkSetBase 
        {
            public Roll100(CollissionHelper ch, int myId) : base(ch, myId) { }
            protected override void DoEvaluate(Position position, Direction direction, int[,] gameSurrond)
            {
                throw new NotImplementedException();
            }
        }
}
