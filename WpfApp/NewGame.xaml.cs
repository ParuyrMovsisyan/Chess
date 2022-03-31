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
    /// Interaction logic for NewGame.xaml. Window foer choosing new game options
    /// </summary>
    public partial class NewGame : Window
    {
        public NewGame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Starts a new game
        /// </summary>
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = (MainWindow)App.Current.MainWindow;
            if (GameType.Text == null || GameType.Text == string.Empty)
            {
                MessageBox.Show("Choose game type");
            }
            else if (GameType.Text == "2 Players")
            {
                w.Game = new Game();
                w.CreateWindow();
                w.PutFigures();
                Close();
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
                        w.Game = new AutoGame(1);                        
                    }
                    else
                    {
                        w.Game = new AutoGame(0);                        
                    }
                    w.CreateWindow();
                    w.PutFigures();
                    Close();
                } 
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

        /// <summary>
        /// event for Create custom game button, opens window for choosing custom game options
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CustomGameWindow game = new ();
            game.ShowDialog();
            Close();
        }
    }
}
