using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Beta : DataSeries
    {
        public Beta(Bars bars, Bars index, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;
            
            DataSeries Stock = bars.Close;
            DataSeries Market = index.Close;
            DataSeries PctStock = ROC.Series(Stock, 1);
            DataSeries PctMarket = ROC.Series(Market, 1);
            DataSeries MarketSq = PctMarket * PctMarket;
            DataSeries ProductSeries = PctStock * PctMarket;

            //  Need to find Linear Regression Slope of Stock return vs. Mkt Return
            double SumMarket = 0;
            double SumMarketSq = 0;
            double SumStock = 0;
            double SumProduct = 0;

            if (bars.Count < period)
                return;

            for (int bar = period; bar < bars.Count; bar++)
            {
                SumMarket = SumMarketSq = SumStock = SumProduct = 0;
                for (int i = bar - period; i <= bar; i++)
                {
                    SumMarket += PctMarket[i];
                    SumMarketSq += MarketSq[i];
                    SumStock += PctStock[i];
                    SumProduct += ProductSeries[i];
                }
                base[bar] = ((period * SumProduct) - (SumStock * SumMarket)) / ((period * SumMarketSq) - (Math.Pow(SumMarket, 2)));
            }
        }

        public static Beta Series(Bars bars, Bars index, int period)
        {
            string description = string.Concat(new object[] { "Beta(", bars.Symbol, "," , index.Symbol, "," , period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (Beta)bars.Cache[description];
            }

            Beta _Beta = new Beta(bars, index, period, description);
            bars.Cache[description] = _Beta;
            return _Beta;
        }
    }

    public class BetaHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static BetaHelper()
        {
            //string index = "^GSPC";
            _paramDefaults = new object[] { BarDataType.Bars, BarDataType.Bars, new RangeBoundInt32(100, 2, 300) };
            _paramNames = new string[] { "Bars", "Index", "Period" };
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
                return "This indicator calculates the Beta of a security to a specific market over a specified period."
                    + "For example, enter ^GSPC for the Yahoo! symbol for S&P 500 Index.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Beta);
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
                return "Beta";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Beta.ashx";
            }
        }
    }
}


