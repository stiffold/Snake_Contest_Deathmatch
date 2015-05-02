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
using SnakeDeathmatch.Views;
using System.IO;
using System.Net;

namespace SnakeDeathmatch
{
    public partial class MainWindow : Window
    {
        public int PlaygroundSizeInDots = 100;
        public const int PlaygroundSizeInPixels = 600;
        public const int Speed = 60;
        public const int TestsSpeed = 5;

        private DispatcherTimer _timer = new DispatcherTimer();
        private DispatcherTimer _replayTimer = new DispatcherTimer();
        private GameEngine _gameEngine;
        private bool _isShowingTests = false;
        private bool _isGameActive = false;

        private string _lastTestName;
        private Random _random = new Random();

        private int[,] _previousArray;
        private bool _shouldClearWindowAfterRestartGame;

        EndGameDialog _endDialog = new EndGameDialog();
        OpenReplayDialog _openDialog = new OpenReplayDialog();
        List<RecordLine> _records = new List<RecordLine>();
        int _round = 1;
        int _replayStep = 1;

        private string _ftpServerIP = "ftp.hostuju.cz/";
        private string _ftpUserName = "snake.hostuju.cz";
        private string _ftpPassword = "123snake123";

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

            _endDialog.Title = Title;
            _endDialog.btnYes.Focus();
            _endDialog.btnYes.Click += (s, n) => { Restart(); };
            _endDialog.btnSave.Click += (s, n) => { Save(); };
            _endDialog.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            _openDialog.btnOpen.Click += (s, n) => { Open(); };

            //Restart();
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
            var players = new List<Player>();
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Colors.Red, new SnakeDeathmatch.Players.Jardik.Jardik(), (int)PlayerId.Jardik, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Colors.Blue, new SnakeDeathmatch.Players.Vazba.VazbaPlayer(), (int)PlayerId.Vazba, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Colors.Aqua, new SnakeDeathmatch.Players.Setal.Setal(), (int)PlayerId.Setal, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Colors.White, new SnakeDeathmatch.Players.SoulEater.SoulEaterBehavior(), (int)PlayerId.SoulEater, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Colors.Yellow, new SnakeDeathmatch.Players.Jirka.Jirka(), (int)PlayerId.Jirka, PlaygroundSizeInDots));
            return players;
        }

        private void Save()
        {
            if (String.IsNullOrEmpty(_endDialog.txtFileName.Text)) 
            {
                MessageBox.Show("Název souboru musí být vyplněn!");
                return;
            }

            //generate csv
            string path = Environment.CurrentDirectory;
            string filename = String.Format("{0}\\{1:MMddyyyy}{2}.csv", path, DateTime.Now, _endDialog.txtFileName.Text);

            if (!File.Exists(filename))
            {
                File.Create(filename).Close();
            }

            using (System.IO.TextWriter writer = File.CreateText(filename))
            {
                writer.WriteLine(PlaygroundSizeInDots);
                foreach (var r in _records.OrderBy(x=>x.Round))
                {
                    writer.WriteLine(String.Format("{0};{1};{2};{3};{4}", r.Round, r.Name, r.Color, r.X, r.Y));
                }
            }
       
            //save to FTP


            FileInfo objFile = new FileInfo(filename);
            FtpWebRequest objFTPRequest;

            // Create FtpWebRequest object 
            objFTPRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + _ftpServerIP + "/" + objFile.Name));

            // Set Credintials
            objFTPRequest.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);

            // By default KeepAlive is true, where the control connection is 
            // not closed after a command is executed.
            objFTPRequest.KeepAlive = false;

            // Set the data transfer type.
            objFTPRequest.UseBinary = true;

            // Set content length
            objFTPRequest.ContentLength = objFile.Length;

            // Set request method
            objFTPRequest.Method = WebRequestMethods.Ftp.UploadFile;

            // Set buffer size
            int intBufferLength = 16 * 1024;
            byte[] objBuffer = new byte[intBufferLength];

            // Opens a file to read
            FileStream objFileStream = objFile.OpenRead();

            try
            {
                // Get Stream of the file
                Stream objStream = objFTPRequest.GetRequestStream();

                int len = 0;

                while ((len = objFileStream.Read(objBuffer, 0, intBufferLength)) != 0)
                {
                    // Write file Content 
                    objStream.Write(objBuffer, 0, len);

                }

                objStream.Close();
                objFileStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            MessageBox.Show(filename +" uploadován :-)");
            _endDialog.txtFileName.Text = null;
        }

        private void Restart()
        {
            _timer.Stop();
            _round = 1;
            _records.Clear();
            _isGameActive = true;

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
            _shouldClearWindowAfterRestartGame = true;
            _timer.Start();
        }

        private void Stop()
        {
            _timer.Stop();
            _round = 1;
            _records.Clear();
            _isGameActive = false;
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
                _round++;
            }
            else
            {
                _timer.Stop();
                _endDialog.lblResult.Content = _gameEngine.ScoreMessage();
                _endDialog.lblRestart.Content = _isShowingTests ? "Spustit další test?" : "Spustit další deathmatch?";
                _endDialog.ShowDialog();
                _isGameActive = false;
            }
        }

        private bool IsPointInArrayChanged(int[,] array, int x, int y)
        {

            if (_previousArray == null)
                return true;

            if (array[x, y] != _previousArray[x, y])
                return true;

            return false;
        }

        private void RenderArray(int[,] array)
        {
            if (_shouldClearWindowAfterRestartGame)
            {
                _canvas.Children.Clear();
                _previousArray = null;
                _shouldClearWindowAfterRestartGame = false;
            }

            int dotSizeInPixels = PlaygroundSizeInPixels / _gameEngine.Size;

            for (int x = 0; x <= array.GetUpperBound(0); x++)
            {
                for (int y = 0; y <= array.GetUpperBound(1); y++)
                {
                    if (array[x, y] != 0 && IsPointInArrayChanged(array, x, y))
                    {
                        PlayerId id = (PlayerId)(array[x, y]);
                        var color = _gameEngine.GetColorForIdentificator((int)id);
                        AddRectangle(x, y, color, dotSizeInPixels);
                        _records.Add(new RecordLine(_round, x,y,color,id.ToString()));
                    }
                }
            }
            _previousArray = (int[,])array.Clone();

        }

        private void RenderReplayArray(object sender, EventArgs e)
        {
            int dotSizeInPixels = PlaygroundSizeInPixels / PlaygroundSizeInDots;
            if (_records.Any(x => x.Round == _replayStep))
            {
                foreach (var s in _records.Where(x => x.Round == _replayStep))
                {
                    AddRectangle(s.X, s.Y, s.Color, dotSizeInPixels);
                }
                _replayStep++;
            }
            else
            {
                _replayTimer.Stop();

                var result = _records.Where(x=>x.Name != "-1").GroupBy(x => x.Name).OrderByDescending(s=>s.Max(l => l.Round)).Select(g => g.Key.ToString() +" "+ g.Max(y => y.Round).ToString());
                string message = "";
                foreach (var r in result)
                {
                    message += r + "\r\n";
                }
                MessageBox.Show(message);
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

        private void _buttonReplay_Click(object sender, RoutedEventArgs e)
        {
            _isShowingTests = false;
            Replay();
        }

        private void _buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            if (_isGameActive)
            {
                return;
            }
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://"+_ftpServerIP);
            request.Method = WebRequestMethods.Ftp.ListDirectory;

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            List<string> files = (reader.ReadToEnd()).Split(new string[] { "\r\n", "\n","\r" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                       
            reader.Close();
            response.Close();

            _openDialog.lstFiles.Items.Clear();

            foreach (var f in files.Where(x=>x.Contains(".csv")))
            {
                _openDialog.lstFiles.Items.Add(f);
            }
            _openDialog.Show();            
        }

        private void Replay()
        {
            if (!_isGameActive)
            {
                StartReplay();
            }             
        }

        private void Open()
        {
             Stop();
             LoadReplay();
             StartReplay();
        }

        private void LoadReplay()
        {
            string fileName = _openDialog.lstFiles.SelectedItem.ToString();
            List<string> records = new List<string>();
            WebClient request = new WebClient();
            string url = "ftp://"+_ftpServerIP + fileName;
            request.Credentials = new NetworkCredential(_ftpUserName, _ftpPassword);
            request.Proxy = null;

            try
            {
                byte[] newFileData = request.DownloadData(url);
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                records = (fileString).Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }            
            catch (WebException e)
            {
                // Do something such as log error, but this is based on OP's original code
                // so for now we do nothing.
            }

            if (records.Any())
            {
                int firstRow = Int32.Parse(records[0]);
                PlaygroundSizeInDots = firstRow;

                _records = records.Skip(1)
                           .Select(x => x.Split(';'))
                           .Select(n => new RecordLine(Int32.Parse(n[0]), Int32.Parse(n[3]), Int32.Parse(n[4]), (Color)ColorConverter.ConvertFromString(n[2]), n[1])).ToList();
            }

            Replay();

        }

        private void StartReplay()
        {
            _replayStep = 1;
            _canvas.Children.Clear();
            _replayTimer.Tick += RenderReplayArray;
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / Speed);
            _replayTimer.Start();
        }

        private void _buttonTests_Click(object sender, RoutedEventArgs e)
        {
            _isShowingTests = true;
            Restart();
        }
    }
}
