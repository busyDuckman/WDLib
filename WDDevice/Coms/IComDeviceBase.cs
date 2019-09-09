/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WD_toolbox.Data.DataStructures;
using WDDevice.Device;
using WD_toolbox;
using WD_toolbox.AplicationFramework;
using System.Threading;

namespace WDDevice.Coms
{
    public abstract class IComDeviceBase : IComDevice
    {
        public string LineEnding { get; set; }
        public DeviceState State { get; protected set; }

        //input buffer
        public StringBuilder InputBuffer { get; protected set; }
        public int MaxInputBufferLen { get; set; }
        public ulong TotalBytesSent { get; protected set; }
        public ulong TotalBytesRecived { get; protected set; }
        private StringBuilder currentLine;


        public event OnCharReciveDelegate OnCharRecived;
        public event OnReciveDelegate OnLineRecived;

        public string ConnectionBanner { get; protected set; }

        public IComDeviceBase()
        {
            State = DeviceState.Init;
            LineEnding = "\n";

            InputBuffer = new StringBuilder(1024 * 512); //512kb start... to prevent regular resizing
            MaxInputBufferLen = (1024 * 512) - 1;
            TotalBytesSent = 0;
            TotalBytesRecived = 0;
            currentLine = new StringBuilder();
            ConnectionBanner = "";
        }

        static ConsoleColor[] cols = new ConsoleColor[] {ConsoleColor.Green, ConsoleColor.Yellow };
        static int colsPos = 0;

        protected void doCharRecieved(char c)
        {
            //debug code
            /*
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = cols[(colsPos++) % cols.Length];
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write(c);
            Console.ForegroundColor = old;
            Console.BackgroundColor = ConsoleColor.Black;
            */

            updateDeviceAfterInput(c);
            if(OnCharRecived != null)
            {
                try
                {
                    OnCharRecived(c);
                }
                catch(Exception ex)
                {
                    WDAppLog.logException(ErrorLevel.Error, ex);
                }
            }

            if ((c == '\r') || (c == '\n'))
            {
                if (currentLine.Length > 0)
                {
                    try
                    {
                        if (OnLineRecived != null) //check this now, because it may bee null as the first chars are coming in
                        {
                            OnLineRecived(currentLine.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        WDAppLog.logException(ErrorLevel.Error, ex);
                    }
                }
                currentLine.Clear();
            }
            else
            {
                currentLine.Append(c);
            }
        }

        private void updateDeviceAfterInput(char newChar)
        {
            InputBuffer.Append(newChar);
            if (InputBuffer.Length > MaxInputBufferLen)
            {
                InputBuffer.Remove(0, InputBuffer.Length - MaxInputBufferLen);
            }
            TotalBytesRecived = TotalBytesRecived + 1;
        }

        public abstract int SendLine(string line);
        public abstract int SendChars(string chars);
        public abstract int SendChar(char c);

        public abstract Why Connect();
        public abstract Why DisConnect();

        public virtual void Dispose()
        {
            
        }

        public override string ToString()
        {
            return string.Format("Com Device ({0})", State.GetName());
        }


        public string ExecuteCommand(string command)
        {
            return ExecuteCommand(command, null, S => S.LooksFinished);
        }

        public string ExecuteCommand(string command, string rootPW)
        {
            return ExecuteCommand(command, rootPW, S => S.LooksFinished);
        }

        public string ExecuteCommand(string command, Predicate<CommandStatus> hasFinished)
        
        {
            return ExecuteCommand(command, null, hasFinished);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">command to execute; use NULL to force a read of the current output</param>
        /// <param name="rootPW"> Text to send if prompted for a password.</param>
        /// <param name="hasFinished"></param>
        /// <returns></returns>
        public string ExecuteCommand(string command, string rootPW, Predicate<CommandStatus> hasFinished)
        {
            if (State == DeviceState.Ready)
            {
                ulong intputStart = TotalBytesRecived;
                if (command != null)
                {
                    SendLine(command);
                }

                CommandStatus commandStatus = new CommandStatus("", 0, 0);
                string returnString = "";

                DateTime cmdSent = DateTime.Now;
                DateTime lastCom = cmdSent;
                do
                {
                    //are we stalled at a password prompt?
                    if ((rootPW != null) && (commandStatus.SecondsSinceLastCom > 0.5))
                    {
                        string lastLine = returnString.GetLines().Last().ToLower();
                        if (lastLine.Contains("password"))
                        {
                            SendLine(rootPW);
                        }
                    }

                    //small delay to prevent exsecive / unnessesary work
                    Thread.Sleep(50);

                    //parse output
                    int totalInputChars = (int)(TotalBytesRecived - intputStart);
                    if (totalInputChars > returnString.Length)
                    {
                        lastCom = DateTime.Now;
                        //lock (device.inputBufferLock) //TODO
                        {
                            int len = totalInputChars - returnString.Length;
                            returnString += InputBuffer.ToString(InputBuffer.Length - len, len);
                        }
                    }

                    int secondsSinceLastCom = (int)(DateTime.Now - lastCom).TotalSeconds;
                    int secondsSinceCmdSent = (int)(DateTime.Now - cmdSent).TotalSeconds;
                    commandStatus = new CommandStatus(returnString, secondsSinceLastCom, secondsSinceCmdSent);
                }
                while (!hasFinished(commandStatus));

                return RemoveVT100ControllCodesAndClean(returnString);
            }
            else
            {
                return string.Format("ERROR: Device {0} was not ready", this.ToString());
            }
        }

        private static bool looksLikeATerminalControllCharacter(char c)
        {
            if (char.IsLetterOrDigit(c) || char.IsPunctuation(c))
            {
                return false;
            }
            else if (" \t\r\n".Contains(c)) //just incase
            {
                return false;
            }

            byte[] bytes = ASCIIEncoding.ASCII.GetBytes(new char[]{c});
            if ((bytes != null) && (bytes.Length == 1))
            {
                switch (bytes[0])
                {
                    case 0: return true; //null
                    case 1: return true;
                    case 2: return true;
                    case 3: return true;
                    case 4: return true;
                    case 5: return true;
                    case 6: return true;
                    case 7: return true;
                    case 8: return true;
                    case 9: return true;
                    case 10:         return false;//10 ok
                    case 11: return true;
                    case 12: return true;
                    case 14:        return false;//10 ok//13 ok
                    case 15: return true;
                    case 32: return true;
                    case 127: return true;
                }
                if (bytes[0] >= 128)
                {
                    //borders?
                    return false;
                }
                if (bytes[0] < 32)
                {
                    //asorted non printable stuff
                    return true;
                }
            }

            //err on the side of nonsense
            return true;
        }

        public static string RemoveVT100ControllCodesAndClean(string s)
        {
            //return s;
            char esc = (char)0x1B;

            StringBuilder result = new StringBuilder();
            StringBuilder currentCode = new StringBuilder();

            bool inControllCode = false;
            foreach(char c in s)
            {
                if (inControllCode)
                {
                    if (c == ';')
                    {
                        //special, codes [no idea, not doco found, its just there]
                        if (currentCode.ToString() == "]0")
                        {
                            currentCode.Clear(); //start new code
                            inControllCode = false;
                            continue;
                        }
                    }
                    if ("\r\n".Contains(c))
                    {
                        //bail, we obviously missed something
                        WDAppLog.logError(ErrorLevel.SmallError, "Newline int VT100 Controll code.");
                        inControllCode = false;
                    }
                    else if(c == esc)
                    {
                        currentCode.Clear(); //start new code
                        WDAppLog.logError(ErrorLevel.SmallError, "Unexpected <ESC> in VT100 Controll code.");
                    }
                    else if (char.IsLetter(c))
                    {
                        //nprmal ending?
                        inControllCode = false;
                        if (!"abcdhsujkmpK".Contains(c))
                        {
                            WDAppLog.logError(ErrorLevel.SmallError, string.Format("Unknown VT100 Controll code terminator {0}: {1}", c, currentCode.ToString()+c));
                        }
                    }
                    else if ((currentCode.Length == 0) && "()78".Contains(c))
                    {
                        //font code
                        inControllCode = false;
                    }

                    //still in the code
                    if (inControllCode)
                    {
                        if ((c != esc) && (!looksLikeATerminalControllCharacter(c)))
                        {
                            currentCode.Append(c);
                        }
                    }
                    else
                    {
                        if (currentCode.Length > 10)
                        {
                            WDAppLog.logError(ErrorLevel.SmallError, "Suspiciously long VT100 Controll code: " + currentCode);
                        }
                        currentCode.Clear();
                    }
                }
                else
                {
                    if (c == esc)
                    {
                        inControllCode = true;
                    }
                    else
                    {
                        if (!looksLikeATerminalControllCharacter(c))
                        {
                            if (result.Length >= 1) //fore windows new lines
                            {
                                if ((c == '\n') && (result[result.Length - 1] != '\r')) //\n but \r was not before
                                {
                                    result.Append('\r'); //force windows new line.
                                }
                                else if ((c != '\n') && (result[result.Length - 1] == '\r')) //\r in the buffer, but next char is not a \n
                                {
                                    result.Append('\n');
                                }
                            }
                            result.Append(c);
                        }
                    }
                }
            }

            return result.ToString();
        }
    }
}
