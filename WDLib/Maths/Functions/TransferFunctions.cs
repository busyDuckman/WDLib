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
    public sealed class TransferFunction
    {
        public static readonly TransferFunction TrigTransfer_0to1 = new TransferFunction("Trig",
            D => (D < 0) ? 0 : ((D > 1) ? 1 :
                0.5 + (Math.Sin((D-0.5)*Math.PI))*0.5 
            ));

        public static readonly TransferFunction TrigTransfer_r_1to0 = TrigTransfer_0to1.Reverse();
        public static readonly TransferFunction TrigTransfer_i_1to0 = TrigTransfer_0to1.Inverse();
        public static readonly TransferFunction TrigTransfer_ri_0to1 = TrigTransfer_0to1.InverseAndReverse();

        public static readonly TransferFunction LinearTransfer_0to1 = new TransferFunction("Linear",
            D => (D < 0) ? 0 : ((D > 1) ? 1 : 
                D 
            ));


        public static readonly TransferFunction SinTransfer_0to1 = new TransferFunction("Sinus",
            D => (D < 0) ? 0 : ((D > 1) ? 1 :
                (Math.Sin(D * Math.PI * 0.5))
            ));

        public static readonly TransferFunction MyLog10_2Transfer_0to1 = new TransferFunction("min(-log10(-(x-1))/2, 1)",
            D => (D < 0) ? 0 : ((D > 1) ? 1 :
                Math.Min(-Math.Log10(-(D - 1)) / 2, 1)
            ));

        public static readonly TransferFunction MyLog10_4Transfer_0to1 = new TransferFunction("min(-log10(-(x-1))/4, 1)",
            D => (D < 0) ? 0 : ((D > 1) ? 1 :
                Math.Min(-Math.Log10(-(D - 1)) / 4, 1)
            ));


        public static readonly TransferFunction MyLog10_6Transfer_0to1 = new TransferFunction("min(-log10(-(x-1))/6, 1)",
            D => (D < 0) ? 0 : ((D > 1) ? 1 :
                Math.Min(-Math.Log10(-(D - 1)) / 4, 1)
            ));

        public static TransferFunction[] AllStandard_0to1
        {
            get
            {
                return new TransferFunction[] {
                    TrigTransfer_0to1,
                    LinearTransfer_0to1,
                    SinTransfer_0to1,
                    MyLog10_2Transfer_0to1,
                    MyLog10_4Transfer_0to1,
                    MyLog10_6Transfer_0to1
                };
            }
        }


        public Func<double, double> TransferFunc { get; private set; }

        private TransferFunction() { }
        private TransferFunction(string name, Func<double, double> transferFunc)
        {
            this.TransferFunc = transferFunc;
            this.Name = name;
        }

        public string Name { get; private set; }
        public double DoTransfer(double val)
        {
            return this.TransferFunc.Invoke(val);
        }

        public override string ToString()
        {
            return Name;
        }

        public TransferFunction Reverse()
        {
            return new TransferFunction("reverse-"+this.Name, D => this.TransferFunc(-D+1));
        }

        public TransferFunction Inverse()
        {
            return new TransferFunction("inv-" + this.Name, D => -this.TransferFunc(D));
        }

        public TransferFunction InverseAndReverse()
        {
            return new TransferFunction("inv-" + this.Name, D => -this.TransferFunc(-D+1));
        }

        public TransferFunction Adjust(double startPos, double endpos)
        {
            double dist = endpos - startPos;
            return new TransferFunction("custom-" + this.Name, D => this.TransferFunc(((D - startPos) /dist)));
        }
    }

}
