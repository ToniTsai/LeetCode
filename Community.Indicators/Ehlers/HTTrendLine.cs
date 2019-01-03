using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Community.Indicators
{
    public class HTTrendLine : DataSeries
    {
        public HTTrendLine(DataSeries ds, string description)
            : base(ds, description)
        {
            DataSeries Period = HTPeriod.Series(ds);

            var rangePartitioner = Partitioner.Create(1, ds.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    base[bar] = Community.Indicators.FastSMA.Series(ds, (int)Math.Truncate(Period[bar]) + 2)[bar];
                }
            });
        }

        public static HTTrendLine Series(DataSeries ds)
        {
            string description = string.Concat(new object[] { "HTTrendLine(", ds.Description, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (HTTrendLine)ds.Cache[description];
            }

            HTTrendLine _HTTrendLine = new HTTrendLine(ds, description);
            ds.Cache[description] = _HTTrendLine;
            return _HTTrendLine;
        }
    }

    public class HTTrendLineHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static HTTrendLineHelper()
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
                return typeof(HTTrendLine);
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