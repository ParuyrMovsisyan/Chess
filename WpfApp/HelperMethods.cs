using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp
{
    internal class HelperMethods
    {        
        /// <summary>
        /// Gets uri for figure image
        /// </summary>
        /// <param name="c">char: figure unicode symbol</param>
        /// <returns>string: uri for gigure image</returns>
        static string GetImgUri(char c)
        {
            string s = @"\Figures\";
            switch (c)
            {
                case '\u2654':
                    {
                        s += "WKing.gif";
                        break;
                    }
                case '\u265A':
                    {
                        s += "BKing.gif";
                        break;
                    }
                case '\u2655':
                    {
                        s += "WQueen.gif";
                        break;
                    }
                case '\u265B':
                    {
                        s += "BQueen.gif";
                        break;
                    }
                case '\u2656':
                    {
                        s += "WRook.gif";
                        break;
                    }
                case '\u265C':
                    {
                        s += "BRook.gif";
                        break;
                    }
                case '\u2657':
                    {
                        s += "WBishop.gif";
                        break;
                    }
                case '\u265D':
                    {
                        s += "BBishop.gif";
                        break;
                    }
                case '\u2658':
                    {
                        s += "WKnight.gif";
                        break;
                    }
                case '\u265E':
                    {
                        s += "BKnight.gif";
                        break;
                    }
                case '\u2659':
                    {
                        s += "WPawn.gif";
                        break;
                    }
                case '\u265F':
                    {
                        s += "BPawn.gif";
                        break;
                    }
            }
            return s;
        }

        /// <summary>
        /// Creats figure image from given figure symbol and top and left for image position
        /// </summary>
        /// <param name="c">figure sybol</param>
        /// <param name="top">top for image</param>
        /// <param name="left">left for image</param>
        /// <returns></returns>
        public static Image CreateImage(char c, int top, int left)
        {
            Image image = new();
            image.Width = 40;
            image.Height = 45;
            image.Visibility = Visibility.Visible;
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.VerticalAlignment = VerticalAlignment.Top;
            Thickness th = new(left * 40 + 80, top * 40 + 64, 0, 0);
            image.Margin = th;
            string uri = HelperMethods.GetImgUri(c);
            image.Source = new BitmapImage(new Uri(uri, UriKind.Relative));
            return image;
        }

        /// <summary>
        /// gets figure symbol for given figur
        /// </summary>
        /// <param name="name">figur name</param>
        /// <returns>figure's unicode symbol</returns>
        public static char GetFigureSymbol(string name)
        {
            var c = name switch
            {
                "White King" => '\u2654',
                "White Queen" => '\u2655',
                "White Rook" => '\u2656',
                "White Bishop" => '\u2657',
                "White Knight" => '\u2658',
                "White Pawn" => '\u2659',
                "Black King" => '\u265A',
                "Black Queen" => '\u265B',
                "Black Rook" => '\u265C',
                "Black Bishop" => '\u265D',
                "Black Knight" => '\u265E',
                "Black Pawn" => '\u265F',
                _ => '\u0020',
            };
            return c;
        }
    }
}
