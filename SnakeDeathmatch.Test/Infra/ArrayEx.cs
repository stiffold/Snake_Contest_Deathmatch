using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoulEaterTest.Infrastructure
{
    public static class ArrayEx
    {
        public static int[,] SwapXWithY(this int[,] array)
        {
            var lenght = array.GetLength(0);

            var newAray = new int[lenght,lenght];

            for ( int y = 0; y < lenght; y++)
            {
                for (int x = 0; x < lenght; x++)
                {
                    newAray[x, y] = array[y, x];
                }
            }

            return newAray;
        }
    }
}
