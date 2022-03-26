using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    /// <summary>
    /// class for Queen. implements Figures abstract class
    /// </summary>
    public sealed class Queen : Figure
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="col">ColorEnum: figure color</param>
        /// <param name="pos">Point: figure position</param>
        public Queen(FigureColorEnum col, Point pos) : base(col, pos)
        {
            Weight = 9;
        }

        /// <summary>
        /// Gets all possible moves
        /// </summary>
        /// <returns>Array of possible moves positiones</returns>
        public override Point[] GetAllPossibleMoves(Chessboard chessboard)
        {
            Rook rook = new(Color, Position);
            Point[] ortogonals = rook.GetAllPossibleMoves(chessboard);
            Bishop bishop = new(Color, Position);
            Point[] diagonals = bishop.GetAllPossibleMoves(chessboard);
            int length = diagonals.Length + ortogonals.Length;
            Point[] pos = new Point[length];
            for (int i = 0; i < diagonals.Length; i++)
            {
                pos[i] = diagonals[i];
            }
            for (int i = 0; i < ortogonals.Length; i++)
            {
                pos[diagonals.Length + i] = ortogonals[i];
            }
            return pos;
        }

        /// <summary>
        /// implements GetSymbol method from Figure abstract class
        /// </summary>
        /// <returns>char: figure symbol</returns>
        public override char GetSymbol()
        {
            if (Color == FigureColorEnum.White)
                return '\u2655';
            else
                return '\u265B';
        }
    }
}
