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
        protected bool _isPro;
        protected int[,] _simulateGameSurround;
        protected List<Walk> _walks = new List<Walk>();

        public int[,] SimulateGameSurround { get { return (int[,])_simulateGameSurround.Clone(); } }

        protected WalkSetBase(CollissionHelper ch, int myId, bool isPro = false)
        {
            _myId = myId;
            _ch = ch;
            _isPro = isPro;
        }

        public void Evaluate(int round, Position position, Direction direction, int[,] gameSurrond)
        {
            _round = round;
            _score = 0;
            _walks.Clear();
            DoEvaluate(position, direction, gameSurrond);
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
                var w = _walks.Where(x => x.Round == maxRound - i).FirstOrDefault();
                if (w != null && !w.Position.IsInCollission(_ch.Max))
                {
                    _simulateGameSurround[w.Position.X, w.Position.Y] = 0;
                }
                _walks.Remove(w);
            }
        }

        protected abstract void DoEvaluate(Position position, Direction direction, int[,] gameSurrond);

        public abstract WalkSetType Type();

        public int Score { get { return _score; } }

        public List<Walk> Walks {get { return _walks;}}
    }
}
