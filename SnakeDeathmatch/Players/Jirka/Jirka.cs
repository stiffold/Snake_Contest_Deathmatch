using System.Collections.Generic;
using SnakeDeathmatch.Interface;
using System;
using System.Diagnostics;

namespace SnakeDeathmatch.Players.Jirka
{
    public class Jirka : IPlayerBehaviour2
    {
        private int identifier;
        private int playgroundSize;
        private Direction direction;
        private Vector position;

        private int[,] localBoard;

        public Jirka()
        {
        }

        public void Init(int identifier, int playgroundSize, int x, int y, Direction direction)
        {
            this.identifier = identifier;
            this.playgroundSize = playgroundSize;
            this.position = new Vector(x, y);
            this.direction = direction;
            //Console.WriteLine("StartingPosition:" + position.ToString());
        }

        public Move GetNextMove(int[,] playground)
        {
            Vector nextPosition = position + direction;

            if (nextPosition.WithinRangeBoth(0, playgroundSize) && playground[nextPosition.X, nextPosition.Y] == 0)
            {
                Vector nextPosition2 = nextPosition + direction;
                if (nextPosition2.WithinRangeBoth(0, playgroundSize) && playground[nextPosition2.X, nextPosition2.Y] == 0)
                {
                    return moveTo(Move.Straight, nextPosition, (Vector)direction, direction);
                }
                nextPosition2 = nextPosition + direction.TurnLeft();
                if (nextPosition2.WithinRangeBoth(0, playgroundSize) && playground[nextPosition2.X, nextPosition2.Y] == 0)
                {
                    return moveTo(Move.Left, position + direction.TurnLeft(), (Vector)direction, direction);
                }
                return moveTo(Move.Right, position + direction.TurnRight(), (Vector)direction, direction);
            }
            nextPosition = position + direction.TurnLeft();
            if (nextPosition.WithinRangeBoth(0, playgroundSize) && playground[nextPosition.X, nextPosition.Y] == 0)
            {
                return moveTo(Move.Left, nextPosition, (Vector)direction, direction);
            }
            nextPosition = position + direction.TurnRight();
            return moveTo(Move.Right, nextPosition,(Vector)direction,direction);
        }

        private Move moveTo(Move move, Vector position, Vector directionV, Direction direction)
        {
            //Console.WriteLine("turning: " + move.ToString() + "to position: " + position + "[" + directionV + "|" + direction + "]");
            this.position = position;
            switch (move)
            {
                case Move.Left:
                    this.direction = this.direction.TurnLeft();
                    break;
                case Move.Right:
                    this.direction = this.direction.TurnRight();
                    break;
            }
            return move;
        }

        public string Name
        {
            get { return "Jirka"; }
        }
    }

}