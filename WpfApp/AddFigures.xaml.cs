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
using GameLibrary;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for AddFigures.xaml
    /// </summary>
    public partial class AddFigures : Window
    {
        /// <summary>
        /// chessboard
        /// </summary>
        readonly char[,] board = new char[8, 8];
        public AddFigures()
        {
            InitializeComponent();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    board[i, j] = '\u0020';
        }
        
        /// <summary>
        /// Gets position and writes it in PositionTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            if (mousePos.X >= 80 && mousePos.X < 400 && mousePos.Y >= 64 && mousePos.Y < 384)
            {
                Position position = new (mousePos);
                PositionTextBox.Text = position.ToString();
            }
        }

        /// <summary>
        /// Adds figure to board
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Position pos = new (PositionTextBox.Text);
            if (board[pos.X, pos.Y] == '\u0020')
            {                
                char symbol = HelperMethods.GetFigureSymbol(FigureComboBox.Text);
                board[pos.X, pos.Y] = symbol;
                Image image = HelperMethods.CreateImage(symbol, pos.X, pos.Y);
                MainGrid.Children.Add(image);
            }
            else
                MessageBox.Show("There are another figure");
        }

        /// <summary>
        /// Start's a new game if it is a possible, otherwise shows mistak's meesage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isStartingWhites = WhoseTurnComboBox.Text == "White's";
                Game game = new (board,isStartingWhites);
                MainWindow w = (MainWindow)App.Current.MainWindow;
                w.Game = game;
                w.CreateWindow();
                w.PutFigures();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
