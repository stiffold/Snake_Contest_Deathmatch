using System;
using System.Collections.Generic;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Setal
{
    public class Setal : IPlayerBehavior
    {
        private byte[,] _possibilities;
        private int _identificator;
        private int _size;
        private Interface.Direction _direction;
        private GamePoint _actualPosition;
        private bool _firstRun;

        public Setal()
        {
            _firstRun = true;
        }

        public void Init(int direction, int identificator)
        {
            _direction = (Interface.Direction)direction;
            _identificator = identificator;
        }

        public int NextMove(int[,] gameSurrond)
        {
            if (_firstRun)
            {
                _size = gameSurrond.GetUpperBound(0) + 1;
                _possibilities = new byte[_size, _size];
                _actualPosition = FindMySelf(gameSurrond);
                _firstRun = false;
            }

            EvaluateGame(gameSurrond);

            var steps = PossibleSteps(_direction, _actualPosition);

            foreach (var step in steps)
            {
                //zkontroluj ze muzes tahnout i dalsi tah
                var innerStep = PossibleSteps(step.FinalDirection, step.FinalPosition);
                if (innerStep.Count >= 2)
                {
                    //rozhodl jsem se pro tah, aktualizuj polohu
                    _direction = UpdateDirection(_direction, step.Move);
                    _actualPosition = step.FinalPosition;
                    return (int)step.Move;
                }
            }
            return 2;
        }

        public string MyName()
        {
            return "Setal(Šimík)";
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
        }

        private List<Step> PossibleSteps(Interface.Direction direction, GamePoint gamePoint)
        {
            var steps = new List<Step>();

            switch (direction)
            {
                case Interface.Direction.Top:
                    if (IsViable(gamePoint.X - 1, gamePoint.Y - 1))
                    {

                        var move = Interface.Move.Left;
                        var dir = UpdateDirection(direction, Interface.Move.Left);

                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = Interface.Move.Straight;
                        var dir = UpdateDirection(direction, Interface.Move.Straight);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y - 1))
                    {

                        var move = Interface.Move.Right;
                        var dir = UpdateDirection(direction, Interface.Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case Interface.Direction.TopRight:
                    if (IsViable(gamePoint.X, gamePoint.Y - 1))
                    {

                        var move = Interface.Move.Left;
                        var dir = UpdateDirection(direction, Interface.Move.Left);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y - 1))
                    {

                        var move = Interface.Move.Straight;
                        var dir = UpdateDirection(direction, Interface.Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = Interface.Move.Right;
                        var dir = UpdateDirection(direction, Interface.Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case Interface.Direction.Right:
                    if (IsViable(gamePoint.X + 1, gamePoint.Y - 1))
                    {
                        var move = Interface.Move.Left;
                        var dir = UpdateDirection(direction, Interface.Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y))
                    {

                        var move = Interface.Move.Straight;
                        var dir = UpdateDirection(direction, Interface.Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = Interface.Move.Right;
                        var dir = UpdateDirection(direction, Interface.Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case Interface.Direction.BottomRight:
                    if (IsViable(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = Interface.Move.Left;
                        var dir = UpdateDirection(direction, Interface.Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y + 1))
                    {

                        var move = Interface.Move.Straight;
                        var dir = UpdateDirection(direction, Interface.Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Interface.Move.Right;
                        var dir = UpdateDirection(direction, Interface.Move.Right);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case Interface.Direction.Bottom:
                    if (IsViable(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = Interface.Move.Left;
                        var dir = UpdateDirection(direction, Interface.Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Interface.Move.Straight;
                        var dir = UpdateDirection(direction, Interface.Move.Straight);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Interface.Move.Right;
                        var dir = UpdateDirection(direction, Interface.Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case Interface.Direction.BottomLeft:
                    if (IsViable(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Interface.Move.Left;
                        var dir = UpdateDirection(direction, Interface.Move.Left);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Interface.Move.Straight;
                        var dir = UpdateDirection(direction, Interface.Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Interface.Move.Right;
                        var dir = UpdateDirection(direction, Interface.Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case Interface.Direction.Left:
                    if (IsViable(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Interface.Move.Left;
                        var dir = UpdateDirection(direction, Interface.Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Interface.Move.Straight;
                        var dir = UpdateDirection(direction, Interface.Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = Interface.Move.Right;
                        var dir = UpdateDirection(direction, Interface.Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case Interface.Direction.TopLeft:
                    if (IsViable(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Interface.Move.Left;
                        var dir = UpdateDirection(direction, Interface.Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = Interface.Move.Straight;
                        var dir = UpdateDirection(direction, Interface.Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = Interface.Move.Right;
                        var dir = UpdateDirection(direction, Interface.Move.Right);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
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

        private Interface.Direction UpdateDirection(Interface.Direction direction, Interface.Move move)
        {
            switch (move)
            {
                case Interface.Move.Straight:
                    break;

                case Interface.Move.Left:
                    if (direction == (Interface.Direction)1)
                    {
                        direction = (Interface.Direction)8;
                    }
                    else
                    {
                        direction -= 1;
                    }
                    break;

                case Interface.Move.Right:
                    if (direction == (Interface.Direction)8)
                    {
                        direction = (Interface.Direction)1;
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
        public Step(Interface.Move move, Interface.Direction finalDirection, GamePoint finalPosition, bool isSafe = true)
        {
            Move = move;
            FinalDirection = finalDirection;
            FinalPosition = finalPosition;
            IsSafe = isSafe;
        }

        public Interface.Move Move { get; set; }
        public Interface.Direction FinalDirection { get; set; }
        public GamePoint FinalPosition { get; set; }
        public bool IsSafe { get; set; }
    }

    internal class Map
    {

    }

}