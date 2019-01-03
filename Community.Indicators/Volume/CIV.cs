using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// Cumulative Intraday Volume
    /// Sums volume from start to stop times, and holds the result constant until it is reset the following day.
    /// Use in conjunction with SetScaleDaily to determine the average daily volume during a specified time interval, for example.
    /// </summary>
    /// 
    public class CIV : DataSeries
    {
        public CIV(Bars bars, int startTime, int stopTime, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 0;

            double cv = 0d;
            for (int bar = 0; bar < bars.Count; bar++)
            {
                if (bars.IntradayBarNumber(bar) == 0) cv = 0d;

                int time = bars.Date[bar].Hour * 100 + bars.Date[bar].Minute;
                if (time > startTime && time <= stopTime)
                {
                    cv += bars.Volume[bar];
                }
                this[bar] = cv;
            }
        }

        public static CIV Series(Bars bars, int startTime, int stopTime)
        {
            string description = string.Concat(new object[] { "CIV(),", startTime.ToString(), ",", stopTime.ToString() });

            if (bars.Cache.ContainsKey(description))
            {
                return (CIV)bars.Cache[description];
            }

            CIV civ = new CIV(bars, startTime, stopTime, description);
            bars.Cache[description] = civ;
            return civ;
        }
    }

    public class CIVHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CIVHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(0930, 0000, 2359), new RangeBoundInt32(1130, 0000, 2359) };
            _paramNames = new string[] { "Bars", "Start Time", "Stop Time" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Brown;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Histogram;
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
                return "CIV is the Cumulative Intraday Volume between the supplied start and stop times. " +
                    "Use in conjunction with SetScaleDaily to determine the average daily volume during a specified time interval, for example.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CIV);
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
                return "CIV";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CIV.ashx";
            }
        }
    }
}