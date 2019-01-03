using System;
using System.Text;
using System.Collections.Generic;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class HullMA : DataSeries
    {
        public HullMA(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            if (ds.Count < period)
                return;

            WMA SlowWMA = WMA.Series(ds, period);
            WMA FastWMA = WMA.Series(ds, (int)(period / 2));
            DataSeries hma = WMA.Series((FastWMA + (FastWMA - SlowWMA)), (int)Math.Sqrt(period));

            for (int bar = period; bar < ds.Count; bar++)
            {
                base[bar] = hma[bar];
            }            
        }

        public static HullMA Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "HullMA(", ds.Description, ", ", period, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (HullMA)ds.Cache[description];
            }

            HullMA _HullMA = new HullMA(ds, period, description);
            ds.Cache[description] = _HullMA;
            return _HullMA;
        }
    }

    public class HullMAHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static HullMAHelper()
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
                return "The Hull Moving Average (HMA) by Alan Hull is a combination of weighted moving averages (WMA) more responsive to current price fluctuations while smoothing prices.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HullMA);
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
                return "http://www2.wealth-lab.com/WL5Wiki/HullMA.ashx";
            }
        }
    }

}
