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
using System.Windows.Threading;
using WD_toolbox.Timers;

namespace WD_toolbox.Threading
{
    public class UpdateFilter : IDisposable
    {
        protected enum UpdateFilterState {Idle, UpdateInProgress, UpdateRequested, UpdateRequestQueued, Terminate};
        Action OnUpdate;
        volatile UpdateFilterState State;

        Dispatcher dispatcher;

        Thread UpdateThread;
        TimeSpan UpdateFeq;

        FloodGate floodGate;

        object stateLock = new object();

        public UpdateFilter(Action onUpdate, TimeSpan maxFreq)
        {
            this.OnUpdate = onUpdate;
            dispatcher = Dispatcher.CurrentDispatcher;
            UpdateThread = new Thread(new ThreadStart(doUpdateThread));
            UpdateThread.Priority = ThreadPriority.BelowNormal;
            UpdateThread.IsBackground = true;
            UpdateThread.Start();
            floodGate = new FloodGate(maxFreq);
            State = UpdateFilterState.Idle;
        }

        public void Update()
        {
            lock (stateLock)
            {
                switch (State)
                {
                    case UpdateFilterState.Idle:
                        State = UpdateFilterState.UpdateRequested;
                        break;
                    case UpdateFilterState.UpdateInProgress:
                        State = UpdateFilterState.UpdateRequestQueued;
                        break;
                    case UpdateFilterState.UpdateRequested:
                        break;
                    case UpdateFilterState.UpdateRequestQueued:
                        break;
                    case UpdateFilterState.Terminate:
                        break;
                }
            }
        }

        protected void doUpdateThread()
        {
            while (State != UpdateFilterState.Terminate)
            {
                Thread.Sleep(10);
                Thread.SpinWait(1);

                switch (State)
                {
                    case UpdateFilterState.Idle:
                        break;
                    case UpdateFilterState.UpdateInProgress:
                        break;
                    case UpdateFilterState.UpdateRequestQueued:
                        break;
                    case UpdateFilterState.UpdateRequested:
                        lock(stateLock)
                        {
                            State = UpdateFilterState.UpdateInProgress;
                        }
                        Task.Run(() => {
                            try
                            {
                                DispatcherOperation op = dispatcher.InvokeAsync(OnUpdate);
                                op.Wait();
                            }
                            finally
                            {
                                switch (State)
	                            {
                                    case UpdateFilterState.UpdateInProgress:
                                        State = UpdateFilterState.Idle;
                                        break;
                                    case UpdateFilterState.UpdateRequestQueued:
                                        State = UpdateFilterState.UpdateRequested;
                                        break;
	                            }
                            }
                        });
                        break;
                }
            }
        }

        public void Dispose()
        {
            State = UpdateFilterState.Terminate;
        }
    }
}
