using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class KVO : DataSeries
    {
        public KVO(Bars bars, int period1, int period2, string description)
            : base(bars, description)
        {
            FirstValidValue = Math.Max(period1, period2) * 3;
            if (FirstValidValue < 2) FirstValidValue = 2;

            WealthLab.Indicators.EMACalculation m = WealthLab.Indicators.EMACalculation.Modern;
            DataSeries TSum = bars.High + bars.Low + bars.Close;
            DataSeries Range = bars.High - bars.Low;
            DataSeries VForce = new DataSeries(bars, ("VForce(" + period1 + "," + period2 + ")"));
            double[] trend = new double[2];
            double[] cm = new double[2];

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                if (TSum[bar] > TSum[bar - 1])
                    trend[1] = 1;
                else
                    trend[1] = -1;
                if (trend[1] == trend[0])
                    cm[1] = cm[0] + Range[bar];
                else
                    cm[1] = Range[bar] + Range[bar - 1];
                if (cm[1] != 0)
                    VForce[bar] = bars.Volume[bar] * Math.Abs(2 * (Range[bar] / cm[1] - 1)) * trend[1] * 100;
            }

            EMA FXAvg = EMA.Series(VForce, period1, m);
            EMA SXAvg = EMA.Series(VForce, period2, m);

            for (int bar = 1; bar < bars.Count; bar++)
            {
                base[bar] = FXAvg[bar] - SXAvg[bar];
            }
        }

        public static KVO Series(Bars bars, int period1, int period2)
        {
            string description = string.Concat(new object[] { "Klinger Volume Oscillator(", period1, ",", period2, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (KVO)bars.Cache[description];
            }

            KVO _Klinger = new KVO(bars, period1, period2, description);
            bars.Cache[description] = _Klinger;
            return _Klinger;
        }
    }

    public class KVOHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static KVOHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(34, 2, 300), new RangeBoundInt32(55, 2, 300) };
            _paramNames = new string[] { "Bars", "Shorter EMA", "Longer EMA" };
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
                return 1;
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
                return "KVO (Klinger Volume oscillator) developed by Stephen Klinger is used to determine trends of money flow by comparing the volume flowing in and out of a security to price movement.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(KVO);
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
                return "KVOPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/KVO.ashx";
            }
        }
    }
}