using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class HTInstTrendLine : DataSeries
    {
        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public HTInstTrendLine(DataSeries ds, string description)
            : base(ds, description)
        {
            DataSeries Result = new DataSeries(ds, "Result");
            DataSeries DC = HTPeriod.Series(ds);
            DataSeries Value1ht = new DataSeries(ds, "Value1ht");
            DataSeries Value2ht = new DataSeries(ds, "Value2ht");

            for (int bar = 4; bar < ds.Count; bar++)
            {
                if (DC[bar] != 0)
                {
                    Value1ht[bar] = 0.0542 * ds[bar] + 0.0210 * ds[bar - 1]
                      + 0.0210 * ds[bar - 2] + 0.0542 * ds[bar - 3]
                      + 1.9733 * Value1ht[bar - 1] - 1.6067 * Value1ht[bar - 2]
                      + 0.4831 * Value1ht[bar - 3];

                    Value2ht[bar] = 0.8 * (Value1ht[bar]
                     - 2.0 * Math.Cos(DegreeToRadian(360 / 10))
                     * Value1ht[bar - 1] + Value1ht[bar - 2])
                     + 1.6 * Math.Cos(DegreeToRadian(360 / 10))
                     * Value2ht[bar - 1] - 0.6 * Value2ht[bar - 2];

                    Result[bar] = 0.9 * (Value2ht[bar]
                      - 2.0 * Math.Cos(DegreeToRadian(360 / DC[bar]))
                      * Value2ht[bar - 1] + Value2ht[bar - 2])
                      + 1.8 * Math.Cos(DegreeToRadian(360 / DC[bar]))
                      * Result[bar - 1] - 0.8 * Result[bar - 2];
                }
                else
                    Result[bar] = Result[bar - 1];

                base[bar] = Result[bar];
            }
        }

        public static HTInstTrendLine Series(DataSeries ds)
        {
            string description = string.Concat(new object[] { "HTInstTrendLine(", ds.Description, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (HTInstTrendLine)ds.Cache[description];
            }

            HTInstTrendLine _InstTrendLine = new HTInstTrendLine(ds, description);
            ds.Cache[description] = _InstTrendLine;
            return _InstTrendLine;
        }
    }

    public class HTInstTrendLineHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static HTInstTrendLineHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close };
            _paramNames = new string[] { "Data Series" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 1;
            }
        }

        public override string Description
        {
            get
            {
                return "The Instantaneous Trendline appears much like a Moving Average, but with minimal lag compared with the lag normally associated with such averages for equivalent periods.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HTInstTrendLine);
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
                return "http://www2.wealth-lab.com/WL5Wiki/HTTrendLine.ashx";
            }
        }
    }
}
