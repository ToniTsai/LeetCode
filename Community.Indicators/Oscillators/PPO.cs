using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class PPO : DataSeries
    {
        public PPO(DataSeries ds, int period1, int period2, string description)
            : base(ds, description)
        {
            FirstValidValue = Math.Max(period1, period2) * 3;
            if (FirstValidValue < 2) FirstValidValue = 2;

            WealthLab.Indicators.EMACalculation m = WealthLab.Indicators.EMACalculation.Modern;
            EMA ema1 = EMA.Series(ds, period1, m);
            EMA ema2 = EMA.Series(ds, period2, m);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = (ema1[bar] - ema2[bar]) / ema2[bar];
            }
        }

        public static PPO Series(DataSeries ds, int period1, int period2)
        {
            string description = string.Concat(new object[] { "PPO(", ds.Description, ",", period1, ",", period2, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (PPO)ds.Cache[description];
            }

            PPO _PPO = new PPO(ds, period1, period2, description);
            ds.Cache[description] = _PPO;
            return _PPO;
        }
    }

    public class PPOHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PPOHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close,
                new RangeBoundInt32(12, 2, 300), new RangeBoundInt32(26, 2, 300) };
            _paramNames = new string[] { "Data series", "Shorter EMA", "Longer EMA" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.DarkRed;
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

        public override string Description
        {
            get
            {
                return "The Percentage Price Oscillator (PPO) is a technical momentum indicator showing the relationship between two moving averages.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PPO);
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
                return "PPO";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/PPO.ashx";
            }
        }
    }

}
