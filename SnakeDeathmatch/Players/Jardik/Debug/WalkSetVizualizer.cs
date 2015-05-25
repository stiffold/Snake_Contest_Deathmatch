using System.Drawing;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;

namespace SnakeDeathmatch.Players.Jardik.Debug
{
    public class WalkSetVizualizer : IntArrayVisualizerBase
    {
        protected override Color GetColorForValue(int value)
        {
            switch (value)
            {
                case 0: return Color.Black;
                case (int)PlayerId.Jardik: return Color.DeepPink;
                case (int)PlayerId.Vazba: return Color.Blue;
                case (int)PlayerId.Setal: return Color.Aqua;
                case (int)PlayerId.SoulEater: return Color.White;
                case (int)PlayerId.Jirka: return Color.Yellow;
                case (int)PlayerId.ClockworkMole: return Color.Lime;
                case SnakeDeathmatch.Game.GameEngine.HeadToHeadCrashId: return Color.Magenta;
                case 100: return Color.LightCoral;
                case 101: return Color.LightGreen;
   
            }
            return Color.Magenta;
        }
    }
}
