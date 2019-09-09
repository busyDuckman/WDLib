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
using WDDevice.LinuxBoards;
using WD_toolbox;
using WD_toolbox.Data.DataStructures;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace WDDevice.ControlTasks.Specific
{

    public class TelnetForwardTask : TaskBase, IControlTask
    {
        public enum SerialPortType {ANY=0, COM, USB }

        public int TelnetPort;
        public SerialPortType SerialDeviceType;
        

        public int SerialDeviceNum;
        public SerialPortSettings SerialSettings;
        public int TimeOutSeconds;

        public bool TestFirst { get; protected set; }
        public bool InspectConfigFiles { get; protected set; }


        Predicate<TelnetCom> IsConnectionOk;

        public TelnetForwardTask(ILinuxBoard board, 
                                int telnetPort,
                                SerialPortType serialDeviceType,
                                int serialDeviceNum,
                                SerialPortSettings serialSettings,
                                int timeOutSeconds,
                                Predicate<TelnetCom> IsOk
                                ) : base()
        {
            Board = board;
            SerialSettings = serialSettings;
            SerialDeviceNum = serialDeviceNum;
            SerialDeviceType = serialDeviceType;
            TelnetPort = telnetPort;
            TimeOutSeconds = timeOutSeconds;
            TestFirst = true;
            InspectConfigFiles = true;
            IsConnectionOk = IsOk;
        }

        public bool looksLikeThePort(string device)
        {
            if(device == null)
            {
                return false;
            }

            if((this.SerialDeviceNum < 0) || device.EndsWith(this.SerialDeviceNum.ToString()))
            {
                string lDevice = device.ToLower().Trim();
                switch (this.SerialDeviceType)
                {
                    case SerialPortType.COM:
                        string what = lDevice.TextAfterFirst(@"dev/tty");
                        return ((!string.IsNullOrEmpty(what)) && (char.IsNumber(what[0]))); 
                    case SerialPortType.USB:
                        return lDevice.Contains("usb");
                    case SerialPortType.ANY:
                        return lDevice.Contains("tty");
                    default:
                        return false;
                }  
            }
            return false;
        }

        /// <summary>
        /// Serches ser2net.conf 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="matchSerialPortSettings"></param>
        /// <returns>A port number, or a negative value if one is not found. </returns>
        public int FindExistingSer2NetPort(string device, bool matchSerialPortSettings)
        {
            int port = -1;
            if (initShell())
            {
                string r;
                if (DoSimpleExecute(@"cat /etc/ser2net.conf | grep -i telnet:", out r))
                {
                    string[] configLine = (r??"").GetLines()
                                                 .Where(L => !L.Contains("grep"))
                                                 .Where(L => L.ToLower().Contains("telnet:"))
                                                 .ToArray();

                    this.UpdateStatus(Status.Done(string.Format("Found {0} preset ports fowarded.", configLine.Length)));

                    foreach (string line in configLine)
                    {
                        char[] seperators = ":".ToCharArray();
                        int _port, _timeout, _baud;
                        string _protocol, _device, _settings, _databits, _parity, _stopbit;

                        if(line.ExpectAndRemoveInt(seperators, out _port).
                           ExpectAndRemoveString(seperators, out _protocol).
                           ExpectAndRemoveInt(seperators, out _timeout).
                           ExpectAndRemoveString(seperators, out _device).
                           ExpectAndRemoveString(null, out _settings) != null)
                        {
                            seperators = " ".ToCharArray(); //last token is more settings, with a different token

                            if (_settings.ExpectAndRemoveInt(seperators, out _baud).
                                ExpectAndRemoveString(seperators, out _databits).
                                ExpectAndRemoveString(seperators, out _parity).
                                ExpectAndRemoveString(null, out _stopbit) != null)
                            {
                                //check the baud rate, if we have to
                                if ((!matchSerialPortSettings) || (_baud == this.SerialSettings.BaudRate))
                                {
                                    if (_device == device)
                                    {
                                        this.UpdateStatus(Status.Done("Found suitable setting: " + line));
                                        return _port;
                                    }
                                }
                            }
                        }
                        //string[] tokens = setting.Split(":".ToCharArray()
                    }
                }
            }

            this.UpdateStatus(Status.Done("No preconfigured settings found."));
            return -1;
        }

        public override bool Execute(out string output)
        {
            State = TaskState.running;
            output = null;
            bool successful = false;

            //sheck if conection exists
            if (TestFirst && specifiedTestsPass(TelnetPort))
            {
                this.UpdateStatus(Status.Done("Working connection found. No action taken."));
                State = TaskState.finished;
                return true;
            }

            //begin task via ssh shell
            if (initShell())
            {
                string[] allDevices = GetDevices(LinuxDevices.SerialPort);
                
                    
                //string device = "/dev/ttyUSB0";
                List<string> devices = allDevices
                                        .Where(P => looksLikeThePort(P))
                                        .Select(L => L.Trim())
                                        .Select(T => T.StartsWith(@"/") ? T : (@"/" + T))
                                        .ToList();

                this.UpdateStatus(Status.Starting("Found Serial Devices: " + String.Join(", ", devices).ShortenTextWithEllipsis(50)));

                //
                foreach (string device in devices)
                {
                    if (InspectConfigFiles)
                    {
                        int settingsPort = FindExistingSer2NetPort(device, false);
                        if (specifiedTestsPass(settingsPort))
                        {
                            this.UpdateStatus(Status.Done("Working (preconfigured) connection found. No action taken."));
                            successful = true;
                            TelnetPort = settingsPort; //mutate the class, bit ugly
                            goto __END;
                        }
                    }

                    //Setup the fowarding
                    int cmdMethod = 0; //for (int cmdMethod = 0; cmdMethod < 2; cmdMethod++)
                    {
                        try
                        {
                            bool cmdSuecsess = false;
                            switch (cmdMethod)
                            {
                                case 0:
                                    cmdSuecsess = FowardViaSocat(device, out output);
                                    break;
                                case 1:
                                    cmdSuecsess = FowardViaSer2Net(device, out output);
                                    break;
                            }

                            if (!cmdSuecsess)
                            {
                                this.UpdateStatus(Status.Error("CMD FAiled: " + (output ?? "(NULL)").Trim()));
                                continue;
                            }
                            this.UpdateStatus(Status.Done("CMD Returned: " + (output ?? "(NULL)").Trim()));

                            Thread.Sleep(100);
                            if (specifiedTestsPass(TelnetPort))
                            {
                                successful = true;
                                goto __END;
                            }
                        }
                        catch (Exception ex)
                        {
                            WDAppLog.logException(ErrorLevel.Error, ex);
                        }
                    }
                }
            }
            else
            {
                WDAppLog.logError(ErrorLevel.Warning, string.Format("Could not connect to host {0}", Board));
            }

            __END:
            shellSession.DisConnect();
            this.UpdateStatus(Status.Done("Terminated shell session: " + shellSession));
            State = TaskState.finished;
            return successful;
        }

        protected bool specifiedTestsPass(int port)
        {
            bool pass = false;
            if (IsConnectionOk != null)
            {
                TelnetCom tc = new TelnetCom(Board.IpAdress, port);
                try
                {
                    if (tc.Connect())
                    {
                        if (IsConnectionOk != null)
                        {
                            pass = IsConnectionOk(tc);
                        }
                        else
                        {
                            pass = true;
                        }
                        tc.DisConnect();
                    }
                }
                catch (Exception ex)
                {
                    WDAppLog.logException(ErrorLevel.Error, ex);
                }

                tc.TryDispose();
            }
            else
            {
                pass = true;
            }

            return pass;
        }

        private bool FowardViaSocat(string device, out string output)
        {
            string cmd = string.Format(@"socat TCP-LISTEN:{0},fork,reuseaddr FILE:{2},b{3},raw &",
                                        TelnetPort, TimeOutSeconds, device,
                                        SerialSettings.BaudRate);
            this.UpdateStatus(Status.Starting("Executing: " + cmd));
            return DoSimpleExecute(cmd, out output);

        }

        private bool FowardViaSer2Net(string device, out string output)
        {
            string cmd = string.Format(@"ser2net -C {0}:telnet:{1}:{2}:{3}:8DATABITS:NONE:1STOPBIT",
                                        TelnetPort, TimeOutSeconds, device,
                                        SerialSettings.BaudRate);
            this.UpdateStatus(Status.Starting("Executing: " + cmd));
            output = shellSession.ExecuteCommand(cmd);
            return !(output ?? "").ToLower().Contains("valid parameters are");
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
