using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    /// <summary>
    /// class for Rook. implements Figures abstract class
    /// </summary>
    public sealed class Rook : Figure
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="col">ColorEnum: figure color</param>
        /// <param name="pos">Point: figure position</param>
        public Rook(FigureColorEnum col, Point pos) : base(col, pos)
        {
            Weight = 5;
        }

        /// <summary>
        /// Gets all possible moves
        /// </summary>
        /// <returns>Array of possible moves positiones</returns>
        public override List<Point> GetAllPossibleMoves(Chessboard chessboard)
        {
            List<Point> movesList = new();
            Point p;
            for (int i = 1; i < 8 - Position.Y; i++)
            {
                p = new Point(Position.X, Position.Y + i);
                if (chessboard.Board[p.X, p.Y] == '\u0020')
                {
                    movesList.Add(p);
                }
                else
                {
                    if (chessboard.GetFigure(p).Color != Color)
                        movesList.Add(p);
                    break;
                }
            }
            for (int i = 1; i <= Position.Y; i++)
            {
                p = new Point(Position.X, Position.Y - i);
                if (chessboard.Board[p.X, p.Y] == '\u0020')
                {
                    movesList.Add(p);
                }
                else
                {
                    if (chessboard.GetFigure(p).Color != Color)
                        movesList.Add(p);
                    break;
                }
            }
            for (int i = 1; i < 8 - Position.X; i++)
            {
                p = new Point(Position.X + i, Position.Y);
                if (chessboard.Board[p.X, p.Y] == '\u0020')
                {
                    movesList.Add(p);
                }
                else
                {
                    if (chessboard.GetFigure(p).Color != Color)
                        movesList.Add(p);
                    break;
                }
            }
            for (int i = 1; i <= Position.X; i++)
            {
                p = new Point(Position.X - i, Position.Y);
                if (chessboard.Board[p.X, p.Y] == '\u0020')
                {
                    movesList.Add(p);
                }
                else
                {
                    if (chessboard.GetFigure(p).Color != Color)
                        movesList.Add(p);
                    break;
                }
            }
            return movesList;
        }

        /// <summary>
        /// implements GetSymbol method from Figure abstract class
        /// </summary>
        /// <returns>char: figure symbol</returns>
        public override char GetSymbol()
        {
            if (Color == FigureColorEnum.White)
                return '\u2656';
            else
                return '\u265C';
        }
    }
}
