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
    public class player
    {
        public player()
        {
            playerName = "Player";
            isCPU = false;
            teamNumber = 0;
            playerScore = 0;
            playerIsInGame = true;
            playerOverIP = false;
        }

        public bool isCPU;
        public int teamNumber;
        public bool isHuman { get { return !isCPU; } }

        public bool playerIsInGame;
        public int playerScore;

        public bool playerOverIP;
        public System.Net.IPAddress ip;

        string playerName;
    }
}
