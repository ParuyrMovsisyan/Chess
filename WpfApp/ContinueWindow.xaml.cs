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

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            NewGame newGame = new NewGame();
            newGame.Show();
            this.Close();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            var main = (MainWindow)Application.Current.MainWindow;
            if (IsAutoGame(out int autoPlayer))
            {
                main.Game = new AutoGame(autoPlayer, true);
            }
            else
                main.Game = new Game(true);
            main.CreateWindow();
            main.PutFigures();
            this.Close();
        }
        bool IsAutoGame(out int autoPlayer)
        {
            string conString = ConfigurationManager.ConnectionStrings["ChessDB"].ConnectionString;
            using (var con = new SqlConnection(conString))
            {
                string commandText = "Select isAutoGame, autoPlayerColor FROM Game;";
                using (var cmd = new SqlCommand(commandText, con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    if (reader.GetString(1) == "Black")
                    {
                        autoPlayer = 1;
                    }
                    else
                        autoPlayer = 0;
                    return reader.GetBoolean(0);
                }
            }
        }
    }
}
