using System;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Next
    {
        public Me? Left;
        public Me? Straight;
        public Me? Right;

        public override string ToString()
        {
            return string.Format("Left: {0}, Straight: {1}, Right: {2}", Left, Straight, Right);
        }
    }
}
