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
    public class CTI : DataSeries
    {
        public CTI(Bars bars, DataSeries ds, int period, bool positiveOnly, string description)
            : base(ds, description)
        {
            Helper.CompatibilityCheck();

            base.FirstValidValue = period;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            #region Parallelization (removed in 2018.08 for crash/hang issues)

            //var rangePartitioner = Partitioner.Create(FirstValidValue, ds.Count);
            //var rangePartitioner = Partitioner.Create(0, ds.Count);

            //Parallel.ForEach(rangePartitioner, (range, loopState) =>
            //{
            //    for (int i = range.Item1; i < range.Item2; i++)
            //    {
            //        base[i] = LNRet.Series(bars, ds, period)[i] / (StdDev.Series(LNRet.Series(bars, ds, 1), period, StdDevCalculation.Sample)[i] * Math.Sqrt(period));
            //        if (positiveOnly)
            //            base[i] = Math.Abs(base[i]);
            //    }
            //});

            #endregion

            for (int i = 0; i < ds.Count; i++)
            {
                base[i] = LNRet.Series(bars, ds, period)[i] / (StdDev.Series(LNRet.Series(bars, ds, 1), period, StdDevCalculation.Sample)[i] * Math.Sqrt(period));
                if (positiveOnly)
                    base[i] = Math.Abs(base[i]);
            }
        }

        public static CTI Series(Bars bars, DataSeries ds, int period, bool positiveOnly)
        {
            string description = string.Concat(new object[] { "CTI(", ds.Description, ",", period, ",", positiveOnly, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (CTI)ds.Cache[description];
            }

            CTI _CTI = new CTI(bars, ds, period, positiveOnly, description);
            ds.Cache[description] = _CTI;
            return _CTI;
        }
    }

    public class CTIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CTIHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, CoreDataSeries.Close, new RangeBoundInt32(14, 2, 300), new Boolean() };
            _paramNames = new string[] { "Bars", "Data Series", "Period", "Positive Only" };
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
                return "Chande Trend Index was created by Tushar S. Chande. This indicator is based on some ideas from quantitative finance, " +
                    "giving a somewhat noisy measure of the trendiness or mean reverting behaviour of the data series.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CTI);
            }
        }

        public override string TargetPane
        {
            get
            {
                return "CTI";
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
                return "http://www2.wealth-lab.com/WL5Wiki/CTI.ashx";
            }
        }
    }
}