using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class SMMA : DataSeries
    {
        public SMMA(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            if (ds.Count < period)
                return;

            // First value is just the SMA
            double mySum = Sum.Value(period - 1, ds, period);
            base[period - 1] = mySum / period;

            for (int bar = period; bar < ds.Count; bar++)
            {
                // New Sum = Previous Sum - Previous Smoothed Average + Current Price
                mySum = mySum - base[bar - 1] + ds[bar];
                base[bar] = mySum / period;
            }
        }

        public static SMMA Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "SMMA(", ds.Description, ", ", period, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (SMMA)ds.Cache[description];
            }

            SMMA _SMMA = new SMMA(ds, period, description);
            ds.Cache[description] = _SMMA;
            return _SMMA;
        }
    }

    public class SMMAHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SMMAHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 300) };
            _paramNames = new string[] { "Series", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override string Description
        {
            get
            {
                return "A Smoothed Moving Average is an Exponential Moving Average, only with a longer period applied.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SMMA);
            }
        }

        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return _paramDefaults;
            }
        }

        public override IList<string> ParameterDescriptions
        {
            get
            {
                return _paramNames;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/SMMA.ashx";
            }
        }
    }
}