/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.AplicationFramework;
using WD_toolbox.Data.DataStructures;
using WDDevice.LinuxBoards;

namespace WDDevice.Coms
{
    public class PortFinder
    {

        //public string PingCmd {get; protected set;}
        //public Predicate<string> PingCorrect { get; protected set; }

        /// <summary>
        /// Finds an open port that responds to a given command.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="portLow"></param>
        /// <param name="portHigh"></param>
        /// <param name="pingCmd"></param>
        /// <param name="pingCorrect"></param>
        /// <returns></returns>
        public static int FindExistingPort(IPAddress ipAdress,
                                           int portLow, 
                                           int portHigh,
                                           Func<IPAddress, int, INetworkCom> getComDevice,
                                           string pingCmd, 
                                           Predicate<string> pingCorrect,
                                           IStatusProvider status=null)
        {
            //NOTE: status.UpdateStatus is a null safe extension method

            status.UpdateStatus(Status.Starting("Searhing for available port."));

            int thePort = -1;

            //using threads to speed up tha scan
            Parallel.For(portLow, portHigh - 1, (P, S) => 
            { 
                try
                {
                    //is the port open
                    TcpClient client = new TcpClient();
                    client.SendTimeout = 2000;
                    client.ReceiveTimeout = 2000;
                    client.Connect(ipAdress, P);
                    bool open = client.Connected;
                    client.Close();

                    //test to see if it will do what we want
                    if (open)
                    {
                        INetworkCom com = getComDevice(ipAdress, P);
                        if(TestConection(com, pingCmd, pingCorrect, status))
                        {
                            thePort = P;
                            S.Break();
                        }
                    }
                    client.Close();
                    
                }
                catch
                {
                }
            });
         
            status.UpdateStatus((thePort >= 0) ? Status.Done("Found working port: " + thePort):
                                                 Status.Error("Could not find suitable port. "));
            return thePort;
        }


        public  static bool TestConection(INetworkCom connection,
                                    string pingCmd,
                                    Predicate<string> pingCorrect,
                                    IStatusProvider status=null)
        {
            //NOTE: status.UpdateStatus is a null safe extension method

            bool successful = false;
            //TelnetCom tc = new SimpleTelnetCom(Board.IpAdress, TelnetPort);
            Why telnetConected = connection.Connect();

            if (telnetConected)
            {
                Task.Delay(500);
                status.UpdateStatus(Status.Done("Coms available: " + connection));
                status.UpdateStatus(Status.Done("Banner data: " + connection.InputBuffer.ToString()));
                successful = true;

                if (!string.IsNullOrEmpty(pingCmd))
                {
                    string telnetResponce = connection.ExecuteCommand(pingCmd);
                    if (pingCorrect != null)
                    {
                        successful = pingCorrect(telnetResponce);
                        status.UpdateStatus(successful ? Status.Done("Telnet test passed: " + telnetResponce) :
                                                       Status.Error("Telnet test failed: " + telnetResponce));
                    }
                    else
                    {
                        successful = !string.IsNullOrWhiteSpace(telnetResponce);
                        status.UpdateStatus(successful ? Status.Done("Telnet responded to ping command: " + telnetResponce) :
                                                    Status.Error("Telnet didn't respond to ping command: " + telnetResponce));
                    }
                }
            }
            else
            {
                status.UpdateStatus(Status.Error("Telnet un-available: " + telnetConected.Reason ?? "(no information)"));
            }
            return successful;
        }
    }
}
