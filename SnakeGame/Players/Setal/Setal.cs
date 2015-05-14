using System;
using System.Collections.Generic;
using System.Linq;
using SnakeGame.Interface;

namespace SnakeGame.Players.Setal
{
    public class Setal : IPlayerBehaviour2
    {
        private byte[,] _possibilities;
        private int _identificator;
        private int _size;
        private int _roundCounter;

        private Direction _direction;
        private GamePoint _actualPosition;
        private List<GamePoint> _possibleHeads;

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _size = playgroundSize;
            _direction = direction;
            _identificator = playerId;
            _actualPosition = new GamePoint(x, y);
            _possibilities = new byte[_size, _size];
            _roundCounter = 0;
        }

        public Move GetNextMove(int[,] playground)
        {
            _roundCounter++;

            EvaluateGame(playground);

            SafeMap safePath = new SafeMap(10, _actualPosition, _direction, _possibilities, _size);
            safePath.Start();

            if (safePath.Steps.Count > 0)
            {
                var next = safePath.Steps.Pop();
                _actualPosition = next.FinalPosition;
                _direction = Service.UpdateDirection(_direction, next.Move);
               // MessageBox.Show("Jdu:" + next.Move);
                return next.Move;
            }

            DangerMap dangerPath = new DangerMap(4, _actualPosition, _direction, _possibilities, _size);
            dangerPath.Start();

            if (dangerPath.Steps.Count > 0)
            {
                var next = dangerPath.Steps.Pop();
                _actualPosition = next.FinalPosition;
                _direction = Service.UpdateDirection(_direction, next.Move);
                //MessageBox.Show("Jdu:" + next.Move);
                return next.Move;
            }

            return Move.Straight;
        }

        public string Name
        {
            get { return "Setal(Šimík)"; }
        }

        private void EvaluateGame(int[,] gameSurrond)
        {
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                {
                    if ((gameSurrond[i, j] != 0) && (!_actualPosition.IsMatch(i, j)))
                    {
                        MarkPoint(i, j);
                    }
                }
        }

        [Obsolete]
        private GamePoint FindMySelf(int[,] gameSurrond)
        {
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                {
                    if (gameSurrond[i, j] == _identificator)
                    {
                        return new GamePoint(i, j);
                    }
                }
            throw new InvalidOperationException("Nejsem na herní ploše!");
        }

        private bool IsTaken(int x, int y)
        {
            //overit jeslti vubec bod existuje
            if (x < 0 || x >= _size || y < 0 || y >= _size)
                return true;

            //overit jestli bod je obsazeny
            return _possibilities[x, y] == 2;
        }

        private bool IsHead(int x, int y)
        {
            //Jestli je obsazene proveruji sousedni pole
            if (IsTaken(x, y))
            {
                if ((IsTaken(x - 1, y)) || (IsTaken(x + 1, y)) || (IsTaken(x, y - 1)) || (IsTaken(x, y + 1)))
                {
                    return false;
                }
                if ((IsTaken(x - 1, y + 1)) || (IsTaken(x + 1, y + 1)) || (IsTaken(x - 1, y - 1)) || (IsTaken(x + 1, y - 1)))
                {
                    return false;
                }

                return true;
            }
            else return false;
        }

        /// <summary>
        /// Oznacení nedostupnosti pole
        /// </summary>
        private void MarkPoint(int x, int y)
        {
            _possibilities[x, y] = 2;

            if (x - 1 > 0)
            {
                if (_possibilities[x - 1, y] == 0)
                    _possibilities[x - 1, y] = 1;
            }
            if (x + 1 < _size)
            {
                if (_possibilities[x + 1, y] == 0)
                    _possibilities[x + 1, y] = 1;
            }
            if (y - 1 > 0)
            {
                if (_possibilities[x, y - 1] == 0)
                    _possibilities[x, y - 1] = 1;
            }
            if (y + 1 < _size)
            {
                if (_possibilities[x, y + 1] == 0)
                    _possibilities[x, y + 1] = 1;
            }

            if ((x - 1 > 0) && (y - 1 > 0))
            {
                if (_possibilities[x - 1, y - 1] == 0)
                    _possibilities[x - 1, y - 1] = 1;
            }

            if ((x + 1 < _size) && (y - 1 > 0))
            {
                if (_possibilities[x + 1, y - 1] == 0)
                    _possibilities[x + 1, y - 1] = 1;
            }

            if ((x + 1 < _size) && (y + 1 < _size))
            {
                if (_possibilities[x + 1, y + 1] == 0)
                    _possibilities[x + 1, y + 1] = 1;
            }

            if ((x - 1 > 0) && (y + 1 < _size))
            {
                if (_possibilities[x - 1, y + 1] == 0)
                    _possibilities[x - 1, y + 1] = 1;
            }

        }

    }

    internal class GamePoint
    {
        public GamePoint()
        {
        }

        public GamePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public bool IsMatch(int x, int y)
        {
            return ((X == x) && (Y == y));
        }

    }

    internal class Step
    {
        public Step(Move move, Direction finalDirection, GamePoint finalPosition, bool isSafe)
        {
            Move = move;
            FinalDirection = finalDirection;
            FinalPosition = finalPosition;
            IsSafe = isSafe;
        }

        public Move Move { get; set; }
        public Direction FinalDirection { get; set; }
        public GamePoint FinalPosition { get; set; }
        public bool IsSafe { get; set; }
    }

    //STD cestovani
    internal class SafeMap
    {
        public Stack<Step> Steps { get; set; }
        private byte[,] _possibilities;
        private int _depth;
        private readonly int _size;
        private readonly GamePoint _start;
        private readonly Direction _origin;

        public SafeMap(int depth, GamePoint start, Direction origin, byte[,] possibilities, int size)
        {
            _depth = depth;
            _size = size;
            _possibilities = possibilities;
            _start = start;
            _origin = origin;
            Steps = new Stack<Step>();
        }

        public void Start()
        {
            Count(_start, _origin, 0, new List<GamePoint>());
        }

        /// <summary>
        /// Vraci bezpecne kroky v mape
        /// </summary>
        public int Fitness
        {
            get { return Steps.Where(x => x.IsSafe).Count(); }
        }

        private List<Step> PossibleSteps(Direction direction, GamePoint gamePoint)
        {
            var steps = new List<Step>();

            switch (direction)
            {
                case Direction.Top:
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y - 1))
                    {

                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y - 1);

                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y - 1))
                    {

                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.TopRight:
                    if (!IsTaken(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y - 1))
                    {

                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.Right:
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y - 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y))
                    {

                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.BottomRight:
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y + 1))
                    {

                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.Bottom:
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.BottomLeft:
                    if (!IsTaken(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.Left:
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.TopLeft:
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;
            }

            return steps;
        }

        private bool IsViable(GamePoint gamePoint)
        {
            //overit jeslti vubec bod existuje
            if (gamePoint.X < 0 || gamePoint.X >= _size || gamePoint.Y < 0 || gamePoint.Y >= _size)
                return false;

            //overit jestli je bod dostupny
            return _possibilities[gamePoint.X, gamePoint.Y] == 0;
        }

        private bool IsViable(int x, int y)
        {
            //overit jeslti vubec bod existuje
            if (x < 0 || x >= _size || y < 0 || y >= _size)
                return false;

            //overit jestli je bod dostupny
            return _possibilities[x, y] == 0;
        }

        private bool IsTaken(int x, int y)
        {
            //overit jeslti vubec bod existuje
            if (x < 0 || x >= _size || y < 0 || y >= _size)
                return true;

            //overit jestli bod je obsazeny
            return _possibilities[x, y] == 2;
        }

        private bool Count(GamePoint point, Direction direction, int depth, List<GamePoint> occupied)
        {
            if (depth == _depth)
                return true;

            var steps = PossibleSteps(direction, point).Where(x => x.IsSafe);

            foreach (var step in steps)
            {
                if (occupied.Contains(step.FinalPosition))
                    continue;

                List<GamePoint> copy = occupied.Select(x => new GamePoint(x.X, x.Y)).ToList();
                copy.Add(step.FinalPosition);

                if (Count(step.FinalPosition, step.FinalDirection, depth + 1, copy))
                {
                    Steps.Push(step);
                    return true;
                }
            }

            return false;
        }
    }

    //Plan B
    internal class DangerMap
    {
        public Stack<Step> Steps { get; set; }
        private byte[,] _possibilities;
        private int _depth;
        private readonly int _size;
        private readonly GamePoint _start;
        private readonly Direction _origin;

        public DangerMap(int depth, GamePoint start, Direction origin, byte[,] possibilities, int size)
        {
            _depth = depth;
            _size = size;
            _possibilities = possibilities;
            _start = start;
            _origin = origin;
            Steps = new Stack<Step>();
        }

        public void Start()
        {
            Count(_start, _origin, 0, new List<GamePoint>());
        }


        /// <summary>
        /// Vraci kroky v mape
        /// </summary>
        public int Fitness
        {
            get { return Steps.Count(); }
        }

        private List<Step> PossibleSteps(Direction direction, GamePoint gamePoint)
        {
            var steps = new List<Step>();

            switch (direction)
            {
                case Direction.Top:
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y - 1))
                    {

                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y - 1);

                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y - 1))
                    {

                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.TopRight:
                    if (!IsTaken(gamePoint.X, gamePoint.Y - 1))
                    {

                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y - 1))
                    {

                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.Right:
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y - 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y))
                    {

                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.BottomRight:
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y + 1))
                    {

                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.Bottom:
                    if (!IsTaken(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.BottomLeft:
                    if (!IsTaken(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.Left:
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;

                case Direction.TopLeft:
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    if (!IsTaken(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);
                        var safe = IsViable(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, safe));
                    }
                    break;
            }

            return steps;
        }

        private bool IsViable(int x, int y)
        {
            //overit jeslti vubec bod existuje
            if (x < 0 || x >= _size || y < 0 || y >= _size)
                return false;

            //overit jestli je bod dostupny
            return _possibilities[x, y] == 0;
        }

        private bool IsTaken(int x, int y)
        {
            //overit jeslti vubec bod existuje
            if (x < 0 || x >= _size || y < 0 || y >= _size)
                return true;

            //overit jestli bod je obsazeny
            return _possibilities[x, y] == 2;
        }

        private bool Count(GamePoint point, Direction direction, int depth, List<GamePoint> occupied)
        {
            if (depth == _depth)
                return true;

            var steps = PossibleSteps(direction, point);

            foreach (var step in steps)
            {
                if (occupied.Contains(step.FinalPosition))
                    continue;

                List<GamePoint> copy = occupied.Select(x => new GamePoint(x.X, x.Y)).ToList();
                copy.Add(step.FinalPosition);

                if (Count(step.FinalPosition, step.FinalDirection, depth + 1, copy))
                {
                    Steps.Push(step);
                    return true;
                }
            }

            return false;
        }

    }

    internal static class Service
    {
        public static Direction UpdateDirection(Direction direction, Move move)
        {
            switch (move)
            {
                case Move.Straight:
                    break;

                case Move.Left:
                    if (direction == (Direction)1)
                    {
                        direction = (Direction)8;
                    }
                    else
                    {
                        direction -= 1;
                    }
                    break;

                case Move.Right:
                    if (direction == (Direction)8)
                    {
                        direction = (Direction)1;
                    }
                    else
                    {
                        direction += 1;
                    }
                    break;
            }

            return direction;
        }



    }

}