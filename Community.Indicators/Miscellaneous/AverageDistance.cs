using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Windows.Forms;

namespace Community.Indicators
{
    public class AverageDistance : DataSeries
    {
        public AverageDistance(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period * 3;
            DataSeries sma = Community.Indicators.FastSMA.Series(bars.Close, period);
            ATR atr = ATR.Series(bars, period);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = (bars.Close[bar] - sma[bar]) / atr[bar];
            }
        }

        public static AverageDistance Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "AverageDistance(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (AverageDistance)bars.Cache[description];
            }

            AverageDistance _AverageDistance = new AverageDistance(bars, period, description);
            bars.Cache[description] = _AverageDistance;
            return _AverageDistance;
        }
    }

    public class AverageDistanceHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AverageDistanceHelper()
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
                return "The Average Distance indicator calculates the average distance in ATRs from the N day mean.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AverageDistance);
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
                return "AverageDistance";
            }
        }
    }
}
