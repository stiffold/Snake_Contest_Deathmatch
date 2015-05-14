using System.Drawing;
using System.Windows.Forms;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Debugger;

namespace SnakeDeathmatch.Players.Vazba.Debug
{
    // TODO: Přejmenovat na IntPlaygroundVisualizer?
    public partial class IntPlaygroundVisualizer : UserControl, IVisualizer
    {
        public IntPlaygroundVisualizer()
        {
            InitializeComponent();
        }

        public void Update(object obj)
        {
            var intPlayground = (IntPlayground)obj;
            _pictureBox.Image = (obj == null) ? null : CreateBitmapFromPlayground(intPlayground);
        }

        private Bitmap CreateBitmapFromPlayground(IntPlayground playground)
        {
            var bitmap = new Bitmap(playground.Size * 4, playground.Size * 4);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawRectangle(Pens.Black, 0, 0, playground.Size * 4, playground.Size * 4);
            }
            for (int x = 0; x < playground.Size; x++)
            {
                for (int y = 0; y < playground.Size; y++)
                {
                    Color color = IntToColor(playground[x, y]);

                    bitmap.SetPixel(4 * x + 0, 4 * y + 0, color);
                    bitmap.SetPixel(4 * x + 1, 4 * y + 0, color);
                    bitmap.SetPixel(4 * x + 2, 4 * y + 0, color);
                    bitmap.SetPixel(4 * x + 3, 4 * y + 0, Color.Black);
                    bitmap.SetPixel(4 * x + 0, 4 * y + 1, color);
                    bitmap.SetPixel(4 * x + 1, 4 * y + 1, color);
                    bitmap.SetPixel(4 * x + 2, 4 * y + 1, color);
                    bitmap.SetPixel(4 * x + 3, 4 * y + 1, Color.Black);
                    bitmap.SetPixel(4 * x + 0, 4 * y + 2, color);
                    bitmap.SetPixel(4 * x + 1, 4 * y + 2, color);
                    bitmap.SetPixel(4 * x + 2, 4 * y + 2, color);
                    bitmap.SetPixel(4 * x + 3, 4 * y + 2, Color.Black);
                    bitmap.SetPixel(4 * x + 0, 4 * y + 3, Color.Black);
                    bitmap.SetPixel(4 * x + 1, 4 * y + 3, Color.Black);
                    bitmap.SetPixel(4 * x + 2, 4 * y + 3, Color.Black);
                    bitmap.SetPixel(4 * x + 3, 4 * y + 3, Color.Black);
                }
            }
            return bitmap;
        }

        private Color IntToColor(int value)
        {
            switch (value)
            {
                case 0: return Color.Black;
                case (int)PlayerId.Jardik: return Color.Red;
                case (int)PlayerId.Vazba: return Color.Blue;
                case (int)PlayerId.Setal: return Color.Aqua;
                case (int)PlayerId.SoulEater: return Color.White;
                case (int)PlayerId.Jirka: return Color.Yellow;
                case SnakeDeathmatch.Game.GameEngine.HeadToHeadCrashId: return Color.Magenta;
            }
            return Color.Magenta;
        }
    }
}
