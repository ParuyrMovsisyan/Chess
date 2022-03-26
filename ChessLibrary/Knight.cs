using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    /// <summary>
    /// class for Knight. implements Figures abstract class
    /// </summary>
    public sealed class Knight : Figure
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="col">ColorEnum: figure color</param>
        /// <param name="pos">Point: figure position</param>
        public Knight(FigureColorEnum col, Point pos) : base(col, pos)
        {
            Weight = 3;
        }

        /// <summary>
        /// Gets all possible moves
        /// </summary>
        /// <returns>Array of possible moves positiones</returns>
        public override Point[] GetAllPossibleMoves(Chessboard chessboard)
        {
            int x = Position.X;
            int y = Position.Y;
            Point[] movesPoints = new Point[8];
            movesPoints[0] = new Point(x - 1, y - 2);
            movesPoints[1] = new Point(x - 1, y + 2);
            movesPoints[2] = new Point(x + 1, y - 2);
            movesPoints[3] = new Point(x + 1, y + 2);
            movesPoints[4] = new Point(x - 2, y - 1);
            movesPoints[5] = new Point(x - 2, y + 1);
            movesPoints[6] = new Point(x + 2, y - 1);
            movesPoints[7] = new Point(x + 2, y + 1);
            int index = 0;
            Point[] tempPoints = new Point[8];
            for (int i = 0; i < 8; i++)
            {
                if (Chessboard.IsInBoard(movesPoints[i]))
                {
                    if (chessboard.Board[movesPoints[i].X, movesPoints[i].Y] != '\u0020'
                        && chessboard.GetFigure(movesPoints[i]).Color == Color)
                    { }
                    else
                    {
                        tempPoints[index] = movesPoints[i];
                        index++;
                    }                    
                }
            }
            if (index == 8)
            {
                return tempPoints;
            }
            else
            {
                Point[] returnPoints = new Point[index];
                Array.Copy(tempPoints, returnPoints, index);
                return returnPoints;
            }
        }

        /// <summary>
        /// implements GetSymbol method from Figure abstract class
        /// </summary>
        /// <returns>char: figure symbol</returns>
        public override char GetSymbol()
        {
            if (Color == FigureColorEnum.White)
                return '\u2658';
            else
                return '\u265E';
        }
    }
}
