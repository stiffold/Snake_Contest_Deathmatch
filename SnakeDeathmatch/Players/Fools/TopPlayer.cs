using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Players.Fools
{
    using Direction = SnakeDeathmatch.Interface.Direction;
    using Move = SnakeDeathmatch.Interface.Move;

    public class TopPlayer : IPlayerBehavior
    {
        private FoolHelper _foolHelper;
        private Direction _direction;

        public void Init(int direction, int identificator)
        {
            _foolHelper = new FoolHelper();
            _direction = (Direction)direction;
        }

        public int NextMove(int[,] gameSurrond)
        {
            if (_direction == Direction.Top)
            {
                return (int)Move.Straight;
            }
            else
            {
                _direction = _foolHelper.GetNextDirection(_direction, Move.Left);
                return (int)Move.Left;
            }
        }

        public string MyName()
        {
            return "Top Fool";
        }
    }
}
