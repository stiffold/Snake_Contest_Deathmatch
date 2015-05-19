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
        public const int PlaygroundSizeInPixels = 600;
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

        #region GameInitialize

        private IEnumerable<Player> GetPlayers()
        {
            var players = new List<Player>();
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.Red, new SnakeDeathmatch.Players.Jardik.Jardik(), (int)PlayerId.Jardik, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.Blue, new SnakeDeathmatch.Players.Vazba.VazbaPlayer(), (int)PlayerId.Vazba, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.Aqua, new SnakeDeathmatch.Players.Setal.Setal(), (int)PlayerId.Setal, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.White, new SnakeDeathmatch.Players.SoulEater.SoulEaterBehavior(), (int)PlayerId.SoulEater, PlaygroundSizeInDots));
            players.Add(new Player(GetRandomPosition(), GetRandomDirection(), Color.Yellow, new SnakeDeathmatch.Players.Jirka.Jirka(), (int)PlayerId.Jirka, PlaygroundSizeInDots));

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

            _gameEngine = new GameEngine(PlaygroundSizeInDots, Color.Magenta, GetPlayers());
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
