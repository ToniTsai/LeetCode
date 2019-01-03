using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Linq;

namespace Community.Indicators
{
    public class MAD : DataSeries
    {
        public MAD(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            Helper.CompatibilityCheck();

            FirstValidValue = period;

            if (ds.Count < period)
                return;

            //Based on https://stackoverflow.com/questions/43610304/			
            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                List<double> lst = new List<double>(period);
                for (int i = bar; i > bar - period; i--)
                    lst.Add(ds[i]);

                double[] source = lst.ToArray();

                Array.Sort(source);

                double med = source.Length % 2 == 0
                    ? (source[source.Length / 2 - 1] + source[source.Length / 2]) / 2.0
                    : source[source.Length / 2];

                double[] d = source
                    .Select(x => Math.Abs(x - med))
                    .OrderBy(x => x)
                    .ToArray();

                double MADe = 1.483 * (d.Length % 2 == 0
                    ? (d[d.Length / 2 - 1] + d[d.Length / 2]) / 2.0
                    : d[d.Length / 2]);

                base[bar] = MADe;
            }
        }

        public static MAD Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "MedianAbsoluteDeviation(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (MAD)ds.Cache[description];
            }

            MAD _mad = new MAD(ds, period, description);
            ds.Cache[description] = _mad;
            return _mad;
        }
    }

    public class MADHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MADHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200) };
            _paramNames = new string[] { "Data Series", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
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
                return "Returns the Median Absolute Deviation (MAD) of the specified DataSeries which is a robust measure of how spread out the price is. The MAD is less affected by extremely high or extremely low values and non normality than standard deviation.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MAD);
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
                return "MAD";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www.statisticshowto.com/median-absolute-deviation/";
            }
        }
    }
}