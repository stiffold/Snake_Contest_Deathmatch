using System.Drawing;
using SnakeDeathmatch.Debugger;

namespace SnakeDeathmatch.Players.Vazba.Debug
{
    public class ZeroToOneDecimalArrayVisualizer : DecimalArrayVisualizerBase
    {
        protected override Color GetColorForValue(decimal value)
        {
            if (value > 1) value = 1;
            if (value < 0) value = 0;

            int intValue = (int)(value * 255);
            return Color.FromArgb(intValue, intValue, intValue);
        }
    }
}
