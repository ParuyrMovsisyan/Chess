using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    /// <summary>
    /// Enum for chessboard letters
    /// </summary>
    public enum ChessLettersEnum
    {
        A,
        B,
        C,
        D,
        E,
        F,
        G,
        H
    }
    /// <summary>
    /// position in chessboard. x is digital value, and y is letters enum
    /// </summary>
    public struct Point
    {
        public int X;
        public int Y;

        /// <summary>
        /// Constructor: sets position fields values from input integers
        /// </summary>
        /// <param name="xPos">int: x position</param>
        /// <param name="yPos">int: y position</param>
        public Point(int xPos, int yPos)
        {
            X = xPos;
            Y = yPos;
        }

        /// <summary>
        /// Constructor: sets position fields values from input string, if could not set (0,0)
        /// </summary>
        /// <param name="pos">string: input position</param>
        public Point(string pos)
        {
            X = -1;
            Y = -1;
            if (pos is not null)
            {
                if (pos.Length == 2)
                {                    
                    if (Enum.TryParse(pos[0].ToString(), out ChessLettersEnum e))
                    {
                        if (Int32.TryParse(pos[1].ToString(), out int x))
                        {
                            Y = (int)e;
                            if (x > 0 && x < 9)
                            {
                                X = 8 - x;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Point changes to chees coordinade
        /// </summary>
        /// <returns>string: chess coordinate</returns>
        public override string ToString()
        {
            string s;
            s = (ChessLettersEnum)Y+(8-X).ToString();
            return s;
        }

        /// <summary>
        /// Compares for equality two positions
        /// </summary>
        /// <param name="p1">Point: first point</param>
        /// <param name="p2">Point: second Point</param>
        /// <returns>True if points are the same position, otherwise false</returns>
        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        /// <summary>
        /// Compares for enequality two positions
        /// </summary>
        /// <param name="p1">Point: first point</param>
        /// <param name="p2">Point: second Point</param>
        /// <returns>True if points are not the same position, otherwise false</returns>
        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
    }
}
