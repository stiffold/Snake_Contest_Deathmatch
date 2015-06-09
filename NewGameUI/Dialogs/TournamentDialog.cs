using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SnakeDeathmatch.Game;

namespace NewGameUI.Dialogs
{
    public partial class TournamentDialog : Form
    {
        //špagetááááááá :-D
        private Func<GameEngine> _createGameDelegate;

        private Queue<GameEngine> _gamesReadyToRun;
        private IList<GameEngine> _currentlyRunning = new List<GameEngine>();
        private List<GameEngine> _finishedGames = new List<GameEngine>();
        private Stopwatch _stopwatch;

        public TournamentDialog()
        {
            InitializeComponent();
        }

        public void OpenDialog(Func<GameEngine> createGameDelegate)
        {

            // jeden konec špagety
            _createGameDelegate = createGameDelegate;

            this.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_currentlyRunning.Count == 0 && _gamesReadyToRun.Count == 0)
            {
                timer1.Stop();
                _stopwatch.Stop();
            }

            lblFinished.Text = _finishedGames.Count.ToString();
            lblToRun.Text = _gamesReadyToRun.Count.ToString();
            lblRunning.Text = _currentlyRunning.Count().ToString();
            lblTime.Text = _stopwatch.Elapsed.ToString();

            RefreshGameStats();

            CheckAndStartNewGame();

        }

        private void CheckAndStartNewGame()
        {

            //každý refresh okna se nastartuje jedna hra
            if (_currentlyRunning.Count < upDownThreads.Value && _gamesReadyToRun.Count > 0)
            {
                var gameEngine = _gamesReadyToRun.Dequeue();
                _currentlyRunning.Add(gameEngine);
                gameEngine.StartGame(1001);
            }


        }

        private void RefreshGameStats()
        {

            IEnumerable<GameEngine> finishedGames = _currentlyRunning.Where(x => x.GameOver).ToList();

            if (finishedGames.Any())
            {
                var listItems = listPlayers.Items.Cast<PlayerItem>().ToList();

                foreach (var finishedGame in finishedGames)
                {
                    int rankPoints = finishedGame.Players.Count();
                    foreach (var group in finishedGame.Players.GroupBy(x => x.Score).OrderByDescending(x => x.Key))
                    {
                        var scoreForEachPlayerInGroup = CalculateScoreForPlayers(rankPoints, group.Count());

                        foreach (var player in group)
                        {
                            var item = listItems.Single(x => x.Id == player.Identifier);
                            item.UpdateTotalScore(scoreForEachPlayerInGroup);
                        }
                        rankPoints -= group.Count();

                    }


                    _currentlyRunning.Remove(finishedGame);
                    _finishedGames.Add(finishedGame);

                }

                //TODO TR: nejak jsem neprisel jak setridit listview. No celkove je to nejake divne s tim podedenym Itemem, predelat
                listPlayers.Items.Clear();
                listItems.OrderByDescending(x => x.Score).Select(x => listPlayers.Items.Add(x)).ToList();
            }

        }

        private decimal CalculateScoreForPlayers(int startingPoints, int playerCount)
        {
            int sum = 0;
            for (int i = 0; i < playerCount; i++)
            {
                sum += startingPoints;
                startingPoints--;
            }
            return ((decimal)sum) / (decimal)playerCount;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            StopAllGames();
            this.Close();
        }


        private void StopAllGames()
        {
            foreach (var gameEngine in _currentlyRunning)
            {
                gameEngine.StopGame();
                if (_stopwatch != null)
                    _stopwatch.Stop();
            }
        }

        private void btStartGames_Click(object sender, EventArgs e)
        {

            lblGameTotalCount.Text = upDownGamesCount.Value.ToString();
            _stopwatch = new Stopwatch();
            _stopwatch.Start();

            _gamesReadyToRun = new Queue<GameEngine>();

            for (int i = 0; i < upDownGamesCount.Value; i++)
            {
                var game = _createGameDelegate.Invoke();
                _gamesReadyToRun.Enqueue(game);
            }

            InitializeList(_gamesReadyToRun.First().Players);

            timer1.Interval = 300;
            timer1.Start();
        }

        private void InitializeList(IEnumerable<Player> players)
        {
            var imagelist = new ImageList();
            listPlayers.SmallImageList = imagelist;

            listPlayers.Items.Clear();

            foreach (var player in players)
            {

                var playerIcon = new Bitmap(16, 16);
                using (Graphics graphics = Graphics.FromImage(playerIcon))
                {
                    graphics.Clear(listPlayers.BackColor);
                    var rectangle = new Rectangle(2, 2, 12, 12);
                    graphics.FillEllipse(new SolidBrush((Color)player.Color), rectangle);
                }
                imagelist.Images.Add(player.Identifier.ToString(CultureInfo.InvariantCulture), playerIcon);

                var item = new PlayerItem(player.Identifier, player.Name);
                item.ImageKey = player.Identifier.ToString(CultureInfo.InvariantCulture);

                listPlayers.Items.Add(item);
            }

        }

        public class PlayerItem : ListViewItem
        {

            private ListViewSubItem _scoreSubItem;
            private ListViewSubItem _nameSubItem;


            public PlayerItem(int id, string name)
            {
                Id = id;
                _scoreSubItem = SubItems.Add("0");
                _scoreSubItem.Tag = 0m;
                _nameSubItem = SubItems.Add(name);
            }

            public int Id { get; private set; }

            public void UpdateTotalScore(decimal score)
            {
                _scoreSubItem.Tag = (decimal)_scoreSubItem.Tag + score;
                _scoreSubItem.Text = _scoreSubItem.Tag.ToString();
            }

            public decimal Score
            {
                get { return (decimal)_scoreSubItem.Tag; }
            }

        }

        private void TournamentDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
            StopAllGames();
        }

    }
}
