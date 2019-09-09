/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDMedia.Game
{
    public interface IHasBasicGameState
    {
        /// <summary>
        /// Retrives the current game state;
        /// </summary>
        BasicGameState GameState { get; }

        /// <summary>
        /// A delay to make the user realise a move has occured.
        /// </summary>
        TimeSpan MinCpuTime { get; set; }

        /// <summary>
        /// The maximum time the cpu can spen thinking.
        /// </summary>
        TimeSpan MaxCpuTime { get; set; }
    }
}
