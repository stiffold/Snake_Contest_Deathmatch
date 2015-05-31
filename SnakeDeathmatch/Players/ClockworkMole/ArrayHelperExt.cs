using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SnakeDeathmatch.Players.ClockworkMole
{
    public static class ArrayHelperExt
    {
        public static IEnumerable<ArrayItem<T>> Enumerate<T>(this T[,] array, int xFrom, int xTo, int yFrom, int yTo)
        {

            int xSize = array.GetUpperBound(0);
            int ySize = array.GetUpperBound(1);

            xFrom = xFrom >= 0 ? xFrom : 0;
            xTo = xTo <= xSize ? xTo : xSize;

            yFrom = yFrom >= 0 ? yFrom : 0;
            yTo = yTo <= ySize ? yTo : ySize;

            for (int x = xFrom; x <= xTo; x++)
            {
                for (int y = yFrom; y <= yTo; y++)
                {
                    yield return new ArrayItem<T>(array, x, y);
                }
            }
        }

        public static IEnumerable<ArrayItem<T>> Enumerate<T>(this T[,] array, Position position, int sizeArroundPosition)
        {

            var xFrom = position.X - sizeArroundPosition;
            var xTo = position.X + sizeArroundPosition;
            var yFrom = position.Y - sizeArroundPosition;
            var yTo = position.Y + sizeArroundPosition;

            return Enumerate(array, xFrom, xTo, yFrom, yTo);

        }

        public static IEnumerable<ArrayItem<T>> Enumerate<T>(this T[,] array)
        {
            return Enumerate(array, 0, array.GetUpperBound(0), 0, array.GetUpperBound(1));
        }





        public static IEnumerable<ArrayItem<T>> Apply<T>(this IEnumerable<ArrayItem<T>> arrayItems, Action<ArrayItem<T>> predicate)
        {
            foreach (ArrayItem<T> x in arrayItems)
            {
                predicate(x);
                yield return x;
            }
        }
    }

    public class ArrayItem<T>
    {

        private T[,] _array;

        public ArrayItem(T[,] array, int x, int y)
        {
            _array = array;
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public T Value
        {
            get { return _array[X, Y]; }

            set { _array[X, Y] = value; }
        }

        public int DistanceTo(int compareToX, int compareToY)
        {
            return Math.Max(Math.Abs(compareToX - X), Math.Abs(compareToY - Y));
        }
    }


}
