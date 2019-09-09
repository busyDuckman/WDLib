/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDDevice.Coms
{
    public class SerialPortSettings
    {
        public Parity Parity { get; protected set; }
        public int BaudRate { get; protected set; }
        public int DataBits { get; protected set; }
        public StopBits StopBits { get; protected set; }

        public SerialPortSettings(int _baudRate, Parity _parity, int _dataBits, StopBits _stopBits)
        {
            Parity = _parity;
            BaudRate = _baudRate;
            DataBits = _dataBits;
            StopBits = _stopBits;
        }

        public SerialPortSettings(int _baudRate) : this(_baudRate, Parity.None, 8, StopBits.One)
        {
        }
    }
}
