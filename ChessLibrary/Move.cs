using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public class Move
    {
        /// <summary>
        /// Figure's symbol
        /// </summary>
        private char figureSymbol;
        /// <summary>
        /// Property for figureSymbol
        /// </summary>
        public char FigureSymbol
        {
            //get;set;
            get
            {
                
                return figureSymbol;
            }
             set
            {

                int index = (int)value;
                if (index >= 9812 && index <= 9823)
                    figureSymbol = value;
                else
                    throw new ArgumentException("There is no figure for move");
            }
        }
        /// <summary>
        /// move's from position
        /// </summary>
        public Point StartPos{get;}
        /// <summary>
        /// move's to position
        /// </summary>
        public Point EndPos { get; }
        public char EatenFigureSymbol { get; } 
        public Move(char symbol,Point startPoint,Point endPoint, char eatenFigureSymbol)
        {
            FigureSymbol= symbol;
            StartPos= startPoint;
            EndPos= endPoint;
            EatenFigureSymbol= eatenFigureSymbol;
        }
        public static bool operator ==(Move move1,Move move2)
        {
            return move1.figureSymbol == move2.figureSymbol && move1.StartPos == move2.StartPos && move1.EndPos == move2.EndPos;
        }
        public static bool operator !=(Move move1, Move move2)
        {
            return !(move1 == move2);
        }
    }
}
