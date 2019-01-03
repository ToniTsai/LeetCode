using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// T3 Created by Gary Fritz 
    /// </summary> 
    public class T3 : DataSeries
    {
        public T3(DataSeries ds, double period, double damp, string description)
            : base(ds, description)
        {
            base.FirstValidValue = ds.FirstValidValue + 1;
            if (base.FirstValidValue > ds.Count) base.FirstValidValue = ds.Count;

            double b, c1, c2, c3, c4, e1, e2, e3, e4, e5, e6, XAlpha, XBeta;
            if (period == 0) XAlpha = 1.0 / 3.0;
            else XAlpha = 2.0 / (1.0 + Math.Abs(period));
            XBeta = 1.0 - XAlpha;
            b = damp;
            c1 = -b * b * b;
            c2 = 3 * b * b + 3 * b * b * b;
            c3 = -6 * b * b - 3 * b - 3 * b * b * b;
            c4 = 1 + 3 * b + b * b * b + 3 * b * b;
            e1 = e2 = e3 = e4 = e5 = e6 = ds[FirstValidValue];

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                e1 = e1 * XBeta + ds[bar] * XAlpha;
                e2 = e2 * XBeta + e1 * XAlpha;
                e3 = e3 * XBeta + e2 * XAlpha;
                e4 = e4 * XBeta + e3 * XAlpha;
                e5 = e5 * XBeta + e4 * XAlpha;
                e6 = e6 * XBeta + e5 * XAlpha;
                base[bar] = c1 * e6 + c2 * e5 + c3 * e4 + c4 * e3;
            }
        }

        public static T3 Series(DataSeries ds, double period, double damp)
        {
            //Build description
            string description = "T3(" + ds.Description + "," + period + "," + damp + ")";

            //See if it exists in the cache
            if (ds.Cache.ContainsKey(description))
                return (T3)ds.Cache[description];

            //Create T3, cache it, return it
            return (T3)(ds.Cache[description] = new T3(ds, period, damp, description));
        }
    }

    public class T3Helper : IndicatorHelper
    {
        private static object[] _paramDefaults = { CoreDataSeries.Close, new RangeBoundDouble(2, 0.01, 20.0), new RangeBoundDouble(0.7, 0.01, 2.0) };
        private static string[] _paramNames = { "Source", "Period", "Damp" };

        public override IList<object> ParameterDefaultValues
        {
            get { return _paramDefaults; }
        }

        public override IList<string> ParameterDescriptions
        {
            get { return _paramNames; }
        }

        public override Color DefaultColor
        {
            get { return Color.DarkMagenta; }
        }

        public override int DefaultWidth
        {
            get { return 1; }
        }

        public override Type IndicatorType
        {
            get { return typeof(T3); }
        }

        public override string Description
        {
            get
            {
                return @"Tim Tillson's T3 average was published in the January 1998 issue of TASC";
            }
        }

        public override string URL
        {
            get { return @"http://www2.wealth-lab.com/WL5Wiki/T3MA.ashx"; }
        }
    }
}