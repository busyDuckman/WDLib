/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WD_toolbox;
using WD_toolbox.Data.DataStructures;
using WDDevice.Device;

namespace WDDevice.Coms
{

    public delegate void OnCharReciveDelegate(char c);
    public delegate void OnReciveDelegate(string s);

    public interface IComDevice : IDevice, IDisposable
    {
        int SendLine(string line);
        int SendChars(string chars);
        int SendChar(char c);
        Why Connect();
        Why DisConnect();
        string LineEnding { get; set; }

        event OnCharReciveDelegate OnCharRecived;// { get; set; }
        event OnReciveDelegate OnLineRecived;// { get; set; }

        StringBuilder InputBuffer { get; }
        int MaxInputBufferLen { get; set;  }
        ulong TotalBytesSent { get; }
        ulong TotalBytesRecived { get; }
        string ConnectionBanner { get; }

        string ExecuteCommand(string command);
        string ExecuteCommand(string command, Predicate<CommandStatus> hasFinished);
        string ExecuteCommand(string command, string input);
        string ExecuteCommand(string command, string input, Predicate<CommandStatus> hasFinished);
    }

    public class CommandStatus
    {
        public string LastLine { get; internal set; }
        public string Output { get; internal set; }
        public double SecondsSinceLastCom { get; internal set; }
        public double SecondsSinceCommandSent { get; internal set; }

        public bool LooksFinished { get { return string.IsNullOrEmpty(Output) ? (SecondsSinceCommandSent > 5) : (SecondsSinceLastCom > 2); } }

        public CommandStatus (string output, double secondsSinceLastCom, double secondsSinceCommandSent)
	    {
            Output = output;
            LastLine = output.GetLines(StringExtension.PruneOptions.EmptyLines).LastOrDefault();
            SecondsSinceCommandSent = secondsSinceCommandSent;
            SecondsSinceLastCom = secondsSinceLastCom;
	    }
    }

}
