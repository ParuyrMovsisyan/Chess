using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace ChessLibrary
{
    /// <summary>
    /// Saves or gets saved game
    /// </summary>
    public class ChessDB
    {
        /// <summary>
        /// Clears database
        /// </summary>
        static void DeleteSavedGame()
        {
            string conString = ConfigurationManager.ConnectionStrings["ChessDB"].ConnectionString;
            using (var con = new SqlConnection(conString))
            {
                SqlCommand cmd = new SqlCommand("TRUNCATE TABLE BoardHistory; " +
                                                "TRUNCATE TABLE Figures; " +
                                                "TRUNCATE TABLE FiguresMovesHistory; " +
                                                "TRUNCATE TABLE MovesHistory; " +
                                                "TRUNCATE TABLE GAME", con);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Saves game
        /// </summary>
        /// <param name="chessboard"></param>
        public static void Save(Chessboard chessboard, int isAutoGame, string autoPlayerColor)
        {
            DeleteSavedGame();
            string conString = ConfigurationManager.ConnectionStrings["ChessDB"].ConnectionString;
            using (var con = new SqlConnection(conString))
            {
                con.Open();
                string commandText = "INSERT INTO GAME(isAutoGame,autoPlayerColor) VALUES(@isAutoGame,@autoPlayerColor);";
                using (var cmd = new SqlCommand(commandText, con))
                {
                    var paramIsAutoGame = new SqlParameter("@isAutoGame", isAutoGame);
                    var paramAutoPlayerColor = new SqlParameter("@autoPlayerColor", autoPlayerColor);
                    cmd.Parameters.Add(paramIsAutoGame);
                    cmd.Parameters.Add(paramAutoPlayerColor);
                    cmd.ExecuteNonQuery();
                }
                commandText = "INSERT INTO BoardHistory(board,whoseTurn) VALUES(@board,@whoseTurn);";
                using (var cmd = new SqlCommand(commandText, con))
                {
                    var paramBoard = new SqlParameter("@board", SqlDbType.NChar);
                    var paramWhoseTurn = new SqlParameter("@whoseTurn", SqlDbType.Char);
                    cmd.Parameters.Add(paramBoard);
                    cmd.Parameters.Add(paramWhoseTurn);
                    for (int i = 0; i < chessboard.History.Count; i++)
                    {
                        paramBoard.Value = chessboard.History[i].board;
                        paramWhoseTurn.Value = chessboard.History[i].whoseTurn;
                        cmd.ExecuteNonQuery();
                    }
                }
                commandText = "INSERT INTO MovesHistory(FigureSymbol,StartPos,EndPos,EatenFigureSymbol) " +
                                                "VALUES(@figureSymbol,@startPos,@endPos,@eatenFigureSymbol);";
                using (var cmd = new SqlCommand(commandText, con))
                {
                    var paramFigureSymbol = new SqlParameter("@figureSymbol", SqlDbType.NChar);
                    var paramStartPos = new SqlParameter("@startPos", SqlDbType.Char);
                    var paramEndPos = new SqlParameter("@endPos", SqlDbType.Char);
                    var paramEatenFigureSymbol = new SqlParameter("@eatenFigureSymbol", SqlDbType.NChar);
                    cmd.Parameters.Add(paramFigureSymbol);
                    cmd.Parameters.Add(paramStartPos);
                    cmd.Parameters.Add(paramEndPos);
                    cmd.Parameters.Add(paramEatenFigureSymbol);
                    for (int i = 0; i < chessboard.Moves.Count; i++)
                    {
                        paramFigureSymbol.Value = chessboard.Moves[i].FigureSymbol;
                        paramStartPos.Value = chessboard.Moves[i].StartPos.ToString();
                        paramEndPos.Value = chessboard.Moves[i].EndPos.ToString();
                        paramEatenFigureSymbol.Value = chessboard.Moves[i].EatenFigureSymbol;
                        cmd.ExecuteNonQuery();
                    }
                }
                commandText = "INSERT INTO Figures(ID, symbol, position) " +
                                           "VALUES(@ID, @symbol, @position);";
                using (var cmd = new SqlCommand(commandText, con))
                {
                    var paramID = new SqlParameter("@ID", SqlDbType.Int);
                    var paramSymbol = new SqlParameter("@symbol", SqlDbType.NChar);
                    var paramPosition = new SqlParameter("@position", SqlDbType.Char);
                    cmd.Parameters.Add(paramID);
                    cmd.Parameters.Add(paramSymbol);
                    cmd.Parameters.Add(paramPosition);
                    string commandText2 = "INSERT INTO FiguresMovesHistory(figureID, PreviousPosition) " +
                                                                   "VALUES(@figureID, @PreviousPosition);";
                    var cmd2 = new SqlCommand(commandText2, con);
                    var paramFigureID = new SqlParameter("@figureID", SqlDbType.Int);
                    var paramPreviousPosition = new SqlParameter("@PreviousPosition", SqlDbType.Char);
                    cmd2.Parameters.Add(paramFigureID);
                    cmd2.Parameters.Add(paramPreviousPosition);
                    for (int i = 0; i < chessboard.WhiteFigures.Count; i++)
                    {
                        paramID.Value = i;
                        paramSymbol.Value = chessboard.WhiteFigures[i].GetSymbol();
                        paramPosition.Value = chessboard.WhiteFigures[i].Position.ToString();
                        cmd.ExecuteNonQuery();
                        for (int j = 0; j < chessboard.WhiteFigures[i].PreviousPositions.Count; j++)
                        {
                            paramFigureID.Value = i;
                            paramPreviousPosition.Value = chessboard.WhiteFigures[i].PreviousPositions[j].ToString();
                            cmd2.ExecuteNonQuery();
                        }
                    }
                    for (int i = 0; i < chessboard.BlackFigures.Count; i++)
                    {
                        paramID.Value = chessboard.WhiteFigures.Count + i;
                        paramSymbol.Value = chessboard.BlackFigures[i].GetSymbol();
                        paramPosition.Value = chessboard.BlackFigures[i].Position.ToString();
                        cmd.ExecuteNonQuery();
                        for (int j = 0; j < chessboard.BlackFigures[i].PreviousPositions.Count; j++)
                        {
                            paramFigureID.Value = chessboard.WhiteFigures.Count + i;
                            paramPreviousPosition.Value = chessboard.BlackFigures[i].PreviousPositions[j].ToString();
                            cmd2.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets saved game from database
        /// </summary>
        /// <returns>chessboard</returns>
        public static Chessboard GetSavedGame()
        {
            var figurs = GetFigures();
            var chessboard = new Chessboard(figurs);
            chessboard.History.AddRange(GetBoardHistory());
            chessboard.Moves.AddRange(GetMovesHistory());
            chessboard.WhoseMoves=chessboard.History[chessboard.History.Count-1].whoseTurn==FigureColorEnum.White?
                                    FigureColorEnum.Black:FigureColorEnum.White;
            DeleteSavedGame();
            return chessboard;
        }

        /// <summary>
        /// Get figures from database
        /// </summary>
        /// <returns>list of figures</returns>
        static List<Figure> GetFigures()
        {
            List<Figure> figures = new List<Figure>();
            string conString = ConfigurationManager.ConnectionStrings["ChessDB"].ConnectionString;
            conString += ";MultipleActiveResultSets = True";
            using (var con = new SqlConnection(conString))
            {
                using (var cmd = new SqlCommand("SELECT ID,symbol,position FROM Figures", con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {                        
                        Figure figure = Figure.CreateFigure(Convert.ToChar(reader.GetString("symbol")), new Point(reader.GetString("position")));
                        using (var cmd1 = new SqlCommand("SELECT PreviousPosition " +
                                                         "FROM FiguresMovesHistory " +
                                                         "WHERE figureID=@figurID" +
                                                         " ORDER BY ID", con))
                        {
                            var figurID = new SqlParameter("@figurID", SqlDbType.Int);
                            figurID.Value = reader.GetInt32("ID");
                            cmd1.Parameters.Add(figurID);
                            var reader1 = cmd1.ExecuteReader();
                            while (reader1.Read())
                            {
                                figure.PreviousPositions.Add(new Point(reader1.GetString("PreviousPosition")));
                            }
                        }
                        figures.Add(figure);
                    }
                }
            }
            return figures;
        }

        /// <summary>
        /// Gets chessboard history from database
        /// </summary>
        /// <returns>list of chessboardhistory</returns>
         static List<ChessboardHistory> GetBoardHistory()
        {
            var histories = new List<ChessboardHistory>();
            string conString = ConfigurationManager.ConnectionStrings["ChessDB"].ConnectionString;
            using (var con = new SqlConnection(conString))
            {
                using (var cmd = new SqlCommand("SELECT board, whoseturn FROM BoardHistory ORDER BY ID", con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var history = new ChessboardHistory(reader.GetString("board"), 
                                                           (FigureColorEnum)Enum.Parse(typeof(FigureColorEnum), reader.GetString("whoseturn")));
                        histories.Add(history);
                    }
                }
            }
            return histories;
        }

        /// <summary>
        /// Gets moves histore from database
        /// </summary>
        /// <returns>list of move</returns>
        static List<Move> GetMovesHistory()
        {
            var histories = new List<Move>();
            string conString = ConfigurationManager.ConnectionStrings["ChessDB"].ConnectionString;
            using (var con = new SqlConnection(conString))
            {
                using (var cmd = new SqlCommand("SELECT FigureSymbol, StartPos, EndPos, EatenFigureSymbol FROM MovesHistory ORDER BY ID", con))
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var move = new Move(Convert.ToChar(reader.GetString("FigureSymbol")),
                                            new Point(reader.GetString("StartPos")),
                                            new Point(reader.GetString("EndPos")),
                                            Convert.ToChar(reader.GetString("EatenFigureSymbol")));
                        histories.Add(move);
                    }
                }
            }
            return histories;
        }
    }
}
