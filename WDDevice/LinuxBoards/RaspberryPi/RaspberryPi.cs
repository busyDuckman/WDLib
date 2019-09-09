/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WDDevice.Coms;
using WDDevice.Device;

namespace WDDevice.LinuxBoards.RaspberryPi
{
    public enum RaspberryPiBoards {Model_2, Model_A, Model_B, Model_APlus, Model_BPlus, Bannana, Other};

    public class RaspberryPi : ILinuxBoard
    {
        #region Instance data
        public RaspberryPiBoards BoardType {get; protected set;}
        public IPAddress IpAdress {get; set;}
        public DeviceState State {get; protected set;}

        public string LoninName {get; set;}
        public string LoginPassword {get; set;}

        public EmbeddedOS OperatingSystem { get; protected set; }
        public EmbeddedSOC SOC
        { 
            get
            {
                switch (BoardType)
                {
                    case RaspberryPiBoards.Model_2:
                        return EmbeddedSOC.Broadcom_BCM2836;
                    case RaspberryPiBoards.Model_A:
                        return EmbeddedSOC.Broadcom_BCM2835;
                    case RaspberryPiBoards.Model_B:
                        return EmbeddedSOC.Broadcom_BCM2835;
                    case RaspberryPiBoards.Model_APlus:
                        return EmbeddedSOC.Broadcom_BCM2835;
                    case RaspberryPiBoards.Model_BPlus:
                        return EmbeddedSOC.Broadcom_BCM2835;
                    case RaspberryPiBoards.Bannana:
                        return EmbeddedSOC.AllWinner_A20;
                    case RaspberryPiBoards.Other:
                        return EmbeddedSOC.Other;
                    default:
                        return EmbeddedSOC.Other;
                }
            } 
        }

        public bool RTCPresent { get { return false; } }

        public bool Mpeg2LicencePresent { get; protected set; }

        private int _cpuSpeedMHz; //set to >0 if board is not running at default clock speeds.
        public int CPUSpeedMHz 
        { 
            get 
            { 
                if(_cpuSpeedMHz > 0)
                {
                    return _cpuSpeedMHz;
                }

                switch (BoardType)
	            {
                    case RaspberryPiBoards.Model_2:
                        return 900;
                    case RaspberryPiBoards.Model_A:
                        return 700;
                    case RaspberryPiBoards.Model_B:
                        return 700;
                    case RaspberryPiBoards.Model_APlus:
                        return 700;
                    case RaspberryPiBoards.Model_BPlus:
                        return 700;
                    case RaspberryPiBoards.Bannana:
                        return 700;
                    case RaspberryPiBoards.Other:
                        return 700; //if in doubt
                    default:
                        return 700; //if in doubt
	            }
            } 
        }

        int _gpuSpeedMHz;
        public int GPUSpeedMHz
        { 
            get 
            { 
                if(_gpuSpeedMHz > 0)
                {
                    return _gpuSpeedMHz;
                }

                switch (BoardType)
	            {
                    case RaspberryPiBoards.Bannana:
                        return 700; //if in doubt
                    default:
                        return 250; //if in doubt
	            }
            } 
        }


        int _memSpeedMHz;
        public int MemSpeedMHz
        { 
            get 
            {
                if (_memSpeedMHz > 0)
                {
                    return _memSpeedMHz;
                }

                switch (BoardType)
	            {
                    case RaspberryPiBoards.Model_2:
                        return 450;
                    case RaspberryPiBoards.Model_A:
                        return 400;
                    case RaspberryPiBoards.Model_B:
                        return 400;
                    case RaspberryPiBoards.Model_APlus:
                        return 400;
                    case RaspberryPiBoards.Model_BPlus:
                        return 400;
                    case RaspberryPiBoards.Bannana:
                        return 432; //first models were 480Mhz.. stability was a problem.
                    case RaspberryPiBoards.Other:
                        return 400; //if in doubt
                    default:
                        return 400; //if in doubt
	            }
            } 
        }

        public int TotalRamMB
        {
            get
            {
                switch (BoardType)
                {
                    case RaspberryPiBoards.Model_2:
                        return 1024;
                    case RaspberryPiBoards.Model_A:
                        return 256;
                    case RaspberryPiBoards.Model_B:
                        return 512;
                    case RaspberryPiBoards.Model_APlus:
                        return 256;
                    case RaspberryPiBoards.Model_BPlus:
                        return 512;
                    case RaspberryPiBoards.Bannana:
                        return 1024;
                    case RaspberryPiBoards.Other:
                        return 256; //if in doubt
                    default:
                        return 256; //if in doubt
                }
            }
        }

        public int FreeRamMB { get; protected set; }
        public int TotalStorageMB { get; protected set; }
        public int FreeStorageMB { get; protected set; }
        public int CPUCores
        {
            get
            {
                switch (BoardType)
                {
                    case RaspberryPiBoards.Model_2:
                        return 4;
                    case RaspberryPiBoards.Model_A:
                        return 1;
                    case RaspberryPiBoards.Model_B:
                        return 1;
                    case RaspberryPiBoards.Model_APlus:
                        return 1;
                    case RaspberryPiBoards.Model_BPlus:
                        return 1;
                    case RaspberryPiBoards.Bannana:
                        return 1;
                    case RaspberryPiBoards.Other:
                        return 1; //if in doubt
                    default:
                        return 1; //if in doubt
                }
            }
        }

        public EmbeddedFeatureSuppot MPEG2_Decode { get { return Mpeg2LicencePresent ? EmbeddedFeatureSuppot.Hardware : EmbeddedFeatureSuppot.Disabled; } }
        public EmbeddedFeatureSuppot MPEG2_Encode { get { return Mpeg2LicencePresent ? EmbeddedFeatureSuppot.Hardware : EmbeddedFeatureSuppot.Disabled; } }
        public EmbeddedFeatureSuppot MPEG4_Decode
        {
            get
            {
                switch (BoardType)
                {
                    case RaspberryPiBoards.Bannana:
                        return EmbeddedFeatureSuppot.NotSupported;
                    default:
                        return EmbeddedFeatureSuppot.Hardware;
                }
            }
        }
        public EmbeddedFeatureSuppot MPEG4_Encode
        {
            get
            {
                switch (BoardType)
                {
                    case RaspberryPiBoards.Bannana:
                        return EmbeddedFeatureSuppot.NotSupported;
                    default:
                        return EmbeddedFeatureSuppot.Hardware;
                }
            }
        }
        public EmbeddedFeatureSuppot H264_Decode
        {
            get
            {
                switch (BoardType)
                {
                    case RaspberryPiBoards.Bannana:
                        return EmbeddedFeatureSuppot.NotSupported;
                    default:
                        return EmbeddedFeatureSuppot.Hardware;
                }
            }
        }
        public EmbeddedFeatureSuppot H264_Encode
        {
            get
            {
                switch (BoardType)
                {
                    case RaspberryPiBoards.Bannana:
                        return EmbeddedFeatureSuppot.NotSupported;
                    default:
                        return EmbeddedFeatureSuppot.Hardware;
                }
            }
        }

        public EmbeddedCPUArch CPUArch
        {
            get
            {
                switch (BoardType)
                {
                    case RaspberryPiBoards.Model_2:
                        return EmbeddedCPUArch.ARMv7;
                    case RaspberryPiBoards.Bannana:
                        return EmbeddedCPUArch.ARMv7;
                    case RaspberryPiBoards.Other:
                        return EmbeddedCPUArch.Other;
                    default:
                        return EmbeddedCPUArch.ARMv6; //all other boards
                }
            }
        }

        public EmbeddedCPUArchType CPUArchType
        {
            get
            {
                switch (BoardType)
                {
                    case RaspberryPiBoards.Bannana:
                        return EmbeddedCPUArchType.CPU_64_and_32_Bit;
                    case RaspberryPiBoards.Other:
                        return EmbeddedCPUArchType.Other;
                    default:
                        return EmbeddedCPUArchType.CPU_32_Bit; //all other boards
                }
            }
        }

        public EmbeddedGPU GPU
        {
            get
            {
                switch (BoardType)
                {
                    case RaspberryPiBoards.Bannana:
                        return EmbeddedGPU.ARM_Mali_400;
                    case RaspberryPiBoards.Other:
                        return EmbeddedGPU.Other;
                    default:
                        return EmbeddedGPU.Broadcom_VideoCore_IV; //all other boards
                }
            }
        }

        public IReadOnlyList<EmbeddedNetworkConnection> NetworkConnections
        {
            get
            {
                List<EmbeddedNetworkConnection> connections = new List<EmbeddedNetworkConnection>();
                switch (BoardType)
                {
                    case RaspberryPiBoards.Bannana:
                        connections.Add(EmbeddedNetworkConnection.Gitabit);
                        break;
                    case RaspberryPiBoards.Other:
                        break;
                    default:
                        connections.Add(EmbeddedNetworkConnection.Ethernet_10_100); //all other boards
                        break;
                }

                return connections.AsReadOnly();
            }
        }

        public string OperatingSystemVer { get; protected set; }
        public string OperatingSystemName { get; protected set; }
        public string OperatingKernelVersion { get; protected set; }

        public bool RealtimeSystem { get; protected set; }

        #endregion

        public RaspberryPi(IPAddress ipAddress, RaspberryPiBoards boardType, string userName, string password)
	    {
            State = DeviceState.Init;
            BoardType = boardType;
            IpAdress = ipAddress;

            LoninName = userName;
            LoginPassword = password;

            _cpuSpeedMHz = -1; //default
            _gpuSpeedMHz = -1;
            _memSpeedMHz = -1;

            Mpeg2LicencePresent = false;
            if (boardType == RaspberryPiBoards.Bannana)
            {
                Mpeg2LicencePresent = true;
            }


            FreeRamMB = -1;
            TotalStorageMB = 8 * 1024;
            FreeStorageMB = -1;
            OperatingSystemVer = "?";
            RealtimeSystem = false;

            OperatingSystem = EmbeddedOS.Other;
	    }

        public static RaspberryPi getFromIPAddress(IPAddress ipAddress, string userName, string password)
        {
            return null;
        }

        public IComDevice GetShellSession()
        {
            IComDevice session =  new SSHCom(IpAdress, 22, LoninName, LoginPassword);
            return session;
        }

        public override string ToString()
        {
            string name = "Unknown/Generic PI";
            switch (BoardType)
            {
                case RaspberryPiBoards.Model_2:
                    name = "Raspberry Pi 2";
                    break;
                case RaspberryPiBoards.Model_A:
                    name = "Raspberry Pi, Model A";
                    break;
                case RaspberryPiBoards.Model_B:
                    name = "Raspberry Pi, Model B";
                    break;
                case RaspberryPiBoards.Model_APlus:
                    name = "Raspberry Pi, Model A+";
                    break;
                case RaspberryPiBoards.Model_BPlus:
                    name = "Raspberry Pi, Model B+";
                    break;
                case RaspberryPiBoards.Bannana:
                    name = "Bannana Pi, original";
                    break;
            }

            string ip = (IpAdress != null) ? IpAdress.ToString() : "(N/A)";

            return string.Format("{0} @ {1}", name, ip);
        }

    }
}
