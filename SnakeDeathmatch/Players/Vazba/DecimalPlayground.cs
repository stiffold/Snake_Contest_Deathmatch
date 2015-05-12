using Microsoft.VisualStudio.DebuggerVisualizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SnakeDeathmatch.Interface;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace SnakeDeathmatch.Players.Vazba
{
    [DebuggerVisualizer(typeof(DecimalPlaygroundVisualizer))]
    [Serializable]
    public class DecimalPlayground
    {
        private decimal[,] _array;

        public int Size { get; private set; }

        public DecimalPlayground(int size)
        {
            _array = new decimal[size, size];
            Size = size;
        }

        public decimal this[int x, int y]
        {
            get { return _array[x, y]; }
            set { _array[x, y] = value; }
        }
    }

    public class DecimalPlaygroundVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null) throw new ArgumentNullException("windowService");
            if (objectProvider == null) throw new ArgumentNullException("objectProvider");

            DecimalPlayground playground = (DecimalPlayground)objectProvider.GetObject();

            using (var form = new Form())
            {
                var pictureBox = new PictureBox();
                ((ISupportInitialize)pictureBox).BeginInit();
                form.SuspendLayout();

                pictureBox.Location = new Point(0, 0);
                pictureBox.Size = new Size(400, 400);
                pictureBox.TabIndex = 0;
                pictureBox.TabStop = false;

                form.ClientSize = pictureBox.Size;
                form.Controls.Add(pictureBox);
                form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                form.Text = "Decimal Playground Visualizer";

                ((ISupportInitialize)(pictureBox)).EndInit();
                form.ResumeLayout(false);

                pictureBox.Image = CreateBitmapFromPlayground(playground);

                pictureBox.MouseMove += (sender, e) =>
                {
                    int xx = e.X / 4;
                    int yy = e.Y / 4;
                    form.Text = string.Format("Decimal Playground Visualizer [{0},{1}] {2:0.0000000000}", xx, yy, (xx >= 0 && xx < playground.Size && yy >= 0 && yy < playground.Size) ? playground[xx, yy] : -1);
                };

                windowService.ShowDialog(form);
            }
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

        public static void TestShowVisualizer(object objectToVisualize)
        {
            new VisualizerDevelopmentHost(objectToVisualize, typeof(DecimalPlaygroundVisualizer)).ShowVisualizer();
        }
    }
}
