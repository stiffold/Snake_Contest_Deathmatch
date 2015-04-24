using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Snake2.game;

namespace Snake2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Canvas _c = new Canvas();
        DispatcherTimer _timer = new DispatcherTimer();
        GameEngine _engine = new GameEngine(100);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _c.Width = 1000;
            _c.Height = 1000;
            _c.HorizontalAlignment = HorizontalAlignment.Left;
            _c.VerticalAlignment = VerticalAlignment.Top;
            _c.Background = Brushes.Black;
            Content = _c;

            _timer.Tick += UpdateGameSurround;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60); //update at 60 fps
            _timer.Start();
        }

        void UpdateGameSurround(object sender, EventArgs e)
        {
            if (!_engine.GameOver)
            {
                RenderArray(_engine.Move());
            }
            else
            {
                _timer.Stop();
                MessageBox.Show(_engine.ScoreMessage());
            }

        }

        void RenderArray(int[,] array)
        {
            _c.Children.Clear();
            for (int x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    if (array[x, y] != 0)
                    {
                        AddRectangle(x, y, _engine.GetColorForIdentificator(array[x, y]));
                    }
                }
            }
        }

        void AddRectangle(int x, int y, Color color)
        {
            var rect = new Ellipse
                           {
                               Stroke = new SolidColorBrush(Colors.White),
                               StrokeThickness = 2,
                               Fill = new SolidColorBrush(color),
                               Width = 10,
                               Height = 10
                           };

            Canvas.SetLeft(rect, x * 10);
            Canvas.SetTop(rect, y * 10);
            _c.Children.Add(rect);
        }
    }
}
