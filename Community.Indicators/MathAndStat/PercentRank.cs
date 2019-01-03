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
    /// <summary>
    /// Re-implemented by DartboardTrader, and verified with Excel.
    /// </summary>
    public class PercentRank : DataSeries
    {
        public PercentRank(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            DataSeries r = ds * 0;
            int iperiod = period - 1;
            base.FirstValidValue = iperiod;

            if (ds.Count < period)
                return;

            var rangePartitioner = Partitioner.Create(FirstValidValue, ds.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    double lt = 0;
                    double gte = 0;

                    for (int x = i - FirstValidValue; x < i; ++x) // DO NOT include the current value.
                    {
                        if (ds[x] < ds[i])
                            ++lt;
                        else
                            ++gte;
                    }
                    // Equivalent to Excel's RoundDown(value,digits).
                    double precision = Math.Pow(10, 3);
                    base[i] = Math.Floor((lt / (lt + gte)) * precision) / precision;
                }
            });
        }

        public static PercentRank Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "PercentRank(", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (PercentRank)ds.Cache[description];
            }

            PercentRank _PercentRank = new PercentRank(ds, period, description);
            ds.Cache[description] = _PercentRank;
            return _PercentRank;
        }
    }

    public class PercentRankHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PercentRankHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(2, 2, 300) };
            _paramNames = new string[] { "DataSeries", "Period" };
        }

        public override Color DefaultColor { get { return Color.Black; } }
        public override int DefaultWidth { get { return 1; } }
        public override string Description { get { return "Returns percent rank of the current element of the dataseries within all elements over the specified period"; } }
        public override Type IndicatorType { get { return typeof(PercentRank); } }
        public override IList<object> ParameterDefaultValues { get { return _paramDefaults; } }
        public override IList<string> ParameterDescriptions { get { return _paramNames; } }
        public override string TargetPane { get { return "PercentRank"; } }
        public override string URL { get { return "http://www2.wealth-lab.com/WL5Wiki/PercentRank.ashx"; } }
    }

    /// <summary>
    /// Courtesy avishn
    /// </summary>
    public class PrcRank : DataSeries
    {
        DataSeries ds;
        int period;

        public PrcRank(DataSeries ds, int period, string description)
            : base(ds, description)
        {

            this.ds = ds;
            this.period = period;

            FirstValidValue = period + ds.FirstValidValue - 1;

            if (ds.Count < period)
                return;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                int countLessThan = 0;
                for (int i = 0; i < period - 1; i++)
                {
                    if (ds[bar] > ds[bar - period + 1 + i])
                    {
                        countLessThan++;
                    }
                }
                base[bar] = (double)countLessThan / (period - 1);
            }

        }

        public static PrcRank Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "PrcRank(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (PrcRank)ds.Cache[description];
            }
            PrcRank _pr = new PrcRank(ds, period, description);
            ds.Cache[description] = _pr;
            return _pr;
        }

        public override void CalculatePartialValue()
        {
            ds.CalculatePartialValue();
            if (Double.IsNaN(ds.PartialValue))
            {
                PartialValue = Double.NaN;
            }
            else
            {
                int countLessThan = 0;
                for (int i = 1; i < period; i++)
                {
                    if (ds.PartialValue > ds[ds.Count - period + i])
                    {
                        countLessThan++;
                    }
                }
                PartialValue = (double)countLessThan / (period - 1);
            }
        }
    }

    public class PrcRankHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PrcRankHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(2, 2, 300) };
            _paramNames = new string[] { "DataSeries", "Period" };
        }

        public override Color DefaultColor { get { return Color.Black; } }
        public override int DefaultWidth { get { return 1; } }
        public override string Description { get { return "Returns percent rank of the current element of the dataseries within all elements over the specified period"; } }
        public override Type IndicatorType { get { return typeof(PrcRank); } }
        public override IList<object> ParameterDefaultValues { get { return _paramDefaults; } }
        public override IList<string> ParameterDescriptions { get { return _paramNames; } }
        public override string TargetPane { get { return "PercentRank"; } }
        public override string URL { get { return "http://www2.wealth-lab.com/WL5Wiki/PercentRank.ashx"; } }

    }
}