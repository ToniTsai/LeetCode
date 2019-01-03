using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class MassIndex : DataSeries
    {
        public MassIndex(Bars bars, int period, int sumPeriod, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(period, sumPeriod);

            base.FirstValidValue = period;

            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < period)
                return;

            EMACalculation em = EMACalculation.Modern;
            DataSeries hl = bars.High - bars.Low;
            EMA ema1 = EMA.Series(hl, period, em);
            EMA ema2 = EMA.Series(ema1, period, em);
            DataSeries emaRatio = ema1 / ema2;
            Sum sumRatio = Sum.Series(emaRatio, sumPeriod);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                this[bar] = sumRatio[bar];
            }
        }

        public static MassIndex Series(Bars bars, int period, int sumPeriod)
        {
            string description = string.Concat(new object[] { "MassIndex(),", period.ToString(), ",", sumPeriod.ToString() });

            if (bars.Cache.ContainsKey(description))
            {
                return (MassIndex)bars.Cache[description];
            }

            MassIndex massIndex = new MassIndex(bars, period, sumPeriod, description);
            bars.Cache[description] = massIndex;
            return massIndex;
        }
    }

    public class MassIndexHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MassIndexHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(9, 2, 300), new RangeBoundInt32(25, 2, 300) };
            _paramNames = new string[] { "Bars", "Lookback period", "Sum period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
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
                return "The Mass Index by Donald Dorsey is designed to spot trend reversals by gauging price volatility. Looks for \"reversal bulge\" patterns, indicating a reversal in price.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MassIndex);
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
                return "MassIndex";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MassIndex.ashx";
            }
        }
    }
}
