using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class UpDown : DataSeries
    {
        public UpDown(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 1;
            int updown = 0;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                if ((bars.High[bar] > bars.High[bar - 1]) && (bars.Low[bar] > bars.Low[bar - 1]))
                    updown += 1; // move up
                if ((bars.High[bar] < bars.High[bar - 1]) && (bars.Low[bar] < bars.Low[bar - 1]))
                    updown -= 1; // move down

                base[bar] = updown;
            }
        }

        public static UpDown Series(Bars bars)
        {
            string description = string.Concat(new object[] { "UpDown()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (UpDown)bars.Cache[description];
            }

            UpDown _UpDown = new UpDown(bars, description);
            bars.Cache[description] = _UpDown;
            return _UpDown;
        }
    }

    public class StoneTrend : DataSeries
    {
        public StoneTrend(Bars bars, int period, string description)
            : base(bars, description)
        {
            if (bars.Count < period)
                return;

            DataSeries updown_s = UpDown.Series(bars);

            base.FirstValidValue = period;

            for (int i = FirstValidValue; i < bars.Count; i++)
            {
                base[i] = (@updown_s[i] - @updown_s[i - period]) * 100.0 / period;
            }
        }

        public static StoneTrend Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "StoneTrend(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (StoneTrend)bars.Cache[description];
            }

            StoneTrend _StoneTrend = new StoneTrend(bars, period, description);
            bars.Cache[description] = _StoneTrend;
            return _StoneTrend;
        }
    }

    public class StoneTrendHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static StoneTrendHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) };
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
                return "The StoneTrend (idea by David Stone) indicator follows a simple definition for trends: trend goes up if there are higher highs and higher lows.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(StoneTrend);
            }
        }

        public override string TargetPane
        {
            get
            {
                return "StoneTrend";
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

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/StoneTrend.ashx";
            }
        }
    }
}