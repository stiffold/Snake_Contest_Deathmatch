using SnakeDeathmatch.Debugger;
using System.Collections.Generic;

namespace SnakeDeathmatch.Players.Vazba.Debug
{
    public class VazbaBreakpointNames : IBreakpointNames
    {
        public const string Strategy2Initialized = "Vazba.Strategy2.Initialized";
        public const string Strategy2TrackChanged = "Vazba.Strategy2.TrackChanged";
        public const string Strategy3Initialized = "Vazba.Strategy3.Initialized";
        public const string Strategy4DeathPlaygroundsRecalculated = "Vazba.Strategy4.DeathPlaygroundsRecalculated";
        public const string Strategy4TrackChanged = "Vazba.Strategy4.TrackChanged";
        public const string Strategy4Enqueue = "Vazba.Strategy4.Enqueue";
        public const string Strategy4Enqueue1Top = "Vazba.Strategy4.Enqueue1Top";
        public const string Strategy4Enqueue2TopRight = "Vazba.Strategy4.Enqueue2TopRight";
        public const string Strategy4Enqueue3Right = "Vazba.Strategy4.Enqueue3Right";
        public const string Strategy4Enqueue4BottomRight = "Vazba.Strategy4.Enqueue4BottomRight";
        public const string Strategy4Enqueue5Bottom = "Vazba.Strategy4.Enqueue5Bottom";
        public const string Strategy4Enqueue6BottomLeft = "Vazba.Strategy4.Enqueue6BottomLeft";
        public const string Strategy4Enqueue7Left = "Vazba.Strategy4.Enqueue7Left";
        public const string Strategy4Enqueue8TopLeft = "Vazba.Strategy4.Enqueue8TopLeft";
       

        public IEnumerable<string> GetNames()
        {
            yield return Strategy2Initialized;
            yield return Strategy2TrackChanged;
            yield return Strategy3Initialized;
            yield return Strategy4DeathPlaygroundsRecalculated;
            yield return Strategy4TrackChanged;
            yield return Strategy4Enqueue;
            yield return Strategy4Enqueue1Top;
            yield return Strategy4Enqueue2TopRight;
            yield return Strategy4Enqueue3Right;
            yield return Strategy4Enqueue4BottomRight;
            yield return Strategy4Enqueue5Bottom;
            yield return Strategy4Enqueue6BottomLeft;
            yield return Strategy4Enqueue7Left;
            yield return Strategy4Enqueue8TopLeft;
        }
    }
}
