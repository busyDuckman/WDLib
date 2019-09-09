/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WD_toolbox;
using WD_toolbox.AplicationFramework;
using WD_toolbox.Data.DataStructures;
using WD_toolbox.Data.Text;
using WDDevice.Device;

namespace WDDevice.Coms
{
    public class SSHCom : IComDeviceBase, INetworkCom, IAuthenticatedSession, IStatusProvider
    {
        public IPAddress IpAdress { get; set; }
        public int Port { get; set; }

        //IAuthenticatedSession
        public string UserName { get; set; }
        public string Password { get; set; }
        public int LoginRetyLimit { get; set; }
        public TimeSpan LoginRetryDelay { get; set; }
        public bool AuthenticationFailed { get; protected set; }

        //IStatusProvider
        public OnStatusChangeDelegate OnStatusChange { get; set; }

        //ssh
        SshClient client;

        StreamWriter swInput;
        StreamReader srOutput;
        ShellStream sshShellStream;

        public SSHCom(IPAddress ipAdress, int port, string userName, string password) : base()
        {
            LineEnding = "\n";
            IpAdress = ipAdress;
            Port = port;
            UserName = userName??"";
            Password = password ?? "";
            LoginRetyLimit = 3;
            LoginRetryDelay = TimeSpan.FromSeconds(3);
            AuthenticationFailed = false;

            client = new SshClient(ipAdress.ToString(), UserName, Password);
            
            State = DeviceState.Init;
        }

        public override Why Connect()
        {
            try
            {
                State = DeviceState.Init;

                client.Connect();
                //sshShellStream = client.CreateShellStream("xterm", 120, 24, 800, 600, 1024 * 10);
                sshShellStream = client.CreateShellStream("xterm", 2048, 24, 800, 600, 1024 * 10);

                sshShellStream.ErrorOccurred += sshShell_ErrorOccurred;
                sshShellStream.DataReceived += sshShellStream_DataReceived;
                swInput = new StreamWriter(sshShellStream);
                srOutput = new StreamReader(sshShellStream);
                swInput.AutoFlush = true;

                State = DeviceState.Ready;
                Thread.Sleep(1000);

                /*
                StringBuilder sb = new StringBuilder();
                while (sshShellStream.DataAvailable)
                {
                    sb.Append(sshShellStream.Read());
                }
                this.ConnectionBanner = sb.ToString();*/
                //this.ConnectionBanner = srOutput.ReadToEnd();
                //srOutput.
                //swInput.WriteLine("ls --help");
                this.ConnectionBanner = srOutput.ReadToEnd();
                
            }
            catch (SshAuthenticationException ex)
            {
                AuthenticationFailed = true;
                State = DeviceState.Error;
                return Why.FalseBecause(ex.Message);
            }
            catch (Exception ex)
            {
                State = DeviceState.Error;
                return Why.FalseBecause(ex.Message);
            }

            return true;
        }

        void sshShellStream_DataReceived(object sender, ShellDataEventArgs e)
        {
            bool useStream = false;
            if (useStream)
            {
                string s = srOutput.ReadToEnd();
                foreach(char c in s)
                {
                    //the base class provides for this
                    doCharRecieved(c);
                }
            }
            else
            {
                char[] chars = Encoding.ASCII.GetChars(e.Data);
                for (int i = 0; i < chars.Length; i++)
                {
                    //the base class provides for this
                    doCharRecieved(chars[i]);
                }

                //srOutput.ReadToEnd();
            }
        }

        void sshShell_ErrorOccurred(object sender, ExceptionEventArgs e)
        {
            string msg = "(NO INFO)";
            if((e != null) && (e.Exception != null) && (!string.IsNullOrWhiteSpace(e.Exception.Message)))
            {
                msg = e.Exception.Message;
            }
            WDAppLog.logError(ErrorLevel.Error, string.Format("Error in SSH connection [{0}]: {1}", this.ToString(), msg));
        }

        public override Why DisConnect()
        {
            try
            {
                sshShellStream.Close();
                sshShellStream.TryDispose();
                client.Disconnect();
                //client.TryDispose();
            }
            catch (ObjectDisposedException ex)
            {

            }

            State = DeviceState.NotConnected;
            return true;
        }

        public override int SendLine(string line)
        {
            //line = line.EnsureEndsWith(LineEnding);
            //byte[] bytes = Encoding.ASCII.GetBytes(line);
            //return socket.Send(bytes);
            swInput.WriteLine(line);// + "\r\n");
            return line.Length;
        }

        public override int SendChars(string chars)
        {
            //byte[] bytes = Encoding.ASCII.GetBytes(chars.ToCharArray());
            //return socket.Send(bytes);
            swInput.Write(chars);
            return chars.Length;
        }

        public override int SendChar(char c)
        {
            //byte[] bytes = Encoding.ASCII.GetBytes(new char[] { c });
            //return socket.Send(bytes);
            swInput.Write(c);
            return 1;
        }

        public override void Dispose()
        {
            if (State != DeviceState.NotConnected)
            {
                DisConnect();
            }
            //socket.TryDispose();
            client.TryDispose();
        }

        public override string ToString()
        {
            return string.Format("SSH Session ({0}, {1}:{2} user={3})", State.GetName(), IpAdress, Port, UserName??"(none)");
        }
        
    }
}
