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
    /// This window for choosing pawn exchange to what when promotion time
    /// </summary>
    public partial class PawnChangeToWindow : Window
    {
        public PawnChangeToWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event for Queen butten, changes pawn to queen
        /// </summary>
        private void QueenButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = (MainWindow)App.Current.MainWindow;
            w.Game.PawnPromotionTo("Queen");
            Close();
        }

        /// <summary>
        /// Event for Rook butten, changes pawn to rook
        /// </summary>
        private void RookButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = (MainWindow)App.Current.MainWindow;
            w.Game.PawnPromotionTo("Rook");
            Close();
        }

        /// <summary>
        /// Event for Knight butten, changes pawn to knight
        /// </summary>
        private void KnightButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = (MainWindow)App.Current.MainWindow;
            w.Game.PawnPromotionTo("Knight");
            Close();
        }

        /// <summary>
        /// Event for Bishop butten, changes pawn to bishop
        /// </summary>
        private void BishopButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = (MainWindow)App.Current.MainWindow;
            w.Game.PawnPromotionTo("Bishop");
            Close();
        }
    }
}
