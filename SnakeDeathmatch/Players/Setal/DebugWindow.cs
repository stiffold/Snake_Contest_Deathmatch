using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SnakeDeathmatch.Players.Setal
{
    public partial class DebugWindow : Form
    {
        public DebugWindow(byte[,] field, int size)
        {
            InitializeComponent();
            _size = size;
            _field = field;
        }

        private int _size;
        private byte[,] _field;

        private void DebugWindow_Load(object sender, EventArgs e)
        {
            Graphics g = this.CreateGraphics();

            g.Clear(Color.Red);
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(100, 100, 200, 200);
            g.DrawRectangle(System.Drawing.Pens.Red, rectangle);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // Call the OnPaint method of the base class.
            base.OnPaint(pe);

            Graphics g = this.CreateGraphics();

            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    if (_field[i, j] == 0)
                    {
                        g.DrawString("0", new Font("Tahoma", 8), Brushes.White, new PointF(i * 10, j * 10));
                    }
                    else if (_field[i, j] == 1)
                    {
                        g.DrawString(_field[i, j].ToString(), new Font("Tahoma", 8), Brushes.Orange, new PointF(i * 10, j * 10));
                    }
                    else if (_field[i, j] == 2)
                    {
                        g.DrawString(_field[i, j].ToString(), new Font("Tahoma", 8), Brushes.Red, new PointF(i * 10, j * 10));
                    }
                }

            }
        }

        private void DebugWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                this.Close();
        }
    }
}
