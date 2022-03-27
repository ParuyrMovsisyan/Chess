using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessLibrary;

namespace GameLibrary
{
    internal class PossibleMove
    {
        /// <summary>
        /// start position for move
        /// </summary>
        public Point StartPoint;
        /// <summary>
        /// target position for move
        /// </summary>
        public Point EndPoint;
        /// <summary>
        /// if move's result is eating enemy figure EnemyWeight is enememy figure's weight, otherwise is zero
        /// </summary>
        public int EnemyWeight;
        /// <summary>
        /// moving figure's weight
        /// </summary>
        public int MyWeight;
        /// <summary>
        /// if move's result is eating enemy and after that moving figure can be eaten then diferense enemy fgure's weight and moving figure's weigt,
        /// otherwise enemy fgure's weight
        /// </summary>
        public int EffectiveWeight;
    }
}
