﻿using System;
using System.Collections.Generic;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater
{

    public class SoulEaterBehavior : IPlayerBehavior
    {
        #region fields and props
        private int _sizeOfTable;

        private Interface.Direction _myCurrentDirection;

        private int _myIdentificatior;

        private Point _myCurrentPosition;
        private Point MyCurrentPosition
        {

            get { return _myCurrentPosition; }

            set { _myCurrentPosition = value; }

        }

        private int[,] _currentSurrond;

        private readonly IList<Interface.Move> _moveList = new List<Interface.Move> { Interface.Move.Straight, Interface.Move.Left, Interface.Move.Right };

        private bool _isFirstMove = true;

        private IList<OtherPlayerInfo> _otherPlayers = new List<OtherPlayerInfo>();
        private IList<OtherPlayerInfo> OtherPlayers
        {

            get { return _otherPlayers; }

        }

        #endregion

        public void Init(int direction, int identificator)
        {
            _myCurrentDirection = (Interface.Direction)direction;
            _myIdentificatior = identificator;
        }

        public int NextMove(int[,] gameSurrond)
        {
            _currentSurrond = gameSurrond;

            if (_isFirstMove)
            { 
                _sizeOfTable = gameSurrond.GetUpperBound(0) + 1;
                InitPositionsOfPlayers(gameSurrond);
                _isFirstMove = false;
            }
            //
            ValidateCurrentPosition();
            Interface.Move nextMove = GetSafeMove(20);
            RecalculateNextPositionAndDirection(nextMove);

            return (int)nextMove;
        }

        private void ValidateCurrentPosition()
        {
            if (_currentSurrond[MyCurrentPosition.X, MyCurrentPosition.Y] != _myIdentificatior)
            {
                throw new Exception();
            }
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

        private void InitPositionsOfPlayers(int[,] gameSurrond)
        {
            for (int x = 0; x < _sizeOfTable; x++)
            {
                for (int y = 0; y < _sizeOfTable; y++)
                {
                    if (gameSurrond[x, y] == _myIdentificatior)
                    {
                        MyCurrentPosition = new Point(x, y);
                        return;
                    }

                    //if (gameSurrond[x, y] != 0)
                    //{
                    //    CreateNewPlayer(x, y, gameSurrond[x, y]);
                    //}
                }
            }
        }

        private void CreateNewPlayer(int x, int y, int identificator)
        {
            _otherPlayers.Add(new OtherPlayerInfo(x, y, identificator));
        }

        #region move

        private void RecalculateNextPositionAndDirection(Interface.Move move)
        {
            _myCurrentDirection = GetAbsoluteDirection(_myCurrentDirection, move);
            MyCurrentPosition = GetNextPoint(MyCurrentPosition, _myCurrentDirection);
        }

        private Interface.Move GetSafeMove(int numberOfSafeMoves)
        {
            if (numberOfSafeMoves == 0)
                return Interface.Move.Straight;

            foreach (Interface.Move move in _moveList)
            {
                bool isMoveSafe = GetIfMoveIsSafe(MyCurrentPosition, _myCurrentDirection, move, numberOfSafeMoves);
                if (isMoveSafe)
                    return move;
            }

            return GetSafeMove(--numberOfSafeMoves);
        }

        private bool GetIfMoveIsSafe(Point currentPosition, Interface.Direction currentDirection, Interface.Move move, int numberOfSafeMoves)
        {
            if (numberOfSafeMoves == 0)
                return true;

            Interface.Direction absoluteDirection = GetAbsoluteDirection(currentDirection, move);

            Point nextPoint = GetNextPoint(currentPosition, absoluteDirection);

            if (nextPoint.X >= _sizeOfTable || nextPoint.X < 0 || nextPoint.Y >= _sizeOfTable || nextPoint.Y < 0)
                return false;

            if (_currentSurrond[nextPoint.X, nextPoint.Y] != 0)
                return false;

            if (IsCrossColision(nextPoint, absoluteDirection))
                return false;

            numberOfSafeMoves--;

            foreach (var nextMove in _moveList)
            {
                if (GetIfMoveIsSafe(nextPoint, absoluteDirection, nextMove, numberOfSafeMoves))
                    return true;
            }
            return false;
        }

        private bool IsCrossColision(Point newPosition, Interface.Direction direction)
        {
            switch (direction)
            {
                case Interface.Direction.TopRight: return (_currentSurrond[newPosition.X, newPosition.Y + 1] != 0) && (_currentSurrond[newPosition.X - 1, newPosition.Y] != 0);
                case Interface.Direction.BottomRight: return (_currentSurrond[newPosition.X, newPosition.Y - 1] != 0) && (_currentSurrond[newPosition.X - 1, newPosition.Y] != 0);
                case Interface.Direction.BottomLeft: return (_currentSurrond[newPosition.X, newPosition.Y - 1] != 0) && (_currentSurrond[newPosition.X + 1, newPosition.Y] != 0);
                case Interface.Direction.TopLeft: return (_currentSurrond[newPosition.X, newPosition.Y + 1] != 0) && (_currentSurrond[newPosition.X + 1, newPosition.Y] != 0);
            }

            return false;

        }

        #endregion

        private Point GetNextPoint(Point myPosition, Interface.Direction absoluteDirection)
        {
            switch (absoluteDirection)
            {
                case Interface.Direction.Top:
                    return new Point(myPosition.X, myPosition.Y - 1);
                case Interface.Direction.TopRight:
                    return new Point(myPosition.X + 1, myPosition.Y - 1);
                case Interface.Direction.Right:
                    return new Point(myPosition.X + 1, myPosition.Y);
                case Interface.Direction.BottomRight:
                    return new Point(myPosition.X + 1, myPosition.Y + 1);
                case Interface.Direction.Bottom:
                    return new Point(myPosition.X, myPosition.Y + 1);
                case Interface.Direction.BottomLeft:
                    return new Point(myPosition.X - 1, myPosition.Y + 1);
                case Interface.Direction.Left:
                    return new Point(myPosition.X - 1, myPosition.Y);
                case Interface.Direction.TopLeft:
                    return new Point(myPosition.X - 1, myPosition.Y - 1);
            }
            throw new InvalidProgramException("uuu");
        }

        private Interface.Direction GetAbsoluteDirection(Interface.Direction direction, Interface.Move move)
        {
            if (move == Interface.Move.Left)
            {
                if (direction == Interface.Direction.Top)
                    return Interface.Direction.TopLeft;
                return direction - 1;
            }

            if (move == Interface.Move.Right)
            {
                if (direction == Interface.Direction.TopLeft)
                    return Interface.Direction.Top;
                return direction + 1;
            }

            return direction;
        }
    }
}

