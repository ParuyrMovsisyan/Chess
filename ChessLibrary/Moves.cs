using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    internal class Moves
    {
        private char figureSymbol;
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
        public Point StartPos{get;}
        public Point EndPos { get; }
        public Moves(char symbol,Point startPoint,Point endPoint)
        {
            FigureSymbol= symbol;
            StartPos= startPoint;
            EndPos= endPoint;
        }

    }
}
