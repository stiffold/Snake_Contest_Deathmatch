using System.Drawing;
using System.Windows.Forms;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch.Debugger
{
    public abstract partial class IntArrayVisualizerBase : UserControl, IVisualizer
    {
        public IntArrayVisualizerBase()
        {
            InitializeComponent();
        }

        public void Update(object obj)
        {
            var intArray = (IIntArray)obj;
            _pictureBox.Image = (obj == null) ? null : CreateBitmapFromArray(intArray);
        }

        private Bitmap CreateBitmapFromArray(IIntArray intArray)
        {
            int size = intArray.InnerArray.GetUpperBound(0) + 1;

            var bitmap = new Bitmap(size * 4, size * 4);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawRectangle(Pens.Black, 0, 0, size * 4, size * 4);
            }
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    Color color = GetColorForValue(intArray.InnerArray[x, y]);

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

        protected abstract Color GetColorForValue(int value);
    }
}
