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
using WDLibApplicationFramework.ViewAndEdit2D;

namespace WDMedia.Game
{
    public enum DificultyType { Easy = 0, Normal, Hard, VeryHard };

    public enum BasicGameState
    {
        loading = 0, cpuThinking, cpuMoveTimeUp,
        waitingForUserInput, waitingForMultiplayerInput,
        playingAnimation, inRealTimeSimulation, doingDragDrop,
        gameOverWin, gameOverLose,
        pause,
        INVALID
    };

    public interface IGame : IInputView2D
    {
        DificultyType Difficulty { get; }
        string Title { get; }

        bool setDificulty(DificultyType dificulty);

        bool newGame(DificultyType difficulty);

        bool checkRefreshNeeded();

        
    }
}
