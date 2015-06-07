using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardik
{
    abstract class WalkSetBase
    {
        protected CollissionHelper _ch;
        protected int _score = 0;
        protected int _myId = 0;
        protected int _round = 0;
        protected Position _position;
        protected Direction _direction;
        protected Position _simulatePosition;
        protected Direction _simulateDirection;
        protected Move _nextMove;
        protected List<Other> _others;
        protected bool _isPro;
        protected int[,] _simulateGameSurround;
        protected int[,] _originalGameSurround;
        protected List<Walk> _walks = new List<Walk>();

        public int[,] SimulateGameSurround { get { return (int[,])_simulateGameSurround.Clone(); } }

        protected WalkSetBase(CollissionHelper ch, int myId, bool isPro = false)
        {
            _myId = myId;
            _ch = ch;
            _isPro = isPro;
        }

        public void Evaluate(int round, Position position, Direction direction, int[,] gameSurrond, List<Other> others )
        {
            _round = round;
            _score = 0;
            _others = others;
            _position = position;
            _direction = direction;
            _originalGameSurround = gameSurrond;

            _simulateDirection = direction;
            _simulatePosition = position.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            _walks.Clear();
            DoEvaluate(position, direction, gameSurrond);
            if (!_simulatePosition.IsInCollission(_ch.Max))
            {
                if (_simulateGameSurround[_simulatePosition.X, _simulatePosition.Y] == 0)
                {
                    _simulateGameSurround[_simulatePosition.X, _simulatePosition.Y] = _myId;
                }
            }
            if (CutCount != 0)
            {
                Cut(CutCount); 
            }
            
        }

        protected int TryGetCollissionTurn(Move move, Position _myPosition, Direction _myDirection, int[,] gameSurrond, int movestToStraight)
        {
            _score = 1;
            Direction simulateDirection = _myDirection;
            Position simulatePosition = _myPosition.Copy();
            _simulateGameSurround = (int[,])gameSurrond.Clone();

            simulateDirection = simulateDirection.GetNewDirection(move);
            simulatePosition.Update(simulateDirection);
            _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
            if (_score == movestToStraight) move = Move.Straight;

            while (!_ch.Collission(simulateDirection, _simulateGameSurround, simulatePosition))
            {
                _simulateGameSurround[simulatePosition.X, simulatePosition.Y] = _myId;
                simulateDirection = simulateDirection.GetNewDirection(move);
                simulatePosition.Update(simulateDirection);
                _score++;
                _round++;
                _walks.Add(new Walk(_round, move, simulateDirection, simulatePosition.Copy()));
                if (_score == movestToStraight) move = Move.Straight;
            }
            return _score;
        }

        protected int TryGetCollission(Move move, Position _myPosition, Direction _myDirection, int[,] gameSurrond)
        {
            _score = 1;
            Direction simulateDirection = _myDirection;
            Position simulatePosition = _myPosition.Copy();
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
            }

            return _score;
        }

        protected void Cut(int count)
        {
            if (count > _walks.Count)
            {
                _walks.Clear();
                return;
            }
            int maxRound = _walks.Max(x => x.Round);
            for (int i = 0; i < count; i++)
            {
                var w = _walks.FirstOrDefault(x => x.Round == maxRound - i);
                if (w != null && !w.Position.IsInCollission(_ch.Max))
                {
                    if (_originalGameSurround[w.Position.X, w.Position.Y] == 0)
                    {
                        _simulateGameSurround[w.Position.X, w.Position.Y] = 0;
                    }
                }
                _walks.Remove(w);
            }
        }

        protected void Update()
        {
            if (!_simulatePosition.IsInCollission(_ch.Max))
            {
                if (_simulateGameSurround[_simulatePosition.X, _simulatePosition.Y] == 0)
                {
                    _simulateGameSurround[_simulatePosition.X, _simulatePosition.Y] = _myId;
                }
            }
            _simulateDirection = _simulateDirection.GetNewDirection(_nextMove);
            _simulatePosition.Update(_simulateDirection);
            _walks.Add(new Walk(_round, _nextMove, _simulateDirection, _simulatePosition.Copy()));
            _round++;
            _score++;
            
        }

        protected abstract void DoEvaluate(Position position, Direction direction, int[,] gameSurrond);

        protected virtual int CutCount {
            get { return 0; }
        }

        public virtual WalkSetType Type {
            get { return WalkSetType.Straight;}
        }

        public int Score { get { return _score; } }

        public List<Walk> Walks {get { return _walks;}}
    }
}
