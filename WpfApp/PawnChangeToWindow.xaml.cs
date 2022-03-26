using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for PawnChangeTo.xaml
    /// This window foe promotion pawn
    /// </summary>
    public partial class PawnChangeToWindow : Window
    {
        public PawnChangeToWindow()
        {
            InitializeComponent();
        }

        private void QueenButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = (MainWindow)App.Current.MainWindow;
            w.Game.PawnPromotionTo("Queen");
            this.Close();
        }

        private void RookButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = (MainWindow)App.Current.MainWindow;
            w.Game.PawnPromotionTo("Rook");
            this.Close();
        }

        private void KnightButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = (MainWindow)App.Current.MainWindow;
            w.Game.PawnPromotionTo("Knight");
            this.Close();
        }

        private void BishopButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = (MainWindow)App.Current.MainWindow;
            w.Game.PawnPromotionTo("Bishop");
            this.Close();
        }
    }
}
