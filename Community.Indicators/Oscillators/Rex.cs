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
    public class Rex : DataSeries
    {
        public Rex(Bars bars, int period, ChoiceOfMA option, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;
            DataSeries TVB = (bars.Close - bars.Low) + (bars.Close - bars.Open) - (bars.High - bars.Close);
            //DataSeries TVB = 3 * bars.Close - (bars.Low + bars.Open + bars.High);

            var rangePartitioner = Partitioner.Create(FirstValidValue, bars.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    if (option == ChoiceOfMA.EMA)
                        base[bar] = EMA.Series(TVB, period, EMACalculation.Modern)[bar];
                    else
                        if (option == ChoiceOfMA.SMA)
                            base[bar] = Community.Indicators.FastSMA.Series(TVB, period)[bar];
                        else
                            if (option == ChoiceOfMA.WMA)
                                base[bar] = WMA.Series(TVB, period)[bar];
                            else
                                if (option == ChoiceOfMA.SMMA)
                                    base[bar] = SMMA.Series(TVB, period)[bar];
                }
            });
        }

        public static Rex Series(Bars bars, int period, ChoiceOfMA option)
        {
            string description = string.Concat(new object[] { "Rex(", period, ",", option, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (Rex)bars.Cache[description];
            }

            Rex _Rex = new Rex(bars, period, option, description);
            bars.Cache[description] = _Rex;
            return _Rex;
        }
    }

    public class RexHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static RexHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(20, 2, 300), ChoiceOfMA.SMA };
            _paramNames = new string[] { "Bars", "MA Period", "MA Type" };
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
                return "The Rex Oscillator measures market behavior based on the relationship of the close to the open, high and low values of the same bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Rex);
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
                return "Rex";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Rex.ashx";
            }
        }
    }
}