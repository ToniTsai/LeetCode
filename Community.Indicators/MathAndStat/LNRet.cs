using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class LNRet : DataSeries
    {
        public LNRet(Bars bars, DataSeries ds, int period, string description)
            : base(bars, description)
        {
            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            DataSeries yesterday_s = (ds >> period);
            for (int bar = 0; bar < period; bar++)
                yesterday_s[bar] = bars.Close[0]; // avoid numerical problems

            DataSeries ds2 = ds / yesterday_s;

            //for (int bar = FirstValidValue; bar < bars.Count; bar++)
            for (int bar = 0; bar < bars.Count; bar++)
            {
                base[bar] = Math.Log(ds2[bar]);
            }
        }

        public static LNRet Series(Bars bars, DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "LNRet(", ds.Description, ",", period.ToString(), ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (LNRet)bars.Cache[description];
            }

            LNRet _LNRet = new LNRet(bars, ds, period, description);
            bars.Cache[description] = _LNRet;
            return _LNRet;
        }
    }

    public class LNRetHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static LNRetHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, CoreDataSeries.Close, new RangeBoundInt32(20, 5, 300) };
            _paramNames = new string[] { "Bars", "Close", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "LNRetPane";
            }
        }

        public override string Description
        {
            get
            {
                return "The LNRet function by Dr.Rene Koch displays logarithm of daily returns.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(LNRet);
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
                return "http://www2.wealth-lab.com/WL5Wiki/LNRet.ashx";
            }
        }
    }
}