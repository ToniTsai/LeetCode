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
    public class AccelerationBandsU : DataSeries
    { 
        public AccelerationBandsU(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;

            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < period)
                return;

            DataSeries UB = Community.Indicators.FastSMA.Series(bars.High * (1 + 2 * (((bars.High - bars.Low) / ((bars.High + bars.Low) / 2)) * 1000) * 0.001), period);

            //for (int bar = FirstValidValue; bar < bars.Count; bar++)
            //{
            //    base[bar] = UB[bar];
            //}

            var rangePartitioner = Partitioner.Create(0, bars.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    base[bar] = UB[bar];
                }
            });
        }

        public static AccelerationBandsU Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "Upper Acceleration Band(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (AccelerationBandsU)bars.Cache[description];
            }

            AccelerationBandsU _AccelerationBandsU = new AccelerationBandsU(bars, period, description);
            bars.Cache[description] = _AccelerationBandsU;
            return _AccelerationBandsU;
        }
    }

    public class AccelerationBandsUHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AccelerationBandsUHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(20, 5, 300) };
            _paramNames = new string[] { "Bars", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override Color DefaultBandColor
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
                return "The Acceleration Bands indicator created by Price Headley serve as a trading envelope that factor in a stock's typical volatility over standard settings of 20 or 80 bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AccelerationBandsU);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(AccelerationBandsL);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/AccelerationBands.ashx";
            }
        }
    }

    public class AccelerationBandsL : DataSeries
    {
         public AccelerationBandsL(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;

            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < period)
                return;

            DataSeries LB = Community.Indicators.FastSMA.Series(bars.Low * (1 - 2 * (((bars.High - bars.Low) / ((bars.High + bars.Low) / 2)) * 1000) * 0.001), period);

            //for (int bar = FirstValidValue; bar < bars.Count; bar++)
            //{
            //    base[bar] = LB[bar];
            //}

            var rangePartitioner = Partitioner.Create(0, bars.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    base[bar] = LB[bar];
                }
            });
        }

        public static AccelerationBandsL Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "Lower Acceleration Band(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (AccelerationBandsL)bars.Cache[description];
            }

            AccelerationBandsL _AccelerationBandsL = new AccelerationBandsL(bars, period, description);
            bars.Cache[description] = _AccelerationBandsL;
            return _AccelerationBandsL;
        }
    }

    public class AccelerationBandsLHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AccelerationBandsLHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(20, 5, 300) };
            _paramNames = new string[] { "Bars", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override Color DefaultBandColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override string Description
        {
            get
            {
                return "The Acceleration Bands indicator created by Price Headley serve as a trading envelope that factor in a stock's typical volatility over standard settings of 20 or 80 bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AccelerationBandsL);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(AccelerationBandsU);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/AccelerationBands.ashx";
            }
        }
    }
}