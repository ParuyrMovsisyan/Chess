using ChessLibrary;

namespace GameLibrary
{
    public class Game
    {
        /// <summary>
        /// defines whose moves
        /// </summary>        
        public string WhoseMoves { get { return Chessboard.WhoseMoves.ToString(); } }
        /// <summary>
        /// Property for chessboard
        /// </summary>
        protected Chessboard Chessboard { get; set; }

        /// <summary>
        /// defines is time to promoton pawn
        /// It is a great feat for a Pawn to get all the way to the other side of the board 
        /// (8th rank for white and 1st rank for black). 
        /// Because of this, is a brave little Pawn manages to get that far, they are rewarded with a promotion.
        /// A player that manages to get it’s Pawn all the way to the other side of the board 
        /// may transform that Pawn into any piece they want (most players chose a Queen).
        /// </summary>
        public bool IsTimeToPromotionPawn { get { return Chessboard.IsTimeToPromotionPawn(); } }
        
        /// <summary>
        /// Constructor: Takes a empty chessboard
        /// </summary>
        public Game()
        {
            Chessboard = new Chessboard();
        }              

        /// <summary>
        /// Constructor: Creates a game from given board's.
        /// This constructor can throw exception use it in try/catch statement 
        /// </summary>
        /// <param name="board">Two dimensional character 8x8 array</param>
        public Game(char[,] board, bool isWhitesTurn = true)
        {        
                Chessboard = new Chessboard(board,isWhitesTurn);
        }

        /// <summary>
        /// Checks if the correct figure is selected for the game 
        /// </summary>
        /// <param name="pos">string: position for checking</param>
        /// <returns>bool: true if chosen right coplor's any figure, and false otherwise</returns>
        public bool IsRightChosen(string pos)
        {
            Point positon=new (pos);
            if (Chessboard.Board[positon.X, positon.Y] != '\u0020')
            {
                var color = Chessboard.GetFigure(positon).Color;
                if(color==Chessboard.WhoseMoves)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Pawn promotions to given figure
        /// </summary>
        /// <param name="changeTo">string: Figure name for changing pawn</param>
        public void PawnPromotionTo(string changeTo)
        {
            Chessboard.PawnPromotionTo(changeTo);
        }
        // <summary>
        /// moves figure from current position to target position if it is possible.
        /// </summary>
        /// <param name="startPos">string: current position</param>
        /// <param name="targetPos">string: target position</param>
        public void Move(string startPos, string targetPos)
        {
            Point stPos = new (startPos);
            Point trgPos = new (targetPos);
            Move(stPos, trgPos);
        }

        protected void Move(Point startpos, Point targetPos)
        {
            if (Chessboard.IsInBoard(startpos) && Chessboard.IsInBoard(targetPos))
                Chessboard.Move(startpos, targetPos);
        }

        /// <summary>
        /// Checks is king under threat of capture on their opponent's
        /// </summary>
        /// <returns>true if under check,otherwise false</returns>
        public bool IsUnderCheck()
        {
            return Chessboard.IsCheck(Chessboard.WhoseMoves);               
        }        

        /// <summary>
        /// Checks is player lost
        /// </summary>
        /// <returns>bool</returns>
        public bool IsCheckmate()
        {
            return Chessboard.IsCheckmate(Chessboard.WhoseMoves);
        }

        // <summary>
        /// Gets is stalemate for player. 
        /// A player is stalemated when on his turn he has no legal move but is not in check
        /// </summary>
        /// <returns>returns true if stalemate, false otherwise</returns>
        public bool IsDraw()
        {
            return Chessboard.IsDraw();
        }
              
        /// <summary>
        /// Gets board 
        /// </summary>
        /// <returns>Array of chessboard cells</returns>
        public char[,] GetBoard()
        {
            return Chessboard.Board;
        }
    }
}