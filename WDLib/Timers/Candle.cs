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

namespace WD_toolbox.Timers
{
    /// <summary>
    /// Burns for so long (its a stopwatch type thing to time out loops)
    /// </summary>
    public class Candle
    {
        public DateTime StartTime { get; protected set; }
        public TimeSpan Duration { get; protected set; }

        public bool BurntOut { get { return (DateTime.Now - StartTime) >= Duration;} }
        public bool IsBurning { get { return (DateTime.Now - StartTime) < Duration;} }
        
        protected Candle(TimeSpan duration)
        {
            this.Duration = duration;
        }

        public static Candle StartNew(TimeSpan duration)
        {
            Candle c = new Candle(duration);
            c.ReStartAsync();
            return c;
        }

        public static Candle StartNewFromMs(int ms)
        {
            return StartNew(TimeSpan.FromMilliseconds(ms));
        }

        public static Candle StartNewFromSeconds(int sec)
        {
            return StartNew(TimeSpan.FromSeconds(sec));
        }

        public void ReStartAsync()
        {
            StartTime = DateTime.Now;
        }

        public static implicit operator bool(Candle c)
        {
            return c.IsBurning;
        }

    }
}
