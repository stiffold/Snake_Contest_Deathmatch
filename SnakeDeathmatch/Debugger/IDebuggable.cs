using System;

namespace SnakeDeathmatch.Debugger
{
    public delegate void BreakpointEventHandler(object sender, BreakpointEventArgs e);

    public class BreakpointEventArgs : EventArgs
    {
        public string BreakpointName { get; private set; }

        public BreakpointEventArgs(string breakpointName)
        {
            BreakpointName = breakpointName;
        }
    }

    public interface IDebuggable
    {
        event BreakpointEventHandler Breakpoint;
    }
}
