using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class MSR : DataSeries
    {
        static double GetMedian(double[] pNumbers)
        {
            int size = pNumbers.Length;
            int mid = size / 2;
            double median = (size % 2 != 0) ? (double)pNumbers[mid] :
                ((double)pNumbers[mid] + (double)pNumbers[mid - 1]) / 2;
            return median;
        }

        static double GetMedian(List<double> pNumbers)
        {
            return GetMedian(pNumbers.ToArray());
        }

        public MSR(Bars bars, int per1, int per2, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 252;

            //MSR= (10-day median of (H, L, C) – 20-day  MAX (H, L, C))/(20-day  MAX (H, L, C))
            //then take the 252-day percentrank of MSR or percentile ranking

            DataSeries msrTemp = new DataSeries(bars, "msrTemp(" + per1 + "," + per2 + ")" );

            for (int bar = Math.Max(per1, per2); bar < bars.Count; bar++)
            {
                double max20 = Highest.Series(bars.High, per2)[bar];
                List<double> prices = new List<double>(per1 * 3);
                for (int i = bar; i > bar - per1; i--)
                {
                    prices.Add(bars.High[i]); prices.Add(bars.Low[i]); prices.Add(bars.Close[i]);
                }

                prices.Sort();
                msrTemp[bar] = (GetMedian(prices) - max20) / max20;
            }

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = PercentRank.Series(msrTemp, 252)[bar];
                //base[bar] = PrcRank.Series(msrTemp, 252)[bar];
            }
        }

        public static MSR Series(Bars bars, int per1, int per2)
        {
            string description = string.Concat(new object[] { "MSR(", per1, ",", per2, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (MSR)bars.Cache[description];
            }

            MSR _MSR = new MSR(bars, per1, per2, description);
            bars.Cache[description] = _MSR;
            return _MSR;
        }
    }

    public class MSRHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MSRHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(10, 2, 252), new RangeBoundInt32(20, 2, 252) };
            _paramNames = new string[] { "Bars", "Period 1", "Period 2" };
        }

        public override string TargetPane
        {
            get
            {
                return "MSRPane";
            }
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
                return "MSR by David Varadi is a support and resistance indicator that captures the typical or middle price over a short to intermediate time frame.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MSR);
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
                return "http://cssanalytics.wordpress.com/2010/10/27/a-new-trend-indicator-msr/";
            }
        }
    }
}