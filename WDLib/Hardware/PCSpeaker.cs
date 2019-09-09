/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Runtime.InteropServices;

namespace WD_toolbox.Hardware
{
    /// <summary>
    /// Controls the pcSpeaker.
    /// </summary>
    public class PCSpeaker : IBeeper
    {
        /// <summary>
        /// Beeps to PC speaker..
        /// </summary>
        /// <param name="dwFreq">Frequency of the beep.</param>
        /// <param name="dwDuration">Duration of the beep.</param>
        /// <returns>True if sucessful; otherwise false.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        protected static extern bool Beep(uint dwFreq, uint dwDuration);

        /// <summary>
        /// Plays a noise.
        /// </summary>
        /// <param name="frequency">Frequency in Hz</param>
        /// <param name="duration">Duration in ms</param>
        /// <param name="amplitude">Ignored for PC Speaker.</param>
        public void PlayBeep(uint frequency, uint duration, uint amplitude = 255)
        {
            Beep(frequency, duration);
        }
    }
}
