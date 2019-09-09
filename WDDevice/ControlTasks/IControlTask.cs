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
using WD_toolbox.AplicationFramework;
using WDDevice.Coms;

namespace WDDevice.ControlTasks
{
    public enum TaskState {notExecuted, running, waiting, stating, stopping, finished, abortedUser, abortedError, unknown, paused};
    public interface IControlTask : IDisposable, IStatusProvider
    {
        TaskState State { get; }
        bool Execute();
        bool Execute(out string output);
        bool Stop();

        bool Pause();
        bool UnPause();

        double ProgressPercent {get;}

        EventHandler<string> OnMessage { get; set; }

        //IComDevice commandShell { get; set; }
    }
}
