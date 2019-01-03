using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class VarianceRatio : DataSeries
    {
        public VarianceRatio(Bars bars, DataSeries ds, int lookback, int period, string description)
            : base(bars, description)
        {
            LNRet lnr = LNRet.Series(bars, ds, lookback);
            LNRet lnr1 = LNRet.Series(bars, ds, 1);
            StdDevCalculation sample = StdDevCalculation.Sample;

            for (int bar = Math.Max(lookback, period); bar < bars.Count; bar++)
                base[bar] = StdDev.Series(lnr, period, sample)[bar] / (StdDev.Series(lnr1, period, sample)[bar] * Math.Sqrt(lookback));
        }

        public static VarianceRatio Series(Bars bars, DataSeries ds, int lookback, int period)
        {
            string description = string.Concat(new object[] { "VarianceRatio(", ds.Description, ",", lookback.ToString(), ",", period.ToString(), ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (VarianceRatio)bars.Cache[description];
            }

            VarianceRatio _VarianceRatio = new VarianceRatio(bars, ds, lookback, period, description);
            bars.Cache[description] = _VarianceRatio;
            return _VarianceRatio;
        }
    }

    public class VarianceRatioHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static VarianceRatioHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, CoreDataSeries.Close, new RangeBoundInt32(20, 5, 300), new RangeBoundInt32(400, 10, 1000) };
            _paramNames = new string[] { "Bars", "Close", "Lookback", "Period" };
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
                return "The Variance Ratio indicator by Dr.Rene Koch is a measure for the trendiness or degree of mean reversion in a price series.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(VarianceRatio);
            }
        }


        public override string TargetPane
        {
            get
            {
                return "VarianceRatioPane";
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
                return "http://www2.wealth-lab.com/WL5Wiki/VarianceRatio.ashx";
            }
        }
    }
}
