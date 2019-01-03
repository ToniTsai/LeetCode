using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Community.Indicators
{
    public class AwesomeOscillator : DataSeries
    {
        public AwesomeOscillator(Bars bars, int period1, int period2, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period1 + period2;

            AveragePrice avg = AveragePrice.Series(bars);

            var rangePartitioner = Partitioner.Create(0, bars.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    base[bar] = Community.Indicators.FastSMA.Series(avg, period1)[bar] - Community.Indicators.FastSMA.Series(avg, period2)[bar];
                }
            });
        }

        public static AwesomeOscillator Series(Bars bars, int period1, int period2)
        {
            string description = string.Concat(new object[] { "Awesome Oscillator(", period1, ",", period2, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (AwesomeOscillator)bars.Cache[description];
            }

            AwesomeOscillator _AwesomeOscillator = new AwesomeOscillator(bars, period1, period2, description);
            bars.Cache[description] = _AwesomeOscillator;
            return _AwesomeOscillator;
        }
    }

    public class AwesomeOscillatorHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AwesomeOscillatorHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(5, 1, 300), new RangeBoundInt32(34, 1, 300) };
            _paramNames = new string[] { "DataSeries", "Period 1", "Period 2" };
        }

        public override string Description
        {
            get
            {
                return "Awesome Oscillator (AO) by Bill Williams is a MACD-like indicator that measures market momentum of the median price of the last 5 bars, compared to the momentum of the last 34 bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AwesomeOscillator);
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

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/AwesomeOscillator.ashx";
            }
        }


        public override string TargetPane
        {
            get
            {
                return "WilliamsAOPane";
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Histogram;
            }
        }
    }
}
