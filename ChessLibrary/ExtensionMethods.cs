using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLibrary
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Removes given position figure
        /// </summary>
        /// <param name="figures">list of figures</param>
        /// <param name="point">position for removing</param>
        /// <returns>list of figures</returns>
        public static List<Figure> RemoveFigure(this List<Figure> figures, Point point)
        {
            foreach (Figure f in figures)
            {
                if (f.Position==point)
                {
                    figures.Remove(f);
                    break;
                }
            }
            return figures;
        }

        /// <summary>
        /// Gets index of given position figure
        /// </summary>
        /// <param name="figures">List of figures</param>
        /// <param name="position">Point: position</param>
        /// <returns>returns index of given position figure or -1 if does not find</returns>
        public static int IndexOf(this List<Figure> figures, Point position)
        {
            int index = -1;
            for (int i = 0; i < figures.Count; i++)
            {
                if (figures[i].Position.X == position.X && figures[i].Position.Y == position.Y)
                {
                    index= i;
                    break;
                }
            }
            return index;
        }

        /// <summary>
        /// changes board to string
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public static string ArrayToString(this char[,] board)
        {
            StringBuilder sb = new StringBuilder(64);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    sb.Append(board[i, j]);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// gets board from string
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static char[,] StringToArray(this string s)
        {
            char[,] result = new char[8, 8];
            for (int i = 0; i < 64; i++)
            {
                result[i / 8, i % 8]= s[i];
            }
            return result;
        }
    }
}
