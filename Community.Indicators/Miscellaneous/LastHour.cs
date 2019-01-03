using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Windows.Forms;

namespace Community.Indicators
{
    public class LastHour : DataSeries
    {
        public int GetTime(Bars bars, int bar)
        {
            return bars.Date[bar].Hour * 100 + bars.Date[bar].Minute;
        }

        public LastHour(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 1;
            double am = 0; double pm = 0;
            double tc = 0; double yc = 0;

            if (!bars.IsIntraday)
                return;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                if (bars.IntradayBarNumber(bar) == 0)
                    yc = bars.Close[bar - 1];

                if (GetTime(bars, bar) == 1030)
                    am = bars.Close[bar];
                if (GetTime(bars, bar) == 1500)
                    pm = bars.Close[bar];

                if (bars.IsLastBarOfDay(bar))
                {
                    tc = bars.Close[bar];

                    base[bar] = (tc - pm) - (am - yc);
                }
                else
                    base[bar] = base[bar - 1];
            }
        }

        public static LastHour Series(Bars bars)
        {
            string description = string.Concat(new object[] { "LastHour()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (LastHour)bars.Cache[description];
            }

            LastHour _LastHour = new LastHour(bars, description);
            bars.Cache[description] = _LastHour;
            return _LastHour;
        }
    }

    public class LastHourHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static LastHourHelper()
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
                return "According to Rennie Yang, the Last Hour Indicator has a good record of tracking 'smart money'.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(LastHour);
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
    }
}
