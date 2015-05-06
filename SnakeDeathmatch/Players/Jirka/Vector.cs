using SnakeDeathmatch.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jirka
{
    struct Vector
    {
        public int X;
        public int Y;

        public static Vector operator +(Vector one, Vector two)
        {
            return new Vector(one.X + two.X, one.Y + two.Y);
        }

        public static Vector operator +(Vector one, int number)
        {
            return new Vector(one.X + number, one.Y + number);
        }

        public static Vector operator +(Vector one, Direction direction)
        {
            return one + (Vector)direction;
        }

        public static Vector operator -(Vector one, Vector two)
        {
            return new Vector(one.X + two.X, one.Y + two.Y);
        }

        public static Vector operator -(Vector one, int number)
        {
            return new Vector(one.X + number, one.Y + number);
        }

        public static Vector operator -(Vector one, Direction direction)
        {
            return one - (Vector)direction;
        }

        public static explicit operator Vector(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return new Vector(0, -1);
                case Direction.TopRight:
                    return new Vector(1, -1);
                case Direction.Right:
                    return new Vector(1, 0);
                case Direction.BottomRight:
                    return new Vector(1, 1);
                case Direction.Bottom:
                    return new Vector(0, 1);
                case Direction.BottomLeft:
                    return new Vector(-1, 1);
                case Direction.Left:
                    return new Vector(-1, 0);
                case Direction.TopLeft:
                    return new Vector(-1, -1);
            }
            throw new Exception("cast Interface.Direction -> Jirka.Vector failed");
        }

        /// <summary>
        /// Tests if X factor is in supplied range. Counts including minimal value but excluding maximal value
        /// </summary>
        /// <param name="min">minimal value (including)</param>
        /// <param name="max">maximal value (excluding)</param>
        /// <returns></returns>
        public bool WithinRangeX(int min, int max)
        {
            return withinRange(min, max, X);
        }

        /// <summary>
        /// Tests if Y factor is in supplied range. Counts including minimal value but excluding maximal value
        /// </summary>
        /// <param name="min">minimal value (including)</param>
        /// <param name="max">maximal value (excluding)</param>
        /// <returns></returns>
        public bool WithinRangeY(int min, int max)
        {
            return withinRange(min, max, Y);
        }

        /// <summary>
        /// Tests if both factors are in supplied range. Counts including minimal value but excluding maximal value
        /// </summary>
        /// <param name="min">minimal value (including)</param>
        /// <param name="max">maximal value (excluding)</param>
        /// <returns></returns>
        public bool WithinRangeBoth(int min, int max)
        {
            return withinRange(min, max, X) && withinRange(min, max, Y);
        }

        private bool withinRange(int min, int max, int value)
        {
            return value >= min && value < max;
        }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

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

        public override string ToString()
        {
            return string.Format("[{0},{1}]",X,Y);
        }

    }
}
