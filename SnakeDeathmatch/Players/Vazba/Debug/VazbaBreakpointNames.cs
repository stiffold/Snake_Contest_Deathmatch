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
        public const string Strategy4StopEvery100Steps = "Vazba.Strategy4.StopEvery100Steps";
        public const string Strategy5Initialized = "Vazba.Strategy5.Initialized";
        public const string Strategy5ExploreTrackRecursionStep = "Vazba.Strategy5.ExploreTrackRecursionStep";
        public const string Strategy5StopEvery10Steps = "Vazba.Strategy5.StopEvery10Steps";
       
        public IEnumerable<string> GetNames()
        {
            yield return Strategy2Initialized;
            yield return Strategy2TrackChanged;
            yield return Strategy3Initialized;
            yield return Strategy4DeathPlaygroundsRecalculated;
            yield return Strategy4TrackChanged;
            yield return Strategy4StopEvery100Steps;
            yield return Strategy5Initialized;
            yield return Strategy5ExploreTrackRecursionStep;
            yield return Strategy5StopEvery10Steps;
        }
    }
}
