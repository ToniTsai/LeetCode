using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    // Both Adaptive Lookback and Wealth-Lab Moving Average were created by Eugene

    public class WLMA : DataSeries
    {
        public WLMA(Bars bars, DataSeries ds, int swings, string description)
            : base(bars, description)
        {
            AdaptiveLookback al = AdaptiveLookback.Series(bars, swings, false);
            base.FirstValidValue = al.FirstValidValue;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = Community.Indicators.FastSMA.Series(ds, Math.Max(1, (int)al[bar]))[bar];
            }
        }

        public static WLMA Series(Bars bars, DataSeries ds, int swings)
        {
            string description = string.Concat(new object[] { "WLMA(", ds.Description, ",", swings, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (WLMA)ds.Cache[description];
            }

            WLMA _WLMA = new WLMA(bars, ds, swings, description);
            ds.Cache[description] = _WLMA;
            return _WLMA;
        }
    }

    public class WLMAHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static WLMAHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, CoreDataSeries.Close, new RangeBoundInt32(6, 1, 20) };
            _paramNames = new string[] { "Bars", "Series", "Swings" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 2;
            }
        }

        public override string Description
        {
            get
            {
                return "Wealth-Lab Moving Average by Eugene. is a responsible, adaptive, and market-driven moving average based on the Adaptive Lookback.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(WLMA);
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
                return "http://www2.wealth-lab.com/WL5Wiki/WLMA.ashx";
            }
        }
    }  
}
