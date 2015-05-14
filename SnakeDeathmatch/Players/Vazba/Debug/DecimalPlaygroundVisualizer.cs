using System.Drawing;
using System.Windows.Forms;
using SnakeDeathmatch.Debugger;

namespace SnakeDeathmatch.Players.Vazba.Debug
{
    public partial class DecimalPlaygroundVisualizer : UserControl, IVisualizer
    {
        public DecimalPlaygroundVisualizer()
        {
            InitializeComponent();
        }

        public void Update(object obj)
        {
            var decimalPlayground = (DecimalPlayground)obj;
            _pictureBox.Image = (obj == null) ? null : CreateBitmapFromPlayground(decimalPlayground);
        }

        private Bitmap CreateBitmapFromPlayground(DecimalPlayground playground)
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
                    Color color = DecimalToColor(playground[x, y]);

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

        private Color DecimalToColor(decimal value)
        {
            if (value > 1) value = 1;
            if (value < 0) value = 0;

            int intValue = (int)(value * 255);
            return Color.FromArgb(intValue, intValue, intValue);
        }
    }
}
