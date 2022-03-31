using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChessLibrary
{
    /// <summary>
    /// Chessboard class contains 
    /// 1.board, it is a 8X8 char matrix:
    /// if there is a figure in cell then board's cell value figure symbole otherwise cell value is space
    /// 2.list of white figures
    /// 3.list of black figures
    /// 4.whose moves
    /// </summary>
    public class Chessboard
    {
        /// <summary>
        /// Array of chessboard cells
        /// </summary>
        char[,] board=new char[8,8];
        /// <summary>
        /// defines whose moves
        /// </summary>
        FigureColorEnum whoseMoves;
        /// <summary>
        /// List of white figures
        /// </summary>
        internal List<Figure> WhiteFigures { get; private set; } = new();
        /// <summary>
        /// List of black figures
        /// </summary>
        internal List<Figure> BlackFigures { get; private set; } = new ();
        /// <summary>
        /// board's history
        /// </summary>
        internal List<ChessboardHistory> History {  get; private set; } = new();
        /// <summary>
        /// List of moves
        /// </summary>
        public List<Move> Moves { get; private set; } = new();

        /// <summary>
        /// Property for chessboard
        /// </summary>
        public char[,] Board
        {
            get
            {
                return board;
            }
            set
            {
                board = value;
            }
        }
        /// <summary>
        /// Propery for whose moves
        /// </summary>
        public FigureColorEnum WhoseMoves { get { return whoseMoves; } internal set { whoseMoves = value; } }

        /// <summary>
        /// Constructor: builds empty chessboard
        /// </summary>
        public Chessboard()
        {
            SetEmptyBoard();
            BlackFigures = GetAllBlackFigures();
            PutFigures(BlackFigures);
            WhiteFigures = GetAllWhiteFigures();
            PutFigures(WhiteFigures);
        }

        /// <summary>
        /// Constructor: builds chessboard with given figures
        /// </summary>
        /// <param name="figures">List of figures</param>
        internal Chessboard(List<Figure> figures)
        {
            SetEmptyBoard();
            foreach (Figure figure in figures)
            {
                if (figure.Color == FigureColorEnum.White)
                    WhiteFigures.Add(figure);
                else
                    BlackFigures.Add(figure);
                PutFigure(figure);
            }
        }

        /// <summary>
        /// Constructor: builds chessboard with given figures
        /// </summary>
        /// <param name="wFigures">List of figures: white figures</param>
        /// <param name="bFigures">List of figures: black figures</param>
        public Chessboard(List<Figure> wFigures, List<Figure> bFigures) 
        {
            SetEmptyBoard();
            WhiteFigures = wFigures;
            BlackFigures = bFigures;
            PutFigures(WhiteFigures);
            PutFigures(BlackFigures);
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="chessboard"></param>
        public Chessboard(Chessboard chessboard):this(chessboard.GetFigures())
        {
            foreach( var move in chessboard.Moves)
                Moves.Add(move);
            foreach(var history in chessboard.History)
                History.Add(history);
            whoseMoves=chessboard.WhoseMoves;
        }

        /// <summary>
        /// Constructor: builds chessboard from given board's.
        /// This constructor can throw exception use it in try/catch statement 
        /// </summary>
        /// <param name="newBoard">Two dimensional character 8x8 array</param>
        /// <param name="isWhitesTurn">Boolean, if true then game will start whites, otherwise blacks</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        public Chessboard(char[,] newBoard, bool isWhitesTurn = true)
        {
            if (newBoard.GetLength(0) != 8 || newBoard.GetLength(1) != 8)
                throw new ArgumentException("Given board is not for chess...");
            for (int i = 0; i < 8; i++)
            {
                if(newBoard[0,i]== '\u2659' || newBoard[0, i] == '\u2659')
                    throw new Exception("The white pawn can not be in 8-th or 1-st line.");
                if(newBoard[1,i]== '\u265F')
                    throw new Exception("The black  pawn can not be in 8-th or 1-st line.");
            }
            board = newBoard;
            List<Figure> figures = GetFigures();
            if (figures is null)
                throw new ArgumentException("I can not create figures from your board...");
            else if (figures.Count == 0)
                throw new Exception("There is not any figure");
            else if (figures.Count < 3)
                throw new Exception("There are few figures for starting game...");
            else if (!Chessboard.IsTwoKingsExist(figures))
                throw new Exception("There are more or less then two kings");            
            else
            {
                Chessboard chessboard = new (figures);
                if (chessboard.WhiteFigures.Count > 16)
                    throw new Exception("The white figures more than 16...");
                if (chessboard.BlackFigures.Count > 16)
                    throw new Exception("The black figures more than 16...");
                FigureColorEnum color;
                if (isWhitesTurn)
                    color = FigureColorEnum.White;
                else
                    color = FigureColorEnum.Black;
                chessboard.whoseMoves = color;
                if (chessboard.IsDraw())
                    throw new Exception("This game can not be started, becouse there is alredy draw.");
                else if(chessboard.IsCheckmate(color))
                    throw new Exception("This game can not be started, becouse there is alredy checkmate.");
                else
                {
                    color = Chessboard.GetEnemyColor(color);
                    if (chessboard.IsCheck(color))
                        throw new Exception("This game can not be started, becouse the second player's is under check.");
                }
                whoseMoves = chessboard.WhoseMoves;
                WhiteFigures = chessboard.WhiteFigures;
                BlackFigures = chessboard.BlackFigures;
            }
        }

        /// <summary>
        /// Cheks are there to Kings
        /// </summary>
        /// <param name="figures">list of figures</param>
        /// <returns>Returns true, if there are two kings and false otherwise</returns>
        private static bool IsTwoKingsExist(List<Figure> figures)
        {
            bool isTwoKingsExist = false;
            if (figures != null)
            {
                List<Figure> kings=new ();
                if (figures.Count < 2)
                    return isTwoKingsExist;
                else
                {
                    foreach(Figure f in figures)
                        if(f is King)
                            kings.Add(f);
                }
                if (kings.Count != 2)
                    return isTwoKingsExist;
                else
                {
                    if(kings[0].Color!=kings[1].Color)
                        isTwoKingsExist=true;
                }
            }
            return isTwoKingsExist;
        }

        /// <summary>
        /// Cheks is position in cheesboard
        /// </summary>
        /// <param name="p">Point: figure position</param>
        /// <returns>returns true if position in chessboard, otherwise returns false</returns>
        public static bool IsInBoard(Point p)
        {
            return (p.X >= 0 && p.X < 8 && p.Y >= 0 && p.Y < 8);
        }

        /// <summary>
        /// Moves figure from start position to targetPosition.
        /// If in target position is enemy figure removes enemy figure
        /// </summary>
        /// <param name="startPos">Point: start position for figure</param>
        /// <param name="targetPos">Point: target position for figure</param>
        public void Move(Point startPos, Point targetPos)
        {
            Figure figure = GetFigure(startPos);
            char eatenFigureSymbol= '\u0020';
            if (figure is not null)
            {
                if (figure.Color == whoseMoves)
                {
                    if (figure.CanMove(targetPos, this))
                    {                        
                        History.Add(new(Board,WhoseMoves));
                        var friendFigures = GetFriendFigures(figure);
                        Figure? enemyFigure = null;
                        if (figure is Pawn pawn)
                        {
                            if (pawn.HasSpecialMove(this, out Point spetialPos))
                            {
                                if (spetialPos == targetPos)
                                {
                                    enemyFigure = GetFigure(new Point(startPos.X, targetPos.Y));
                                    eatenFigureSymbol = Board[startPos.X, targetPos.Y];
                                    Board[startPos.X, targetPos.Y] = '\u0020';
                                }
                            }
                            else
                            {
                                enemyFigure = GetFigure(targetPos);
                            }
                        }
                        else
                        {
                            enemyFigure = GetFigure(targetPos);
                        }
                        if (enemyFigure is not null)
                        {
                            if (enemyFigure.Color == FigureColorEnum.White)
                                WhiteFigures = WhiteFigures.RemoveFigure(enemyFigure.Position);
                            else
                                BlackFigures = BlackFigures.RemoveFigure(enemyFigure.Position);
                        }
                        int i = friendFigures.IndexOf(figure.Position);
                        friendFigures[i].PreviousPositions.Add(friendFigures[i].Position);
                        friendFigures[i].Position = targetPos;
                        char figureSymbol = Board[startPos.X, startPos.Y];
                        if ((int)eatenFigureSymbol < 9812 || (int)eatenFigureSymbol > 9823)
                            eatenFigureSymbol = Board[targetPos.X, targetPos.Y];
                        Board[targetPos.X, targetPos.Y] = figureSymbol;
                        Board[startPos.X, startPos.Y] = '\u0020';
                        Moves.Add(new Move(figureSymbol, startPos, targetPos,eatenFigureSymbol));
                        int distance = startPos.Y - targetPos.Y;
                        if (figure is King && Math.Abs(distance) == 2)
                        {
                            int indexOfRook;
                            Point rookStartPos;
                            Point rookTargetPos;
                            if (distance == 2)
                            {
                                rookStartPos = new Point(startPos.X, 0);
                                rookTargetPos = new Point(startPos.X, 3);
                            }
                            else
                            {
                                rookStartPos = new Point(startPos.X, 7);
                                rookTargetPos = new Point(startPos.X, 5);
                            }
                            indexOfRook = friendFigures.IndexOf(rookStartPos);
                            friendFigures[indexOfRook].Position = rookTargetPos;
                            friendFigures[indexOfRook].PreviousPositions.Add(rookStartPos);
                            figureSymbol = Board[rookStartPos.X, rookStartPos.Y];
                            Board[rookTargetPos.X, rookTargetPos.Y] = figureSymbol;
                            Board[rookStartPos.X, rookStartPos.Y] = '\u0020';
                        }
                        whoseMoves = GetEnemyColor(WhoseMoves);
                    }
                }
            }
        }

        /// <summary>
        /// Gets enemy's color for given color
        /// </summary>
        /// <param name="color">FigureColoreEnnum</param>
        /// <returns>FigureColorEnum: returns given color's enemy color</returns>
        public static FigureColorEnum GetEnemyColor(FigureColorEnum color)
        {
            if (color == FigureColorEnum.White)
                return FigureColorEnum.Black;
            else
                return FigureColorEnum.White;
        }


        /// <summary>
        /// Gets figure from given position
        /// </summary>
        /// <returns>Figure</returns>        
        /// <param name="p">Point: position for geting figure</param>
        /// <returns>Figure: Returns figure if there are, otherwise returns null </returns>
        public Figure GetFigure(Point p)
        {
            char c = Board[p.X, p.Y];            
            return Figure.CreateFigure(c,p);
        }        

        /// <summary>
        /// Returns all figures from board
        /// </summary>
        /// <returns>List of Figure</returns>
        public List<Figure> GetFigures()
        {
            List<Figure> figures = new ();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (Board[i, j] != '\u0020')
                        figures.Add(GetFigure(new Point(i, j)));
                }
            }
            return figures;
        }

        /// <summary>
        /// Gets all white figures. Figure's position is starting position
        /// </summary>
        /// <returns>List of figure: All white figures</returns>
        static List<Figure> GetAllWhiteFigures()
        {
            var color = FigureColorEnum.White;
            var returnWhiteFigures = new List<Figure>
            {
                new King(color, new Point("E1")),
                new Queen(color, new Point("D1")),
                new Bishop(color, new Point("C1")),
                new Bishop(color, new Point("F1")),
                new Knight(color, new Point("B1")),
                new Knight(color, new Point("G1")),
                new Rook(color, new Point("A1")),
                new Rook(color, new Point("H1"))
            };
            for (int i = 0; i < 8; i++)
            {
                returnWhiteFigures.Add( new Pawn(color, new Point(6, i)));
            }
            return returnWhiteFigures;
        }

        /// <summary>
        /// Gets all black figures. Figure's position is starting position
        /// </summary>
        /// <returns>List of figure: All black figures</returns>
        static List<Figure> GetAllBlackFigures()
        {
            var color = FigureColorEnum.Black;
            var returnBlackFigures = new List<Figure>
            {
                new King(color, new Point("E8")),
                new Queen(color, new Point("D8")),
                new Bishop(color, new Point("C8")),
                new Bishop(color, new Point("F8")),
                new Knight(color, new Point("B8")),
                new Knight(color, new Point("G8")),
                new Rook(color, new Point("A8")),
                new Rook(color, new Point("H8"))
            };
            for (int i = 0; i < 8; i++)
            {
                returnBlackFigures.Add(new Pawn(color, new Point(1, i)));
            }
            return returnBlackFigures;
        }

        /// <summary>
        /// Puts given figure in board
        /// </summary>
        /// <param name="figure">Figure</param>
        public void PutFigure(Figure figure)
        {
            board[figure.Position.X, figure.Position.Y] = figure.GetSymbol();
        }

        /// <summary>
        /// Putes given figures in board
        /// </summary>
        /// <param name="figures">Figure's list</param>
        public void PutFigures(List<Figure> figures)
        {
            for (int i = 0; i < figures.Count; i++)
            {
                PutFigure(figures[i]);
            }
        }

        /// <summary>
        /// Gets enemy figures for given figure
        /// </summary>
        /// <param name="figure">Figure</param>
        /// <returns>List of Figure</returns>
        public List<Figure> GetEnemyFigures(FigureColorEnum color)
        {
            if (color == FigureColorEnum.Black)
                return WhiteFigures;
            else
                return BlackFigures;
        }

        /// <summary>
        /// Gets friend figures for given figure
        /// </summary>
        /// <param name="figure">Figure</param>
        /// <returns>List of Figure</returns>
        public List<Figure> GetFriendFigures(Figure figure)
        {
            return GetFriendFigures(figure.Color);
        }

        /// <summary>
        /// Gets given color's figures
        /// </summary>
        /// <param name="figure">Figure's color</param>
        /// <returns>List of Figure</returns>
        public List<Figure> GetFriendFigures(FigureColorEnum color)
        {
            if (color == FigureColorEnum.White)
                return WhiteFigures;
            else
                return BlackFigures;
        }

        /// <summary>
        /// Gets given color's king
        /// </summary>
        /// <param name="figure">Figure's color</param>
        /// <returns>returns given color's king</returns>
        public King GetOwnKing(FigureColorEnum color)
        {
            List<Figure> friendFigures = GetFriendFigures(color);
            int index = -1;
            for (int i = 0; i < friendFigures.Count; i++)
            {
                if (friendFigures[i] is King)
                {
                    index = i;
                    break;
                }
            }
            return (King)friendFigures[index];
        }

        /// <summary>
        /// gets is given color's king is under attack
        /// </summary>
        /// <param name="color">FigureColorEnum</param>
        /// <returns>returnes true if king is under attack,false otherwise</returns>
        public bool IsCheck(FigureColorEnum color)
        {
            var king = GetOwnKing(color);
            if (king.IsUnderCheck(this))
            {
                return  true;     
            }
            return false;
        }

        /// <summary>
        /// Gets is checkmate for given color's player.
        /// A player is checkmated when on his turn he has no legal move and is in check
        /// </summary>
        /// <param name="color">player's color</param>
        /// <returns>returns true if stalemate, false otherwise</returns>
        public bool IsCheckmate(FigureColorEnum color)
        {
            bool isCheckmate = false;
            if (IsCheck(color))
            {
                isCheckmate = true;
                foreach (Figure figure in GetFriendFigures(color))
                {
                    if (figure.HasMove(this))
                    {
                        return false;
                    }
                }
            }
            return isCheckmate;
        }

        /// <summary>
        /// Cheks is pawn reaches end of board
        /// </summary>
        /// <returns>return true if is time to change pawn to onother figure</returns>
        public bool IsTimeToPromotionPawn()
        {
            bool isTime = false;
            if(Moves.Count==0)
                return isTime;
            Move move = Moves[^1];//Moves.Count - 1
            bool isPawn=false;
            if(move.FigureSymbol=='\u2659'||move.FigureSymbol=='\u265F')
                isPawn=true;
            if (isPawn)
            {
                if(move.EndPos.X==0||move.EndPos.X==7)
                    isTime=true;
            }
            return isTime;
        }


        /// <summary>
        /// checks is there draw
        /// </summary>
        /// <returns>true if draw, false otherwise</returns>
        public bool IsDraw()
        {
            if (Moves.Count >= 12) // when a position is reached (or is about to be reached) at least three times in the same game.
                                  // This repetition is only possible when all the pieces of the same size
                                  // and color are occupying identical squares as they were before, and all the possible moves are also the same. 
            {
                var newState = new ChessboardHistory(Board, WhoseMoves);
                int count = 0;
                for (int i = 0; i < History.Count; i++)
                {
                    if (newState == History[i])
                        count++;
                }
                    if(count==2)
                        return true;
            }
            if (IsStalemate(WhoseMoves)) //when stalemate
                return true;
            if (WhiteFigures.Count == 1 && BlackFigures.Count == 1) //when King vs. king
                return true;
            if (WhiteFigures.Count == 1) // when King and bishop vs. king
            {
                if (BlackFigures.Count == 2)
                {
                    if ((BlackFigures[0] is Bishop) || (BlackFigures[1] is Bishop))
                        return true;
                }
            }
            if (BlackFigures.Count == 1) // when King and bishop vs. king
            {
                if (WhiteFigures.Count == 2)
                {
                    if ((WhiteFigures[0] is Bishop) || (WhiteFigures[1] is Bishop))
                        return true;
                }
            }
            if (WhiteFigures.Count == 1) // when King and knight vs. king
            {
                if (BlackFigures.Count == 2)
                {
                    if ((BlackFigures[0] is Knight) || (BlackFigures[1] is Knight))
                        return true;
                }
            }
            if (BlackFigures.Count == 1) // when King and knight  vs. king
            {
                if (WhiteFigures.Count == 2)
                {
                    if ((WhiteFigures[0] is Knight) || (WhiteFigures[1] is Knight))
                        return true;
                }
            }
            if (WhiteFigures.Count == 2 && BlackFigures.Count == 2) // if King and bishop vs. king and bishop of the same color as the opponent's bishop
            {
                Figure whiteBishop=null;
                if (WhiteFigures[0] is Bishop)
                {
                    whiteBishop = WhiteFigures[0];
                }
                else if (WhiteFigures[1] is Bishop)
                {
                    whiteBishop= WhiteFigures[1];
                }
                if (whiteBishop is not null)
                {
                    Figure blackBishop = null;
                    if (BlackFigures[0] is Bishop)
                    {
                        blackBishop = BlackFigures[0];
                    }
                    else if (BlackFigures[1] is Bishop)
                    {
                        blackBishop = BlackFigures[1];
                    }
                    if (blackBishop is not null)
                        if ((whiteBishop.Position.X + whiteBishop.Position.Y) % 2 == (blackBishop.Position.X + blackBishop.Position.Y) % 2)
                            return true;
                }
            }
            if (Moves.Count >= 50) //If both players make 50 consecutive moves without capturing any pieces or moving any pawns
            {
                bool isNotTrue = false;
                for (int i = 1; i < 51; i++)
                {
                    if (Moves[^i].FigureSymbol == '\u2659' || Moves[^i].FigureSymbol == '\u265F')
                    {
                        isNotTrue = true;
                        break;
                    }
                    if (Moves[^i].EatenFigureSymbol >= 9812 && Moves[^i].EatenFigureSymbol <= 9823)
                    {
                        isNotTrue = true;
                        break;
                    }
                }
                if (!isNotTrue)
                    return true;
            }
            return false;
        }
                
        /// <summary>
        /// Gets is stalemate for given color's player. 
        /// A player is stalemated when on his turn he has no legal move but is not in check
        /// </summary>
        /// <param name="col">player's color</param>
        /// <returns>returns true if stalemate, false otherwise</returns>
        bool IsStalemate(FigureColorEnum color)
        {
            bool isStalemate = false;            
            if (!IsCheck(color))
            {
                isStalemate = true;
                foreach (Figure figure in GetFriendFigures(color))
                {
                    if (figure.HasMove(this))
                    {
                        isStalemate = false;
                        break;
                    }
                }
            }
            return isStalemate;
        }

        /// <summary>
        /// Pawn promotions to given figure
        /// It is a great feat for a Pawn to get all the way to the other side of the board 
        /// (8th rank for white and 1st rank for black). 
        /// Because of this, is a brave little Pawn manages to get that far, they are rewarded with a promotion.
        /// A player that manages to get it’s Pawn all the way to the other side of the board 
        /// may transform that Pawn into any piece they want (most players chose a Queen).
        /// </summary>
        /// <param name="changeTo">string: Figure name for changing pawn</param>
        /// <exception cref="ArgumentOutOfRangeException">throws exeption if given parameter can not be changable figure</exception>
        public void PawnPromotionTo(string changeTo)
        {
            List<Figure> figures = GetEnemyFigures(whoseMoves);
            int index = figures.IndexOf(Moves[^1].EndPos);//Moves.Count - 1
            if (index != -1)
            {
                if (figures[index] is Pawn pawn)
                {
                    Figure figure = changeTo switch
                    {
                        "Queen" => ChangeToQueen(pawn),
                        "Rook" => ChangeToRook(pawn),
                        "Knight" => ChangeToKnight(pawn),
                        "Bishop" => ChangeToBishop(pawn),
                        _ => throw new ArgumentOutOfRangeException($"Oops something wrong, Pawn can not be changed to {changeTo}..." ),
                    };
                    figure.PreviousPositions = pawn.PreviousPositions;
                    PutFigure(figure);
                    figures[index] = figure;
                }
            }
            
        }

        /// <summary>
        /// changes pawn to queen
        /// </summary>
        /// <param name="pawn">Pawn</param>
        /// <returns>Queen: Returns queen in pawn's position with pawn's color and previous moves </returns>
        public static Queen ChangeToQueen(Pawn pawn)
        {
            Queen queen = new (pawn.Color, pawn.Position);
            return queen;                     
        }

        /// <summary>
        /// changes pawn to rook
        /// </summary>
        /// <param name="pawn">Pawn</param>
        /// <returns>Rook: Returns rook in pawn's position with pawn's color and previous moves </returns>
        public static Rook ChangeToRook(Pawn pawn)
        {
            Rook rook = new (pawn.Color, pawn.Position);
            return rook;
        }

        /// <summary>
        /// changes pawn to bishop
        /// </summary>
        /// <param name="pawn">Pawn</param>
        /// <returns>Bishop: Returns bishop in pawn's position with pawn's color and previous moves </returns>
        public static Bishop ChangeToBishop(Pawn pawn)
        {
            Bishop bishop = new (pawn.Color, pawn.Position);
            return bishop;
        }

        /// <summary>
        /// changes pawn to knight
        /// </summary>
        /// <param name="pawn">Pawn</param>
        /// <returns>Knight: Returns knight in pawn's position with pawn's color and previous moves </returns>
        public static Knight ChangeToKnight(Pawn pawn)
        {
            Knight knight = new (pawn.Color, pawn.Position);
            return knight;
        }

        /// <summary>
        /// Sets '\u0020' to each item in board
        /// </summary>
        public void SetEmptyBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    board[i, j] = '\u0020';
                }
            }
        }
    }
}
