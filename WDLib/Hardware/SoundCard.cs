/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Media;
using System.IO;

namespace WD_toolbox.Hardware
{
    public class SimpleSoundCard : IBeeper
    {
        //adapted from code by John Wein.
        public void PlayBeep(uint amplitude, uint frequency, uint duration)
        {
            double A = ((amplitude * 32768.0) / 1000.0) - 1.0;
            double DeltaFT = (2 * Math.PI * frequency) / 44100.0;
            int Samples = (44100 * (int)duration) / 1000;
            int Bytes = Samples * 4;
            int[] Hdr = new int[] { 0x46464952, 0x24 + Bytes, 0x45564157, 0x20746d66, 0x10, 0x20001, 0xac44, 0x2b110, 0x100004, 0x61746164, Bytes };
            using (MemoryStream MS = new MemoryStream(0x24 + Bytes))
            {
                using (BinaryWriter BW = new BinaryWriter(MS))
                {
                    int length = Hdr.Length - 1;
                    for (int I = 0; I <= length; I++)
                    {
                        BW.Write(Hdr[I]);
                    }
                    int sample = Samples - 1;
                    for (int T = 0; T <= sample; T++)
                    {
                        short Sample = (short)Math.Round((double)(A * Math.Sin(DeltaFT * T)));
                        BW.Write(Sample);
                        BW.Write(Sample);
                    }
                    BW.Flush();
                    MS.Seek(0L, SeekOrigin.Begin);
                    using (SoundPlayer SP = new SoundPlayer(MS))
                    {
                        SP.PlaySync();
                    }
                }
            }
        }

        //adapted from code by John Wein.
        

        //public static void PlayBeeps(int Amplitude, IList<IBeeper.Beep> beeps)
        //{
        //    double A = ((Amplitude * 32768.0) / 1000.0) - 1.0;
        //    int totalSamples = 0;
        //    foreach (Beep beep in beeps)
        //    {
        //        totalSamples += (44100 * beep.Duration) / 1000;
        //    }
        //    int totalBytes = totalSamples * 4;

        //    //Console.WriteLine(Samples);

        //    int[] Hdr = new int[] { 0x46464952, 0x24 + totalBytes, 0x45564157, 0x20746d66, 0x10, 0x20001, 0xac44, 0x2b110, 0x100004, 0x61746164, totalBytes };
        //    using (MemoryStream MS = new MemoryStream(0x24 + totalBytes))
        //    {
        //        using (BinaryWriter BW = new BinaryWriter(MS))
        //        {
        //            int length = Hdr.Length - 1;
        //            for (int I = 0; I <= length; I++)
        //            {
        //                BW.Write(Hdr[I]);
        //            }

        //            foreach (Beep beep in beeps)
        //            {
        //                double DeltaFT = (2 * Math.PI * beep.Freq) / 44100.0;
        //                int Samples = (44100 * beep.Duration) / 1000;
        //                int sample = Samples - 1;
        //                for (int T = 0; T <= sample; T++)
        //                {
        //                    short Sample = (short)Math.Round((double)(A * Math.Sin(DeltaFT * T)));
        //                    BW.Write(Sample);
        //                    BW.Write(Sample);
        //                }
        //            }
        //            BW.Flush();
        //            MS.Seek(0L, SeekOrigin.Begin);
        //            using (SoundPlayer SP = new SoundPlayer(MS))
        //            {
        //                SP.PlaySync();
        //            }
        //        }
        //    }
        //}
    }
}
