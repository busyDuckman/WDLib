/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WDMedia.Game
{
    public abstract class StateBasedBasicGame<GAME_STATE> : BasicGame, IHasBasicGameState
    {
        public /*volatile*/ BasicGameState GameState { get; protected set; }
        private TimeSpan minCpuTime;
        private TimeSpan maxCpuTime;

        Thread cpuThinkThread = null;

        #region IBasicGameState Members

        public TimeSpan MinCpuTime
        {
            get { return minCpuTime; }
            set { minCpuTime = value; }
        }

        public TimeSpan MaxCpuTime
        {
            get { return maxCpuTime; }
            set { maxCpuTime = value; }
        }


        #endregion

        public StateBasedBasicGame(IGameHostWindow host)
            : base(host)
        {
            cpuThinkThread = null;
            GameState = BasicGameState.loading;
        }

        public override bool newGame(DificultyType difficulty)
        {
            //if the game does not initialize the gamestate after doing a new game then we do it here.
            if (GameState == BasicGameState.loading)
            {
                GameState = BasicGameState.waitingForUserInput;
            }
            return base.newGame(difficulty);
        }

        /// <summary>
        /// Must handle gmaestate change to  cpuMoveTimeUp
        /// Should alter the game state to the next sate when the move is done.
        /// </summary>
        public virtual void onDoCPUMove()
        {
            if (GameState == BasicGameState.cpuThinking)
            {
                GameState = BasicGameState.waitingForUserInput;
            }
        }

        protected bool expectingUserInput()
        {
            return (GameState == BasicGameState.inRealTimeSimulation) || (GameState == BasicGameState.waitingForUserInput);
        }

        public bool gameOver()
        {
            return (GameState == BasicGameState.gameOverWin) || (GameState == BasicGameState.gameOverLose);
        }



        public void doCpuMove()
        {
            if (cpuThinkThread != null)
            {
                throw new Exception("CPU is making two moves at once...");
            }
            cpuThinkThread = new Thread(new ThreadStart(onDoCPUMove));
            GameState = BasicGameState.cpuThinking;
            DateTime maxFinish = DateTime.Now + maxCpuTime;
            DateTime minFinish = DateTime.Now + maxCpuTime;
            while ((DateTime.Now < minFinish) || (GameState == BasicGameState.cpuThinking))
            {
                Thread.Sleep(5);
                if (DateTime.Now > maxFinish)
                {
                    GameState = BasicGameState.cpuMoveTimeUp;
                }

                //do pause here
                if (GameState == BasicGameState.pause)
                {
                    cpuThinkThread.Suspend();
                    TimeSpan timeLeftTimMax = maxFinish - DateTime.Now;
                    TimeSpan timeLeftTimMin = minFinish - DateTime.Now;

                    while (GameState == BasicGameState.pause)
                    {
                        //give up large priority to other things (for pda or phone)
                        Thread.Sleep(10);
                    }

                    maxFinish = DateTime.Now + timeLeftTimMax;
                    minFinish = DateTime.Now + timeLeftTimMin;

                    Thread.CurrentThread.Priority = ThreadPriority.Normal;
                    cpuThinkThread.Resume();
                }
            }
            cpuThinkThread = null;
            GameState = BasicGameState.waitingForUserInput;
            _refreshNeeded = true;
        }

        public void finishGame(bool userWin)
        {
            GameState = userWin ? BasicGameState.gameOverWin : BasicGameState.gameOverLose;
        }

        public BasicGameState stateBeforePause = BasicGameState.INVALID;
        public void pause()
        {
            if (GameState == BasicGameState.pause)
            {
                return;
            }
            stateBeforePause = GameState;
            GameState = BasicGameState.pause;
        }

        public void unPause()
        {
            if (GameState != BasicGameState.pause)
            {
                return;
            }
            GameState = stateBeforePause;
        }

        public virtual void onUpdateGameState(GAME_STATE newState)
        {
            _refreshNeeded = true;
        }
    }
}
