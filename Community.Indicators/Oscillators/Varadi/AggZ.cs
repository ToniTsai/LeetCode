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
    public class AggZ : DataSeries
    {
        /*  Similar to AggM, the AggZ is a composite trend and mean-reversion indicator rolled into one. 
         * The concept of both is to anchor a long term trending measure to a short-term mean-reverting measure so that you can have an indicator 
         * that can be traded for both long and short-term intervals. The calculation is dead simple:
         
         * AggZ= (-1x( 10-day z-score)+(200-day z-score))/2
         * where z-score = (close-sma (closing prices over last n periods))/(standard deviation( closing prices over last n periods))

         * buy above 0, sell below 0 as a basic strategy.
         * Users may try different variations of z-lengths as well as entry/exits.
		*/

        public AggZ(DataSeries ds, int period1, int period2, StdDevCalculation sd, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period1, period2);

            if (base.FirstValidValue < 2)
                base.FirstValidValue = 2;

            DataSeries sma10 = Community.Indicators.FastSMA.Series(ds, period1);
            DataSeries sma200 = Community.Indicators.FastSMA.Series(ds, period2);
            StdDev sd10 = StdDev.Series(ds, period1, sd);
            StdDev sd200 = StdDev.Series(ds, period2, sd);

            var rangePartitioner = Partitioner.Create(FirstValidValue, ds.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    double zscore10 = (ds[bar] - sma10[bar]) / sd10[bar];
                    double zscore200 = (ds[bar] - sma200[bar]) / sd200[bar];
                    double aggz = (-1 * zscore10 + zscore200) / 2.0d;
                    base[bar] = aggz;
                }
            });
        }

        public static AggZ Series(DataSeries ds, int period1, int period2, StdDevCalculation sd)
        {
            string description = string.Concat(new object[] { "AggZ(", ds.Description, ",", period1, ",", period2, ",", sd, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (AggZ)ds.Cache[description];
            }

            AggZ _AggZ = new AggZ(ds, period1, period2, sd, description);
            ds.Cache[description] = _AggZ;
            return _AggZ;
        }
    }

    public class AggZHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AggZHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 300), new RangeBoundInt32(200, 2, 300), StdDevCalculation.Sample };
            _paramNames = new string[] { "DataSeries", "Period1", "Period2", "StdDev calculation" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Violet;
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
                return "The AggZ by David Varadi is a composite trend and mean-reversion indicator rolled into one.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AggZ);
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
                return "AggZ";
            }
        }

        public override string URL
        {
            get
            {
                return "http://cssanalytics.wordpress.com/2010/03/19/aggz-another-composite-trendmean-reversion-indicator/";
            }
        }
    }
}