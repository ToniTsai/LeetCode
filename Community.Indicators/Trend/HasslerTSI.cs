using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class HasslerTSI : DataSeries
    {
        public HasslerTSI(Bars bars, DataSeries ds, int period1, int period2, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period2, period1);

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period1 || ds.Count < period2)
                return;

            ATR atr = ATR.Series(bars, period1);
            DataSeries Ratio = DataSeries.Abs(ds - (ds >> period1)) / atr;
            DataSeries HasslerTSI = Community.Indicators.FastSMA.Series(Community.Indicators.FastSMA.Series(Ratio, period1), period2);

            for (int i = FirstValidValue; i < ds.Count; i++)
            {
                base[i] = HasslerTSI[i];
            }
        }

        public static HasslerTSI Series(Bars bars, DataSeries ds, int period1, int period2)
        {
            string description = string.Concat(new object[] { "HasslerTSI(", ds.Description, ",", period1, ",", period2, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (HasslerTSI)ds.Cache[description];
            }

            HasslerTSI _HasslerTSI = new HasslerTSI(bars, ds, period1, period2, description);
            ds.Cache[description] = _HasslerTSI;
            return _HasslerTSI;
        }
    }

    public class HasslerTSIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static HasslerTSIHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, CoreDataSeries.Close, new RangeBoundInt32(10, 2, 300), new RangeBoundInt32(100, 2, 300), };
            _paramNames = new string[] { "Bars", "Data Series", "Period 1", "Period 2" };
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
                return "TSI by Frank Hassler identifies true trend strength. A high TSI value (greater than 1.65) indicates that short-term trend continuation is more likely than mean reversion.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HasslerTSI);
            }
        }

        public override string TargetPane
        {
            get
            {
                return "HasslerTSI";
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
                return "http://engineering-returns.com/tsi/";
            }
        }
    }
}