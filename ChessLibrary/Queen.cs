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
        public override List<Point> GetAllPossibleMoves(Chessboard chessboard)
        {

            Rook rook = new(Color, Position);
            List<Point> moves = rook.GetAllPossibleMoves(chessboard);
            Bishop bishop = new(Color, Position);
            moves.AddRange(bishop.GetAllPossibleMoves(chessboard));
            return moves;
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
