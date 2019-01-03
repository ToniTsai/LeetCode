using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Skewness : DataSeries
    {
        public Skewness(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            double S = 0; double S1 = 0; double S2 = 0; double S3 = 0;
            double Mean = 0; double Variance = 0;

            if (ds.Count < period)
                return;

            for (int bar = 0; bar <= period - 2; bar++)
            {
                S = ds[bar];
                S1 += S;
                S2 += Math.Pow(S, 2);
                S3 += (Math.Pow(S, 2) * S);
            }

            for (int bar = period - 1; bar <= ds.Count - 1; bar++)
            {
                S = ds[bar];
                S1 += S;
                S2 += Math.Pow(S, 2);
                S3 += (Math.Pow(S, 2) * S);

                Mean = S1 / period;
                Variance = (S2 - S1 * Mean) / (period - 1);
                if (Variance > 0)
                    base[bar] = (period * S3 - 3 * S2 * S1 + 2 * S1 * S1 * Mean)
                    / ((period - 1) * (period - 2) * Math.Pow(Variance, 1.5));

                S = ds[bar - period + 1];
                S1 -= S;
                S2 -= Math.Pow(S, 2);
                S3 -= (Math.Pow(S, 2) * S);
            }
        }

        public static Skewness Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "Skewness(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (Skewness)ds.Cache[description];
            }

            Skewness _Skew = new Skewness(ds, period, description);
            ds.Cache[description] = _Skew;
            return _Skew;
        }
    }

    public class SkewnessHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SkewnessHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(50, 3, 300) };
            _paramNames = new string[] { "Data Series", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Orange;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 1;
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return false;
            }
        }

        public override string Description
        {
            get
            {
                return "Returns skewness (asymmetry from the normal distribution) of values from the specified DataSeries.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Skewness);
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
                return "Skewness";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www.investopedia.com/terms/s/skewness.asp";
            }
        }
    }
}