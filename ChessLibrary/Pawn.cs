
namespace ChessLibrary
{
    /// <summary>
    /// class for Pawn. implements Figures abstract class
    /// </summary>
    public sealed class Pawn : Figure
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="col">ColorEnum: figure color</param>
        /// <param name="pos">Point: figure position</param>
        public Pawn(FigureColorEnum col, Point pos) : base(col, pos)
        {
            Weight = 1;
        }

        /// <summary>
        /// Gets all possible moves
        /// </summary>
        /// <returns>Array of possible moves positiones</returns>
        public override List<Point> GetAllPossibleMoves(Chessboard chessboard)
        {
            List<Point> movesList = new();
            int moveDirection;
            if (Color == FigureColorEnum.White)
            {
                if (Position.X == 6 && chessboard.Board[5, Position.Y] == '\u0020' && chessboard.Board[4, Position.Y] == '\u0020')
                {
                    movesList.Add(new Point(4, Position.Y));
                }
                moveDirection = -1;
            }
            else
            {
                if (Position.X == 1 && chessboard.Board[2, Position.Y] == '\u0020' && chessboard.Board[3, Position.Y] == '\u0020')
                {
                    movesList.Add(new Point(3, Position.Y));
                }
                moveDirection = 1;
            }            
            Point p = new (Position.X + moveDirection, Position.Y);
            if (Chessboard.IsInBoard(p) && chessboard.Board[p.X, p.Y] == '\u0020')
                movesList.Add(p);
            p = new (Position.X + moveDirection, Position.Y - 1);
            if (Chessboard.IsInBoard(p) && chessboard.Board[p.X, p.Y] != '\u0020' && chessboard.GetFigure(p).Color != Color)
                movesList.Add(p);
            p = new (Position.X + moveDirection, Position.Y + 1);
            if (Chessboard.IsInBoard(p) && chessboard.Board[p.X, p.Y] != '\u0020' && chessboard.GetFigure(p).Color != Color)
                movesList.Add(p);
            if (HasSpecialMove(chessboard, out Point pos))
                movesList.Add(pos);
            return movesList;
        }

        /// <summary>
        /// Cheks is there En Passant.
        /// En Passant (meaning “in passing”) is a very special move that can be done by Pawns, 
        /// but only in certain places and situations.  
        /// Pawns move forward (1-2 spaces on their first move and 1 space after that) and attack on the diagonal.
        /// The special case of En Passant  occurs on the 5th rank for white Pawns, and the 4th rank for black Pawns.
        /// </summary>
        /// <param name="chessboard">Chessboard: information for other figures</param>
        /// <param name="spetialMovePoint">Point: out parameter. Sets position for spetial move</param>
        /// <returns>returns true if pawn has spetial move, otherwise returns false</returns>
        public bool HasSpecialMove(Chessboard chessboard, out Point spetialMovePoint)
        {
            spetialMovePoint = Position;
            bool hasSpecialMove=false;
            int count = chessboard.Moves.Count;
            if (count == 0)
                return hasSpecialMove;
            Move move = chessboard.Moves[count - 1];
            if (Color == FigureColorEnum.White && Position.X == 3)
            {
                if (move.FigureSymbol == '\u265F')
                {
                    if (move.EndPos.X == 3 && Math.Abs(Position.Y - move.EndPos.Y) == 1)
                    {
                        if ((move.EndPos.X - move.StartPos.X) == 2)
                        {
                            hasSpecialMove = true;
                            spetialMovePoint=new Point(move.EndPos.X-1,move.EndPos.Y);
                        }
                    }
                }
            }
            else if (Color == FigureColorEnum.Black && Position.X == 4)
            {
                if (move.FigureSymbol == '\u2659')
                {
                    if (move.EndPos.X == 4 && Math.Abs(Position.Y - move.EndPos.Y) == 1)
                    {
                        if ((move.StartPos.X - move.EndPos.X) == 2)
                        {
                            hasSpecialMove = true;
                            spetialMovePoint = new Point(move.EndPos.X + 1, move.EndPos.Y);
                        }                            
                    }
                }
            }
            return hasSpecialMove;
        }

        /// <summary>
        /// implements GetSymbol method from Figure abstract class
        /// </summary>
        /// <returns>char: figure symbol</returns>
        public override char GetSymbol()
        {
            if (Color == FigureColorEnum.White)
                return '\u2659';
            else
                return '\u265F';
        }
    }
}
