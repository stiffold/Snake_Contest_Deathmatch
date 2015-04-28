using System.Collections.Generic;
using SnakeDeathmatch.Interface;

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
        }

        public Move GetNextMove(int[,] playground)
        {



            return Move.Straight;
        }

        public string Name
        {
            get { return "Jirka"; }
        }
    }


    internal struct Vector
    {
        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X;
        public int Y;

        public override bool Equals(object obj)
        {
            if (!(obj is Vector))
                return false;
            Vector other = (Vector)obj;
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }

}