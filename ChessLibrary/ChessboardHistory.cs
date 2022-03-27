using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    internal class ChessboardHistory
    {
        char[,] board = new char[8, 8];
        FigureColorEnum whoseTurn;
        public ChessboardHistory(char[,] chessboard,FigureColorEnum color)
        {
            for(int i=0;i<8;i++)
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = chessboard[i, j];
                }
            whoseTurn = color;
        }
        public static bool operator ==(ChessboardHistory op1, ChessboardHistory op2)
        {
           
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                    {
                        if (op1.board[i, j] != op2.board[i, j])
                            return false;
                    }
            if (op1.whoseTurn != op2.whoseTurn)
                return false;
            return true;
        }
        public static bool operator !=(ChessboardHistory op1, ChessboardHistory op2)
        {
            return!(op1 == op2);
        }
    }
}
