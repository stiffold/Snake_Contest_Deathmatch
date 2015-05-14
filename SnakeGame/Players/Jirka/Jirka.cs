using System.Collections.Generic;
using SnakeGame.Interface;
using System;

namespace SnakeGame.Players.Jirka
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
        }

        public Move GetNextMove(int[,] playground)
        {
            Vector nextPosition = position + direction;

            if (nextPosition.WithinRangeBoth(0, playgroundSize) && playground[nextPosition.X, nextPosition.Y] == 0)
            {
                Vector nextPosition2 = nextPosition + direction;
                if (nextPosition2.WithinRangeBoth(0, playgroundSize) && playground[nextPosition2.X, nextPosition2.Y] == 0)
                {
                    position = nextPosition;
                    return Move.Straight;
                }
                nextPosition2 = nextPosition + direction.TurnLeft();
                if (nextPosition2.WithinRangeBoth(0, playgroundSize) && playground[nextPosition2.X, nextPosition2.Y] == 0)
                {
                    position += direction.TurnLeft();
                    return Move.Left;
                }
                position += direction.TurnRight();
                return Move.Right;
            }
            nextPosition = position + direction.TurnLeft();
            if (nextPosition.WithinRangeBoth(0, playgroundSize) && playground[nextPosition.X, nextPosition.Y] == 0)
            {
                position = nextPosition;
                return Move.Left;
            }
            nextPosition = position + direction.TurnRight();
            position = nextPosition;
            return Move.Right;
        }

        public string Name
        {
            get { return "Jirka"; }
        }
    }

}