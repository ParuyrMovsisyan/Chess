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
using System.Configuration;
using System.Data.SqlClient;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for ContinueWindow.xaml
    /// </summary>
    public partial class ContinueWindow : Window
    {
        public ContinueWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// event for New Game button, opens window for choosing new game options
        /// </summary>
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            NewGame newGame = new ();
            newGame.Show();
            Close();
        }

        /// <summary>
        /// Event for Continue button. Continues saved game
        /// </summary>
        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = (MainWindow)Application.Current.MainWindow;
            if (IsAutoGame(out int autoPlayer))
            {
                main.Game = new AutoGame(autoPlayer, true);
            }
            else
            {
                main.Game = new Game(true);
            }
            main.CreateWindow();
            main.PutFigures();
            Close();
        }

        /// <summary>
        /// Cheks is saved game was playing with computer
        /// </summary>
        private static bool IsAutoGame(out int autoPlayer)
        {
            string conString = ConfigurationManager.ConnectionStrings["ChessDB"].ConnectionString;
            using (SqlConnection con = new (conString))
            {
                string commandText = "Select isAutoGame, autoPlayerColor FROM Game;";
                using (SqlCommand cmd = new (commandText, con))
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    if (reader.GetString(1) == "Black")
                    {
                        autoPlayer = 1;
                    }
                    else
                    {
                        autoPlayer = 0;
                    }
                    return reader.GetBoolean(0);
                }
            }
        }
    }
}
