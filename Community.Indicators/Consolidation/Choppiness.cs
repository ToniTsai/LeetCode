using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Choppiness : DataSeries
    {
        public Choppiness(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;

            double true_high, true_low, true_rng, sum;
            DataSeries n_high = bars.High - bars.High;
            DataSeries n_low = bars.Low - bars.Low;
            for (int bar = bars.FirstActualBar + period; bar < bars.Count; bar++)
            {
                true_high = Math.Max(bars.High[bar], bars.Close[bar - 1]);
                true_low = Math.Min(bars.Low[bar], bars.Close[bar - 1]);
                true_rng = TrueRange.Series(bars)[bar];
                n_high[bar] = true_high;
                n_low[bar] = true_low;
            }

            DataSeries trueHigh = Highest2.Series(bars.High, bars.Close >> 1, period);
            DataSeries trueLow = Lowest2.Series(bars.Low, bars.Close >> 1, period);

            double nHigh, nLow, nRange, ratio, log_ratio, log_n;
            for (int bar = bars.FirstActualBar + period; bar < bars.Count; bar++)
            {
                // OLD:
                /* nHigh = Highest.Series( n_high, period )[bar];
                nLow = Lowest.Series( n_low, period )[bar]; */

                // NEW:
                nHigh = trueHigh[bar];
                nLow = trueLow[bar];

                nRange = nHigh - nLow;
                sum = Sum.Series(TrueRange.Series(bars), period)[bar];
                ratio = sum / nRange;
                log_ratio = Math.Log(ratio);
                log_n = Math.Log(period);

                if (bar <= period)
                    base[bar] = 50;
                else
                    base[bar] = 100 * log_ratio / log_n;
            }
        }

        public static Choppiness Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "Choppiness(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (Choppiness)bars.Cache[description];
            }

            Choppiness _Choppiness = new Choppiness(bars, period, description);
            bars.Cache[description] = _Choppiness;
            return _Choppiness;
        }
    }

    public class ChoppinessHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ChoppinessHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 50) };
            _paramNames = new string[] { "Bars", "Indicator Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 2;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }

        }

        public override string Description
        {
            get
            {
                return "Dreiss Choppiness Index (developed by E.W. Dreiss) is an indicator " +
                    "that measures the market's directional movement versus its choppiness.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Choppiness);
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 61.8;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return 38.2;
            }
        }

        public override Color OscillatorOverboughtColor
        {
            get
            {
                return Color.Yellow;
            }
        }

        public override Color OscillatorOversoldColor
        {
            get
            {
                return Color.Red;
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
                return "Choppiness";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Choppiness.ashx";
            }
        }
    }
}
