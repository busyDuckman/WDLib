/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WD_toolbox.Maths.Units;

namespace WDLibApplicationFramework.ViewAndEdit2D.Grid
{
    //--------------------------------------------------------------------------------------------------------------------------------------
    // GridUnitMode
    //--------------------------------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// Controls how to display numbers on a grid or ruler.
    /// </summary>
    public abstract class GridUnitMode
    {
        protected readonly static double smidgen = 0.0000001;

        public static GridUnitMode MetricMM = new GridUnitModeMetric(DistanceUnits.MilliMetres, null, 0);
        public static GridUnitMode MetricMetres = new GridUnitModeMetric(DistanceUnits.Metres, null, 2);
        public static GridUnitMode MetricKM = new GridUnitModeMetric(DistanceUnits.KiloMetres, null, 3);
        public static GridUnitMode MetricRulerMarks = new GridUnitModeMetric(DistanceUnits.CentiMetres, DistanceUnits.MilliMetres, 0, true);

        public static GridUnitMode ImpMils = GridUnitModeImperial.fromDecimalPlaces(DistanceUnits.ImpMils, null, 0, false);
        public static GridUnitMode ImpInchFractionFixed16th = GridUnitModeImperial.fromFaction(DistanceUnits.ImpInches, null, 16, false);
        public static GridUnitMode ImpFootAndInchFraction = GridUnitModeImperial.fromFaction(DistanceUnits.ImpFeet, DistanceUnits.ImpInches, 64, true);
        public static GridUnitMode ImpFootAndInchFractionRuler = GridUnitModeImperial.fromFaction(DistanceUnits.ImpFeet, DistanceUnits.ImpInches, 64, true, true);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="rulerTickPrefix">If in ruler mode, and the string is realative to the last actual distance, this is prefixed to the output.</param>
        /// <returns></returns>
        public abstract string GetDistanceString(Distance distance, string rulerTickPrefix="");

        protected virtual DistanceUnits? NextSmallerUnit(DistanceUnits unit)
        {
            switch (unit)
            {
                case DistanceUnits.ImpInches:
                    return null;
                case DistanceUnits.ImpFeet:
                    return DistanceUnits.ImpInches;
                case DistanceUnits.ImpMils:
                    return null;
                case DistanceUnits.ImpMiles:
                    return DistanceUnits.ImpFeet;
                case DistanceUnits.Metres:
                    return DistanceUnits.CentiMetres;
                case DistanceUnits.CentiMetres:
                    return DistanceUnits.MilliMetres;
                case DistanceUnits.MilliMetres:
                    return null;
                case DistanceUnits.KiloMetres:
                    return DistanceUnits.Metres;
                default:
                    return null;
            }
        }

        public abstract int LastUnitModulus { get; }
        public virtual bool UsesFractions { get { return false; } }
        public string InterUnitSeperator { get; protected set; }


        public GridUnitMode()
        {
            InterUnitSeperator = " ";
        }

         
        protected virtual int stickyTruncate(double value)
        {
            double smidgen = 0.0000001;
            return (int)Math.Truncate(value + smidgen);
        }

        protected virtual double stickyAbsoluteRemainder(double value)
        {
            double remainder = Math.Abs(value - stickyTruncate(value));
            return (remainder < smidgen) ? 0 : remainder;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------
    // GridUnitModeCascade
    //--------------------------------------------------------------------------------------------------------------------------------------
    internal class GridUnitModeCascade : GridUnitMode
    {
        protected DistanceUnits[] UnitList;
        protected int decimalPlaces;
        /// <summary>
        /// In ruler mode output goes: 1cm, 1, 2, 3 ... 9, 2cm
        /// </summary>
        protected bool rulerMode;

        protected virtual bool rulerModeHandeledInFormatLastValue { get { return false; } }

        public GridUnitModeCascade(DistanceUnits from, DistanceUnits? to, int decimalPlaces, bool rulerMode)
        {
            List<DistanceUnits> units = new List<DistanceUnits>();
            
            DistanceUnits? current = from;
            do
            {
                units.Add(current.Value);
                current = NextSmallerUnit(current.Value);
            } while ((current != to) && (current != null) && (to != null));

            if (to != null)
            {
                units.Add(to.Value);
            }

            UnitList = units.ToArray();

            this.decimalPlaces = decimalPlaces;
            this.rulerMode = rulerMode;
        }

        public override string GetDistanceString(Distance distance, string rulerTickPrefix = "")
        {
            Distance remaining = distance;
            StringBuilder sb = new StringBuilder();
            if (remaining < Distance.Zero)
            {
                sb.Append("-");
                remaining = -remaining;
            }

            bool startedOutPut = false;
            for (int i = 0; i < UnitList.Length; i++)
            {
                DistanceUnits unit = UnitList[i];
                bool last = i >= (UnitList.Length - 1);

                if (!last)
                {
                    int totalCurrentUnit = stickyTruncate(remaining.In(unit));
                    remaining = Distance.FromDistance(stickyAbsoluteRemainder(remaining.In(unit)), unit);

                    if (totalCurrentUnit > 0)
                    {
                        startedOutPut = true;
                    }

                    //startedOutPut trims leading 0's
                    if (startedOutPut)
                    {
                        sb.AppendFormat("{0}{1}{2}", totalCurrentUnit, DistanceUnitInfo.GetPreferedShortUnitString(unit), InterUnitSeperator??"");
                    }
                }
                else
                {
                    double value = remaining.In(unit);
                    bool isRulerTick;
                    string lastValue = FormatLastValue(value, out isRulerTick);
                    if(isRulerTick)
                    {
                        //ruler ticks are odd, they may be handled by GetDistanceString or
                        //they may be handled by FormatLastValue

                        //in this case, FormatLastValue said it was a tick
                        return (rulerTickPrefix ?? "") + lastValue + DistanceUnitInfo.GetPreferedShortUnitString(unit);
                    }

                    if(!string.IsNullOrWhiteSpace(lastValue))
                    {
                        startedOutPut = true;
                        if (rulerMode && (!rulerModeHandeledInFormatLastValue))
                        {
                            //in this case, the tick is handled here
                            sb.Clear();
                            sb.Append(rulerTickPrefix??"");
                        }
                        sb.AppendFormat("{0}{1} ", lastValue, DistanceUnitInfo.GetPreferedShortUnitString(unit), InterUnitSeperator ?? "");
                    }
                }

            }

            if (!startedOutPut)
            {
                sb.Append("0");
            }

            return sb.ToString().TrimEnd();
        }

        protected virtual string FormatLastValue(double value, out bool isRulerTick)
        {
            isRulerTick = false;
            return value.ToString("N" + decimalPlaces);
        }

        public override int LastUnitModulus 
        { 
            get 
            {
                if (UnitList.Length >= 2)
                {
                    double secondLast = Distance.FromDistance(1, UnitList[UnitList.Length - 2]).In(DistanceUnits.Metres);
                    double last = Distance.FromDistance(1, UnitList[UnitList.Length - 1]).In(DistanceUnits.Metres);
                    return (int)Math.Round(secondLast / last);
                }
                else
                {
                    return 1;
                }
            } 
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------
    // GridUnitModeMetric
    //--------------------------------------------------------------------------------------------------------------------------------------
    internal class GridUnitModeMetric : GridUnitModeCascade
    {
        public GridUnitModeMetric(DistanceUnits from, DistanceUnits? to, int decimalPlaces, bool rulerMode=false)
            : base(from, to, decimalPlaces, rulerMode)
        {

        }

    }

    //--------------------------------------------------------------------------------------------------------------------------------------
    // GridUnitModeImperial
    //--------------------------------------------------------------------------------------------------------------------------------------
    internal class GridUnitModeImperial : GridUnitModeCascade
    {
        int fractionDenominator;
        bool useLowestTerms;

        public override bool UsesFractions { get { return fractionDenominator > 0; } }
        protected override bool rulerModeHandeledInFormatLastValue { get { return true; } }

        protected GridUnitModeImperial(DistanceUnits from, DistanceUnits? to, 
                                       int decimalPlaces, int fractionDenominator, 
                                       bool useLowestTerms, bool rulerMode)
            : base(from, to, decimalPlaces, rulerMode)
        {
            this.fractionDenominator = fractionDenominator;
            this.useLowestTerms = useLowestTerms;
            InterUnitSeperator = "";
        }

        public static GridUnitModeImperial fromFaction(DistanceUnits from, DistanceUnits? to, int fractionDenominator, bool useLowestTerms, bool rulerMode=false)
        {
            return new GridUnitModeImperial(from, to, -1, fractionDenominator, useLowestTerms, rulerMode);
        }

        public static GridUnitModeImperial fromDecimalPlaces(DistanceUnits from, DistanceUnits? to, int decimalPlaces, bool rulerMode = false)
        {
            return new GridUnitModeImperial(from, to, decimalPlaces, - 1, false, rulerMode);
        }

        private static int greatestCommonDivider(int n, int m)
        {
            int gcd, remainder;

            while (n != 0)
            {
                remainder = m % n;
                m = n;
                n = remainder;
            }

            gcd = m;

            return gcd;
        }


        protected override string FormatLastValue(double value, out bool isRulerTick)
        {
            isRulerTick = false;
            StringBuilder sb = new StringBuilder();
            int totalCurrentUnit = stickyTruncate(value);
            double fraction = stickyAbsoluteRemainder(value);

            if (totalCurrentUnit != 0)
            {
                sb.AppendFormat("{0}", totalCurrentUnit);
            }

            int numerator = (int)((fraction + smidgen) * fractionDenominator);
            if (numerator != 0)
            {
                //we are here because we have a fraction

                //any fraction is a ruler tick
                if (rulerMode)
                {
                    isRulerTick = true;
                    sb.Clear();
                }

                //eg 1&1/2'
                if ((sb.Length > 0))
                {
                    sb.Append("&");
                }                

                int actualNumerator = numerator, actualDenominator = fractionDenominator;
                if (useLowestTerms)
                {
                    int gcd = greatestCommonDivider(actualNumerator, actualDenominator);
                    actualNumerator /= gcd;
                    actualDenominator /= gcd;
                }
                sb.AppendFormat("{0}/{1}", actualNumerator, actualDenominator);
            }

            return sb.ToString();
        }
    }
}
