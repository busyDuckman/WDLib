/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WD_toolbox.Data.DataStructures;

namespace WD_toolbox.Maths.Statistics
{
    public class DescriptiveStatisticsSlidingWindow : SlidingWindow<double>
    {
        protected List<double> sortedList;
        protected double SumX = 0;
        protected double SumXSq = 0;

        public double Max { get; protected set; }
        public double Min { get; protected set; }
        public double Count { get; protected set; }

        public double Mean
        {
            get
            {
                return SumX / Count;
            }
        }

        public double StdDev
        {
            get
            {
                return SumXSq / Count - System.Math.Pow(Mean, 2);
            }
        }

        public double Range
        {
            get
            {
                return Max - Min;
            }
        }

        public DescriptiveStatisticsSlidingWindow(int windowSize) : base(windowSize)
        {
            Max = Double.MinValue;
            Min = Double.MaxValue;
            Count = 0;
            sortedList = new List<double>();
        }

        #region SlidingWindow
        public override void Next(double value)
        {
            Count++;
            SumX += value;
            SumXSq += value * value;

            sortedList.Add(value);
            sortedList.Sort();
            base.Next(value);

            while ((WindowSize > 0) & (WindowSizeInUse > WindowSize))
            {
                // Decrement the statistics associated with the dequeued value:
                double exitValue = _list[0];
                _list.RemoveAt(0);
                SumX -= exitValue;
                SumXSq -= exitValue * exitValue;
                Count--;

                //min max
                sortedList.RemoveAt(sortedList.IndexOf(exitValue));
            }

            Min = sortedList[0];
            Max = sortedList[sortedList.Count - 1];
        }

        #endregion


    }
}
