using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OldGameUI.Views
{
    /// <summary>
    /// Interaction logic for OpenReplay.xaml
    /// </summary>
    public partial class OpenReplayDialog : Window
    {
        public OpenReplayDialog()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
