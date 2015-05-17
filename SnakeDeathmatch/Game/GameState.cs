using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Game
{
    public class GameState
    {
        public int[,] GameSurround { get; set; }

        public int Round { get; set; }

        public List<RecordLine> RecordLines { get; set; }

    }
}
