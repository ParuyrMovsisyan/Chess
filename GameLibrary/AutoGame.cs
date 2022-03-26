﻿using System;
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
    public class AutoGame:Game
    {
        /// <summary>
        /// Auto player's color
        /// </summary>
        public readonly FigureColorEnum autoPlayerColor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="color">color for autoplayer, default value is white</param>
        public AutoGame(int color=0):base()
        {
            autoPlayerColor = (FigureColorEnum)color;
            if (autoPlayerColor == FigureColorEnum.White)
                Think();
        }

        /// <summary>
        /// thinks what to play
        /// </summary>
        public void Think()
        {
            Thread.Sleep(2000);
            List<PossibleMove> possibleMoves = new();
            foreach (Figure figure in Chessboard.GetFriendFigures(autoPlayerColor))
            {
                Point[] targetPos=figure.GetAllPossibleMoves(Chessboard);
                if (targetPos.Length > 0)
                {
                    PossibleMove possibleMove = new();
                    foreach (var pos in targetPos)
                    {
                        if (figure.CanMove(pos, Chessboard))
                        {
                            possibleMove.StartPoint = figure.Position;
                            possibleMove.EndPoint = pos;
                            possibleMove.MyWeight = figure.Weight;
                            if (Chessboard.Board[pos.X, pos.Y] != '\u0020')
                            {
                                possibleMove.EnemyWeight = Chessboard.GetFigure(pos).Weight;
                            }
                            else
                            {
                                possibleMove.EnemyWeight = 0;
                            }
                            possibleMoves.Add(possibleMove);
                        }                        
                    }                    
                }
            }
            if (possibleMoves.Count > 0)
            {
                PossibleMove myMove = ChooseWhatToPlay(possibleMoves);
                Move(myMove.StartPoint, myMove.EndPoint);
            }
        }

        /// <summary>
        /// choose a one move to play
        /// </summary>
        /// <param name="possibleMoves"></param>
        /// <returns></returns>
        static PossibleMove ChooseWhatToPlay(List<PossibleMove> possibleMoves)
        {            
            var query1 = from e in possibleMoves
                         where e.EnemyWeight == possibleMoves.Max(e => e.EnemyWeight)
                         select e;
            if (query1.Count() > 1)
            {
                Random random = new ();
                int i=random.Next(0, query1.Count());
                return query1.ElementAt(i);
            }
            else //if(query1.Count()==1)
                return query1.Single();
        }
    }
}