using SnakeDeathmatch.Debugger;
using System.Collections.Generic;

namespace SnakeDeathmatch.Game
{
    public class GameEngineBreakpointNames : IBreakpointNames
    {
        public const string NoBreakpoint = "## No Breakpoint ##";
        public const string MoveBegin = "_GameEngine.MoveBegin";
        public const string MoveEnd = "_GameEngine.MoveEnd";

        public IEnumerable<string> GetNames()
        {
            yield return NoBreakpoint;
            yield return MoveBegin;
            yield return MoveEnd;
        }
    }
}
