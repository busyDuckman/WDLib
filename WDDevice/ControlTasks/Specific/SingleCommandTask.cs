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

namespace WDDevice.ControlTasks.Specific
{
    class SingleCommandTask : TaskBase
    {
        public string Command { get; protected set; }
        public string RootPW { get; protected set; }

        public SingleCommandTask(string command) : this(command, null)
        {
        }

        public SingleCommandTask(string command, string rootPW)
        {
            Command = command;
            RootPW = rootPW;
        }

        public override bool Execute(out string output)
        {
            return base.DoSimpleExecute(Command, RootPW, out output);
        }

        public override bool Stop()
        {
            throw new NotImplementedException();
        }

        public override bool Pause()
        {
            throw new NotImplementedException();
        }

        public override bool UnPause()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
        }

    }
}
