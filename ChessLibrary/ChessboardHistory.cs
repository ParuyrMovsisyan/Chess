using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    /// <summary>
    /// for saving chessboard history. Contains board and whose turn information
    /// </summary>
    internal class ChessboardHistory
    {
        /// <summary>
        /// information about board in string
        /// </summary>
        internal string board;
        /// <summary>
        /// information about whose turn
        /// </summary>
        internal FigureColorEnum whoseTurn;
        public ChessboardHistory(char[,] chessboard,FigureColorEnum color)
        {
            board = chessboard.ArrayToString();
            whoseTurn = color;
        }

        /// <summary>
        /// for checking two instances enequality
        /// </summary>
        /// <returns>true if two instances all properties are have the same value, otherwise false</returns>
        public ChessboardHistory(string chessboard, FigureColorEnum color)
        {
            board = chessboard;
            whoseTurn = color;
        }

        /// <summary>
        /// for checking two instances enequality
        /// </summary>
        /// <returns>false if two instances all properties are have the same value, otherwise true</returns>
        public static bool operator ==(ChessboardHistory op1, ChessboardHistory op2)
        {
           return op1.board == op2.board && op1.whoseTurn == op2.whoseTurn;
        }
        public static bool operator !=(ChessboardHistory op1, ChessboardHistory op2)
        {
            return !(op1 == op2);
        }
    }
}
