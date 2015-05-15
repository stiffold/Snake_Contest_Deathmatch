using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SnakeDeathmatch.Game;

namespace SnakeGame.Game
{
    public class SavedGame
    {
        public int PlayGroundSizeInDots { get; set; }

        public List<RecordLine> RecordLines { get; set; }
    }
}
