using SnakeDeathmatch.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardos.Debug
{
    public class DebugablePlayground : IIntArray
    {
            string _tostring = "";
            public int[,] InnerArray { get; private set; }

            public int Size { get; private set; }

            public DebugablePlayground(int[,] array, string tostring)
            {
                InnerArray = (int[,])array.Clone();
                Size = array.GetUpperBound(0) + 1;
                _tostring = tostring;
            }

            public int this[int x, int y]
            {
                get { return InnerArray[x, y]; }
                set
                {
                    InnerArray[x, y] = value;
                }
            }

            public DebugablePlayground Clone()
            {
                return new DebugablePlayground(InnerArray, _tostring);
            }

            public override string ToString()
            {
                return _tostring; ;
            }
        }
    
}
