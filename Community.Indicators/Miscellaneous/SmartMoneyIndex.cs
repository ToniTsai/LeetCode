using System;
using System.Collections.Generic;
using WealthLab;
using System.Drawing;

namespace Community.Indicators
{
    public class SmartMoneyIndex : DataSeries
    {
        public int GetTime(Bars bars, int bar)
        {
            return bars.Date[bar].Hour * 100 + bars.Date[bar].Minute;
        }

        public SmartMoneyIndex(Bars bars, string description)
            : base(bars, description)
        {
            Helper.CompatibilityCheck();

            base.FirstValidValue = 1;
            double am = 0, pm = 0, tc = 0, yc = 0, op = 0;

            if (!bars.IsIntraday)
                return;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                if (bars.IntradayBarNumber(bar) == 0)
                    yc = bars.Close[bar - 1];

                if (GetTime(bars, bar) == 1000)
                    am = bars.Close[bar];
                if (GetTime(bars, bar) == 1500)
                    pm = bars.Close[bar];
                if (GetTime(bars, bar) == 0930)
                    op = bars.Open[bar];

                if (bars.IsLastBarOfDay(bar))
                {
                    tc = bars.Close[bar];

                    //base[bar] = (tc - pm) - (op - am) + base[bar];
                    base[bar] = base[bar] - (op - am) + (tc - pm);
                }
                else
                    base[bar] = base[bar - 1];
            }
        }

        public static SmartMoneyIndex Series(Bars bars)
        {
            string description = string.Concat(new object[] { "SmartMoneyIndex()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (SmartMoneyIndex)bars.Cache[description];
            }

            SmartMoneyIndex _smartMoneyIndex = new SmartMoneyIndex(bars, description);
            bars.Cache[description] = _smartMoneyIndex;
            return _smartMoneyIndex;
        }
    }

    public class SmartMoneyIndexHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SmartMoneyIndexHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
        }

        public override Color DefaultColor => Color.Blue;

        public override string Description => "See Last Hour Indicator.";

        public override Type IndicatorType => typeof(SmartMoneyIndex);

        public override IList<object> ParameterDefaultValues => _paramDefaults;

        public override IList<string> ParameterDescriptions => _paramNames;
    }
}