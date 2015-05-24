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
        public const string Behind = "Jardik.Behind";
        public const string KillMode = "Jardik.KillMode";
        public const string VictimFinded = "Jardik.VictimFinded";

        public IEnumerable<string> GetNames()
        {
            yield return Repair;
            yield return Planner;
            yield return Collission;
            yield return Behind;
            yield return KillMode;
            yield return VictimFinded;
        }
    }
}
