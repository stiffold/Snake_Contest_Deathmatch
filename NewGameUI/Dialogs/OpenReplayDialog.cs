using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NewGameUI.Services.FTP;

namespace NewGameUI.Dialogs
{
    public partial class OpenReplayDialog : Form
    {
        private string _selectedFile;

        public OpenReplayDialog()
        {
            InitializeComponent();
        }

        private void OpenReplayDialog_Load(object sender, EventArgs e)
        {
            var ftpService = new FTPFileRepository();
            var files = ftpService.ListFiles();

            files.Where(x => x.Contains(".csv")).OrderByDescending(x => x).Select(x => lstFiles.Items.Add(x)).ToList();
        }

        public string OpenDialog()
        {
            _selectedFile = null;
            this.ShowDialog();
            return _selectedFile;
        }

        private void btOpen_Click(object sender, EventArgs e)
        {
            if (lstFiles.SelectedItem != null)
            {
                _selectedFile = lstFiles.SelectedItem.ToString();
            }
            this.Hide();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            _selectedFile = null;
            this.Hide();
        }
    }
}
