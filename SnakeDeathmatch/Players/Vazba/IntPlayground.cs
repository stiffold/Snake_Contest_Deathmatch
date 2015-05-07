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
    [DebuggerVisualizer(typeof(IntPlaygroundVisualizer))]
    [Serializable]
    public class IntPlayground
    {
        private int[,] _array;

        public int Size { get; private set; }

        public IntPlayground(int[,] array)
        {
            _array = (int[,])array.Clone();
            Size = array.GetUpperBound(0) + 1;
        }

        public int this[int x, int y]
        { 
            get { return _array[x, y]; }
            set { _array[x, y] = value; }
        }

        public IntPlayground Clone()
        {
            return new IntPlayground(_array);
        }
    }

    public class IntPlaygroundVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            if (windowService == null) throw new ArgumentNullException("windowService");
            if (objectProvider == null) throw new ArgumentNullException("objectProvider");

            IntPlayground playground = (IntPlayground)objectProvider.GetObject();

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
                form.Text = "Int Playground Visualizer";

                ((ISupportInitialize)(pictureBox)).EndInit();
                form.ResumeLayout(false);

                pictureBox.Image = CreateBitmapFromPlayground(playground);
                pictureBox.MouseMove += (sender, e) =>
                {
                    int xx = e.X / 4;
                    int yy = e.Y / 4;
                    int id = (xx >= 0 && xx < playground.Size && yy >= 0 && yy < playground.Size) ? playground[xx, yy] : -999;
                    PlayerId player = ((PlayerId)id);
                    form.Text = string.Format("Int Playground Visualizer [{0},{1}] {2} ({3})", xx, yy, id, player);
                };
                windowService.ShowDialog(form);
            }
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

        public static void TestShowVisualizer(object objectToVisualize)
        {
            new VisualizerDevelopmentHost(objectToVisualize, typeof(IntPlaygroundVisualizer)).ShowVisualizer();
        }
    }
}
