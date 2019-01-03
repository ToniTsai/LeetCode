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
    public class Turnover : DataSeries
    {
        public Turnover(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 0;

            var rangePartitioner = Partitioner.Create(0, bars.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    base[bar] = bars.Close[bar] * bars.Volume[bar];
                }
            });
        }

        public static Turnover Series(Bars bars)
        {
            string description = string.Concat(new object[] { "Turnover()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (Turnover)bars.Cache[description];
            }

            Turnover _Turnover = new Turnover(bars, description);
            bars.Cache[description] = _Turnover;
            return _Turnover;
        }
    }

    public class TurnoverHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TurnoverHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.PowderBlue;
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
                return "Turnover is the dollar value (as of most recent close) multiplied by a stock's volume for the day.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Turnover);
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
                return "Turnover";
            }
        }
    }
}
