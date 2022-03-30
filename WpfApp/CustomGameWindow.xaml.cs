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
    public partial class CustomGameWindow : Window
    {
        /// <summary>
        /// chessboard
        /// </summary>
        readonly char[,] board = new char[8, 8];
        public CustomGameWindow()
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
                MainWindow w = (MainWindow)App.Current.MainWindow;
                if (GameType.Text == "2 Players")
                {
                    w.Game = new(board, isStartingWhites);
                    w.CreateWindow();
                    w.PutFigures();
                    this.Close();
                }
                else
                {
                    if (Color.Text == null || Color.Text == string.Empty)
                    {
                        MessageBox.Show("Choose Your color");
                    }
                    else
                    {
                        if (Color.Text == "White")
                        {
                            w.Game = new AutoGame(1, board, isStartingWhites);
                        }
                        else
                        {
                            w.Game = new AutoGame(0, board, isStartingWhites);
                        }
                        w.CreateWindow();
                        w.PutFigures();
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// if selected Play with computer visibles combo box for chossing color
        /// </summary>
        private void GameType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GameType.Text == "2 Players")
            {
                Color.Visibility = Visibility.Visible;
                ColorLabel.Visibility = Visibility.Visible;
            }
            else if (GameType.Text == "Play with computer")
            {
                Color.Visibility = Visibility.Hidden;
                ColorLabel.Visibility = Visibility.Hidden;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MainWindow w = (MainWindow)Application.Current.MainWindow;
            if (w.Game is null)
                Application.Current.Shutdown();
        }
    }
}
