using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Alpha : DataSeries
    {
        public Alpha(Bars bars, Bars index, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;
            if (bars.Count < period)
                return;

            Beta beta = Beta.Series(bars, index, period);
            var alpha = (Sum.Series(ROC.Series(bars.Close, 1), period) - (beta) * Sum.Series(ROC.Series(index.Close, 1), period)) / period;

            for (int bar = period; bar < bars.Count; bar++)
            {
                base[bar] = alpha[bar];
            }
        }

        public static Alpha Series(Bars bars, Bars index, int period)
        {
            string description = string.Concat(new object[] { "Alpha(", bars.Symbol, ",", index.Symbol, ",", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (Alpha)bars.Cache[description];
            }

            Alpha _Alpha = new Alpha(bars, index, period, description);
            bars.Cache[description] = _Alpha;
            return _Alpha;
        }
    }

    public class AlphaHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AlphaHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, BarDataType.Bars, new RangeBoundInt32(100, 2, 300) };
            _paramNames = new string[] { "Bars", "Index", "Period" };
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
                return "This indicator calculates the Alpha of a security. For example, enter ^GSPC for the Yahoo! symbol of the S&P 500 Index.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Alpha);
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
                return "Alpha";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Alpha.ashx";
            }
        }
    }
}


