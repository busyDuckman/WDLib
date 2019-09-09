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
using WDDevice.Device;

namespace WDDevice.Robot
{
    //public enum RobotState { Error=-1, Init, Working, Ready, Unknown}

    public interface IRobot : IDevice, IStatusProvider
    {
        IComDevice Com {get; set;}
        //RobotState State { get; }
    }

    public class RobotBase : IRobot
    {
        public IComDevice Com { get; set; }
        public DeviceState State { get; protected set; }

        public RobotBase(IComDevice com)
        {
            this.Com = com;
        }

        public RobotBase()
        {

        }

        //IStatusProvider
        public OnStatusChangeDelegate OnStatusChange { get; set; }

        public void sendCommand(string command)
        {
            if(Com != null)
            {
                Com.SendLine(command);
            }
        }

        public void sendCommand(string format, params object[] args)
        {
            sendCommand(string.Format(format, args));
        }

        public override string ToString()
        {
            string comString = (Com != null) ? Com.ToString() : "N/A";
            return "Robot at " + comString + " [" + State + "]";
        }

    }
}
