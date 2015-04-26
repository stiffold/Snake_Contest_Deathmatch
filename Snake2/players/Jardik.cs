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
        private int _myID;
        private int _round;
        private int _max = 0;
        Random r = new Random(Environment.TickCount);
        private List<Tuple<int, Move, Direction, Position>> _myPlanedMoves = new List<Tuple<int, Move, Direction, Position>>();
        public void Init(int direction, int identificator)
        {
            _myFirstDirection = (Direction)direction;
            _myID = identificator;
            _round = 0;
        }

        public int NextMove(int[,] gameSurrond)
        {
            if (_max == 0)
                _max = gameSurrond.GetUpperBound(0) + 1;

            _round++;
            if (!_myPlanedMoves.Any(p=>p.Item1 ==_round))
            {
                PlanMyMoves(gameSurrond);
            }
            
            var nextMove = _myPlanedMoves.Where(x => x.Item1 == _round).FirstOrDefault();
            if (nextMove!=null)
            {
                return (int)nextMove.Item2;
            }
            else
            {
                return (int)Move.Straight;
            }
        }

        private void PlanMyMoves(int[,] gameSurrond)
        {
            Direction myDirection;
            Position mypostition = new Position(1,1);
            if (_round == 1)
            {
                myDirection = _myFirstDirection;
                //TODO: zjistic co vrací Array.Length
                //_max = gameSurrond.Length;
                for (int x = 0; x < _max; x++)
                {
                    for (int y = 0; y < _max; y++)
                    {
                        if (gameSurrond[x,y] ==_myID)
                        {
                            mypostition = new Position(x,y);
                        }
                    }
                }
            }
            else
            {
                var lastMove = _myPlanedMoves.OrderByDescending(m => m.Item1).First();
                mypostition = lastMove.Item4;
                myDirection = lastMove.Item3;
            }
            int StraightScore = TryGetCollission(Move.Straight, mypostition, myDirection,gameSurrond);
            int LeftScore = TryGetCollission(Move.Left, mypostition, myDirection,gameSurrond);
            int RightScore = TryGetCollission(Move.Right, mypostition, myDirection,gameSurrond);
            int TurnRightScore90 = TryGetCollissionTurn(Move.Right, mypostition, myDirection, gameSurrond, 2);
            int TurnLeftScore90 = TryGetCollissionTurn(Move.Left, mypostition, myDirection, gameSurrond, 2);
            int TurnRightScore180 = TryGetCollissionTurn(Move.Right, mypostition, myDirection, gameSurrond, 4);
            int TurnLeftScore180 = TryGetCollissionTurn(Move.Left, mypostition, myDirection, gameSurrond, 4);
            
            int max = new[] { StraightScore, LeftScore, RightScore, TurnRightScore90, TurnLeftScore90, TurnRightScore180, TurnLeftScore180 }.Max();

            if (StraightScore == max)
            {
                for (int i = 0; i < 3; i++)
                {
                    myDirection = GetNewDirection(myDirection, Move.Straight);
                    mypostition.Update(myDirection);
                    _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round+i,Move.Straight,myDirection,mypostition.Copy()));
                } 
                return;
            }

            if (LeftScore == max)
            {
                for (int i = 0; i < 3; i++)
                {
                    myDirection = GetNewDirection(myDirection, Move.Left);
                    mypostition.Update(myDirection);
                    _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round+i, Move.Left, myDirection, mypostition.Copy()));
                }
                return;
            }

            if (RightScore == max)
            {
                for (int i = 0; i < 3; i++)
                {
                    myDirection = GetNewDirection(myDirection, Move.Right);
                    mypostition.Update(myDirection);
                    _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round+i, Move.Right, myDirection, mypostition.Copy()));
                }
                return;
            }

            if (TurnRightScore90 == max)
            {
                for (int i = 0; i < 2; i++)
                {
                    myDirection = GetNewDirection(myDirection, Move.Right);
                    mypostition.Update(myDirection);
                    _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round + i, Move.Right, myDirection, mypostition.Copy()));
                }

                myDirection = GetNewDirection(myDirection, Move.Straight);
                mypostition.Update(myDirection);
                _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round + 2, Move.Straight, myDirection, mypostition.Copy()));
                return;
            }

            if (TurnLeftScore90 == max)
            {
                for (int i = 0; i < 2; i++)
                {
                    myDirection = GetNewDirection(myDirection, Move.Left);
                    mypostition.Update(myDirection);
                    _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round + i, Move.Left, myDirection, mypostition.Copy()));
                }

                myDirection = GetNewDirection(myDirection, Move.Straight);
                mypostition.Update(myDirection);
                _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round + 2, Move.Straight, myDirection, mypostition.Copy()));
                return;
            }

            if (TurnLeftScore180 == max)
            {
                for (int i = 0; i < 4; i++)
                {
                    myDirection = GetNewDirection(myDirection, Move.Left);
                    mypostition.Update(myDirection);
                    _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round + i, Move.Left, myDirection, mypostition.Copy()));
                }

                myDirection = GetNewDirection(myDirection, Move.Straight);
                mypostition.Update(myDirection);
                _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round + 4, Move.Straight, myDirection, mypostition.Copy()));
                return;
            }

            if (TurnRightScore180 == max)
            {
                for (int i = 0; i < 4; i++)
                {
                    myDirection = GetNewDirection(myDirection, Move.Right);
                    mypostition.Update(myDirection);
                    _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round + i, Move.Right, myDirection, mypostition.Copy()));
                }

                myDirection = GetNewDirection(myDirection, Move.Straight);
                mypostition.Update(myDirection);
                _myPlanedMoves.Add(new Tuple<int, Move, Direction, Position>(_round + 4, Move.Straight, myDirection, mypostition.Copy()));
                return;
            }
        }

        private int TryGetCollission(Move move, Position mypostition, Direction myDirection, int[,] gameSurrond)
        {
            int score = 1;
            Direction simulateDirection = myDirection;
            Position simulatePosition = mypostition.Copy();
            simulateDirection = GetNewDirection(simulateDirection, move);
            simulatePosition.Update(simulateDirection);
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();


            while (!simulatePosition.IsInCollission(_max) && simulateGameSurround[simulatePosition.X, simulatePosition.Y] == 0 && !CrossColision(simulateDirection, simulateGameSurround,simulatePosition))
            {
                simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myID;
                simulateDirection = GetNewDirection(simulateDirection, move);
                simulatePosition.Update(simulateDirection);
                score++;
            }

            return score;
        }

        private int TryGetCollissionTurn(Move move, Position mypostition, Direction myDirection, int[,] gameSurrond, int movestToStraight)
        {
            int score = 1;
            Direction simulateDirection = myDirection;
            Position simulatePosition = mypostition.Copy();
            simulateDirection = GetNewDirection(simulateDirection, move);
            simulatePosition.Update(simulateDirection);
            int[,] simulateGameSurround = (int[,])gameSurrond.Clone();
            if (score == movestToStraight) move = Move.Straight;


            while (!simulatePosition.IsInCollission(_max) && simulateGameSurround[simulatePosition.X, simulatePosition.Y] == 0 && !CrossColision(simulateDirection, simulateGameSurround, simulatePosition))
            {
                simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myID;
                simulateDirection = GetNewDirection(simulateDirection, move);
                simulatePosition.Update(simulateDirection);
                score++;
                if (score == movestToStraight) move = Move.Straight;
            }

            return score;
        }

        public string MyName()
        {
            return "Jardík";
        }

        bool CrossColision(Direction direction, int[,] gameSurround, Position position)
        {
            if (position.X == 0 || position.Y == 0 || position.X == _max - 1 || position.Y == _max-1)
            {
                return false;
            }

            switch (direction)
            {
                case Direction.TopRight: return !((gameSurround[position.X, position.Y + 1] == 0) && (gameSurround[position.X - 1, position.Y] == 0));

                case Direction.BottomRight: return !((gameSurround[position.X, position.Y - 1] == 0) && (gameSurround[position.X - 1, position.Y] == 0));

                case Direction.BottomLeft: return !((gameSurround[position.X, position.Y - 1] == 0) && (gameSurround[position.X + 1, position.Y] == 0));

                case Direction.TopLeft: return !((gameSurround[position.X, position.Y + 1] == 0) && (gameSurround[position.X + 1, position.Y] == 0));
            }
            return false;
        }

        private Direction GetNewDirection(Direction oldDirection, Move move)
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

    enum Move
    {
        Left = 1,
        Straight = 2,
        Right = 3
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
}
