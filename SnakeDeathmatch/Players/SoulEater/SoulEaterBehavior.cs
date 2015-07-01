﻿using System;
using System.Collections.Generic;
﻿using System.Linq;
﻿using System.Security.Cryptography.X509Certificates;
﻿using System.Threading;
﻿using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater
{
    public class SoulEaterBehavior : IPlayerBehavior
    {
        #region fields and props

        private Direction _myInitialDirection;

        private int _myIdentificatior;

        private GameGround _currentGameGround;

        private IList<Move> _moveList = new List<Move> { Move.Left, Move.Straight, Move.Right };

        private bool _isFirstMove = true;

        private IList<PlayerInfo> _otherPlayers = new List<PlayerInfo>();

        private Mode _mode = Mode.FindWall;

        #endregion

        public void Init(int direction, int identificator)
        {
            _myInitialDirection = (Direction)direction;
            _myIdentificatior = identificator;
        }

        public int NextMove(int[,] gameSurrond)
        {
            if (_isFirstMove)
            {
                _currentGameGround = new GameGround(gameSurrond, _myIdentificatior, _myInitialDirection);

                _isFirstMove = false;
            }
            else
            {
                _currentGameGround.Update(gameSurrond);
            }

            Move nextMove = GetSafeMove(14);

            ProcessMove(nextMove);

            return (int)nextMove;
        }

        private void ProcessMove(Move nextMove)
        {
            //if (_mode == Mode.WallFinded)
            //    return;

            //if (nextMove == Move.Left)
            //{
            //    _mode = Mode.WallFinded;
            //    _moveList = new List<Move> { Move.Right, Move.Straight, Move.Left };
            //}
            //if (nextMove == Move.Right)
            //{
            //    _mode = Mode.WallFinded;
            //    _moveList = new List<Move> { Move.Left, Move.Straight, Move.Right };
            //}
        }

        public string MyName()
        {
            return "Soul Eater";
        }

        private void Shuffle<T>(IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        #region move

        private Move GetSafeMove(int numberOfMovesToTry)
        {
            var copyOfGround = _currentGameGround.MakeACopy();

            copyOfGround.Analyze();

            IDictionary<Move, decimal> movesDictionary = new Dictionary<Move, decimal>();
            foreach (Move move in _moveList)
            {
                var result = GetPointsForMove(copyOfGround, move, numberOfMovesToTry);
                movesDictionary.Add(move, result);

                if (result == numberOfMovesToTry)
                    break;

                copyOfGround.VersionDownTo(0);
            }

            var bestMove = movesDictionary.OrderByDescending(x => x.Value).First().Key;

            return bestMove;
        }

        private decimal GetPointsForMove(GameGround gameGround, Move move, int numberOfSafeMoves)
        {
            if (numberOfSafeMoves == 0)
                return 0;

            decimal pointsForThisMove = 0;

            var ourPlayer = gameGround.OurHeroicPlayer;

            Direction absoluteDirection = MySuperClass.GetAbsoluteDirection(ourPlayer.Direction.Value, move);

            Point nextPoint = MySuperClass.GetNextPoint(ourPlayer.CurrentPosition, absoluteDirection);

            if (gameGround.IsValidPoint(nextPoint) == false)
                return pointsForThisMove;
            else if (gameGround[nextPoint.X, nextPoint.Y] != 0 && gameGround[nextPoint.X, nextPoint.Y] < 100)
                return pointsForThisMove;
            else if (IsCrossColision(nextPoint, absoluteDirection))
                return pointsForThisMove;
            else if (gameGround[nextPoint.X, nextPoint.Y] == GameGround.PotentionalyCollisionWithPlayerId)
                pointsForThisMove = (decimal)0.6;
            else if (gameGround[nextPoint.X, nextPoint.Y] == GameGround.DangerId)
                pointsForThisMove = (decimal)0.7;
            else if (gameGround[nextPoint.X, nextPoint.Y] == GameGround.LightDangerId)
                pointsForThisMove = (decimal)0.8;
            else
                pointsForThisMove = 1;


            gameGround.VersionUp(nextPoint);
            var ourVersion = gameGround.CurrentVersion;

            IDictionary<Move, decimal> movesDictionary = new Dictionary<Move, decimal>();
            foreach (var nextMove in _moveList)
            {
                var result = GetPointsForMove(gameGround, nextMove, numberOfSafeMoves - 1);

                movesDictionary.Add(nextMove, result);

                if (result == numberOfSafeMoves - 1)
                    break;

                gameGround.VersionDownTo(ourVersion);
            }

            return (movesDictionary.OrderByDescending(x => x.Value).First().Value + pointsForThisMove);
        }

        private bool IsCrossColision(Point newPosition, Direction direction)
        {
            switch (direction)
            {
                case Direction.TopRight: return (_currentGameGround[newPosition.X, newPosition.Y + 1] != 0) && (_currentGameGround[newPosition.X - 1, newPosition.Y] != 0);
                case Direction.BottomRight: return (_currentGameGround[newPosition.X, newPosition.Y - 1] != 0) && (_currentGameGround[newPosition.X - 1, newPosition.Y] != 0);
                case Direction.BottomLeft: return (_currentGameGround[newPosition.X, newPosition.Y - 1] != 0) && (_currentGameGround[newPosition.X + 1, newPosition.Y] != 0);
                case Direction.TopLeft: return (_currentGameGround[newPosition.X, newPosition.Y + 1] != 0) && (_currentGameGround[newPosition.X + 1, newPosition.Y] != 0);
            }

            return false;

        }

        #endregion

        private enum Mode
        {
            FindWall = 0,
            WallFinded = 1
        }
    }
}

