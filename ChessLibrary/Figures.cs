namespace ChessLibrary
{
    /// <summary>
    /// abstract figures class
    /// </summary>
    public  abstract class Figure
    {
        /// <summary>
        /// figure's color
        /// </summary>
        private readonly FigureColorEnum color;
        /// <summary>
        /// figure's position
        /// </summary>
        private Point position;             
        /// <summary>
        /// gets figure color
        /// </summary>
        public FigureColorEnum Color
        {
            get 
            {
                return color;
            }
        }
        /// <summary>
        /// gets and sets figure position
        /// </summary>
        public Point Position
        {
            get
            {
                return position;
            }
            set
            {
                if (Chessboard.IsInBoard(value))
                {
                    position = value;
                }
                else
                    throw new Exception("Given position is out of board.");
            }
        }
        /// <summary>
        /// previous positions collection
        /// </summary>
        public List<Point> PreviousPositions;
        /// <summary>
        /// figure's weight
        /// </summary>
        public int Weight { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="col">ColorEnum: figure color</param>
        /// <param name="pos">Point: figure position</param>
        public Figure(FigureColorEnum col, Point pos)
        {
            color = col;
            position = pos;
            PreviousPositions= new List<Point>();
        }

        /// <summary>
        /// Gets all possible moves
        /// </summary>
        /// <returns>Array of possible moves positiones</returns>
        public abstract Point[] GetAllPossibleMoves(Chessboard chessboard);

        /// <summary>
        /// returnes true if figure can move from start position to finish position
        /// </summary>
        /// <param name="startPos">Point: start position</param>
        /// <param name="finishPos">Point: target position</param>
        /// <returns>bool: true if figure can move from start position to target position</returns>
        public bool CanMove(Point targetPos, Chessboard chessboard)
        {
            bool canMove = false;
            bool isTargetInPossibleMoves=false;
            bool hasSpetialMove = false ;
            if (this is Pawn pawn)
            {
                hasSpetialMove = pawn.HasSpecialMove(chessboard, out Point spetialPos);
                if (hasSpetialMove)
                {
                    if (spetialPos.Equals(targetPos))
                        isTargetInPossibleMoves = true;
                }
            }
            else if (this is King king)
            {
                if (king.color == chessboard.WhoseMoves)
                {
                    if (king.HasSpecialMoves(chessboard, out List<Point> castlPositions))
                    {
                        foreach (Point pos in castlPositions)
                        {
                            if (pos.Equals(targetPos))
                                return true;
                        }
                    }
                }                
            }
            if (!isTargetInPossibleMoves)
            {
                foreach (var item in GetAllPossibleMoves(chessboard))
                {
                    if (item.Equals(targetPos))
                    {
                        isTargetInPossibleMoves = true;
                        break;
                    }
                }
            }            
            if (isTargetInPossibleMoves)
            {
                Chessboard fakeChessboard = new (chessboard);
                fakeChessboard.Board[Position.X, Position.Y] = '\u0020';
                if (fakeChessboard.Board[targetPos.X, targetPos.Y] != '\u0020')
                {
                    List<Figure> enemyFigurs = fakeChessboard.GetEnemyFigures(Color);
                    enemyFigurs.RemoveFigure(targetPos);
                }
                else if (hasSpetialMove)
                {
                    List<Figure> enemyFigurs = fakeChessboard.GetEnemyFigures(Color);
                    enemyFigurs.RemoveFigure(new Point(Position.X,targetPos.Y));
                    fakeChessboard.Board[Position.X, targetPos.Y]= '\u0020';
                }                
                fakeChessboard.Board[targetPos.X, targetPos.Y] = GetSymbol();
                if (this is King)
                {
                    var friendFigures = fakeChessboard.GetFriendFigures(Color);
                    int index = friendFigures.IndexOf(Position);
                    friendFigures[index].Position = targetPos;
                }
                King king = fakeChessboard.GetOwnKing(Color);
                bool isCheck = king.IsUnderAttack(fakeChessboard);
                if (isCheck == false)
                {
                    canMove = true;
                }
            }
            return canMove;
        }

        /// <summary>
        /// Cheks is figure under enemy's attacks
        /// </summary>
        /// <param name="chessboard">Chessboard</param>
        /// <returns>returns true if figure is under attack, false otherwise</returns>
        public bool IsUnderAttack(Chessboard chessboard)
        {
            bool isUnderAttack=false;
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

        /// <summary>
        /// gets is figure has any move
        /// </summary>
        /// <param name="chessboard">Chessboard</param>
        /// <returns>returns true if figure has any move, false otherwise</returns>
        public bool HasMove(Chessboard chessboard)
        {
            bool hasMove = false;
            foreach (Point point in GetAllPossibleMoves(chessboard))
            {
                if (CanMove(point, chessboard))
                {
                    hasMove = true;
                    break;
                }
            }
            return hasMove;
        }

        /// <summary>
        /// gets figure symbol
        /// </summary>
        /// <returns>char: figure symbol</returns>
        public abstract char GetSymbol();        
    }
}
