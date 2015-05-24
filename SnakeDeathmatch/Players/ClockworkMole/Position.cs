using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.ClockworkMole
{
    public class Position
    {

        public int X { get; private set; }

        public int Y { get; private set; }

        public readonly Direction Direction;

        public Position(int x, int y, Direction direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        public Position()
        {
        }

        public Position(Position position)
        {
            X = position.X;
            Y = position.Y;
            Direction = position.Direction;
        }

        public bool CanMove(Move move)
        {
            return true;
        }

        public Position Move(Move move)
        {
            Direction newDirection;
            switch (move)
            {
                case Interface.Move.Right:
                    if (Direction != Direction.TopLeft)
                        newDirection = Direction + 1;
                    else
                        newDirection = Direction.Top;
                    break;
                case Interface.Move.Left:
                    if (Direction != Direction.Top)
                        newDirection = Direction - 1;
                    else
                    {
                        newDirection = Direction.TopLeft;
                    }
                    break;
                default:
                    newDirection = Direction;
                    break;
            }

            return Move(newDirection);

        }

        private Position Move(Direction direction)
        {
            int newX = X, newY = Y;

            switch (direction)
            {
                case Direction.Top:
                    newY--;
                    break;
                case Direction.TopRight:
                    newY--;
                    newX++;
                    break;
                case Direction.Right:
                    newX++;
                    break;
                case Direction.BottomRight:
                    newX++;
                    newY++;
                    break;
                case Direction.Bottom:
                    newY++;
                    break;
                case Direction.BottomLeft:
                    newY++;
                    newX--;
                    break;
                case Direction.Left:
                    newX--;
                    break;
                case Direction.TopLeft:
                    newX--;
                    newY--;
                    break;
            }

            return new Position(newX, newY, direction);

        }

        public void Update(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
