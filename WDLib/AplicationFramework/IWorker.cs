/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WD_toolbox.AplicationFramework
{
    /// <summary>
    /// There is a convenience to writing complicated tasks as if they
    /// were running from a command line.
    /// 
    /// This class gives a command line type environment for complex tasks.
    /// Allows for printing to a log and GUI agnostic status updates.
    /// </summary>
    /// <typeparam name="IN"></typeparam>
    /// <typeparam name="OOUT"></typeparam>
    public interface IWorker<OOUT> : IStatusProvider
    {
        OOUT Process();

        Action<string> OnPrintLine { get; set; }
        Action<string> OnPrint { get; set; }

        Action<OOUT> OnUpdatedData { get; set; }
    }

    public interface IWorker<IN, OOUT> : IWorker<OOUT>
    {
        OOUT Process(IN bmpA);
        OOUT Process();
    }

    public interface IImageWorker:  IWorker<Bitmap, Bitmap>
    {
    }

    public static class IWorkerExtension
    {
        public static void Print(this IImageWorker worker, string text)
        {
            if (worker.OnPrint != null)
            {
                worker.OnPrint(text);
            }
            else
            {
                Console.Write(text);
            }
        }
        public static void Print(this IImageWorker worker, string format, params object[] args)
        {
            Print(worker, string.Format(format, args));
        }

        public static void PrintLine(this IImageWorker worker, string text)
        {
            if (worker.OnPrintLine != null)
            {
                worker.OnPrintLine(text);
            }
            else
            {
                Console.WriteLine(text);
            }
        }
        public static void PrintLine(this IImageWorker worker, string format, params object[] args)
        {
            PrintLine(worker, string.Format(format, args));
        }
    }
}
