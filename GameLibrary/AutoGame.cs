using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessLibrary;

namespace GameLibrary
{
    /// <summary>
    /// For playing with computer
    /// </summary>
    public class AutoGame : Game
    {
        /// <summary>
        /// Auto player's color
        /// </summary>
        public readonly FigureColorEnum autoPlayerColor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color">color for autoplayer, default value is white</param>
        public AutoGame(int color = 0) : base()
        {
            autoPlayerColor = (FigureColorEnum)color;
            if (autoPlayerColor == FigureColorEnum.White)
                Think();
        }

        /// <summary>
        /// Creates new game or continues saved game
        /// </summary>
        /// <param name="color">color for autoplayer</param>
        /// <param name="continueSavedGame">bool: if true continues saved game, otherwise cretes new game</param>        
        public AutoGame(int color,bool continueSavedGame) :base(continueSavedGame)
        {
            autoPlayerColor = (FigureColorEnum)color;
            if (autoPlayerColor.ToString() == WhoseMoves)
                Think();
        }


        /// <summary>
        /// Constructor: Creates a game from given board's and sets whose turn
        /// This constructor can throw exception use it in try/catch statement 
        /// </summary>
        /// <param name="color">color for autoplayer</param>
        /// <param name="board">Two dimensional character 8x8 array</param>
        /// <param name="isWhitesTurn">if true then whit's turn, otherwise black's turn</param>
        public AutoGame(int color, char[,] board, bool isWhitesTurn = true) : base(board, isWhitesTurn)
        {
            autoPlayerColor = (FigureColorEnum)color;
            if (autoPlayerColor.ToString() == WhoseMoves)
                Think();
        }

        /// <summary>
        /// thinks what to play
        /// </summary>
        public void Think()
        {
            //Thread.Sleep(2000);
            List<PossibleMove> possibleMoves = new();
            List<Figure> canBeEaten = new();
            Chessboard fakeChessboard;
            PossibleMove myMove = null;
            foreach (Figure figure in Chessboard.GetFriendFigures(autoPlayerColor))
            {
                if (figure.CanBeEaten(Chessboard))
                    canBeEaten.Add(figure);
                List<Point> targetPos = figure.GetAllPossibleMoves(Chessboard);
                if (targetPos.Count > 0)
                {
                    foreach (var pos in targetPos)
                    {
                        PossibleMove possibleMove; 
                        if (figure.CanMove(pos, Chessboard))
                        {
                            possibleMove= new PossibleMove { StartPoint=figure.Position, EndPoint=pos,MyWeight=figure.Weight };
                            if (Chessboard.Board[pos.X, pos.Y] != '\u0020')
                            {
                                possibleMove.EnemyWeight = Chessboard.GetFigure(pos).Weight;
                            }
                            fakeChessboard = new Chessboard(Chessboard);
                            fakeChessboard.Move(figure.Position, pos);
                            if (fakeChessboard.IsCheckmate(fakeChessboard.WhoseMoves))
                            {
                                myMove = possibleMove;
                                break;
                            }
                            possibleMove.EffectiveWeight = figure.CanBeEatenIfMove(pos, Chessboard) ? (possibleMove.EnemyWeight - possibleMove.MyWeight) : possibleMove.EnemyWeight;
                            possibleMoves.Add(possibleMove);
                        }
                    }
                    if (myMove is not null)
                    {
                        break;
                    }
                }
            }
            if (myMove is not null)
            {
                Move(myMove.StartPoint, myMove.EndPoint);
            }
            else if (possibleMoves.Count > 0)
            {
                myMove = ChooseWhatToPlay(possibleMoves);
                if (canBeEaten.Count > 0)
                {
                    int maxWeight = canBeEaten.Max(e => e.Weight);
                    canBeEaten = (from e in canBeEaten
                                  where e.Weight == maxWeight
                                  select e).ToList();
                    if (maxWeight > myMove.EffectiveWeight)
                    {
                        List<PossibleMove> inDangerFigureMoves = new();
                        foreach (var item in canBeEaten)
                        {
                            var posMoves = possibleMoves.Where(e => e.StartPoint == item.Position).ToList();
                            if (posMoves.Count() > 0)
                            {
                                var query1 = (from e in posMoves
                                              where e.EffectiveWeight == posMoves.Max(e => e.EffectiveWeight)
                                              select e).ToList();
                                inDangerFigureMoves.AddRange(query1);
                            }
                        }
                        if (inDangerFigureMoves.Count > 0)
                        {
                            PossibleMove posMove;

                            if (inDangerFigureMoves.Count() > 1)
                            {
                                inDangerFigureMoves = (from e in inDangerFigureMoves
                                                       where e.EffectiveWeight == inDangerFigureMoves.Max(x => x.EffectiveWeight)
                                                       select e).ToList();
                                if (inDangerFigureMoves.Count() > 1)
                                {
                                    Random random = new();
                                    int i = random.Next(0, inDangerFigureMoves.Count());
                                    posMove = inDangerFigureMoves.ElementAt(i);
                                }
                                else
                                {
                                    posMove = inDangerFigureMoves[0];
                                }
                            }
                            else
                            {
                                posMove = inDangerFigureMoves[0];
                            }
                            if (posMove.MyWeight == 100)
                            {
                                Move(myMove.StartPoint, myMove.EndPoint);
                            }
                            else
                            {
                                if (myMove.EnemyWeight - posMove.MyWeight >= posMove.EffectiveWeight)
                                    Move(myMove.StartPoint, myMove.EndPoint);
                                else
                                    Move(posMove.StartPoint, posMove.EndPoint);
                            }                            
                        }
                        else
                        {
                            Move(myMove.StartPoint, myMove.EndPoint);
                        }
                    }
                    else
                    {
                        Move(myMove.StartPoint, myMove.EndPoint);
                    }
                }
                else
                {
                    Move(myMove.StartPoint, myMove.EndPoint);
                }
            }
        }

        /// <summary>
        /// choose a one move to play
        /// </summary>
        /// <param name="possibleMoves"></param>
        /// <returns></returns>
        PossibleMove ChooseWhatToPlay(List<PossibleMove> possibleMoves)
        {            
            Chessboard fakeChessboard ;
            int maxWeight = possibleMoves.Max(e => e.EffectiveWeight);
            if (maxWeight < 2)
            {
                for (int i = 0; i < possibleMoves.Count; i++)
                {
                    fakeChessboard = new Chessboard(Chessboard);
                    var figure = fakeChessboard.GetFigure(possibleMoves[i].StartPoint);
                    if (!figure.CanBeEatenIfMove(possibleMoves[i].EndPoint, fakeChessboard))
                    {
                        fakeChessboard.Move(possibleMoves[i].StartPoint, possibleMoves[i].EndPoint);
                        if (fakeChessboard.IsCheck(fakeChessboard.WhoseMoves))
                            return possibleMoves[i];
                    }
                }
            }            
            if (Chessboard.Moves.Count > 25)
            {                
                if (maxWeight == 0)
                {
                    var query = (from e in possibleMoves
                                where e.EnemyWeight == e.EffectiveWeight && e.MyWeight == 1
                                select e).ToList();
                    if (query.Count > 0)
                    {
                        Random random = new();
                        int i = random.Next(0, query.Count);
                        return query.ElementAt(i);
                    }
                }
            }            
            var query1 = (from e in possibleMoves
                         where e.EffectiveWeight == maxWeight
                          select e).ToList();
            if (query1.Count > 1)
            {
                Random random = new();
                int i = random.Next(0, query1.Count);
                return query1.ElementAt(i);
            }
            else
                return query1.Single();
        }
    }
}
