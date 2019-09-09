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

namespace WDDevice.LinuxBoards
{
    public enum EmbeddedOS { Raspbian, Archlinux, OpenELEC, Pidora, PuppyLinux, Raspbmc, RISCOS, Win10, Android, Other };
    public enum EmbeddedSOC { Broadcom_BCM2835, Broadcom_BCM2836, AllWinner_A20, Other }
    public enum EmbeddedNetworkConnection { Gitabit, Ethernet_10_100, Ethernet_10, None, Other, Wifi_General,
                                Wifi_80211a, Wifi_80211b, Wifi_80211g, Wifi_80211n, Wifi_80211ac, 
                                Wifi_80211ad, Wifi_80211ah}
    public enum EmbeddedCPUArch { ARMv1, ARMv2, ARMv3, ARMv4, ARMv5, ARMv6, ARMv7, ARMv8, Other }
    public enum EmbeddedCPUArchType { CPU_64_Bit, CPU_64_and_32_Bit, CPU_32_Bit, CPU_32_and_16_Bit, CPU_16_Bit, Other}
    public enum EmbeddedGPU { ARM_Mali_400, Broadcom_VideoCore_IV, Other}
    public enum EmbeddedFeatureSuppot { NotSupported, Hardware, Software, Disabled}

    public interface ILinuxBoard : IDevice
    {
        IPAddress IpAdress { get; set; }

        DeviceState State {get;}

        string LoninName {get; set;}
        string LoginPassword {get; set;}

        bool RTCPresent {get;}
        bool RealtimeSystem { get; }
        EmbeddedFeatureSuppot MPEG2_Decode { get; }
        EmbeddedFeatureSuppot MPEG2_Encode { get; }
        EmbeddedFeatureSuppot MPEG4_Decode { get; }
        EmbeddedFeatureSuppot MPEG4_Encode { get; }
        EmbeddedFeatureSuppot H264_Decode { get; }
        EmbeddedFeatureSuppot H264_Encode { get; }

        int CPUSpeedMHz { get; }
        int GPUSpeedMHz { get; }
        int MemSpeedMHz { get; }
        int TotalRamMB { get; }
        int FreeRamMB { get; }
        int TotalStorageMB { get; }
        int FreeStorageMB { get; }
        int CPUCores { get; }

        EmbeddedCPUArch CPUArch { get; }
        EmbeddedCPUArchType CPUArchType { get; }
        EmbeddedGPU GPU { get; }

        IReadOnlyList<EmbeddedNetworkConnection> NetworkConnections { get; }


        EmbeddedOS OperatingSystem { get; }
        string OperatingSystemVer { get; }
        string OperatingSystemName { get; }
        string OperatingKernelVersion { get; }

        EmbeddedSOC SOC { get; }

        IComDevice GetShellSession();
    }
}
