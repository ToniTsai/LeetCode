using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class HiLoRange : DataSeries
    {
        public HiLoRange(Bars bars, int period, string description)
            : base(bars, description)
        {
            for (int bar = period; bar < bars.Count; bar++)
                base[bar] = Highest.Series(bars.High, period)[bar] - Lowest.Series(bars.Low, period)[bar];
        }

        public static HiLoRange Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "HiLoRange(", period.ToString(), ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (HiLoRange)bars.Cache[description];
            }

            HiLoRange _HiLoRange = new HiLoRange(bars, period, description);
            bars.Cache[description] = _HiLoRange;
            return _HiLoRange;
        }
    }

    /// <summary>
    /// HiLoLimit - coded by Tim Hodder (thodder)
    /// </summary>
    public class HiLoLimit : DataSeries
    {
        public HiLoLimit(Bars bars, int period, double level, double minrange, string description)
            : base(bars, description)
        {
            for (int bar = period; bar < bars.Count; bar++)
            {
                double result = 0.0;

                double ls = Lowest.Series(bars.Low, period)[bar];
                double ds = HiLoRange.Series(bars, period)[bar];

                if (minrange == 0.0)
                {
                    result = ls + (ds * (level / 100));
                }
                else
                {
                    double l = ls;
                    double range = ds;
                    double mid = l + range / 2;
                    double mrange = l * minrange / 100.0;

                    if (range < mrange)
                        range = mrange;

                    result = mid + (level / 100.0 - 0.5) * range;
                }

                base[bar] = result;
            }
        }

        public static HiLoLimit Series(Bars bars, int period, double level, double minrange)
        {
            string description = string.Concat(new object[] { "HiLoLimit(", period.ToString(), ",", level.ToString(), ",", minrange.ToString(), ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (HiLoLimit)bars.Cache[description];
            }

            HiLoLimit _HiLoLimit = new HiLoLimit(bars, period, level, minrange, description);
            bars.Cache[description] = _HiLoLimit;
            return _HiLoLimit;
        }
    }

    public class HiLoLimitHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static HiLoLimitHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300), new RangeBoundDouble(2, 1, 10), new RangeBoundDouble(2, 1, 10) };
            _paramNames = new string[] { "Bars", "Period", "Level", "MinRange" };
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
                return "The HiLoLimit indicator developed by Dr.Koch is a lag-free limit based on highest/lowest levels.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HiLoLimit);
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
                return "http://www2.wealth-lab.com/WL5Wiki/HiLoLimit.ashx";
            }
        }
    }
}