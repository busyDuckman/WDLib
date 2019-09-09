/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WDDevice.Coms;
using WDDevice.LinuxBoards;

namespace WDDevice.ControlTasks.Specific
{
    public class MJpgStreamerTask : TaskBase, IWebCamTask
    {

        public MJpgStreamerTask(ILinuxBoard board) : base()
        {
            Board = board;
        }

        public Bitmap GetImage()
        {
            throw new NotImplementedException();
        }

        public override bool Execute(out string output)
        {
            output = null;
            initShell();

            if (ShellSessionReady)
            {
                State = TaskState.running;
                string devices = shellSession.ExecuteCommand(@"ls /dev/image*", S => S.SecondsSinceLastCom > 1);
            }

            
            return false;
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
