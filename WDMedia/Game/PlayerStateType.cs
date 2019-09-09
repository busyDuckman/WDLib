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
    public class PlayerStateType
    {
        public int currentPlayerTurn;

        public List<player> Players;

        public List<player> activePlayers { get { return Players.FindAll(isInGame); } }

        private bool isInGame(player p)
        {
            return p.playerIsInGame;
        }
    }
}
