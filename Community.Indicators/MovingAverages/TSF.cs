using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Windows.Forms;

namespace Community.Indicators
{
    public class TSF : DataSeries
    {
         public TSF(DataSeries ds, int period, int ProjectionBar, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            if (ds.Count < period)
                return;

            //DataSeries sumY = Sum.Series(ds, period);
            //double sumBars = period * (period - 1) * 0.5;
            //int sumSqrBars = period * (period - 1) * (2 * period - 1) / 6;

            //double sum1 = 0; double sum2 = 0; double num1 = 0; double num2 = 0;
            //double m = 0; double b = 0;

            //for (int bar = base.FirstValidValue; bar < ds.Count; bar++)
            //{
            //    sum1 = 0.0;
            //    for (int counter = (period - 1); counter > -1; counter--)
            //        sum1 += counter * ds[bar - counter];

            //    sum2 = sumBars * sumY[bar];
            //    num1 = period * sum1 - sum2;
            //    num2 = sumBars * sumBars - period * sumSqrBars;

            //    if (num2 != 0)
            //        m = num1 / num2;
            //    else
            //        m = 0;

            //    b = (sumY[bar] - m * sumBars) / period;
            //    base[bar] = m * (period - 1 - ProjectionBar) + b;
            //}

            // Or simply use this one-liner by Giorgio Beltrame :-)
            for (int bar = base.FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = LinearReg.Series(ds, period)[bar] + (LinearRegSlope.Series(ds, period)[bar] * ProjectionBar);
            }
        }

        public static TSF Series(DataSeries ds, int period, int ProjectionBar)
        {
            string description = string.Concat(new object[] { "Time Series Forecast(", ds.Description, ",", period, ",", ProjectionBar, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (TSF)ds.Cache[description];
            }

            TSF _TSF = new TSF(ds, period, ProjectionBar, description);
            ds.Cache[description] = _TSF;
            return _TSF;
        }
    }

    public class TSFUHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TSFUHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(14, 5, 300), new RangeBoundInt32(0, -10, 10) };
            _paramNames = new string[] { "Data Series", "Period", "ProjectionBar" };
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
                return "The Time Series Forecast indicator is based on the linear regression trendline of a security's price over a specified time period.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TSF);
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
                return "http://www2.wealth-lab.com/WL5Wiki/TSF.ashx";
            }
        }
    }
}