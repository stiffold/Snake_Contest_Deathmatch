using System;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Vazba
{
    public class Next
    {
        public Snake? Left;
        public Snake? Straight;
        public Snake? Right;

        public override string ToString()
        {
            return string.Format("Left: {0}, Straight: {1}, Right: {2}", Left, Straight, Right);
        }
    }
}
