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
using System.Net;
using System.Net.Sockets;
using WD_toolbox.Data.DataStructures;
using WD_toolbox;
using WDDevice.Device;
using System.Threading;
using WD_toolbox.Timers;
using WD_toolbox.AplicationFramework;

//NOTE:
//.net sockets suck, specifically the support for data recived events
//There is a way to do it at https://msdn.microsoft.com/en-us/library/fx6588te.aspx
//But, I think its just not going to be a reliable solution. 
//For this reason the class polls the output continiously

namespace WDDevice.Coms
{
    public class SimpleTelnetCom : TelnetCom
    {
        Timer pollTimer;

        byte[] buffer = new byte[256];
        object bufferLock = new object();

        public SimpleTelnetCom(IPAddress ipAdress, int port)
            : base(ipAdress, port)
        {
            
        }

        public override Why Connect()
        {
            try
            {
                State = DeviceState.Init;
                endPoint = new IPEndPoint(IpAdress, Port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(endPoint);
                pollTimer = new Timer(pollForInput, null, 0, 10);
                State = DeviceState.Ready;
            }
            catch (Exception ex)
            {
                socket.Close();
                State = DeviceState.Error;
                return Why.FalseBecause(ex.Message);
            }

            return true;
        }

       
        protected void pollForInput(object state)
        {
            if (State == DeviceState.Ready)
            {
                lock (bufferLock)
                {
                    bufferLock = true;
                    try
                    {
                        int numBytes = socket.Receive(buffer);
                        if (numBytes > 0)
                        {
                            char[] chars = Encoding.ASCII.GetChars(buffer, 0, numBytes);
                            for (int i = 0; i < chars.Length; i++)
                            {
                                doCharRecieved(chars[i]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        State = DeviceState.Error;
                        WDAppLog.logException(ErrorLevel.Error, ex);
                        pollTimer.Dispose();
                    }
                }
            }
        }


        public override Why DisConnect()
        {
            pollTimer.Dispose();
            return base.DisConnect();
        }
    }

    public class TelnetCom : IComDeviceBase, INetworkCom
    {
        public IPAddress IpAdress { get; set; }
        public int Port { get; set; }
        protected IPEndPoint endPoint;
        protected Socket socket;

        public static readonly Why FalseBecauseTimedOut = Why.FalseBecause("Conection timed out");
        public static readonly Why FalseBecauseConectionFailed = Why.FalseBecause("Conection failed");

        //SEE NOTE
        internal static ManualResetEvent allDone = new ManualResetEvent(false);

        public TelnetCom(IPAddress ipAdress, int port) : base()
        {
            IpAdress = ipAdress;
            Port = port;
        }

        public override int SendLine(string line)
        {
            line = line.EnsureEndsWith(LineEnding);
            byte[] bytes = Encoding.ASCII.GetBytes(line);
            int s = socket.Send(bytes);
            if (s > 0)
            {
                TotalBytesSent += (ulong)s;
            }
            return s;
        }

        public override int SendChars(string chars)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(chars.ToCharArray());
            int s = socket.Send(bytes);
            if (s > 0)
            {
                TotalBytesSent += (ulong)s;
            }
            return s;
        }

        public override int SendChar(char c)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(new char[] { c });
            int s = socket.Send(bytes);
            if (s > 0)
            {
                TotalBytesSent += (ulong)s;
            }
            return s;
        }

        public override Why Connect()
        {
            if (State == DeviceState.Ready)
            {
                return true; //already connected
            }

            try
            {
                State = DeviceState.Init;
                endPoint = new IPEndPoint(IpAdress, Port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                socket.BeginConnect(endPoint, OnConnect, socket);

                Candle respondTime = Candle.StartNewFromSeconds(5);
                while (State == DeviceState.Init)
                {
                    Thread.Sleep(10);

                    if (!respondTime)
                    {
                        socket.Close();
                        State = DeviceState.Error;
                        return FalseBecauseTimedOut;
                    }
                }

                if (socket.Connected)
                {
                    State = DeviceState.Ready;
                    return true;
                }
                else
                {
                    State = DeviceState.Error;
                    return FalseBecauseConectionFailed;
                }
            }
            catch(Exception ex)
            {
                State = DeviceState.Error;
                return Why.FalseBecause(ex);
            }

            return true;
        }


        protected void OnConnect(IAsyncResult ar)
        {
            // Socket was the passed in object
            Socket sock = (Socket)ar.AsyncState;
            State = DeviceState.Unknown;

            
            // Check if we were sucessfull
            try
            {
                //    sock.EndConnect( ar );
                if (sock.Connected)
                {
                    SetupRecieveCallback(sock);
                    State = DeviceState.Ready;
                }
                else
                {
                    State = DeviceState.Error;
                }

            }
            catch (Exception ex)
            {
                State = DeviceState.Error;
                WDAppLog.logException(ErrorLevel.SmallError, ex);
            }
            
        }

        private byte []    m_byBuff = new byte[1024];    // Recieved data buffer
        public void SetupRecieveCallback( Socket sock )
        {
            try
            {
                AsyncCallback recieveData = new AsyncCallback( OnRecievedData );
                sock.BeginReceive( m_byBuff, 0, m_byBuff.Length, 
                                   SocketFlags.None, recieveData, sock );
            }
            catch( Exception ex )
            {
                WDAppLog.logException(ErrorLevel.SmallError, ex);
            }
        }

        public void OnRecievedData(IAsyncResult ar)
        {
            // Socket was the passed in object
            Socket sock = (Socket)ar.AsyncState;
            if (!sock.Connected)
            {
                return; //something got fowld up in this tangled web
            }

            // Check if we got any data
            try
            {
                int nBytesRec = sock.EndReceive(ar);
                if (nBytesRec > 0)
                {
                    //input
                    char[] chars = Encoding.ASCII.GetChars(m_byBuff, 0, nBytesRec);
                    for (int i = 0; i < chars.Length; i++)
                    {
                        //the base class provides for this
                        doCharRecieved(chars[i]);
                    }

                    // If the connection is still usable restablish the callback
                    SetupRecieveCallback(sock);
                }
                else
                {
                    // If no data was recieved then the connection is probably dead
                    if (State == DeviceState.Ready)
                    {
                        //sock.Shutdown(SocketShutdown.Both);
                        State = DeviceState.NotConnected;
                    }
                    else
                    {
                        WDAppLog.logError(ErrorLevel.SmallError, "Connection lost: " + this.ToString());
                    }
                    //sock.Shutdown(SocketShutdown.Both);
                    //sock.Close();
                    //SetupRecieveCallback(sock);
                }
            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.SmallError, ex);
            }
        }

        public override Why DisConnect()
        {
            //socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            //socket.Disconnect(true);            
            State = DeviceState.NotConnected;
            return true;
        }

        public override void Dispose()
        {
            if (socket.Connected)
            {
                DisConnect();
            }
            socket.TryDispose();
            base.Dispose();
        }

        public override string ToString()
        {
            return string.Format("Telnet Session ({0}, {1}:{2})", State.GetName(), IpAdress, Port);
        }
    }
}
