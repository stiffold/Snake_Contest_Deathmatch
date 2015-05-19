using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SnakeDeathmatch.Game
{
    public class Game
    {
        public int PlayGroundSizeInDots { get; set; }

        public Bitmap GamePicture { get; set; }

        public string GameStats { get; set; }

        public List<RecordLine> RecordLines { get; set; }
    }
}
