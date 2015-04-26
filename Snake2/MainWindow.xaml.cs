using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Snake2.game;

namespace Snake2
{
    public partial class MainWindow : Window
    {
        public const int PlaygroundSizeInDots = 25;
        public const int PlaygroundSizeInPixels = 500;
        public int DotSizeInPixels;

        private DispatcherTimer _timer = new DispatcherTimer();
        private GameEngine _gameEngine;

        public MainWindow()
        {
            InitializeComponent();

            _buttonRestart.Focus();

            DotSizeInPixels = PlaygroundSizeInPixels / PlaygroundSizeInDots;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _canvas.Width = PlaygroundSizeInPixels;
            _canvas.Height = PlaygroundSizeInPixels;

            _timer.Tick += UpdateGameSurround;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);

            Restart();
        }

        private void Restart()
        {
            _gameEngine = new GameEngine(PlaygroundSizeInDots);
            _timer.Start();
        }

        private void UpdateGameSurround(object sender, EventArgs e)
        {
            if (!_gameEngine.GameOver)
            {
                RenderArray(_gameEngine.Move());
            }
            else
            {
                _timer.Stop();
                if (MessageBox.Show(string.Format("{0}\r\nSpustit další deathmatch?", _gameEngine.ScoreMessage()), Title, MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    Restart();
                }
            }
        }

        private void RenderArray(int[,] array)
        {
            _canvas.Children.Clear();
            for (int x = 0; x < PlaygroundSizeInDots; x++)
            {
                for (int y = 0; y < PlaygroundSizeInDots; y++)
                {
                    if (array[x, y] != 0)
                    {
                        AddRectangle(x, y, _gameEngine.GetColorForIdentificator(array[x, y]));
                    }
                }
            }
        }

        private void AddRectangle(int x, int y, Color color)
        {
            var rect = new Ellipse
                           {
                               Stroke = new SolidColorBrush(Colors.White),
                               StrokeThickness = 0,
                               Fill = new SolidColorBrush(color),
                               Width = DotSizeInPixels,
                               Height = DotSizeInPixels
                           };

            Canvas.SetLeft(rect, x * DotSizeInPixels);
            Canvas.SetTop(rect, y * DotSizeInPixels);
            _canvas.Children.Add(rect);
        }

        private void buttonRestart_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }
    }
}
