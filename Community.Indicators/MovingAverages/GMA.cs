using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class GMA : DataSeries
    {
        public GMA(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            FirstValidValue = period - 1;

            for (int bar = period - 1; bar < ds.Count; bar++)
            {
                double product = 1d;
                for (int i = bar - period + 1; i <= bar; i++)
                {
                    product *= ds[i];
                }
                base[bar] = Math.Pow(product, 1d / (double)period);
            }

        }

        public static GMA Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "GMA(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (GMA)ds.Cache[description];
            }
            GMA _gma = new GMA(ds, period, description);
            ds.Cache[description] = _gma;
            return _gma;
        }

    }

    public class GMAHelper : IndicatorHelper
    {

        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static GMAHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(1, 5, 300) };
            _paramNames = new string[] { "DataSeries", "Period" };
        }

        public override Color DefaultColor { get { return Color.Black; } }
        public override int DefaultWidth { get { return 1; } }
        public override string Description { get { return "Geometric moving average (geometric mean)"; } }
        public override Type IndicatorType { get { return typeof(GMA); } }
        public override IList<object> ParameterDefaultValues { get { return _paramDefaults; } }
        public override IList<string> ParameterDescriptions { get { return _paramNames; } }
    }
}