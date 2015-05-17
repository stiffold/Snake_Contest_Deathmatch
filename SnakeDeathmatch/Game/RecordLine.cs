using System;

namespace SnakeDeathmatch.Game
{
    public class RecordLine
    {
        int _round, _x, _y;
        Object _color;
        string _name;

        public RecordLine(int round, int x, int y, Object color, string name)
        {
            _round = round;
            _x = x;
            _y = y;
            _color = color;
            _name = name;
        }

        public int Round { get { return _round; } }

        public int X { get { return _x; } }

        public int Y { get { return _y; } }

        public Object Color { get { return _color; } }

        public string Name { get { return _name; } }
    }
}
