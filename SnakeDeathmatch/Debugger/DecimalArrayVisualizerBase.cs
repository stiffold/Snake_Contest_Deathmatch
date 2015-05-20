using System.Drawing;
using System.Windows.Forms;

namespace SnakeDeathmatch.Debugger
{
    public abstract partial class DecimalArrayVisualizerBase : UserControl, IVisualizer
    {
        public DecimalArrayVisualizerBase()
        {
            InitializeComponent();
        }

        public void Update(object obj)
        {
            var decimalArray = (IDecimalArray)obj;
            _pictureBox.Image = (obj == null) ? null : CreateBitmapFromArray(decimalArray);
        }

        private Bitmap CreateBitmapFromArray(IDecimalArray decimalArray)
        {
            int size = decimalArray.InnerArray.GetUpperBound(0) + 1;

            var bitmap = new Bitmap(size * 4, size * 4);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawRectangle(Pens.Black, 0, 0, size * 4, size * 4);
            }
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Color color = GetColorForValue(decimalArray.InnerArray[x, y]);

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

        protected abstract Color GetColorForValue(decimal value);
    }
}
