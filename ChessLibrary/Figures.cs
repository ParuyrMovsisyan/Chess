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
        public List<Point> PreviousPositions=new();
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
        }

        /// <summary>
        /// Creates figure from given figure symbol and position
        /// </summary>
        /// <param name="c">figure unicode symbol</param>
        /// <param name="p">position for figure</param>
        /// <returns>Figure if can create or null otherwise</returns>
        internal static Figure CreateFigure(char c, Point p)
        {
            Figure? figure = c switch
            {
                '\u2654' => new King(FigureColorEnum.White, p),
                '\u265A' => new King(FigureColorEnum.Black, p),
                '\u2655' => new Queen(FigureColorEnum.White, p),
                '\u265B' => new Queen(FigureColorEnum.Black, p),
                '\u2656' => new Rook(FigureColorEnum.White, p),
                '\u265C' => new Rook(FigureColorEnum.Black, p),
                '\u2657' => new Bishop(FigureColorEnum.White, p),
                '\u265D' => new Bishop(FigureColorEnum.Black, p),
                '\u2658' => new Knight(FigureColorEnum.White, p),
                '\u265E' => new Knight(FigureColorEnum.Black, p),
                '\u2659' => new Pawn(FigureColorEnum.White, p),
                '\u265F' => new Pawn(FigureColorEnum.Black, p),
                _ => null,
            };
            return figure;
        }

        /// <summary>
        /// Gets all possible moves
        /// </summary>
        /// <returns>Array of possible moves positiones</returns>
        public abstract List<Point> GetAllPossibleMoves(Chessboard chessboard);

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
            if (this is King king)
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
            else if (this is Pawn pawn)
            {
                hasSpetialMove = pawn.HasSpecialMove(chessboard, out Point spetialPos);
                if (hasSpetialMove)
                {
                    if (spetialPos.Equals(targetPos))
                        isTargetInPossibleMoves = true;
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
                king = fakeChessboard.GetOwnKing(Color);
                //if(this is King)
                //    king.Position = targetPos;
                bool isCheck = king.IsUnderCheck(fakeChessboard);
                if (isCheck == false)
                {
                    canMove = true;
                }
            }
            return canMove;
        }

        /// <summary>
        /// Checks can be eaten by enemies
        /// </summary>
        /// <param name="chessboard">Chessboard: state of game</param>
        /// <returns></returns>
        public bool CanBeEaten(Chessboard chessboard)
        {
            var enemies=chessboard.GetEnemyFigures(Color);
            foreach (var figure in enemies)
            {
                if(figure.CanMove(Position,chessboard))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks can be eaten if figure moves to target position
        /// </summary>
        /// <param name="trgPos">Point: position for move</param>
        /// <param name="chessboard">Chessboard: state of game</param>
        /// <returns>true if figure can move target position and enemy can eat, otherwise returns false </returns>
        public bool CanBeEatenIfMove(Point trgPos, Chessboard chessboard)
        {
            if (CanMove(trgPos, chessboard))
            
            {
                Chessboard fakeChessboard = new(chessboard);
                fakeChessboard.Move(Position, trgPos);
                var fakeFigure = fakeChessboard.GetFigure(trgPos);
                return fakeFigure.CanBeEaten(fakeChessboard);
            }
            return false;
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
