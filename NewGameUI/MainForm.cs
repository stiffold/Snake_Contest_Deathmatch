using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using NewGameUI.Dialogs;
using NewGameUI.Services.FTP;
using SnakeDeathmatch.Interface;
using SnakeDeathmatch.Game;
using SnakeDeathmatch.Debugger;

namespace NewGameUI
{
    public partial class MainForm : Form
    {
        private Random _random = new Random();

        //game settings
        public const int PlaygroundSizeInDots = 100;
        public const int GameSpeed = 1001;  // pokud více než 1000, tak maximum, co dá procesor (vypne se sleep ve vlaknu)
        public const int TestsSpeed = 25;

        //render settings
        public const int PlaygroundSizeInPixels = 700;
        public const int RenderTimerIntervalInMilliseconds = 30; // rychlost vykreslovani, cim nizsi, tim rychlejsi
        public const int ReplayTimerIntervalInMilliseconds = 1;

        // game state
        private Stopwatch _stopwatch = new Stopwatch();
        private GameEngine _gameEngine;
        private GameState _previousGameState;
        private Bitmap _arenaPicture;

        private bool _drawingReplaySavedGame = false;
        private int _drawingSavedGameRoundNumber;
        private Game _game;

        #region Load initial state from file

        private GameEngine CreateGameEngineWithInitialStateFromFile(string fileName)
        {
            var imageWithInitialState = new Bitmap(fileName);
            int size = imageWithInitialState.Width;
            int[,] arrayWithInitialState = new int[size, size];
            var heads = new Dictionary<PlayerId, Position>();
            var necks = new Dictionary<PlayerId, Position>();
            var tailColors = new Dictionary<PlayerId, Color>();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    var colorInfo = new ColorInfo(imageWithInitialState.GetPixel(x, y));
                    arrayWithInitialState[x, y] = colorInfo.Value;

                    if (colorInfo.IsHead && !heads.ContainsKey(colorInfo.PlayerId))
                    {
                        heads.Add(colorInfo.PlayerId, new Position(x, y));
                        tailColors.Add(colorInfo.PlayerId, colorInfo.TailColor);
                    }

                    if (colorInfo.IsNeck && !necks.ContainsKey(colorInfo.PlayerId))
                        necks.Add(colorInfo.PlayerId, new Position(x, y));
                }
            }

            if (heads.Count != necks.Count)
                throw new Exception("Snake head count differs from snake neck count.");

            var players = new List<Player>();
            foreach (PlayerId playerId in heads.Keys)
            {
                Direction direction = GetDirection(necks[playerId], heads[playerId]);
                IPlayerBehaviour2 playerBehaviour = GetPlayerBehaviour(playerId);
                players.Add(new Player(heads[playerId], direction, tailColors[playerId], GetPlayerBehaviour(playerId), (int)playerId, size));
            }

            return new GameEngine(arrayWithInitialState, Color.Magenta, players);
        }

        private Direction GetDirection(Position neck, Position head)
        {
            if (neck.X == head.X && neck.Y - 1 == head.Y) return Direction.Top;
            if (neck.X == head.X && neck.Y + 1 == head.Y) return Direction.Bottom;
            if (neck.X + 1 == head.X && neck.Y == head.Y) return Direction.Right;
            if (neck.X - 1 == head.X && neck.Y == head.Y) return Direction.Left;
            if (neck.X + 1 == head.X && neck.Y - 1 == head.Y) return Direction.TopRight;
            if (neck.X - 1 == head.X && neck.Y - 1 == head.Y) return Direction.TopLeft;
            if (neck.X + 1 == head.X && neck.Y + 1 == head.Y) return Direction.BottomRight;
            if (neck.X - 1 == head.X && neck.Y + 1 == head.Y) return Direction.BottomLeft;

            throw new Exception(string.Format("Neck position [{0},{1}] and head position [{2},{3}] are not next to each other.", neck.X, neck.Y, head.X, head.Y));
        }

        private IPlayerBehaviour2 GetPlayerBehaviour(PlayerId playerId)
        {
            if (playerId == PlayerId.Jardik) return new SnakeDeathmatch.Players.Jardik.Jardik();
            if (playerId == PlayerId.Vazba) return new SnakeDeathmatch.Players.Vazba.VazbaPlayer();
            if (playerId == PlayerId.Setal) return new SnakeDeathmatch.Players.Setal.Setal();
            if (playerId == PlayerId.SoulEater) return new PlayerBehaviour1Adapter(new SnakeDeathmatch.Players.SoulEater.SoulEaterBehavior());
            if (playerId == PlayerId.ClockworkMole) return new SnakeDeathmatch.Players.ClockworkMole.ClockWorkMolePlayer();

            throw new NotImplementedException(string.Format("Unknown playerId {0}.", playerId));
        }

        private class ColorInfo
        {
            public Color TailColor { get; private set; }
            public PlayerId PlayerId { get { return (PlayerId)Value; } }
            public int Value { get; private set; }
            public bool IsPlayer { get { return IsHead || IsNeck || IsTail; } }
            public bool IsHead { get; private set; }
            public bool IsNeck { get; private set; }
            public bool IsTail { get; private set; }

            public ColorInfo(Color color)
            {
                if (color == Color.Black)
                {
                    TailColor = color;
                    Value = 0;
                    IsHead = false;
                    IsNeck = false;
                    IsTail = false;
                }

                InitForTailColor(Color.Red, PlayerId.Jardik, color);
                InitForTailColor(Color.Blue, PlayerId.Vazba, color);
                InitForTailColor(Color.Aqua, PlayerId.Setal, color);
                InitForTailColor(Color.White, PlayerId.SoulEater, color);
                InitForTailColor(Color.Lime, PlayerId.ClockworkMole, color);
            }

            private void InitForTailColor(Color tailColor, PlayerId playerId, Color color)
            {
                if (color == GetTailColor(tailColor) || color == GetHeadColor(tailColor) || color == GetNeckColor(tailColor))
                {
                    TailColor = tailColor;
                    Value = (int)playerId;
                    IsHead = (color == GetHeadColor(tailColor));
                    IsNeck = (color == GetNeckColor(tailColor));
                    IsTail = (color == tailColor);
                }
            }

            private Color GetHeadColor(Color color)
            {
                return Color.FromArgb(Math.Max(0, (int)color.R - 1), Math.Max(0, (int)color.G - 1), Math.Max(0, (int)color.B - 1));
            }

            private Color GetNeckColor(Color color)
            {
                return Color.FromArgb(color.R / 2, color.G / 2, color.B / 2);
            }

            private Color GetTailColor(Color color)
            {
                return Color.FromArgb(color.R, color.G, color.B);
            }
        }

        #endregion

        #region GameInitialize

        private IEnumerable<Player> GetPlayers()
        {
            var players = new List<Player>();
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.DeepPink, new SnakeDeathmatch.Players.Jardik.Jardik(), (int)PlayerId.Jardik, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.Blue, new SnakeDeathmatch.Players.Vazba.VazbaPlayer(), (int)PlayerId.Vazba, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.Aqua, new SnakeDeathmatch.Players.Setal.Setal(), (int)PlayerId.Setal, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.White, new PlayerBehaviour1Adapter(new SnakeDeathmatch.Players.SoulEater.SoulEaterBehavior()), (int)PlayerId.SoulEater, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.Lime, new SnakeDeathmatch.Players.ClockworkMole.ClockWorkMolePlayer(), (int)PlayerId.ClockworkMole, PlaygroundSizeInDots));
            
            // Jirko, až updatuješ hada, tak se zas odkomentuj.
            //players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.Yellow, new SnakeDeathmatch.Players.Jirka.Jirka(), (int)PlayerId.Jirka, PlaygroundSizeInDots));

            //jen pro pokusy
            //players.Add(new Player(new Position(1, 5), Direction.TopRight, Color.Lime, new SnakeDeathmatch.Players.ClockworkMole.ClockWorkMolePlayer(), (int)PlayerId.ClockworkMole, PlaygroundSizeInDots));

            return players;
        }

        private Position GetRandomPosition()
        {
            return new Position(_random.Next(4, PlaygroundSizeInDots - 4), _random.Next(4, PlaygroundSizeInDots - 4));
        }

        private Direction GetRandomDirection()
        {
            return (Direction)(_random.Next(1, 8));
        }

        private void RestartGame()
        {
            if (_gameEngine != null)
                _gameEngine.StopGame();

            _gameEngine = new GameEngine(new int[PlaygroundSizeInDots, PlaygroundSizeInDots], Color.Magenta, GetPlayers());
            _gameEngine.StepMode = _checkboxStepping.Checked;
            _gameEngine.StartGame(GameSpeed);

            _previousGameState = null;
            _stopwatch.Start();

            InitializeGraphics();
            _drawingReplaySavedGame = false;
            _timerUI.Interval = RenderTimerIntervalInMilliseconds;
            _timerUI.Start();  //timer ma udalost timerUI_Tick, tam se vzdy provede prekresleni
        }

        private void ReplaySavedGame(string path)
        {
            var fileRepository = new FTPFileRepository();

            var savedGame = fileRepository.LoadSavedGame(path);

            if (savedGame != null)
            {
                InitializeGraphics();
                _game = savedGame;
                _drawingSavedGameRoundNumber = 1;
                _drawingReplaySavedGame = true;

                _timerUI.Interval = ReplayTimerIntervalInMilliseconds;
                _timerUI.Start();
            }
        }

        #endregion

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Width = PlaygroundSizeInPixels + 16;
            this.Height = PlaygroundSizeInPixels + 38 + _panelTop.Height;

            this.Top = (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2;
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;

            RestartGame();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_gameEngine != null)
                _gameEngine.StopGame();
        }

        private void _timerUI_Tick(object sender, EventArgs e)
        {
            _timerUI.Stop();

            if (_drawingReplaySavedGame)
            {
                DrawRoundToWindow();
            }
            else
            {
                DrawFullGameStateToWindow();

                if (_gameEngine.GameOver)
                {
                    DrawFullGameStateToWindow(); // aby se vykreslily poslední změny
                    HandleGameOver();
                    return; //aby se na konci nenastartoval timer!!!
                }

            }

            _timerUI.Start();
        }

        private void HandleGameOver()
        {
            GameState gameState = _gameEngine.GetGameState();

            var game = new Game()
            {
                GamePicture = _arenaPicture,
                PlayGroundSizeInDots = PlaygroundSizeInDots,
                RecordLines = gameState.RecordLines,
                GameStats = _gameEngine.ScoreMessage(),
                Players = _gameEngine.Players,
            };

            var endGameDialog = new EndGameDialog();
            if (endGameDialog.OpenDialog(game) == true)
            {
                RestartGame();
            }

        }

        private void _buttonRestart_Click(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void _buttonReplay_Click(object sender, EventArgs e)
        {
            var dialog = new OpenReplayDialog();
            var selectedFile = dialog.OpenDialog();

            if (string.IsNullOrEmpty(selectedFile) == false)
            {
                ReplaySavedGame(selectedFile);
            }
        }

        private void DrawFullGameStateToWindow()
        {
            GameState gameState = _gameEngine.GetGameState();
            _labelRoundCounter.Text = gameState.Round.ToString(CultureInfo.InvariantCulture);
            UpdateRPS(gameState);

            DrawGameStateToImage(gameState);

            //a tady se to vykreslí do panelu
            var controlGraphics = _panelArena.CreateGraphics();
            controlGraphics.DrawImageUnscaled(_arenaPicture, 0, 0);
        }

        private void DrawRoundToWindow()
        {
            int dotSize = PlaygroundSizeInPixels / _game.PlayGroundSizeInDots;

            _labelRoundCounter.Text = _drawingSavedGameRoundNumber.ToString(CultureInfo.InvariantCulture);

            var recordLines = _game.RecordLines.Where(x => x.Round == _drawingSavedGameRoundNumber).ToList();
            _drawingSavedGameRoundNumber++;

            if (recordLines.Any() == false)
                _timerUI.Stop();

            using (var graphics = Graphics.FromImage(_arenaPicture))
            {
                foreach (var recordLine in recordLines)
                {
                    if (dotSize > 1)
                    {
                        var rectangle = new Rectangle(recordLine.X * dotSize, recordLine.Y * dotSize, dotSize, dotSize);
                        graphics.FillEllipse(new SolidBrush((Color)recordLine.Color), rectangle);
                    }
                    else
                    {
                        //toto je tu proto, ze rectangle se  neumi vykreslit pokud je 1px velky
                        var bitmap = new Bitmap(1, 1);
                        bitmap.SetPixel(0, 0, (Color)recordLine.Color);
                        graphics.DrawImageUnscaled(bitmap, recordLine.X * dotSize, recordLine.Y * dotSize);
                    }
                }
            }

            //a tady se to vykreslí do panelu
            var controlGraphics = _panelArena.CreateGraphics();
            controlGraphics.DrawImageUnscaled(_arenaPicture, 0, 0);
        }

        private void InitializeGraphics()
        {
            _arenaPicture = new Bitmap(PlaygroundSizeInPixels, PlaygroundSizeInPixels);
            using (Graphics graphics = Graphics.FromImage(_arenaPicture))
            {
                graphics.Clear(Color.Black);
                graphics.Dispose();
            }
        }

        private void DrawGameStateToImage(GameState gameState)
        {
            int dotSize = PlaygroundSizeInPixels / PlaygroundSizeInDots;
            using (var graphics = Graphics.FromImage(_arenaPicture))
            {
                for (int x = 0; x <= gameState.GameSurround.GetUpperBound(0); x++)
                {
                    for (int y = 0; y <= gameState.GameSurround.GetUpperBound(1); y++)
                    {
                        if (gameState.GameSurround[x, y] != 0 && IsPointInArrayChanged(gameState, x, y))
                        {
                            var color = (Color)_gameEngine.GetColorForIdentificator(gameState.GameSurround[x, y]);
                            if (dotSize > 1)
                            {
                                var rectangle = new Rectangle(x * dotSize, y * dotSize, dotSize, dotSize);
                                graphics.FillEllipse(new SolidBrush(color), rectangle);
                            }
                            else
                            {
                                //toto je tu proto, ze rectangle se  neumi vykreslit pokud je 1px velky
                                var bitmap = new Bitmap(1, 1);
                                bitmap.SetPixel(0, 0, color);
                                graphics.DrawImageUnscaled(bitmap, x * dotSize, y * dotSize);
                            }
                        }
                    }
                }
            }

            _previousGameState = gameState;
        }

        private void UpdateRPS(GameState gameState)
        {
            //TODO TR: tohle chce přepracovat, je to jen nástřel
            int rounds = gameState.Round - (_previousGameState != null ? _previousGameState.Round : 0);
            _stopwatch.Stop();
            if (_stopwatch.ElapsedMilliseconds > 0)
            {
                double RPS = rounds / (_stopwatch.ElapsedMilliseconds / 1000d);
                _stopwatch.Restart();
                _labelRPS.Text = string.Format("{0:#.00} RPS", RPS);
            }
        }

        private bool IsPointInArrayChanged(GameState gameState, int x, int y)
        {
            if (_previousGameState == null)
                return true;

            if (gameState.GameSurround[x, y] != _previousGameState.GameSurround[x, y])
                return true;

            return false;
        }

        private void _checkboxStepping_CheckedChanged(object sender, EventArgs e)
        {
            _buttonStep.Enabled = _checkboxStepping.Checked;
            if (_gameEngine != null)
            {
                _gameEngine.StepMode = _checkboxStepping.Checked;
            }
        }

        private void _buttonStep_Click(object sender, EventArgs e)
        {
            if (_gameEngine != null)
            {
                _gameEngine.NextStepEnabled = true;
            }
        }

        private void _buttonDebugger_Click(object sender, EventArgs e)
        {
            var form = new DebuggerForm(_gameEngine);
            form.Show();
        }
    }
}
