using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Kurtosis : DataSeries
    {
        public Kurtosis(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            double S = 0; double S1 = 0; double S2 = 0; double S3 = 0; double S4 = 0;
            double Mean = 0; double Variance = 0;

            if (ds.Count < period)
                return;

            for (int bar = 0; bar <= period - 2; bar++)
            {
                S = ds[bar];
                S1 += S;
                S2 += Math.Pow(S, 2);
                S3 += (Math.Pow(S, 2) * S);
                S4 += (Math.Pow((Math.Pow(S, 2)), 2));
            }

            for (int bar = period - 1; bar <= ds.Count - 1; bar++)
            {
                S = ds[bar];
                S1 += S;
                S2 += Math.Pow(S, 2);
                S3 += (Math.Pow(S, 2) * S);
                S4 += (Math.Pow((Math.Pow(S, 2)), 2));

                Mean = S1 / period;
                Variance = (S2 - S1 * Mean) / (period - 1);
                if (Variance > 0)
                    base[bar] = (period * S4 - 4 * S3 * S1 + 6 * S2 * S1 * Mean - 3 * Math.Pow(S1 * Mean, 2)) * (period + 1)
                    / ((period - 1) * (period - 2) * (period - 3) * Math.Pow(Variance, 2))
                    - 3 * Math.Pow(period - 1, 2) / ((period - 2) * (period - 3));

                S = ds[bar - period + 1];
                S1 -= S;
                S2 -= Math.Pow(S, 2);
                S3 -= (Math.Pow(S, 2) * S);
                S4 -= (Math.Pow((Math.Pow(S, 2)), 2));
            }
        }

        public static Kurtosis Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "Kurtosis(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (Kurtosis)ds.Cache[description];
            }

            Kurtosis _Kurt = new Kurtosis(ds, period, description);
            ds.Cache[description] = _Kurt;
            return _Kurt;
        }
    }

    public class KurtosisHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static KurtosisHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(50, 4, 300) };
            _paramNames = new string[] { "Data Series", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
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
                return "Returns kurtosis (the distribution of observed data around the mean) of values from the specified DataSeries.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Kurtosis);
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
                return "Kurtosis";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www.investopedia.com/terms/k/kurtosis.asp";
            }
        }
    }
}