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
using WD_toolbox.Data.DataStructures;
using WDDevice.Coms;
using WDDevice.LinuxBoards;
using WD_toolbox;

namespace WDDevice.ControlTasks
{
    public enum LinuxDevices { SerialPort, WebCam, USB, LinePrinter, PS2, Audio, Joystick, ParrallelPort }
    [Flags]
    public enum LinuxFileTypes { None = 0, RegularFile=1, SymbolicLink=2, Executable=4, Socket=8, Pipe=16, Door=32, Directory=64,
                                AllTypes = 127 }
    

    public abstract class TaskBase : IControlTask
    {
        public ILinuxBoard Board { get; protected set; }

        public double ProgressPercent { get { return 0; } }
        public EventHandler<string> OnMessage { get; set; }
        public TaskState State { get; protected set; }
        protected IComDevice shellSession;

        //IStatusProvider
        public OnStatusChangeDelegate OnStatusChange { get; set; }

        public TaskBase()
        {
            State = TaskState.notExecuted;
        }

        protected bool ShellSessionReady 
        {
            get 
            {
                if (shellSession != null)
                {
                    if (shellSession.State == Device.DeviceState.Ready)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool initShell()
        {
            if(!ShellSessionReady)
            {
                this.UpdateStatus(Status.Starting("Aquiring shell session"));
                shellSession = Board.GetShellSession();
                if (shellSession != null)
                {
                    this.UpdateStatus(Status.Starting("Connecting to shell session. This may take a while..."));
                    Why conected = shellSession.Connect();
                    if (conected)
                    {
                        this.UpdateStatus(Status.Done("Shell session banner: " + shellSession.ConnectionBanner.ShortenTextWithEllipsis(60)));
                        this.UpdateStatus(Status.Done("Shell session : " + shellSession));
                        return true;
                    }
                    else
                    {
                        this.UpdateStatus(Status.Error(string.Format("Error [{0}] Could not connect to [{1}]  ", conected.Reason, shellSession)));
                        State = TaskState.abortedError;
                        return false;
                    }
                }
                else
                {
                    this.UpdateStatus(Status.Error("Could not get shell sesion for: " + Board));
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public bool Execute()
        {
            string output;
            return Execute(out output);
        }

        public abstract bool Execute(out string output);

        public abstract bool Stop();
        public abstract bool Pause();
        public abstract bool UnPause();
        public abstract void Dispose();


        public string[] GetDevices(LinuxDevices device)
        {
           if(!initShell())
           {
               return new string[] { };
           }

            switch (device)
            {
                case LinuxDevices.SerialPort:
                    return parseLSCommand(@"/dev/tty*", LinuxFileTypes.RegularFile);
                case LinuxDevices.WebCam:
                    return parseLSCommand(@"/dev/video*", LinuxFileTypes.RegularFile);
                case LinuxDevices.USB:
                    return parseLSCommand(@"/dev/*usb*", LinuxFileTypes.RegularFile).Concat(
                           parseLSCommand(@"/dev/*USB*", LinuxFileTypes.RegularFile)).Distinct().ToArray();
                case LinuxDevices.LinePrinter:
                    return parseLSCommand(@"/dev/lp* ", LinuxFileTypes.RegularFile);
                case LinuxDevices.PS2:
                    return parseLSCommand(@"/dev/psaux*", LinuxFileTypes.RegularFile);
                case LinuxDevices.Audio:
                    return parseLSCommand(@"/dev/dsp*", LinuxFileTypes.RegularFile);
                case LinuxDevices.Joystick:
                    return parseLSCommand(@"/dev/js*", LinuxFileTypes.RegularFile);
                case LinuxDevices.ParrallelPort:
                        return parseLSCommand(@"/dev/pp*", LinuxFileTypes.RegularFile);
                default:
                    return new string[] { };
            }

            //return new string[] { };
        }

        /// <summary>
        /// Gets files in a directory. Executes "ls -F -1 -d" and parses out files as needed
        /// </summary>
        /// <param name="dirAndPattern"> As you would pass it to the ls command.</param>
        /// <param name="types"></param>
        /// <returns></returns>
        public string[] parseLSCommand(string dirAndPattern, LinuxFileTypes types = LinuxFileTypes.AllTypes)
        {
            if(!initShell())
            {
                return new string[] { };
            }

            string allPossibleEndings = @"/>*|=@";
            string endings = "";
            if ((types & LinuxFileTypes.Directory) != 0)
            {
                endings += @"/";
            }
            if ((types & LinuxFileTypes.Door) != 0)
            {
                endings += @">";
            }
            if ((types & LinuxFileTypes.Executable) != 0)
            {
                endings += @"*";
            }
            if ((types & LinuxFileTypes.Pipe) != 0)
            {
                endings += @"|";
            }
            if ((types & LinuxFileTypes.Socket) != 0)
            {
                endings += @"=";
            }
            if ((types & LinuxFileTypes.SymbolicLink) != 0)
            {
                endings += @"@";
            }

            string res = shellSession.ExecuteCommand(string.Format("ls -F -1 -d {0}", dirAndPattern??""));
            if (! ((res.ToLower().Contains("no such file") && (res.Count(C => C == '\r') < 5))) )
            {
                List<string> files = res.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

                //the -F appenda a special character to the end of special file types
                if ((types & LinuxFileTypes.RegularFile) == 0)
                {
                    //no regular files
                    files.RemoveAll(F => !allPossibleEndings.Contains(F.Last()));
                }

                //remove other files (NB: the logic below is correct and the extra list is needed)
                List<char> toRemove = allPossibleEndings.ToList();
                toRemove.RemoveAll((F => endings.Contains(F)));
                files.RemoveAll(F => toRemove.Contains(F.Last()));

                //prune special shars
                char[] trimChars = allPossibleEndings.ToCharArray();
                files = files.Select(delegate(string name) { return name.Trim(trimChars); }).ToList();

                return files.ToArray();
            }
            return new string[] { };
        }

        protected bool DoSimpleExecute(string command)
        {
            string output;
            return DoSimpleExecute(command, null, out output);
        }

        protected bool DoSimpleExecute(string command, out string output)
        {
            return DoSimpleExecute(command, null, out output);
        }

        protected bool DoSimpleExecute(string command, string rootPW, out string output)
        {
            output = "";

            if (initShell())
            {
                output = shellSession.ExecuteCommand(command, rootPW);
                return (GetErrorLevel() == 0);
            }
            return false;
        }

        /// <summary>
        /// Gets error level (0-255), -1 if unknown responce
        /// </summary>
        /// <returns></returns>
        protected int GetErrorLevel()
        {
            if (initShell())
            {
                string errLvl = shellSession.ExecuteCommand("echo $?");
                int[] nums = (errLvl ?? "").ParseAllIntegers();
                if (nums.Length > 0)
                {
                    return nums[0];
                }
            }

            return -1;
        }


    }
}
