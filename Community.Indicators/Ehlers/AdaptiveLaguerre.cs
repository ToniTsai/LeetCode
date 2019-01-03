using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class AdaptiveLaguerre : DataSeries
    {
        public AdaptiveLaguerre(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period * 3;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            DataSeries Result = new DataSeries(ds, "Adaptive Laguerre");
            DataSeries Diff = new DataSeries(ds, "Diff");
            DataSeries tmp = new DataSeries(ds, "tmp");
            DataSeries L0 = new DataSeries(ds, "L0");
            DataSeries L1 = new DataSeries(ds, "L1");
            DataSeries L2 = new DataSeries(ds, "L2");
            DataSeries L3 = new DataSeries(ds, "L3");
            double HH = 0; double LL = 0; double temp = 0; int i = 0; double alpha = 0;
            double[] a_ord = new double[6];

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                Diff[bar] = Math.Abs(ds[bar] - Result[bar - 1]);
                HH = Diff[bar];
                LL = Diff[bar];

                for (int count = 0; count <= period - 1; count++)
                {
                    if (Diff[bar - count] > HH) HH = Diff[bar - count];
                    if (Diff[bar - count] < LL) LL = Diff[bar - count];
                }

                //Uses Gnome sorting algorithm to calculate the Median.
                tmp[bar] = (Diff[bar] - LL) / (HH - LL);
                for (i = 1; i <= 5; i++)
                    a_ord[i] = tmp[bar - i + 1];

                i = 0;
                while (i < 6)
                {
                    if ((i == 0) || (a_ord[i - 1] <= a_ord[i]))
                        ++i;
                    else
                    {
                        temp = a_ord[i];
                        a_ord[i] = a_ord[i - 1];
                        a_ord[i - 1] = temp;
                        --i;
                    }
                }
                alpha = a_ord[3];

                L0[bar] = alpha * ds[bar] + (1 - alpha) * L0[bar - 1];
                L1[bar] = -(1 - alpha) * L0[bar] + L0[bar - 1] + (1 - alpha) * L1[bar - 1];
                L2[bar] = -(1 - alpha) * L1[bar] + L1[bar - 1] + (1 - alpha) * L2[bar - 1];
                L3[bar] = -(1 - alpha) * L2[bar] + L2[bar - 1] + (1 - alpha) * L3[bar - 1];

                Result[bar] = (L0[bar] + 2 * L1[bar] + 2 * L2[bar] + L3[bar]) / 6;
                base[bar] = Result[bar];
            }
        }

        public static AdaptiveLaguerre Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "Adaptive Laguerre(", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (AdaptiveLaguerre)ds.Cache[description];
            }

            AdaptiveLaguerre _AdaptiveLaguerre = new AdaptiveLaguerre(ds, period, description);
            ds.Cache[description] = _AdaptiveLaguerre;
            return _AdaptiveLaguerre;
        }
    }

    public class AdaptiveLaguerreHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AdaptiveLaguerreHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(30, 2, 300) };
            _paramNames = new string[] { "DataSeries", "Period" };
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
                return "The Adaptive Laguerre filter by John Ehlers can be used as an adaptive moving average.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AdaptiveLaguerre);
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
                return "http://www2.wealth-lab.com/WL5Wiki/AdaptiveLaguerre.ashx";
            }
        }
    }
}
