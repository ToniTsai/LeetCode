using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class DyMoI : DataSeries
    {
        public DyMoI(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 14 * 3; // RSI is being used: an 'unstable' indicator
            StdDev SD5 = StdDev.Series(bars.Close, 5, StdDevCalculation.Population);
            DataSeries VIX = SD5 / Community.Indicators.FastSMA.Series(SD5, 10);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                int per = (int)(14 / VIX[bar]);
                base[bar] = per > 0 ? RSI.Series(bars.Close, per)[bar] : 50;
            }
        }

        public static DyMoI Series(Bars bars)
        {
            string description = string.Concat(new object[] { "DyMoI()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (DyMoI)bars.Cache[description];
            }

            DyMoI _DyMoI = new DyMoI(bars, description);
            bars.Cache[description] = _DyMoI;
            return _DyMoI;
        }
    }

    public class DyMoIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DyMoIHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
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
                return "The Dynamic Momentum Index (DMI) by T.Chande and S.Kroll is an adaptive successor to RSI. Its lookback period varies from the recent market volatility.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DyMoI);
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
                return "DyMoI";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/DyMoI.ashx";
            }
        }
    }
}