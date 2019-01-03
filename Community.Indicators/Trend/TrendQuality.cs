using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class TrendQuality : DataSeries
    {
        public TrendQuality(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;
            double val = 0.0; double net = 0.0; double gross = 0.0;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                net = Momentum.Series(bars.Close, period)[bar];
                gross = Sum.Series(TrueRange.Series(bars), period)[bar];
                val = (gross > 0) ? (net / gross) * 100 : 0;
                base[bar] = val;
            }
        }

        public static TrendQuality Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "TrendQuality(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (TrendQuality)bars.Cache[description];
            }

            TrendQuality _TrendQuality = new TrendQuality(bars, period, description);
            bars.Cache[description] = _TrendQuality;
            return _TrendQuality;
        }
    }

    public class TrendQualityHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TrendQualityHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(20, 2, 300) };
            _paramNames = new string[] { "Bars", "Period" };
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
                return "Trend Quality by Cliff Fiess defines the quality of a trend as a steady uptrend or downtrend with minimal retracements.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TrendQuality);
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

        public override string TargetPane
        {
            get
            {
                return "TrendQuality";
            }
        }
    }
}