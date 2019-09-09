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

namespace WD_toolbox.Maths.Functions
{
    public sealed class AdjustableTransferFunction
    {
        private TransferFunction TransferFunction { get; set; }

        private Func<AdjustableTransferFunction, double, double> Func =
            (A, D) =>
            {
                if (D < A.FromValue)
                {
                    return A.FromValue;
                }
                if (D > A.ToValue)
                {
                    return A.ToValue;
                }

                double val = A.TransferFunction.TransferFunc((D - A.Start) * A.xScale);
                return A.FromValue + (val * A.yScale);
            };


        double start;
        double end;
        double fromVaule;
        double toValue;


        double xScale;
        double yScale;

        public AdjustableTransferFunction(TransferFunction transferFunc, double start, double end, double fromValue, double toValue)
        {
            this.Start = start;
            this.End = end;
            this.FromValue = fromValue;
            this.ToValue = toValue;
            this.TransferFunction = transferFunc;
            update();
        }

        private void update()
        {
            double dist = this.End - this.Start;
            this.xScale = (dist != 0) ? (1.0 / dist) : 0;

            dist = this.ToValue - this.FromValue;
            this.yScale = (dist != 0) ? (1.0 / dist) : 0;
        }



        public double Start
        {
            get { return start; }

            set
            {
                start = value;
                update();
            }
        }

        public double End
        {
            get { return end; }

            set
            {
                end = value;
                update();
            }
        }

        public double FromValue
        {
            get { return fromVaule; }

            set
            {
                fromVaule = value;
                update();
            }
        }

        public double ToValue
        {
            get { return toValue; }

            set
            {
                toValue = value;
                update();
            }
        }


    }
}
