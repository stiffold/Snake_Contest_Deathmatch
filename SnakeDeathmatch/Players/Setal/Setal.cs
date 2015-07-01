using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;

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

        private List<EnemyHead> _enemies;

        public void Init(int playerId, int playgroundSize, int x, int y, Direction direction)
        {
            _size = playgroundSize;
            _direction = direction;
            _identificator = playerId;
            _actualPosition = new GamePoint(x, y);
            _possibilities = new byte[_size, _size];
            _roundCounter = 0;

            _preferMove = (int)_direction < 5 ? Move.Right : Move.Left;
            //_preferMove = Move.Straight;

            _enemies = new List<EnemyHead>();

            //_moveCounter = 0;
        }

        public Move GetNextMove(int[,] playground)
        {
            _roundCounter++;

            _possibilities = new byte[_size, _size];

            EvaluateGame(playground);
            WatchYourEnemies(playground);

            if (_roundCounter > 1)
                EvaluateEnemies(3);

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

            return Move.Left;
        }

        public string Name
        {
            get { return "Setal(Šimík)"; }
        }

        private void EvaluateGame(int[,] playground)
        {
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                {
                    if ((playground[i, j] != 0))
                    {
                        MarkPointAsOccupied(i, j, (playground[i, j] != _identificator));
                    }
                }
            EvadeNarrowPlaces(playground);
        }

        private void EvadeNarrowPlaces(int[,] playground)
        {
            int holder;

            #region Vypocti radky

            for (int i = 0; i < 20; i++)
            {
                holder = 0;

                for (int j = 0; j < 20; j++)
                {
                    //Obsazeno
                    if (playground[i, j] != 0)
                    {
                        //Zkontroluj vzdalenost
                        if (((j - holder) < 5) && (holder != 0))
                        {
                            for (int x = holder; x < j; x++)
                            {
                                MarkPointAsDanger(i, x);
                            }
                        }

                        holder = j;
                    }
                }
            }
            #endregion

            #region Vypocet sloupcu
            for (int j = 0; j < 20; j++)
            {
                holder = 0;

                for (int i = 0; i < 20; i++)
                {
                    //Obsazeno
                    if (playground[i, j] != 0)
                    {
                        //Zkontroluj vzdalenost
                        if (((i - holder) < 5) && (holder != 0))
                        {
                            for (int x = holder; x < i; x++)
                            {
                                MarkPointAsDanger(x, j);
                            }

                        }
                        holder = i;
                    }
                }
            }
            #endregion
        }

        private void WatchYourEnemies(int[,] playground)
        {
            //Prvni kolo
            if (_roundCounter == 1)
            {
                LookUpEnemies(playground);
            }
            //Druhe kolo musim urcit smer
            else if (_roundCounter == 2)
            {
                foreach (var head in _enemies)
                {
                    var position = head.Actual;

                    //Hledam sousedni dilek, protoze v druhem kole je jen jeden
                    #region FindSecondBlock

                    //Direction.TopLeft
                    if ((IsValidPoint(position.X - 1, position.Y - 1)) && (playground[position.X - 1, position.Y - 1] == head.Identifikator))
                        head.NextRound(new GamePoint(position.X - 1, position.Y - 1));

                   //Direction.Top
                    else if ((IsValidPoint(position.X, position.Y - 1)) && (playground[position.X, position.Y - 1] == head.Identifikator))
                        head.NextRound(new GamePoint(position.X, position.Y - 1));

                    //Direction.TopRight
                    else if ((IsValidPoint(position.X + 1, position.Y - 1)) && (playground[position.X + 1, position.Y - 1] == head.Identifikator))
                        head.NextRound(new GamePoint(position.X + 1, position.Y - 1));

                    //Direction.Right
                    else if ((IsValidPoint(position.X - 1, position.Y)) && (playground[position.X + 1, position.Y] == head.Identifikator))
                        head.NextRound(new GamePoint(position.X + 1, position.Y));

                    //Direction.BottomRight
                    else if ((IsValidPoint(position.X + 1, position.Y + 1)) && (playground[position.X + 1, position.Y + 1] == head.Identifikator))
                        head.NextRound(new GamePoint(position.X + 1, position.Y + 1));

                       //Direction.Bottom
                    else if ((IsValidPoint(position.X, position.Y + 1)) && (playground[position.X, position.Y + 1] == head.Identifikator))
                        head.NextRound(new GamePoint(position.X, position.Y + 1));

                    //Direction.BottomLeft
                    else if ((IsValidPoint(position.X - 1, position.Y + 1)) && (playground[position.X - 1, position.Y + 1] == head.Identifikator))
                        head.NextRound(new GamePoint(position.X - 1, position.Y + 1));

                    //Direction.Left
                    else if ((IsValidPoint(position.X - 1, position.Y)) && (playground[position.X - 1, position.Y] == head.Identifikator))
                        head.NextRound(new GamePoint(position.X - 1, position.Y));

                    //Jinak nehral
                    else
                        head.InGame = false;

                    #endregion
                }
            }
            else
            {
                //Zajimaji me jen ti co nevypadli
                foreach (var head in _enemies.Where(x => x.InGame).ToList())
                {
                    var point = FindEnemyHeadByNearPoint(head, playground);
                    //Jestlize nic nenajdu tak uz dohral :P
                    if (point != null)
                        head.NextRound(point);
                }
            }
        }

        private void EvaluateEnemies(int maxDepth)
        {
            foreach (var enemy in _enemies.Where(x => x.InGame).ToList())
            {
                EvaulatePossibleDanger(0, maxDepth, enemy.Actual, enemy.ActualDirection);
            }
        }

        private void EvaulatePossibleDanger(int depth, int maxDepth, GamePoint point, Direction direction)
        {
            if (depth == maxDepth)
                return;

            var possibleSteps = PossibleEnemySteps(direction, point);

            foreach (Step step in possibleSteps)
            {
                MarkPointAsDanger(step.FinalPosition.X, step.FinalPosition.Y);
                EvaulatePossibleDanger(depth + 1, maxDepth, step.FinalPosition, step.FinalDirection);
            }
        }

        private GamePoint FindEnemyHeadByNearPoint(EnemyHead head, int[,] playground)
        {
            switch (head.ActualDirection)
            {
                case Direction.TopLeft:
                    if ((IsValidPoint(head.Actual.X - 1, head.Actual.Y)) && (playground[head.Actual.X - 1, head.Actual.Y] == head.Identifikator))
                        return new GamePoint(head.Actual.X - 1, head.Actual.Y);

                    else if ((IsValidPoint(head.Actual.X - 1, head.Actual.Y - 1)) && (playground[head.Actual.X - 1, head.Actual.Y - 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X - 1, head.Actual.Y - 1);

                    else if ((IsValidPoint(head.Actual.X, head.Actual.Y - 1)) && (playground[head.Actual.X, head.Actual.Y - 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X, head.Actual.Y - 1);
                    else
                        return null;

                case Direction.Top:
                    if ((IsValidPoint(head.Actual.X - 1, head.Actual.Y - 1)) && (playground[head.Actual.X - 1, head.Actual.Y - 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X - 1, head.Actual.Y - 1);

                    else if ((IsValidPoint(head.Actual.X, head.Actual.Y - 1)) && (playground[head.Actual.X, head.Actual.Y - 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X, head.Actual.Y - 1);

                    else if ((IsValidPoint(head.Actual.X + 1, head.Actual.Y - 1)) && (playground[head.Actual.X + 1, head.Actual.Y - 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X + 1, head.Actual.Y - 1);
                    else
                        return null;


                case Direction.TopRight:
                    if ((IsValidPoint(head.Actual.X, head.Actual.Y - 1)) && (playground[head.Actual.X, head.Actual.Y - 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X, head.Actual.Y - 1);

                    else if ((IsValidPoint(head.Actual.X + 1, head.Actual.Y - 1)) && (playground[head.Actual.X + 1, head.Actual.Y - 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X + 1, head.Actual.Y - 1);

                    else if ((IsValidPoint(head.Actual.X + 1, head.Actual.Y)) && (playground[head.Actual.X + 1, head.Actual.Y] == head.Identifikator))
                        return new GamePoint(head.Actual.X + 1, head.Actual.Y);
                    else
                        return null;


                case Direction.Right:
                    if ((IsValidPoint(head.Actual.X + 1, head.Actual.Y - 1)) && (playground[head.Actual.X + 1, head.Actual.Y - 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X + 1, head.Actual.Y - 1);

                    else if ((IsValidPoint(head.Actual.X + 1, head.Actual.Y)) && (playground[head.Actual.X + 1, head.Actual.Y] == head.Identifikator))
                        return new GamePoint(head.Actual.X + 1, head.Actual.Y);

                    else if ((IsValidPoint(head.Actual.X + 1, head.Actual.Y + 1)) && (playground[head.Actual.X + 1, head.Actual.Y + 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X + 1, head.Actual.Y + 1);
                    else
                        return null;


                case Direction.BottomRight:
                    if ((IsValidPoint(head.Actual.X + 1, head.Actual.Y)) && (playground[head.Actual.X + 1, head.Actual.Y] == head.Identifikator))
                        return new GamePoint(head.Actual.X + 1, head.Actual.Y);

                    else if ((IsValidPoint(head.Actual.X + 1, head.Actual.Y + 1)) && (playground[head.Actual.X + 1, head.Actual.Y + 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X + 1, head.Actual.Y + 1);

                    else if ((IsValidPoint(head.Actual.X, head.Actual.Y + 1)) && (playground[head.Actual.X, head.Actual.Y + 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X, head.Actual.Y + 1);
                    else
                        return null;


                case Direction.Bottom:
                    if ((IsValidPoint(head.Actual.X + 1, head.Actual.Y + 1)) && (playground[head.Actual.X + 1, head.Actual.Y + 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X + 1, head.Actual.Y + 1);

                    else if ((IsValidPoint(head.Actual.X, head.Actual.Y + 1)) && (playground[head.Actual.X, head.Actual.Y + 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X, head.Actual.Y + 1);

                    else if ((IsValidPoint(head.Actual.X - 1, head.Actual.Y + 1)) && (playground[head.Actual.X - 1, head.Actual.Y + 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X - 1, head.Actual.Y + 1);
                    else
                        return null;


                case Direction.BottomLeft:
                    if ((IsValidPoint(head.Actual.X, head.Actual.Y + 1)) && (playground[head.Actual.X, head.Actual.Y + 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X, head.Actual.Y + 1);

                    else if ((IsValidPoint(head.Actual.X - 1, head.Actual.Y + 1)) && (playground[head.Actual.X - 1, head.Actual.Y + 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X - 1, head.Actual.Y + 1);

                    else if ((IsValidPoint(head.Actual.X - 1, head.Actual.Y)) && (playground[head.Actual.X - 1, head.Actual.Y] == head.Identifikator))
                        return new GamePoint(head.Actual.X - 1, head.Actual.Y);
                    else
                        return null;


                case Direction.Left:
                    if ((IsValidPoint(head.Actual.X - 1, head.Actual.Y + 1)) && (playground[head.Actual.X - 1, head.Actual.Y + 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X - 1, head.Actual.Y + 1);

                    else if ((IsValidPoint(head.Actual.X - 1, head.Actual.Y)) && (playground[head.Actual.X - 1, head.Actual.Y] == head.Identifikator))
                        return new GamePoint(head.Actual.X - 1, head.Actual.Y);

                    else if ((IsValidPoint(head.Actual.X - 1, head.Actual.Y - 1)) && (playground[head.Actual.X - 1, head.Actual.Y - 1] == head.Identifikator))
                        return new GamePoint(head.Actual.X - 1, head.Actual.Y - 1);
                    else
                        return null;

                default:
                    return null;
            }

            //Just for safety
            return null;
        }

        private bool IsValidPoint(int x, int y)
        {
            return ((x >= 0) && (x < _size) && (y >= 0) && (y < _size));

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
        /// Bezpecne oznaceni obsazenosti pole
        /// </summary>
        private void MarkPointAsOccupied(int x, int y, bool isEnemy)
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

        /// <summary>
        /// Bezpecne oznaceni nebezpeci
        /// </summary>
        private void MarkPointAsDanger(int x, int y)
        {
            if ((IsValidPoint(x, y)) && (_possibilities[x, y] == 0))
            {
                _possibilities[x, y] = 1;
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

        private void LookUpEnemies(int[,] playground)
        {
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                {
                    if ((playground[i, j] != 0) && (playground[i, j] != _identificator))
                    {
                        var EnemyFound = new EnemyHead(playground[i, j], new GamePoint(i, j));
                        _enemies.Add(EnemyFound);

                    }
                }
        }

        private List<Step> PossibleEnemySteps(Direction direction, GamePoint gamePoint)
        {
            var steps = new List<Step>();

            switch (direction)
            {
                case Direction.Top:
                    if (IsValidPoint(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);

                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X + 1, gamePoint.Y - 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    break;

                case Direction.TopRight:
                    if (IsValidPoint(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X + 1, gamePoint.Y - 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    break;

                case Direction.Right:
                    if (IsValidPoint(gamePoint.X + 1, gamePoint.Y - 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X + 1, gamePoint.Y))
                    {

                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    break;

                case Direction.BottomRight:
                    if (IsValidPoint(gamePoint.X + 1, gamePoint.Y))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X + 1, gamePoint.Y + 1))
                    {

                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    break;

                case Direction.Bottom:
                    if (IsValidPoint(gamePoint.X + 1, gamePoint.Y + 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X + 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    break;

                case Direction.BottomLeft:
                    if (IsValidPoint(gamePoint.X, gamePoint.Y + 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    break;

                case Direction.Left:
                    if (IsValidPoint(gamePoint.X - 1, gamePoint.Y + 1))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y + 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    break;

                case Direction.TopLeft:
                    if (IsValidPoint(gamePoint.X - 1, gamePoint.Y))
                    {
                        var move = Move.Left;
                        var dir = Service.UpdateDirection(direction, Move.Left);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X - 1, gamePoint.Y - 1))
                    {
                        var move = Move.Straight;
                        var dir = Service.UpdateDirection(direction, Move.Straight);
                        var point = new GamePoint(gamePoint.X - 1, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    if (IsValidPoint(gamePoint.X, gamePoint.Y - 1))
                    {
                        var move = Move.Right;
                        var dir = Service.UpdateDirection(direction, Move.Right);
                        var point = new GamePoint(gamePoint.X, gamePoint.Y - 1);

                        steps.Add(new Step(move, dir, point, true));
                    }
                    break;
            }

            return steps;
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
            if (steps.Count > 1)
            {
                if (_prefer == Move.Right)
                {
                    steps = steps.OrderByDescending(x => x.Move).ToList();
                }
                else if (_prefer == Move.Left)
                {
                    steps = steps.OrderBy(x => x.Move).ToList();
                }
                else if ((_prefer == Move.Straight))
                {
                    steps = steps.OrderBy(x => x.Move).ToList();
                    for (int i = 0; i < steps.Count; i++)
                    {
                        if (steps[0].Move == Move.Straight)
                            break;
                        else
                        {
                            var m = steps[0];
                            steps.Remove(m);
                            steps.Add(m);
                        }
                    }
                }
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
            if ((start.X == final.X) || (start.Y == final.Y))
                return false;

            return ((_possibilities[start.X, final.Y] == 2) && (_possibilities[final.X, start.Y] == 2));
        }

        private bool Count(GamePoint point, Direction direction, int depth, List<GamePoint> occupied)
        {
            if (depth == _depth)
                return true;

            //Mozne nebezpecne kroky setridene bezecnymi napred
            var steps = PossibleSteps(direction, point).OrderBy(x => x.IsSafe ? 0 : 1).ToList();

            foreach (var step in steps)
            {
                if ((occupied.Contains(step.FinalPosition)) || (IsCrossCollision(point, step.FinalPosition)))
                    continue;

                //List<GamePoint> copy = occupied.Select(x => new GamePoint(x.X, x.Y)).ToList();
                occupied.Add(step.FinalPosition);

                if (Count(step.FinalPosition, step.FinalDirection, depth + 1, occupied))
                {
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

        public static Direction ComputeDirection(GamePoint start, GamePoint end)
        {
            if (start.X > end.X)
            {
                if (start.Y > end.Y)
                {
                    return Direction.TopLeft;
                }
                else if (start.Y < end.Y)
                {
                    return Direction.BottomLeft;
                }
                else if (start.Y == end.Y)
                {
                    return Direction.Left;
                }
            }
            else if (start.X < end.X)
            {
                if (start.Y > end.Y)
                {
                    return Direction.TopRight;
                }
                else if (start.Y < end.Y)
                {
                    return Direction.BottomRight;
                }
                else if (start.Y == end.Y)
                {
                    return Direction.Right;
                }
            }
            else if (start.X == end.X)
            {
                if (start.Y > end.Y)
                {
                    return Direction.Top;
                }
                else if (start.Y < end.Y)
                {
                    return Direction.Bottom;
                }
            }

            //Some terrible shit happens...
            return Direction.Top;
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

    internal class EnemyHead
    {
        public GamePoint Previous { get; set; }
        public GamePoint Actual { get; set; }

        public bool InGame { get; set; }
        public int Identifikator { get; private set; }

        private Direction? _direction;

        public EnemyHead(int identifikator, GamePoint start)
        {
            Identifikator = identifikator;
            Actual = start;
            _direction = null;
            InGame = true;
        }

        /// <summary>
        /// Posune predek hracee na point
        /// </summary>
        /// <param name="point">Novy predek hrace</param>
        public void NextRound(GamePoint point)
        {
            Previous = Actual;
            Actual = point;

            _direction = Service.ComputeDirection(Previous, Actual);

        }

        public Direction ActualDirection
        {
            get { return _direction.Value; }
        }
    }
}