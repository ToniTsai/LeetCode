using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// ALMA indicator created by thodder
    /// </summary>
    public class ALMA : DataSeries
    {
        public ALMA(DataSeries ds, int windowSize, int sigma, double offset, double pctFilter, string description)
            : base(ds, description)
        {
            this.FirstValidValue = windowSize;

            double m = Math.Floor(offset * (windowSize - 1));
            if (sigma == 0)
                throw new System.ArgumentException("Parameter cannot be 0", "sigma");
            double s = windowSize / sigma;

            double[] w = new double[windowSize];
            double wSum = 0.0;

            for (int i = 1; i < windowSize; i++)
            {
                w[i] = Math.Exp(-((i - m) * (i - m)) / (2 * s * s));
                wSum += w[i];
            }

            for (int i = 1; i < windowSize; i++)
            {
                w[i] /= wSum;
            }

            for (int j = Math.Max(2, windowSize); j < ds.Count; j++)
            {
                double alSum = 0.0;

                for (int i = 1; i < windowSize; i++)
                {
                    alSum += ds[j - (windowSize - 1 - i)] * w[i];
                }

                this[j] = alSum;	//outalma

                if (pctFilter > 0.0)
                {
                    // WARNING - WLP caches Series.  StdDev may change this dataseries; therefore, avoid cache and use Value method!!
                    double Filter = StdDev.Value(j, this, windowSize, WealthLab.Indicators.StdDevCalculation.Sample);
                    Filter *= pctFilter;

                    if (Math.Abs(this[j] - this[j - 1]) < Filter)
                        this[j] = this[j - 1];
                }
            }
        }

        public static ALMA Series(DataSeries ds, int windowSize, int sigma, double offset, double pctFilter)
        {
            string description = "ALMA(" + ds.Description + "," + windowSize + "," + sigma + "," + offset + "," + pctFilter + ")";

            if (ds.Cache.ContainsKey(description))
                return (ALMA)ds.Cache[description];

            return (ALMA)(ds.Cache[description] = new ALMA(ds, windowSize, sigma, offset, pctFilter, description));
        }
    }

    public class ALMAHelper : IndicatorHelper
    {
        private static object[] _paramDefaults = { CoreDataSeries.Close, new RangeBoundInt32(9, 5, 201), new RangeBoundInt32(6, 1, 20), 
            new RangeBoundDouble(0.85, 0.05, 1), new RangeBoundDouble(0, 0, 100) };
        private static string[] _paramNames = { "DataSeries", "WindowSize", "Sigma", "Offset", "PctFilter" };

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
            get { return Color.DarkMagenta; }
        }

        public override int DefaultWidth
        {
            get { return 1; }
        }

        public override Type IndicatorType
        {
            get { return typeof(ALMA); }
        }

        public override string Description
        {
            get
            {
                return @"ALMA (Arnaud Legoux Moving Average) is a no-lag moving average.";
            }
        }

        public override string URL
        {
            get { return @"http://en.wikipedia.org/wiki/Arnaud_Legoux_Moving_Average"; }
        }
    }
}
