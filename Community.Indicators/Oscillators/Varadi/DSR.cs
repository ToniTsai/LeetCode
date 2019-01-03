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
    public class DSR : DataSeries
    {
        // http://www.codeproject.com/KB/recipes/DescriptiveStatisticClass.aspx
        // modified by Eugene to work with Lists
        /// <summary>
        /// Calculate percentile of a sorted data set
        /// </summary>
        /// <param name="data">List of double</param>
        /// <param name="p">percentile</param>
        /// <returns></returns>
        public double percentile(List<double> data, double p)
        {
            data.Sort();

            // algo derived from Aczel pg 15 bottom
            if (p >= 100.0d) return data[data.Count - 1];

            double position = (double)(data.Count + 1) * p / 100.0;
            double leftNumber = 0.0d, rightNumber = 0.0d;

            double n = p / 100.0d * (data.Count - 1) + 1.0d;

            if (position >= 1)
            {
                leftNumber = data[(int)System.Math.Floor(n) - 1];
                rightNumber = data[(int)System.Math.Floor(n)];
            }
            else
            {
                leftNumber = data[0]; // first data
                rightNumber = data[1]; // first data
            }

            if (leftNumber == rightNumber)
                return leftNumber;
            else
            {
                double part = n - System.Math.Floor(n);
                return leftNumber + part * (rightNumber - leftNumber);
            }
        }

        public DSR(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 252;
            List<double> lst = new List<double>();
            DataSeries rawDSR = new DataSeries(bars, "raw dsr(" + period + ")");

            for (int bar = period; bar < bars.Count; bar++)
            {
                lst.Clear();

                for (int i = bar; i > bar - period; i--)
                {
                    lst.Add(bars.High[i]); lst.Add(bars.Low[i]); lst.Add(bars.Close[i]);
                }

                //DSR differential=  average(  65th percentile (H,L,C, 20-days), 80th percentile  (H,L,C, 20-days)) –  
                //average(  35th percentile (H,L,C, 20-days), 20th percentile  (H,L,C, 20-days))

                double dsrDiff = (percentile(lst, 65) + percentile(lst, 80) / 2) - (percentile(lst, 35) + percentile(lst, 20) / 2);

                //DSR raw=  DSR differential/(max(H,L,C, 20-days)-min(H,L,C, 20-days))

                double dsrRaw = dsrDiff / (Highest.Series(bars.High, period)[bar] - Lowest.Series(bars.Low, period)[bar]);
                rawDSR[bar] = dsrRaw;
            }

            //DSR= 252-day percentrank of ( 10-day sma of DSR raw ) 
            var rangePartitioner = Partitioner.Create(FirstValidValue, bars.Count);

            Parallel.ForEach(rangePartitioner, (range, loopState) =>
            {
                for (int bar = range.Item1; bar < range.Item2; bar++)
                {
                    base[bar] = PercentRank.Series(Community.Indicators.FastSMA.Series(rawDSR, 10), 252)[bar];
                }
            });
        }

        public static DSR Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "DSR(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (DSR)bars.Cache[description];
            }

            DSR _DSR = new DSR(bars, period, description);
            bars.Cache[description] = _DSR;
            return _DSR;
        }
    }

    public class DSRHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DSRHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(20, 2, 252) };
            _paramNames = new string[] { "Bars", "Period" };
        }

        public override string TargetPane
        {
            get
            {
                return "DSRPane";
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
                return "DSR by David Varadi trend distribution indicator that considers the skew of prices at higher moments versus the skew of prices at lower moments in relation to the price range.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DSR);
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
                return "http://cssanalytics.wordpress.com/2010/11/01/another-short-term-trend-indicator-dsr-distribution-skew-versus-range/";
            }
        }
    }
}