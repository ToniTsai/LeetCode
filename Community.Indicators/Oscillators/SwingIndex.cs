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
    /// Wilder's Swing Index
    /// "How about just the Swing Index since the accumulation requires a special limit for each commodity which complicates the formula:"
    /// "I coded a SwingIndex a year+ ago that ignored the daily futures commodity table"
    /// http://wl4.wealth-lab.com/cgi-bin/WealthLab.DLL/topic?id=11852
    /// 
    /// What you posted for the SwingIndex is exactly what I coded as a WealthScript SwindexIndexSeries 
    /// ( from Wilder's "New Concepts in Technical Trading Systems"). As I indicated in my earlier post, 
    /// I was reluctant to code the Accumulation since it requires a large and variable daily commodity table against which to accumulate.
    /// For a stock, the SwingIndex has a limited utility and accumulation here would be misleading, 
    /// however, it can still be used as a basic trend indicator. 
    /// With the limit sat at -1, a positive SwingIndex represents an uptrend and minus, a downtrend. The scaling magnitude is unimportant.
    /// Depending upon the commodity market(s) being traded, 
    /// I suspect the MetaStock Acc Swing Index constructs a limit table dynamically from the Futures data rather than use Wilder's earlier static values. 
    /// </summary>
    /// 
    public class SwingIndex : DataSeries
    {
        public SwingIndex(Bars bars, int limit, string description)
            : base(bars, description)
        {
            Helper.CompatibilityCheck();

            base.FirstValidValue = 1;

            double c2 = 0; double o2 = 0; double h2 = 0; double l2 = 0;
            double c1 = 0; double o1 = 0; double h1 = 0; double l1 = 0;
            double r = 0; double r1 = 0; double r2 = 0; double r3 = 0; double r4 = 0;
            double k = 0;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                c2 = bars.Close[bar];
                o2 = bars.Open[bar];
                h2 = bars.High[bar];
                l2 = bars.Low[bar];

                c1 = bars.Close[bar - 1];
                o1 = bars.Open[bar - 1];
                h1 = bars.High[bar - 1];
                l1 = bars.Low[bar - 1];

                if (limit <= 1)
                    limit = 10000;

                r1 = Math.Abs(h2 - c1);
                r2 = Math.Abs(l2 - c1);
                r3 = Math.Abs(h2 - l2);
                r4 = Math.Abs(c1 - o1);
                k = Math.Max(r1, r2);

                if (r1 >= Math.Max(r2, r3))
                    r = r1 - r2 / 2 + r4 / 4;
                else if (r2 >= Math.Max(r1, r3))
                    r = r2 - r1 / 2 + r4 / 4;
                else r = r3 + r4 / 4;

                if (r == 0)
                    base[bar] = 0;
                else
                    base[bar] = 50 * ((c2 - c1 + 0.5 * (c2 - o2) + 0.25 * (c1 - o1)) / r) * k / limit;
            }
        }

        public static SwingIndex Series(Bars bars, int limit)
        {
            string description = string.Concat(new object[] { "SwingIndex(", limit, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (SwingIndex)bars.Cache[description];
            }

            SwingIndex _SwingIndex = new SwingIndex(bars, limit, description);
            bars.Cache[description] = _SwingIndex;
            return _SwingIndex;
        }
    }

    public class SwingIndexHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SwingIndexHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(2, 1, 300) };
            _paramNames = new string[] { "Bars", "Limit" };
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
                return "The Swing Index created by Welles Wilder is used primarily as a basis for Wilder's Accumulative Swing Index (ASI) indicator.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SwingIndex);
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
                return "SwingIndex";
            }
        }
    }

    public class AccumSwingIndex : DataSeries
    {
        public AccumSwingIndex(Bars bars, int limit, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 2;

            SwingIndex si = SwingIndex.Series(bars, limit);

            var rangePartitioner = Partitioner.Create(FirstValidValue, bars.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    //ASI(i) = SI(i-1) + SI(i)
                    //base[bar] = si[bar - 1] + si[bar];

                    base[bar] = base[bar - 1] + si[bar];
                }
            });
        }

        public static AccumSwingIndex Series(Bars bars, int limit)
        {
            string description = string.Concat(new object[] { "AccumulationSwingIndex(", limit, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (AccumSwingIndex)bars.Cache[description];
            }

            AccumSwingIndex _ASI = new AccumSwingIndex(bars, limit, description);
            bars.Cache[description] = _ASI;
            return _ASI;
        }
    }

    public class AccumSwingIndexHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AccumSwingIndexHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(2, 1, 300) };
            _paramNames = new string[] { "Bars", "Limit" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Purple;
            }
        }

        public override string Description
        {
            get
            {
                return "Wilder's Accumulation Swing Index is a cumulative total of the Swing Index.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AccumSwingIndex);
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
                return "SwingIndex";
            }
        }
    }
}