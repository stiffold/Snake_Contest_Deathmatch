using System.Drawing;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;

namespace SnakeDeathmatch.Players.Jardos.Debug
{
    public class DebugVizualizer : IntArrayVisualizerBase
    {
        private Size _size = new Size(400,400);
        protected override Size DefaultSize
        {
            get { return _size; }
        }

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
                case 1001: return Color.DodgerBlue;
            }
            return Color.Aquamarine;


        }
    }
}
