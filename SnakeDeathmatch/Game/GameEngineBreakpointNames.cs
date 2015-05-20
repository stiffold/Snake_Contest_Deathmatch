using SnakeDeathmatch.Debugger;
using System.Collections.Generic;

namespace SnakeDeathmatch.Game
{
    public class GameEngineBreakpointNames : IBreakpointNames
    {
        public const string MoveBegin = "_GameEngine.MoveBegin";
        public const string MoveBegin_Running = "_GameEngine.MoveBegin (running)";
        public const string MoveEnd = "_GameEngine.MoveEnd";

        public IEnumerable<string> GetNames()
        {
            yield return MoveBegin;
            yield return MoveBegin_Running;
            yield return MoveEnd;
        }
    }
}
