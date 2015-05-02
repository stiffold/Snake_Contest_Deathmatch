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

namespace SnakeDeathmatch.Views
{
    /// <summary>
    /// Interaction logic for EndGameDialog.xaml
    /// </summary>
    public partial class EndGameDialog : Window
    {
        public EndGameDialog()
        {
            InitializeComponent();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
