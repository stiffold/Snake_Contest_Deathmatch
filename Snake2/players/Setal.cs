using System.Collections.Generic;
using Snake2.game;

namespace Snake2.players
{
    public class Setal : IPlayerBehavior
    {
        private byte[,] _possibilities;
        private int _identificator;
        private int _size;
        private enums.Direction _direction;
        private GamePoint _actualPosition;
        private bool _firstRun;

        public Setal()
        {
            _firstRun = true;
        }

        public void Init(int direction, int identificator)
        {
            _direction = (enums.Direction)direction;
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
                //zkontroluj že mužeš tahnout i další tah
                var innerStep = PossibleSteps(step.FinalDirection, step.FinalPosition);
                if (innerStep.Count > 0)
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
            // throw new InvalidOperationException("Nejsem na herní ploše!");
            return new GamePoint(0, 0);
        }

        /// <summary>
        /// Oznaèení nedostupnosti pole
        /// </summary>
        private void MarkPoint(int x, int y)
        {
            if (x - 1 > 0)
            {
                _possibilities[x - 1, y] = 1;
            }
            if (x + 1 < _size)
            {
                _possibilities[x + 1, y] = 1;
            }
            if (y - 1 > 0)
            {
                _possibilities[x, y - 1] = 1;
            }
            if (y + 1 < _size)
            {
                _possibilities[x, y + 1] = 1;
            }
        }

        private List<Step> PossibleSteps(enums.Direction direction, GamePoint gamePoint)
        {
            var steps = new List<Step>();

            switch (direction)
            {
                case enums.Direction.Top:
                    if (IsViable(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = enums.Move.Left;
                        var dir = UpdateDirection(_direction, enums.Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = enums.Move.Straight;
                        var dir = UpdateDirection(_direction, enums.Move.Straight);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y - 1))
                    {
                        var move = enums.Move.Right;
                        var dir = UpdateDirection(_direction, enums.Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case enums.Direction.TopRight:
                    if (IsViable(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = enums.Move.Left;
                        var dir = UpdateDirection(_direction, enums.Move.Left);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y - 1))
                    {
                        var move = enums.Move.Straight;
                        var dir = UpdateDirection(_direction, enums.Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = enums.Move.Right;
                        var dir = UpdateDirection(_direction, enums.Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case enums.Direction.Right:
                    if (IsViable(gamePoint.X + 1, gamePoint.Y - 1))
                    {
                        var move = enums.Move.Left;
                        var dir = UpdateDirection(_direction, enums.Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = enums.Move.Straight;
                        var dir = UpdateDirection(_direction, enums.Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = enums.Move.Right;
                        var dir = UpdateDirection(_direction, enums.Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case enums.Direction.BottomRight:
                    if (IsViable(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = enums.Move.Left;
                        var dir = UpdateDirection(_direction, enums.Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = enums.Move.Straight;
                        var dir = UpdateDirection(_direction, enums.Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = enums.Move.Right;
                        var dir = UpdateDirection(_direction, enums.Move.Right);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case enums.Direction.Bottom:
                    if (IsViable(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = enums.Move.Left;
                        var dir = UpdateDirection(_direction, enums.Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = enums.Move.Straight;
                        var dir = UpdateDirection(_direction, enums.Move.Straight);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = enums.Move.Right;
                        var dir = UpdateDirection(_direction, enums.Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case enums.Direction.BottomLeft:
                    if (IsViable(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = enums.Move.Left;
                        var dir = UpdateDirection(_direction, enums.Move.Left);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = enums.Move.Straight;
                        var dir = UpdateDirection(_direction, enums.Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = enums.Move.Right;
                        var dir = UpdateDirection(_direction, enums.Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case enums.Direction.Left:
                    if (IsViable(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = enums.Move.Left;
                        var dir = UpdateDirection(_direction, enums.Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = enums.Move.Straight;
                        var dir = UpdateDirection(_direction, enums.Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = enums.Move.Right;
                        var dir = UpdateDirection(_direction, enums.Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;

                case enums.Direction.TopLeft:
                    if (IsViable(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = enums.Move.Left;
                        var dir = UpdateDirection(_direction, enums.Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = enums.Move.Straight;
                        var dir = UpdateDirection(_direction, enums.Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    if (IsViable(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = enums.Move.Right;
                        var dir = UpdateDirection(_direction, enums.Move.Right);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point));
                    }
                    break;
            }

            return steps;
        }

        private bool IsViable(GamePoint gamePoint)
        {
            //ovìøit jeslti vubec bod existuje
            if (gamePoint.X < 0 || gamePoint.X >= _size || gamePoint.Y < 0 || gamePoint.Y >= _size)
                return false;

            //ovìøit jestli je bod možný
            return _possibilities[gamePoint.X, gamePoint.Y] == 0;
        }

        private bool IsViable(int X, int Y)
        {
            //ovìøit jeslti vubec bod existuje
            if (X < 0 || X >= _size || Y < 0 || Y >= _size)
                return false;

            //ovìøit jestli je bod možný
            return _possibilities[X, Y] == 0;
        }

        private enums.Direction UpdateDirection(enums.Direction direction, enums.Move move)
        {
            switch (move)
            {
                case enums.Move.Straight:
                    break;

                case enums.Move.Left:
                    if (direction == (enums.Direction)1)
                    {
                        direction = (enums.Direction)8;
                    }
                    else
                    {
                        direction -= 1;
                    }
                    break;

                case enums.Move.Right:
                    if (direction == (enums.Direction)8)
                    {
                        direction = (enums.Direction)1;
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
        public Step(enums.Move move, enums.Direction finalDirection, GamePoint finalPosition)
        {
            Move = move;
            FinalDirection = finalDirection;
            FinalPosition = finalPosition;
        }

        public enums.Move Move { get; set; }
        public enums.Direction FinalDirection { get; set; }
        public GamePoint FinalPosition { get; set; }
    }

    internal class Map
    {

    }

}