/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WD_toolbox.AplicationFramework;

namespace WD_toolbox.Maths.Units
{
    public enum DistanceUnits { ImpInches = 0, ImpFeet, ImpMils, ImpMiles, Metres, CentiMetres, MilliMetres, KiloMetres }

    /// <summary>
    /// Represents a method to present a unit's unit of mesurement eg cm or " or km or kilometers
    /// </summary>
    public class DistanceUnitInfo
    {
        public string UnitText { get; protected set; }
        public DistanceUnits Unit { get; protected set; }
        public bool PreferedShortFormat { get; protected set; }
        public bool PreferedLongFormat { get; protected set; }

        public static explicit operator string(DistanceUnitInfo info) { return info.UnitText; }

        private static IList<DistanceUnitInfo> unitsTable;

        protected DistanceUnitInfo()
        {
            PreferedLongFormat = false;
            PreferedShortFormat = false;
        }


        public DistanceUnitInfo(DistanceUnits unit, string unitString)
            : this()
        {
            UnitText = unitString;
            Unit = unit;
        }

        public DistanceUnitInfo(DistanceUnits unit, string unitString, bool preferedShortFormat, bool preferedLongFormat)
            : this(unit, unitString)
        {
            PreferedShortFormat = preferedShortFormat;
            PreferedLongFormat = preferedLongFormat;
        }

        static DistanceUnitInfo()
        {
            unitsTable = new List<DistanceUnitInfo>();
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMils, "mils", true, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMils, "thou"));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMils, "mil"));

            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpFeet, "'", true, false));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpFeet, "ft", false, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpFeet, "foot"));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpFeet, "feet"));

            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpInches, "\"", true, false));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpInches, "in", false, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpInches, "inch"));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpInches, "inchs"));

            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMiles, "mi", true, false));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMiles, "mile"));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.ImpMiles, "miles", false, true));
            
            //Metres, CentiMetres, MilliMetres, KiloMetres
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.MilliMetres, "mm", true, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.CentiMetres, "cm", true, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.KiloMetres, "km", true, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.Metres, "m", true, true));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.Metres, "metre"));
            unitsTable.Add(new DistanceUnitInfo(DistanceUnits.Metres, "metres"));

            AddMetricViaPrefix("milli", DistanceUnits.MilliMetres);
            AddMetricViaPrefix("kilo", DistanceUnits.KiloMetres);
            AddMetricViaPrefix("centi", DistanceUnits.CentiMetres);
        }

        private static void AddMetricViaPrefix(string prefix, DistanceUnits unit)
        {
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + "metres"));
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + "metre"));
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + " metre"));
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + "-metre"));
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + " metres"));
            unitsTable.Add(new DistanceUnitInfo(unit, prefix + "-metres"));
        }

        public static DistanceUnits? ParseUnit(string unitText)
        {
            var info = unitsTable.FirstOrDefault(U => U.UnitText == unitText);
            if(info == null)
            {
                info = unitsTable.FirstOrDefault(U => U.UnitText.ToLower().Trim() == unitText.ToLower().Trim());
            }

            return (info != null) ? (DistanceUnits?)info.Unit : null;
        }

        public static String GetPreferedLongUnitString(DistanceUnits unit)
        {
            var info = from U in unitsTable where U.Unit == unit select U;
            var theInfo = info.FirstOrDefault(U => U.PreferedLongFormat) ?? info.First();
            if (theInfo != null)
            {
                return theInfo.UnitText;
            }
            else
            {
                WDAppLog.LogNeverSupposedToBeHere();
                return ("(unnamed unit: " + unit.ToString() + ")");
            }
        }

        public static String GetPreferedShortUnitString(DistanceUnits unit)
        {
            var info = from U in unitsTable where U.Unit == unit select U;
            var theInfo = info.FirstOrDefault(U => U.PreferedShortFormat) ?? info.First();
            if (theInfo != null)
            {
                return theInfo.UnitText;
            }
            else
            {
                WDAppLog.LogNeverSupposedToBeHere();
                return ("(unnamed unit: " + unit.ToString() + ")");
            }
        }
    }

    public struct Distance : IEquatable<Distance>, IComparable<Distance>
    {
        //----------------------------------------------------------------------
        // internal data types
        //----------------------------------------------------------------------    

       
        private static Regex distanceExpresion = new Regex(@"(?<section>(?<num>[\d\.]+) *(?<fraction>\/ *(?<div>[\d]+))? *(?<unit>[^\d]+) *)+");

        public enum DistanceUnitSystem { Imperial, Metric}

        //----------------------------------------------------------------------
        // constants
        //----------------------------------------------------------------------
        static readonly double InchesPerMetre = 39.3700787;
        static readonly double FeetPerMetre = 3.2808399;
        static readonly double MilsPerMetre = 39370.0787;
        static readonly double MilesPereMetre = 0.000621371192;

        static readonly double CentiMetresPerMetre = 100;
        static readonly double MilliMetresPerMetre = 1000;
        static readonly double KiloMetresPerMetre = 0.001;

        public static Distance Zero { get { return Distance.FromMetres(0); } }

        //----------------------------------------------------------------------
        // Instace data
        //----------------------------------------------------------------------
        double distanceInMetres;


        //----------------------------------------------------------------------
        // Factory acessors
        //----------------------------------------------------------------------
        public double ImpFeet
        {
            get { return distanceInMetres * FeetPerMetre; }
            set { distanceInMetres = value / FeetPerMetre; }
        }

        public double ImpInchs
        {
            get { return distanceInMetres * InchesPerMetre; }
            set { distanceInMetres = value / InchesPerMetre; }
        }

        public double ImpMils
        {
            get { return distanceInMetres * MilsPerMetre; }
            set { distanceInMetres = value / MilsPerMetre; }
        }

        public double ImpMiles
        {
            get { return distanceInMetres * MilesPereMetre; }
            set { distanceInMetres = value / MilesPereMetre; }
        }

        public double Metres
        {
            get { return distanceInMetres; }
            set { distanceInMetres = value; }
        }

        public double CentiMetres
        {
            get { return distanceInMetres * CentiMetresPerMetre; }
            set { distanceInMetres = value / CentiMetresPerMetre; }
        }

        public double MilliMetres
        {
            get { return distanceInMetres * MilliMetresPerMetre; }
            set { distanceInMetres = value / MilliMetresPerMetre; }
        }

        public double KiloMetres
        {
            get { return distanceInMetres * KiloMetresPerMetre; }
            set { distanceInMetres = value / KiloMetresPerMetre; }
        }

        /*private Distance()
        {
            distanceInMetres = 0;
        }*/

        



        public static Distance FromMetres(double val)
        {
            Distance d = new Distance();
            d.Metres = val;
            return d;
        }

        public static Distance FromImpFeet(double val)
        {
            Distance d = new Distance();
            d.ImpFeet = val;
            return d;
        }

        public static Distance FromImpInchs(double val)
        {
            Distance d = new Distance();
            d.ImpInchs = val;
            return d;
        }

        public static Distance FromImpMils(double val)
        {
            Distance d = new Distance();
            d.ImpMils = val;
            return d;
        }

        public static Distance FromImpMiles(double val)
        {
            Distance d = new Distance();
            d.ImpMiles = val;
            return d;
        }

        public static Distance FromCentiMetres(double val)
        {
            Distance d = new Distance();
            d.CentiMetres = val;
            return d;
        }

        public static Distance FromMilliMetres(double val)
        {
            Distance d = new Distance();
            d.MilliMetres = val;
            return d;

        }

        public static Distance FromKiloMetres(double val)
        {
            Distance d = new Distance();
            d.KiloMetres = val;
            return d;

        }
        
        public static Distance FromDistance(double val, DistanceUnits units)
        {
            Distance d = new Distance();
            d.Metres = val / GetPerMetreConversion(units);
            return d;
        }

        internal Distance Absolute()
        {
            return Distance.FromMetres(Math.Abs(this.distanceInMetres));
        }

        public static double GetPerMetreConversion(DistanceUnits units)
        {
            switch (units)
            {
                case DistanceUnits.ImpInches:
                    return InchesPerMetre;
                case DistanceUnits.ImpFeet:
                    return FeetPerMetre;
                case DistanceUnits.ImpMils:
                    return MilsPerMetre;
                case DistanceUnits.ImpMiles:
                    return MilesPereMetre;
                case DistanceUnits.Metres:
                    return 1;
                case DistanceUnits.CentiMetres:
                    return CentiMetresPerMetre;
                case DistanceUnits.MilliMetres:
                    return MilliMetresPerMetre;
                case DistanceUnits.KiloMetres:
                    return KiloMetresPerMetre;
            }

            WDAppLog.LogNeverSupposedToBeHere();
            return 1;//wtf
        }

        public double In(DistanceUnits units)
        {
            return distanceInMetres * GetPerMetreConversion(units);
        }

        public static bool Parse(string text, out Distance distance)
        {
            if(!string.IsNullOrWhiteSpace(text))
            {
                Regex distanceExpresion = new Regex(@" *(?<num>[\d\.]+) *(?<fraction>\/ *(?<div>[\d]+))? *(?<unit>[^\d]+) *");
                Match match = distanceExpresion.Match(text);
                int components = 0;
                Distance temp = Distance.Zero;

                while ((match != null) && (match.Success))
                {
                    components++;

                    string num = match.Groups["num"].Value;
                    string div = match.Groups["div"].Value;
                    string unit = match.Groups["unit"].Value;

                    double value = double.Parse(num);
                    if(!string.IsNullOrWhiteSpace(div))
                    {
                        double divisor = double.Parse(div);
                        if(divisor != 0)
                        {
                            value = value / divisor;
                        }
                        else
                        {
                            value = 0;
                        }
                    }

                    //match.Captures
                    temp += Distance.fromNumberAndString(value, unit);

                    //next match
                    match = match.NextMatch();
                }

                if(components > 0)
                {
                    distance = temp;
                    return true;
                }
            }

            distance = Distance.Zero;
            return false;
        }

        private static Distance fromNumberAndString(double value, string unitText)
        {
            DistanceUnits? unit = DistanceUnitInfo.ParseUnit(unitText);

            if (unit != null)
            {
                return Distance.FromDistance(value, unit.Value);
            }
            throw new InvalidOperationException("unknown unit: " + unit);
        }

        //-----------------------------------------------------------------------------------------------
        // Object
        //-----------------------------------------------------------------------------------------------
        public override int GetHashCode()
        {
 	         return Metres.GetHashCode();
        }

        public override string ToString()
        {
 	         return string.Format("{0}m", Metres);
        }

        public string ToString(DistanceUnits unit, bool longVersion)
        {
            double num = this.In(unit);
            string units = longVersion ? DistanceUnitInfo.GetPreferedLongUnitString(unit) : DistanceUnitInfo.GetPreferedShortUnitString(unit);
            return string.Format("{0} {1}", num, units);
        }

        //-----------------------------------------------------------------------------------------------
        // IEquatable<ProformaClass>
        //-----------------------------------------------------------------------------------------------
        public override bool Equals(object obj)
        {
            return (obj is Distance) ? (this == (Distance)obj) : false;
        }

        public bool Equals(Distance other)
        {
            return this == other;
        }
        //-----------------------------------------------------------------------------------------------
        // IComparable<>
        //-----------------------------------------------------------------------------------------------
        public int CompareTo(Distance other)
        {
            //baisly this is (other != nuul), except it safe gards against a infinite recursion possible
            //when overloaded equalty operators that call CompareTo
            if (object.ReferenceEquals(other, null))
            {
                //By definition, any object compares greater than (or follows) Nothing, and two null references compare equal to each other.
                return 1;
            }

            return this.Metres.CompareTo(other.Metres);
        }


        #region unary operators
        public static Distance operator -(Distance i)
        {
            return Distance.FromMetres(-i.Metres);
        }

        public static Distance operator +(Distance i)
        {
            return Distance.FromMetres(+i.Metres);
        }
        #endregion

        #region binary operators (Distance and Distance)
        public static Distance operator +(Distance a, Distance b)
        {
            return Distance.FromMetres(a.Metres + b.Metres);
        }

        public static Distance operator -(Distance a, Distance b)
        {
            return Distance.FromMetres(a.Metres - b.Metres);
        }

        public static Distance operator *(Distance a, Distance b)
        {
            return Distance.FromMetres(a.Metres * b.Metres);
        }

        public static Distance operator /(Distance a, Distance b)
        {
            return Distance.FromMetres(a.Metres / b.Metres);
        }

        public static Distance operator %(Distance a, Distance b)
        {
            return Distance.FromMetres(a.Metres % b.Metres);
        }
        #endregion

        #region comparison operators (Distance and Distance)
        public static bool operator ==(Distance a, Distance b)
        {
            return a.Metres == b.Metres;
        }

        public static bool operator !=(Distance a, Distance b)
        {
            return a.Metres != b.Metres;
        }

        public static bool operator <(Distance a, Distance b)
        {
            return a.Metres < b.Metres;
        }

        public static bool operator >(Distance a, Distance b)
        {
            return a.Metres > b.Metres;
        }

        public static bool operator <=(Distance a, Distance b)
        {
            return a.Metres <= b.Metres;
        }

        public static bool operator >=(Distance a, Distance b)
        {
            return a.Metres >= b.Metres;
        }
        #endregion


        public string ToStringStrictImperialFraction(int fractionBase)
        {
            int whole = (int)Math.Truncate(ImpInchs);
            double fraction = Math.Abs(ImpInchs - whole);

            double smidge = 1.0 / (fractionBase*4);
            double numerator = (fraction + smidge) / (1.0 / fractionBase);


            int feet = whole / 12;
            int inch = whole % 12;
            return string.Format("{0}' {1} {2}/{3}", feet, inch, numerator, fractionBase);
        }

    }
}
