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
    /// Interaction logic for GameOverWindow.xaml
    /// this window for show game's result
    /// </summary>
    public partial class GameOverWindow : Window
    {
        public GameOverWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// starts new game
        /// </summary>
        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w=(MainWindow)App.Current.MainWindow;
            w.NewGame_Click(new object(),new RoutedEventArgs());
            Close();
        }

        /// <summary>
        /// Exits from game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
