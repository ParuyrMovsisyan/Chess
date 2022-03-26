using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    /// <summary>
    /// class for King. implements Figure abstract class
    /// </summary>
    public sealed class King : Figure
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="col">ColorEnum: figure color</param>
        /// <param name="pos">Point: figure position</param>
        public King(FigureColorEnum col, Point pos) : base(col, pos)
        {
            Weight = 100;
        }

        /// <summary>
        /// implements GetSymbol method from Figure abstract class
        /// </summary>
        /// <returns>char: figure symbol</returns>
        public override char GetSymbol()
        {
            if (Color == FigureColorEnum.White)
                return '\u2654';
            else
                return '\u265A';
        }

        /// <summary>
        /// Gets all possible moves
        /// </summary>
        /// <returns>Array of possible moves positiones</returns>
        public override List<Point> GetAllPossibleMoves(Chessboard chessboard)
        {
            List<Point> movesList = new();
            Point pos = new (Position.X - 1, Position.Y - 1);
            if (Chessboard.IsInBoard(pos))
                movesList.Add(pos);
            pos = new (Position.X - 1, Position.Y);
            if (Chessboard.IsInBoard(pos))
                movesList.Add(pos);
            pos = new (Position.X - 1, Position.Y + 1);
            if (Chessboard.IsInBoard(pos))
                movesList.Add(pos);
            pos = new (Position.X, Position.Y - 1);
            if (Chessboard.IsInBoard(pos))
                movesList.Add(pos);
            pos = new (Position.X, Position.Y + 1);
            if (Chessboard.IsInBoard(pos))
                movesList.Add(pos);
            pos = new (Position.X + 1, Position.Y - 1);
            if (Chessboard.IsInBoard(pos))
                movesList.Add(pos);
            pos = new Point(Position.X + 1, Position.Y);
            if (Chessboard.IsInBoard(pos))
                movesList.Add(pos);
            pos = new (Position.X + 1, Position.Y + 1);
            if (Chessboard.IsInBoard(pos))
                movesList.Add(pos);
            List<Point> returnList = new ();
            foreach (var item in movesList)
            {
                if (chessboard.Board[item.X, item.Y] != '\u0020')
                {
                    if (chessboard.GetFigure(item).Color != Color)
                    {
                        returnList.Add(item);
                    }
                }
                else
                {
                    returnList.Add(item);
                }
            }

            return returnList;
        }

        /// <summary>
        /// The king can make a special move, in conjunction with a rook, called castling. 
        /// When castling, the king moves two squares toward one of its rooks, 
        /// and that rook is placed on the square over which the king crossed. 
        /// Castling is permissible only when:
        /// 1. neither the king nor the castling rook have previously moved
        /// 2. no squares between them are occupied
        /// 3. the king is not in check
        /// 4. none of the squares the king would move across or to are under enemy attack
        /// </summary>
        /// <param name="chessboard"></param>
        /// <param name="spetialPositions"></param>
        /// <returns></returns>
        public bool HasSpecialMoves(Chessboard chessboard, out List<Point> specialPositions)
        {
            
            bool hasSpecialMoves=false;
            specialPositions = new List<Point>();
            if (!IsUnderCheck(chessboard))
            {
                if (PreviousPositions.Count == 0)
                {
                    List<Figure> friendFigures=chessboard.GetFriendFigures(Color);
                    int index = friendFigures.IndexOf(new Point(Position.X, 0));
                    if (index >= 0)
                    {
                        Figure figure = friendFigures[index];
                        if (figure is Rook)
                        {
                            if (figure.PreviousPositions.Count == 0)
                            {
                                if (chessboard.Board[Position.X, 1] == '\u0020'
                                    && chessboard.Board[Position.X, 2] == '\u0020'
                                    && chessboard.Board[Position.X, 3] == '\u0020')
                                {
                                    FigureColorEnum enemyColor = Chessboard.GetEnemyColor(Color);
                                    if (chessboard.IsCheck(new Point(Position.X, 3), enemyColor) == false
                                        && chessboard.IsCheck(new Point(Position.X, 2), enemyColor) == false)
                                    {
                                        specialPositions.Add(new Point(Position.X, 2));
                                    }    
                                }
                            }
                        }
                    }
                    index = friendFigures.IndexOf(new Point(Position.X, 7));
                    if (index >= 0)
                    {
                        Figure figure = friendFigures[index];
                        if (figure is Rook)
                        {
                            if (figure.PreviousPositions.Count == 0)
                            {
                                if (chessboard.Board[Position.X, 5] == '\u0020'
                                    && chessboard.Board[Position.X, 6] == '\u0020')
                                {
                                    FigureColorEnum enemyColor = Chessboard.GetEnemyColor(Color);
                                    if (chessboard.IsCheck(new Point(Position.X, 5), enemyColor) == false
                                        && chessboard.IsCheck(new Point(Position.X, 6), enemyColor) == false)
                                    {
                                        specialPositions.Add(new Point(Position.X, 6));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if(specialPositions.Count > 0)
                hasSpecialMoves = true;
            return hasSpecialMoves;            
        }

        /// <summary>
        /// Cheks is king under enemy's attacks
        /// </summary>
        /// <param name="chessboard">Chessboard</param>
        /// <returns>returns true if king  is under attack, false otherwise</returns>
        public bool IsUnderCheck(Chessboard chessboard)
        {
            bool isUnderAttack = false;
            List<Figure> enemyFigures = chessboard.GetEnemyFigures(Color);
            foreach (Figure enemyFigure in enemyFigures)
            {
                foreach (Point pos in enemyFigure.GetAllPossibleMoves(chessboard))
                {
                    if (pos == Position)
                    {
                        return true;
                    }
                }
            }
            return isUnderAttack;
        }
    }
}


    