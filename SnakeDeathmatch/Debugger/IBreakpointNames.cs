using System.Collections.Generic;

namespace SnakeDeathmatch.Debugger
{
    public interface IBreakpointNames
    {
        IEnumerable<string> GetNames();
    }
}
