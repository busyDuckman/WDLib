using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox.Hardware
{
    public interface IBeeper
    {
        /// <summary>
        /// Plays a noise.
        /// </summary>
        /// <param name="frequency">Frequency in Hz</param>
        /// <param name="duration">Duration in ms</param>
        /// <param name="amplitude">How loud to play. (100% defined as 255)</param>
        void PlayBeep(uint frequency, uint duration, uint amplitude = 255);
    }


    public class Beep
    {
        public Beep(uint duration, uint freq)
        {
            this.Duration = duration;
            this.Freq = freq;
        }
        public uint Duration { get; private set; }
        public uint Freq { get; private set; }
    }


    public static class IBeeperExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="beeper"></param>
        /// <param name="beep">Note to play.</param>
        /// <param name="amplitude">How loud to play. (100% defined as 255)</param>
        public static void PlayBeep(this IBeeper beeper, Beep beep, uint amplitude = 255)
        {
            beeper.PlayBeep(beep.Freq, beep.Duration, amplitude);
        }

        /// <summary>
        /// Plays a series of beeps.
        /// </summary>
        /// <param name="beeper"></param>
        /// <param name="beeps">List of beeps to play.</param>
        /// <param name="amplitude">How loud to play. (100% defined as 255)</param>
        public static void PlayBeeps(this IBeeper beeper, List<Beep> beeps, uint amplitude = 255)
        {
            foreach (var beep in beeps)
            {
                beeper.PlayBeep(beep, amplitude);
            }
        }

        /// <summary>
        /// Plays a scale
        /// </summary>
        /// <param name="startFrequency">Starting pitch.</param>
        /// <param name="endFrequency">Ending pitch.</param>
        /// <param name="totalDuration">Total duration for scales playback.</param>
        /// <param name="steps">Amount of notes to play.</param>
        public static void PlayScale(this IBeeper beeper, uint startFrequency, uint endFrequency, uint totalDuration, uint steps, uint amplitude = 255)
        {
            if (steps < 1)
                return;

            int i;
            uint f;
            uint d = (uint)(totalDuration / steps);
            float fs = (endFrequency - startFrequency) / (float)steps;

            for (i = 0; i < steps; i++)
            {
                f = (uint)(startFrequency + (fs * i));
                beeper.PlayBeep(f, d, amplitude);
            }
        }
    }
}
