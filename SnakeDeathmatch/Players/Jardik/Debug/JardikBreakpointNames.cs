using SnakeDeathmatch.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Players.Jardik.Debug
{
    class JardikBreakpointNames :  IBreakpointNames
    {
        public const string Repair = "Jardik.Repair";
        public const string Planner = "Jardik.Planner";
        public const string Collission = "Jardik.Collission";

        public IEnumerable<string> GetNames()
        {
            yield return Repair;
            yield return Planner;
            yield return Collission;
        }
    }
}
