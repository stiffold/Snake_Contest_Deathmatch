using System.Drawing;
using SnakeDeathmatch.Debugger;

namespace SnakeDeathmatch.Players.Vazba.Debug
{
    public class BlackAndWhiteIntArrayVisualizer : IntArrayVisualizerBase
    {
        protected override Color GetColorForValue(int value)
        {
            return (value == 0) ? Color.Black : Color.White;
        }
    }
}
