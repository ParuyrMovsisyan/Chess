using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    internal class ChessboardHistory
    {
        internal string board;
        internal FigureColorEnum whoseTurn;
        public ChessboardHistory(char[,] chessboard,FigureColorEnum color)
        {
            board = chessboard.ArrayToString();
            whoseTurn = color;
        }
        public ChessboardHistory(string chessboard, FigureColorEnum color)
        {
            board = chessboard;
            whoseTurn = color;
        }
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
