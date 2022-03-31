using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp
{
    internal readonly struct Position
    {
        /// <summary>
        /// row
        /// </summary>
        public readonly int X;
        /// <summary>
        /// column
        /// </summary>
        public readonly int Y;
        /// <summary>
        /// Gets chessboard position from mouse position
        /// </summary>
        /// <param name="point">System.Windows.Point: Mouse position</param>
        public Position(Point point)
        {            
            Y = (int)(point.X - 80) / 40;
            X = (int)(point.Y - 64) / 40;
        }

        /// <summary>
        /// Creates position from given string
        /// </summary>
        /// <param name="pos">string for position</param>
        /// <exception cref="ArgumentOutOfRangeException">throws if given string is not chessboard position</exception>
        public Position(string pos)
        {
            X = -1;
            Y = -1;
            if (pos is not null)
            {
                if (pos.Length == 2)
                {
                    if (Enum.TryParse(pos[0].ToString(), out LettersEnum e))
                    {
                        if (int.TryParse(pos[1].ToString(), out int x))
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
        /// changes given position to chessboard position: [A-H] letter and [1-8] digit
        /// </summary>
        /// <returns>chess position </returns>
        public override string ToString()
        {
            string s = string.Empty;
            s += (LettersEnum)Y;
            s += 8 - X;
            return s;
        }
    }
}
