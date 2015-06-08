using System.Drawing;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;

namespace SnakeDeathmatch.Players.Vazba.Debug
{
    public class DeathIntArrayVisualizer : IntArrayVisualizerBase
    {
        public const int InfinityId = 10000;

        protected override Color GetColorForValue(int value)
        {
            switch (value)
            {
                case 00: return Color.FromArgb(255, 000, 000);
                case 01: return Color.FromArgb(255, 192, 000);
                case 02: return Color.FromArgb(255, 255, 000);
                case 03: return Color.FromArgb(000, 255, 000);
                case 04: return Color.FromArgb(000, 255, 192);
                case 05: return Color.FromArgb(000, 192, 255);
                case 06: return Color.FromArgb(000, 000, 255);
                case 07: return Color.FromArgb(192, 000, 255);
                case 08: return Color.FromArgb(255, 000, 255);
                case 09: return Color.FromArgb(255, 000, 192);
                case 10: return Color.FromArgb(255, 000, 128);
                case InfinityId:
                    return Color.FromArgb(0, 0, 0);
            }
            return Color.White;
        }
    }
}
