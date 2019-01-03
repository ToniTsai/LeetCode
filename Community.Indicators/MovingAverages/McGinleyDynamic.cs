using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class McGinleyDynamic : DataSeries
    {
        public McGinleyDynamic(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            this.FirstValidValue = period;// *3;

            DataSeries ema12s = EMA.Series(ds, period, WealthLab.Indicators.EMACalculation.Modern) >> 1;
            DataSeries McGD = ema12s + ((ds - ema12s) / (ds / ema12s * 125));

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = McGD[bar];
            }
        }

        public static McGinleyDynamic Series(DataSeries ds, int period)
        {
            //Build description
            string description = "McGinleyDynamic(" + ds.Description + "," + period + ")";

            //See if it exists in the cache
            if (ds.Cache.ContainsKey(description))
                return (McGinleyDynamic)ds.Cache[description];

            //Create McGinleyDynamic, cache it, return it
            return (McGinleyDynamic)(ds.Cache[description] = new McGinleyDynamic(ds, period, description));
        }
    }

    public class McGinleyDynamicHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = { CoreDataSeries.Close, new RangeBoundInt32(12, 2, 300) };
        private static string[] _paramNames = { "DataSeries", "Period" };

        public override IList<object> ParameterDefaultValues
        {
            get { return _paramDefaults; }
        }

        public override IList<string> ParameterDescriptions
        {
            get { return _paramNames; }
        }

        public override Color DefaultColor
        {
            get { return Color.DarkBlue; }
        }

        public override int DefaultWidth
        {
            get { return 1; }
        }

        public override Type IndicatorType
        {
            get { return typeof(McGinleyDynamic); }
        }

        public override string Description
        {
            get
            {
                return @"The McGinley Dynamic Indicator is a moving average with a volatility filter designed to further smooth out the price action. It would be used similar to a moving average.";
            }
        }

        public override string URL
        {
            get { return @"http://www2.wealth-lab.com/WL5Wiki/McGinleyDynamic.ashx"; }
        }
    }
}