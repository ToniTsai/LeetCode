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
    public class AccelerationDeceleration : DataSeries
    {
        public AccelerationDeceleration(Bars bars, int period1, int period2, int smoothing, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max((period1 + period2), smoothing);

            AwesomeOscillator ao = AwesomeOscillator.Series(bars, period1, period2);

            var rangePartitioner = Partitioner.Create(0, bars.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    base[bar] = ao[bar] - Community.Indicators.FastSMA.Series(ao, smoothing)[bar];
                }
            });
        }

        public static AccelerationDeceleration Series(Bars bars, int period1, int period2, int smoothing)
        {
            string description = string.Concat(new object[] { "Acceleration/Deceleration(", period1, ",", period2, ",", smoothing, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (AccelerationDeceleration)bars.Cache[description];
            }

            AccelerationDeceleration _AccelerationDeceleration = new AccelerationDeceleration(bars, period1, period2, smoothing, description);
            bars.Cache[description] = _AccelerationDeceleration;
            return _AccelerationDeceleration;
        }
    }

    public class AccelerationDecelerationHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AccelerationDecelerationHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(5, 1, 300), new RangeBoundInt32(34, 1, 300), new RangeBoundInt32(5, 1, 300) };
            _paramNames = new string[] { "DataSeries", "AO Period 1", "AO Period 2", "A/C Smoothing Period" };
        }

        public override string Description
        {
            get
            {
                return "Acceleration/Deceleration by Bill Williams measures acceleration and deceleration of the current market trend.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AccelerationDeceleration);
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

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/AccelerationDeceleration.ashx";
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 2;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Histogram;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "WilliamsADPane";
            }
        }
    }
}