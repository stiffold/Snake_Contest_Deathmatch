using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Debugger;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.SoulEater.MK2.Debug
{
    public class MyFirstDebugVizualizer : IntArrayVisualizerBase
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

                case 200: return Color.Khaki;

            }
            return Color.Magenta;
        }
    }
}
