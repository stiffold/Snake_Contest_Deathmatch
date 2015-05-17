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

            files.Where(x => x.Contains(".csv")).OrderByDescending(x => x).Select(x => _listBoxFiles.Items.Add(x)).ToList();
        }

        public string OpenDialog()
        {
            _selectedFile = null;
            this.ShowDialog();
            return _selectedFile;
        }

        private void _buttonOpen_Click(object sender, EventArgs e)
        {
            if (_listBoxFiles.SelectedItem != null)
            {
                _selectedFile = _listBoxFiles.SelectedItem.ToString();
            }
            this.Hide();
        }

        private void _buttonCancel_Click(object sender, EventArgs e)
        {
            _selectedFile = null;
            this.Hide();
        }
    }
}
