using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using SnakeDeathmatch.Game;
using SnakeDeathmatch.Tests;
using SnakeDeathmatch.Interface;

namespace SnakeDeathmatch
{
    public partial class MainWindow : Window
    {
        public const int PlaygroundSizeInDots = 100;
        public const int PlaygroundSizeInPixels = 600;
        public const int Speed = 60;
        public const int TestsSpeed = 5;

        private DispatcherTimer _timer = new DispatcherTimer();
        private GameEngine _gameEngine;
        private bool _isShowingTests = false;
        private string _lastTestName;
        private Random _random = new Random();

        public MainWindow()
        {
            InitializeComponent();

            _buttonRestart.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _canvas.Width = PlaygroundSizeInPixels;
            _canvas.Height = PlaygroundSizeInPixels;

            _timer.Tick += UpdateGameSurround;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / Speed);

            Restart();
        }

        private Direction GetRandomDirection()
        {
            return (Direction)(_random.Next(1, 8));
        }

        private Position GetRandomPosition()
        {
            return new Position(_random.Next(4, PlaygroundSizeInDots - 4), _random.Next(4, PlaygroundSizeInDots - 4));
        }

        private IEnumerable<Player> GetPlayers()
        {
            int nextIdentifier = 1;

            var players = new List<Player>();
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Colors.Red, new SnakeDeathmatch.Players.Jardik(), nextIdentifier++));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Colors.Blue, new SnakeDeathmatch.Players.Vazba.VazbaPlayer(), nextIdentifier++));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Colors.Aqua, new SnakeDeathmatch.Players.Setal.Setal(), nextIdentifier++));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Colors.White, new SnakeDeathmatch.Players.SoulEater.SoulEaterBehavior(), nextIdentifier++));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Colors.Yellow, new SnakeDeathmatch.Players.Jirka.Jirka(), nextIdentifier++));
            return players;
        }

        private void Restart()
        {
            _timer.Stop();

            if (_isShowingTests)
            {
                _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / TestsSpeed);
                _gameEngine = GetGameEngineForNextTest();
                if (_gameEngine == null)
                {
                    _isShowingTests = false;
                    MessageBox.Show("This is the end... my friend.", Title);
                }
            }
            else
            {
                _gameEngine = new GameEngine(PlaygroundSizeInDots, GetPlayers());
                _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / Speed);
                _lastTestName = null;
            }

            _timer.Start();
        }

        private GameEngine GetGameEngineForNextTest()
        {
            _lastTestName = GetNextTestName();
            if (_lastTestName != null)
            {
                var testMethod = typeof(CollisionTests).GetMethods().Single(x => x.Name == _lastTestName);
                var obj = testMethod.Invoke(new CollisionTests(), null);
                return (GameEngine)obj;
            }
            return null;
        }

        private string GetNextTestName()
        {
            List<string> testNames = typeof(CollisionTests).GetMethods().Where(x => x.Name.StartsWith("Test")).Select(x => x.Name).OrderBy(x => x).ToList();

            if (_lastTestName == null)
                return testNames.FirstOrDefault();
            else
            {
                int index = testNames.IndexOf(_lastTestName);
                if (index > -1 && index < testNames.Count - 1)
                    return testNames[index + 1];
            }
            return null;
        }

        private void UpdateGameSurround(object sender, EventArgs e)
        {
            if (_gameEngine == null)
            {
                _canvas.Children.Clear();
                return;
            }

            if (!_gameEngine.GameOver)
            {
                RenderArray(_gameEngine.Move());
            }
            else
            {
                _timer.Stop();
                if (MessageBox.Show(string.Format(_isShowingTests ? "{0}\r\nSpustit další test?" : "{0}\r\nSpustit další deathmatch?", _gameEngine.ScoreMessage()),
                    Title, MessageBoxButton.YesNo, MessageBoxImage.None, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    Restart();
                }
            }
        }

        private void RenderArray(int[,] array)
        {
            _canvas.Children.Clear();

            int dotSizeInPixels = PlaygroundSizeInPixels / _gameEngine.Size;

            for (int x = 0; x <= array.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= array.GetUpperBound(1); y++)
                {
                    if (array[x, y] != 0)
                    {
                        AddRectangle(x, y, _gameEngine.GetColorForIdentificator(array[x, y]), dotSizeInPixels);
                    }
                }
            }
        }

        private void AddRectangle(int x, int y, Color color, int dotSizeInPixels)
        {
            var rect = new Ellipse
                           {
                               Stroke = new SolidColorBrush(Colors.White),
                               StrokeThickness = 0,
                               Fill = new SolidColorBrush(color),
                               Width = dotSizeInPixels,
                               Height = dotSizeInPixels
                           };

            Canvas.SetLeft(rect, x * dotSizeInPixels);
            Canvas.SetTop(rect, y * dotSizeInPixels);
            _canvas.Children.Add(rect);
        }

        private void _buttonRestart_Click(object sender, RoutedEventArgs e)
        {
            _isShowingTests = false;
            Restart();
        }

        private void _buttonTests_Click(object sender, RoutedEventArgs e)
        {
            _isShowingTests = true;
            Restart();
        }
    }
}
