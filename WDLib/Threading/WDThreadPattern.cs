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
using WD_toolbox.AplicationFramework;

namespace WD_toolbox.Threading
{
    /*public abstract class IWDThreadPattern
    {
        public WDThreadPatternStates State { get; internal set; }
        public virtual void Start() {}
        public abstract void Loop();
        public virtual void Stop() {}
    }*/

    public enum WDThreadPatternStates { Created=0, Alive, Stopping, Stoped };

    public class WDThreadPattern
    {
        /// <summary>
        /// One iteration of the thread loop
        /// </summary>
        /// <returns>True if thread is finished.</returns>
        public delegate bool LoopDelegate();
        //-------------------------------------------------------------------------------------------------------
        // Internal data types
        //-------------------------------------------------------------------------------------------------------
        /*public class WDThreadPattern : WDThreadPattern
        {
            Action onLoop, onStart, onStop;
            public override void OnLoop()
            {
                if(onLoop != null)
                {
                }
            }
        
            public override void OnStart()
            {
 	            throw new NotImplementedException();
            }

            public override void OnStop()
            {
 	            throw new NotImplementedException();
            }
        }*/

        //-------------------------------------------------------------------------------------------------------
        // instance data
        //-------------------------------------------------------------------------------------------------------
        volatile WDThreadPatternStates state;

        public uint DefaultMsToWaitForNormalTermination {get; set;}

        protected Thread executionThread;
        
        public WDThreadPatternStates State
        {
            get { return state; }
        }

        //-------------------------------------------------------------------------------------------------------
        // Constructors et al.,
        //-------------------------------------------------------------------------------------------------------
        protected WDThreadPattern() : this(100)
        {   
        }

        protected WDThreadPattern(uint msToWaitForNormalTermination)
        {
            DefaultMsToWaitForNormalTermination = msToWaitForNormalTermination;
        }

        public static WDThreadPattern fromMethods(LoopDelegate onLoop, Action onStop = null, Action onStart = null)
        {
            return fromMethods(100, onLoop, onStop, onStart);
        }

        public static WDThreadPattern fromMethods(uint msToWaitForNormalTermination,
                                                  LoopDelegate onLoop, Action onStop = null, Action onStart = null)
        {
            WDThreadPattern t = new WDThreadPattern(msToWaitForNormalTermination);
            t.OnLoop = onLoop;
            t.OnStart = onStart;
            t.OnStop = onStop;
            return t;
        }

        //-------------------------------------------------------------------------------------------------------
        // events
        //-------------------------------------------------------------------------------------------------------
        protected Action OnStart;
        protected LoopDelegate OnLoop;
        protected Action OnStop;

        //-------------------------------------------------------------------------------------------------------
        // internal
        //-------------------------------------------------------------------------------------------------------
        private void patternThreadMethod()
        {
            bool finished = false;
            while (!finished)
            {
                WDThreadPatternStates _state = state;
                switch (_state)
                {
                    case WDThreadPatternStates.Created:
                        safeRun(OnStart, _state);
                        state = WDThreadPatternStates.Alive;
                        break;
                    case WDThreadPatternStates.Alive:
                        try
                        {
                            if (OnLoop != null) {
                                if (OnLoop())
                                {
                                    //BUG: wont do the time out stuff
                                    state = WDThreadPatternStates.Stopping;
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            WDAppLog.logError(ErrorLevel.Error,
                                        "IWDThreadPattern loop method threw an exception",
                                        ex.Message);
                        }
                        
                        break;
                    case WDThreadPatternStates.Stopping:
                        safeRun(OnStop, _state);
                        state = WDThreadPatternStates.Stoped;
                        finished = true;
                        break;
                    case WDThreadPatternStates.Stoped:
                        finished = true;
                        break;
                    default:
                        break;
                }
                Thread.SpinWait(1);
            }
        }

        void safeRun(Action action, WDThreadPatternStates state)
        {
            if (action != null)
            {
                try
                {
                    action();
                }
                catch(Exception ex)
                {
                    string[] method = { "Start", "Loop", "Stop", "?" };
                    WDAppLog.logError(ErrorLevel.Error, 
                                        "IWDThreadPattern method threw an exception executing: " + method[(int) state], 
                                        ex.Message);
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------
        // Controll
        //-------------------------------------------------------------------------------------------------------
        public void Start()
        {
            switch (state)
            {
                case WDThreadPatternStates.Stoped:
                    goto case WDThreadPatternStates.Created;
                case WDThreadPatternStates.Created:
                    ThreadStart ts = new ThreadStart(patternThreadMethod);
                    executionThread = new Thread(ts);
                    executionThread.IsBackground = true;
                    state = WDThreadPatternStates.Alive;
                    executionThread.Start();
                    break;
            }
        }
        
        public void Stop()
        {
            Stop(DefaultMsToWaitForNormalTermination);
        }

        public void Stop(uint msToWaitForNormalTermination=100)
        {
            state = WDThreadPatternStates.Stopping;
            DateTime startTime = DateTime.Now;

            do
            {
                Thread.SpinWait(1);
            } while (((DateTime.Now - startTime).TotalMilliseconds <= msToWaitForNormalTermination) &&
                    (state == WDThreadPatternStates.Stopping));

            if (state != WDThreadPatternStates.Stoped)
            {
                executionThread.Abort();
                state = WDThreadPatternStates.Stoped;
                executionThread = null;
            }
        }
    }
}
