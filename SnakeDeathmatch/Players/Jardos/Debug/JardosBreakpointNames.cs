using SnakeDeathmatch.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardos.Debug
{
    class JardosBreakpointNames :  IBreakpointNames
    {
        public const string One = "One";
        public const string Two = "Two";


        public IEnumerable<string> GetNames()
        {
            yield return One;
            yield return Two;
        }
    }
}
