﻿using System.Drawing;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;

namespace SnakeDeathmatch.Players.Vazba.Debug
{
    public class PlayersIntArrayVisualizer : IntArrayVisualizerBase
    {
        public const int TrackId = 1000;
        public const int InfinityId = 10000;

        protected override Color GetColorForValue(int value)
        {
            switch (value)
            {
                case 0: return Color.Black;
                case (int)PlayerId.Jardik: return Color.DeepPink;
                case (int)PlayerId.Vazba: return Color.Blue;
                case (int)PlayerId.Setal: return Color.Aqua;
                case (int)PlayerId.SoulEater: return Color.White;
                //case (int)PlayerId.Jirka: return Color.Yellow;
                case (int)PlayerId.ClockworkMole: return Color.Lime;
                case TrackId: return Color.Yellow;
                case SnakeDeathmatch.Game.GameEngine.HeadToHeadCrashId: return Color.Magenta;
            }
            return Color.Magenta;
        }
    }
}
