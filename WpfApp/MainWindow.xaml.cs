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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameLibrary;
using System.Media;
using System.Configuration;
using System.Data.SqlClient;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Game game;
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }
        public MainWindow()
        {
            InitializeComponent();
            if (IsThereSavedGame())
            {
                ContinueWindow continueWindow = new ContinueWindow();
                continueWindow.ShowDialog();
            }
            else
            {
                NewGame newGameWindow = new();
                newGameWindow.ShowDialog();
            }
        }

        public MainWindow(Game game)
        {
            InitializeComponent();
            this.game = game;            
            CreateWindow();
            PutFigures();
        }

        /// <summary>
        /// checks is there saved game inn database
        /// </summary>
        /// <returns></returns>
        bool IsThereSavedGame()
        {
            string conString = ConfigurationManager.ConnectionStrings["ChessDB"].ConnectionString;
            using (var con = new SqlConnection(conString))
            {
                string commandText = "Select isAutoGame FROM Game;";
                using (var cmd = new SqlCommand(commandText, con))
                {
                    con.Open();
                    var reader=cmd.ExecuteReader();
                    return reader.HasRows;
                }
            }
        }

        /// <summary>
        /// Puts figures
        /// </summary>
        public void PutFigures()
        {
            char[,] board = game.GetBoard();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    char c = board[i, j];
                    if (c != '\u0020')
                    {
                        Image image = HelperMethods.CreateImage(c, i, j);
                        Grid1.Children.Add(image);
                    }
                }
            }
        }

        /// <summary>
        /// When pressing mouse left button on chessboard if start position is empty 
        /// then from mouse position sets start position, 
        /// otherwise from mouse position sets target position and tries move figure
        /// </summary>
        private void Grid1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(this);
            if (mousePos.X >= 80 && mousePos.X < 400 && mousePos.Y >= 64 && mousePos.Y < 384)
            {
                Position position = new(mousePos);
                string pos = position.ToString();
                if (pos != string.Empty)
                {
                    if (StartTextBox.Text == string.Empty)
                    {
                        if (game.IsRightChosen(pos))
                            StartTextBox.Text = pos;
                    }
                    else
                    {
                        if (StartTextBox.Text != pos)
                        {
                            TargetTextBox.Text = pos;
                            //MoveButton_Click(new object(), new RoutedEventArgs());
                            Move();
                        }
                    }
                }
            }
        }
        void Move()
        {
            if (game is AutoGame autogame)
            {
                if (autogame.WhoseMoves != autogame.autoPlayerColor.ToString())
                    game.Move(StartTextBox.Text, TargetTextBox.Text);
                else
                {
                    autogame.Think();                    
                }
            }
            else
            {
                game.Move(StartTextBox.Text, TargetTextBox.Text);
            }
            if (game.IsTimeToPromotionPawn)
            {
                if (game is AutoGame autogame1)
                {
                    if(autogame1.autoPlayerColor.ToString() != autogame1.WhoseMoves)
                        game.PawnPromotionTo("Queen");
                    else
                    {
                        PawnChangeToWindow window = new();
                        window.ShowDialog();
                    }
                }
                else
                {
                    PawnChangeToWindow window = new();
                    window.ShowDialog();
                }
            }
            CreateWindow();
            if (game.IsUnderCheck())
            {
                if (game.IsCheckmate())
                {
                    SystemSounds.Beep.Play();
                    GameOverWindow gameOverWindow = new();
                    string winner;
                    if (game.WhoseMoves == "Black")
                        winner = "White";
                    else
                        winner = "Black";
                    gameOverWindow.ResultLabel.Content = $"{winner} player won";
                    gameOverWindow.ShowDialog();
                    return;
                }
                else
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Check");
                }
            }
            else if (game.IsDraw())
            {
                SystemSounds.Beep.Play();
                GameOverWindow gameOverWindow = new();
                gameOverWindow.ResultLabel.Content = "Draw";
                gameOverWindow.Show();
            }
            if (game is AutoGame autoGame)
                if (autoGame.WhoseMoves == autoGame.autoPlayerColor.ToString())
                    Move();
        }
        /// <summary>
        /// moves figure if it is possible
        /// </summary>
        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            game.Move(StartTextBox.Text, TargetTextBox.Text);
            if (game.IsTimeToPromotionPawn)
            {
                if (game is AutoGame)
                {
                    game.PawnPromotionTo("Queen");
                }
                else
                {
                    PawnChangeToWindow window = new();
                    window.ShowDialog();
                }
            }
            CreateWindow();
            if (game.IsUnderCheck())
            {
                if (game.IsCheckmate())
                {
                    SystemSounds.Beep.Play();
                    GameOverWindow gameOverWindow = new();
                    string winner;
                    if (game.WhoseMoves == "Black")
                        winner = "White";
                    else
                        winner = "Black";
                    gameOverWindow.ResultLabel.Content = $"{winner} player won";
                    gameOverWindow.Show();
                }
                else
                {
                    SystemSounds.Beep.Play();
                    MessageBox.Show("Check");
                }
            }
            else if (game.IsDraw())
            {
                SystemSounds.Beep.Play();
                GameOverWindow gameOverWindow = new();
                gameOverWindow.ResultLabel.Content = "Draw";
                gameOverWindow.Show();
            }
        }

        /// <summary>
        /// recrates a Grid1
        /// </summary>
        public void CreateWindow()
        {
            StartTextBox.Text = String.Empty;
            TargetTextBox.Text = String.Empty;
            Grid1.Children.Clear();
            Grid1.Children.Add(Menu);
            Grid1.Children.Add(ChessboardImg);
            Grid1.Children.Add(StartLabel);
            Grid1.Children.Add(StartTextBox);
            Grid1.Children.Add(TargetLabel);
            Grid1.Children.Add(TargetTextBox);
            Grid1.Children.Add(MoveButton);
            WhoseMovesTextBlock.Text = game.WhoseMoves + "\'s turn";
            Grid1.Children.Add(WhoseMovesTextBlock);
            PutFigures();
        }

        /// <summary>
        /// starts a new game
        /// </summary>
        public void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame newGameWindow = new();
            newGameWindow.ShowDialog();
        }

        /// <summary>
        /// Saves game when closing main window, game saves if there are not finished and at least one step is played
        /// </summary>
        private void Chees_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (game is null)
                return;
            int i = 0;
            string s=string.Empty;
            if (game is AutoGame autoGame)
            {
                i = 1;
                s = autoGame.autoPlayerColor.ToString();
            }
            game.Save(i,s);
        }

        /// <summary>
        /// Closes all open windows when closed main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chees_Closed(object sender, EventArgs e)
        {
            foreach (Window item in Application.Current.Windows)
                item.Close();
        }
    }
}
