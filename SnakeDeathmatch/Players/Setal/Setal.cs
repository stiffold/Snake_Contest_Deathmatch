using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Setal
{
    public class Setal : IPlayerBehaviour2
    {
        private byte[,] _possibilities;
        private int _identificator;
        private int _size;
        private int _roundCounter;
        //private int _moveCounter;

        private Direction _direction;
        private GamePoint _actualPosition;

        //private Statistik _statistik;
        private Move _preferMove;
        // private bool _locationChoosed;

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _size = playgroundSize;
            _direction = direction;
            _identificator = playerId;
            _actualPosition = new GamePoint(x, y);
            _possibilities = new byte[_size, _size];
            _roundCounter = 0;

            _preferMove = (int)_direction < 5 ? Move.Right : Move.Left;

            //_moveCounter = 0;
        }

        public Move GetNextMove(int[,] playground)
        {
            _roundCounter++;
           // _statistik = new Statistik();

            EvaluateGame(playground);

            SafeMap safePath = new SafeMap(15, _actualPosition, _direction, _preferMove, _possibilities, _size);
            safePath.Start();

            if (safePath.Result != null)
            {
                _actualPosition = safePath.Result.FinalPosition;
                _direction = Service.UpdateDirection(_direction, safePath.Result.Move);
                // MessageBox.Show("Jdu:" + safePath.Result.Move);
                return safePath.Result.Move;
            }


            DangerMap dangerPath = new DangerMap(6, _actualPosition, _direction, _possibilities, _size);
            dangerPath.Start();

            if (dangerPath.Result != null)
            {
                _actualPosition = dangerPath.Result.FinalPosition;
                _direction = Service.UpdateDirection(_direction, dangerPath.Result.Move);
                //MessageBox.Show("Danger Jdu:" + dangerPath.Result.Move);
                return dangerPath.Result.Move;
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
                    if ((gameSurrond[i, j] != 0))
                    {
                        MarkPoint(i, j, (gameSurrond[i, j] != _identificator));

                        //if (i < (_size / 2))
                        //{
                        //    if (j < (_size / 2))
                        //    {
                        //        _statistik.TopLeft++;
                        //    }
                        //    else
                        //    {
                        //        _statistik.BottomLeft++;
                        //    }
                        //}
                        //else
                        //{
                        //    if (j < (_size / 2))
                        //    {
                        //        _statistik.TopRight++;
                        //    }
                        //    else
                        //    {
                        //        _statistik.BottomRight++;
                        //    }
                        //}
                    }
                }
        }

        private bool IsTakenByEnemy(int[,] playground, int x, int y)
        {
            //overit jeslti vubec bod existuje
            if (x < 0 || x >= _size || y < 0 || y >= _size)
                return false;

            //overit jestli bod je obsazeny
            return ((playground[x, y] != 0) && (playground[x, y] != _identificator));
        }

        /// <summary>
        /// Oznacení nedostupnosti pole
        /// </summary>
        private void MarkPoint(int x, int y, bool isEnemy)
        {
            _possibilities[x, y] = 2;

            if (!isEnemy)
                return;

            if (x - 1 >= 0)
            {
                if (_possibilities[x - 1, y] == 0)
                    _possibilities[x - 1, y] = 1;
            }
            if (x + 1 < _size)
            {
                if (_possibilities[x + 1, y] == 0)
                    _possibilities[x + 1, y] = 1;
            }
            if (y - 1 >= 0)
            {
                if (_possibilities[x, y - 1] == 0)
                    _possibilities[x, y - 1] = 1;
            }
            if (y + 1 < _size)
            {
                if (_possibilities[x, y + 1] == 0)
                    _possibilities[x, y + 1] = 1;
            }

            if ((x - 1 >= 0) && (y - 1 > 0))
            {
                if (_possibilities[x - 1, y - 1] == 0)
                    _possibilities[x - 1, y - 1] = 1;
            }

            if ((x + 1 < _size) && (y - 1 >= 0))
            {
                if (_possibilities[x + 1, y - 1] == 0)
                    _possibilities[x + 1, y - 1] = 1;
            }

            if ((x + 1 < _size) && (y + 1 < _size))
            {
                if (_possibilities[x + 1, y + 1] == 0)
                    _possibilities[x + 1, y + 1] = 1;
            }

            if ((x - 1 >= 0) && (y + 1 < _size))
            {
                if (_possibilities[x - 1, y + 1] == 0)
                    _possibilities[x - 1, y + 1] = 1;
            }

        }

        private bool IsEnemyNearBy(GamePoint head, int[,] playground)
        {
            for (int i = (head.X - 5); i < (head.X + 5); i++)
                for (int j = (head.Y - 5); j < (head.Y + 5); j++)
                {
                    if (IsTakenByEnemy(playground, i, j))
                        return true;
                }

            return false;

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

    //Safe travel and long live the snake
    internal class SafeMap
    {
        public Step Result { get; set; }
        private byte[,] _possibilities;
        private int _depth;
        private readonly int _size;
        private readonly GamePoint _start;
        private readonly Direction _origin;
        private readonly Move _prefer;

        public SafeMap(int depth, GamePoint start, Direction origin, Move prefer, byte[,] possibilities, int size)
        {
            _depth = depth;
            _prefer = prefer;
            _size = size;
            _possibilities = possibilities;
            _start = start;
            _origin = origin;
            Result = null;
        }

        public void Start()
        {
            Count(_start, _origin, 0, new List<GamePoint>());
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

        private bool IsCrossCollision(GamePoint start, GamePoint final)
        {
            return ((_possibilities[start.X, final.Y] == 2) && (_possibilities[final.X, start.Y] == 2));
        }

        private bool Count(GamePoint point, Direction direction, int depth, List<GamePoint> occupied)
        {
            if (depth == _depth)
                return true;

            var steps = PossibleSteps(direction, point);
            if (_prefer == Move.Right)
            {
                steps = steps.OrderByDescending(x => x.Move).ToList();
            }
            else
            {
                steps = steps.OrderBy(x => x.Move).ToList();
            }

            foreach (var step in steps)
            {
                if ((occupied.Contains(step.FinalPosition)) || (IsCrossCollision(point, step.FinalPosition)) || (!step.IsSafe))
                    continue;

                //List<GamePoint> copy = occupied.Select(x => new GamePoint(x.X, x.Y)).ToList();
                occupied.Add(step.FinalPosition);

                if (Count(step.FinalPosition, step.FinalDirection, depth + 1, occupied))
                {
                    //zajima me jen posledni bod
                    if (depth == 0)
                        Result = step;
                    return true;
                }
                else
                {
                    occupied.Remove(step.FinalPosition);
                }
            }

            return false;
        }
    }

    //Plan B
    internal class DangerMap
    {
        public Step Result { get; set; }
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
            Result = null;
        }

        public void Start()
        {
            Count(_start, _origin, 0, new List<GamePoint>());
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

        private bool IsCrossCollision(GamePoint start, GamePoint final)
        {
            return ((_possibilities[start.X, final.Y] == 2) && (_possibilities[start.Y, final.X] == 2));
        }

        private bool Count(GamePoint point, Direction direction, int depth, List<GamePoint> occupied)
        {
            if (depth == _depth)
                return true;

            //Mozne nebezpecne kroky setridene bezecnymi napred
            var steps = PossibleSteps(direction, point).OrderBy(x => x.IsSafe ? 0 : 1).ToList();

            foreach (var step in steps)
            {
                if ((occupied.Contains(step.FinalPosition)) || (!IsCrossCollision(point, step.FinalPosition)))
                    continue;

                List<GamePoint> copy = occupied.Select(x => new GamePoint(x.X, x.Y)).ToList();
                copy.Add(step.FinalPosition);

                if (Count(step.FinalPosition, step.FinalDirection, depth + 1, copy))
                {
                    if (depth == 0)
                        Result = step;
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

        public static Move GuesMove(Direction wanted, Direction actual)
        {
            switch (wanted)
            {
                case Direction.TopLeft:
                    if (actual == Direction.TopLeft)
                        return Move.Straight;
                    if (actual == Direction.Top)
                        return Move.Left;
                    if (actual == Direction.Left)
                        return Move.Right;
                    return Move.Left;

                case Direction.TopRight:
                    if (actual == Direction.TopRight)
                        return Move.Straight;
                    if (actual == Direction.Top)
                        return Move.Right;
                    if (actual == Direction.Right)
                        return Move.Left;
                    return Move.Left;

                case Direction.BottomRight:
                    if (actual == Direction.BottomRight)
                        return Move.Straight;
                    if (actual == Direction.Bottom)
                        return Move.Right;
                    if (actual == Direction.Right)
                        return Move.Left;
                    return Move.Left;

                case Direction.BottomLeft:
                    if (actual == Direction.BottomLeft)
                        return Move.Straight;
                    if (actual == Direction.Bottom)
                        return Move.Left;
                    if (actual == Direction.Left)
                        return Move.Left;
                    return Move.Left;

                default:
                    throw new NotImplementedException();

            }
        }
    }

    internal class Statistik
    {
        public int TopLeft { get; set; }
        public int TopRight { get; set; }
        public int BottomLeft { get; set; }
        public int BottomRight { get; set; }

        public int StarWayToHeaven()
        {
            if ((TopLeft < TopRight) && (TopLeft < BottomLeft) && (TopLeft < BottomRight))
                return (int)Direction.TopLeft;

            if ((TopRight < TopLeft) && (TopRight < BottomLeft) && (TopRight < BottomRight))
                return (int)Direction.TopRight;

            if ((BottomLeft < TopRight) && (BottomLeft < TopLeft) && (BottomLeft < BottomRight))
                return (int)Direction.BottomLeft;

            if ((BottomRight < TopRight) && (BottomRight < BottomLeft) && (BottomRight < TopLeft))
                return (int)Direction.BottomRight;

            return 0;
        }
    }

}