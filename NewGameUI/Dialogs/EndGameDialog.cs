using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewGameUI.Services.FTP;
using SnakeDeathmatch.Game;

namespace NewGameUI.Dialogs
{
    public partial class EndGameDialog : Form
    {

        private Game _game;
        private bool _restartGame;

        public EndGameDialog()
        {
            InitializeComponent();
        }


        private void EndGameDialog_Load(object sender, EventArgs e)
        {

        }

        public bool OpenDialog(Game game)
        {

            _game = game;
            lblGameStats.Text = game.GameStats;
            _restartGame = false;

            this.ShowDialog();

            return _restartGame;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var repository = new FTPFileRepository();

            repository.SaveGame(_game, txtFileName.Text);

            MessageBox.Show("Hra uložena", "Uložení hry", MessageBoxButtons.OK);

        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            _restartGame = true;
            this.Hide();
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            _restartGame = false;
            this.Hide();
        }


    }
}
