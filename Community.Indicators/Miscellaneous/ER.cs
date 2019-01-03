using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Windows.Forms;

namespace Community.Indicators
{
    /// <summary>
    /// Kaufman's Efficiency Ratio
    /// </summary>
    public class ER : DataSeries
    {
        public ER(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period + 1;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            double ER1 = 0; double ER2 = 0;

            for (int bar = base.FirstValidValue; bar < ds.Count; bar++)
            {
                ER1 = Math.Abs(ds[bar] - ds[bar - period]);
                ER2 = 0.0;

                for (int p = 0; p <= period; p++)
                {
                    ER2 += Math.Abs(ds[bar - p - 0] - ds[bar - p - 1]);
                }

                if (ER2 != 0)
                    base[bar] = ER1 / ER2;
            }
        }

        public static ER Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "ER(", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (ER)ds.Cache[description];
            }

            ER _ER = new ER(ds, period, description);
            ds.Cache[description] = _ER;
            return _ER;
        }
    }

    public class ERHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ERHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 300) };
            _paramNames = new string[] { "Data Series", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
            }
        }

        public override string Description
        {
            get
            {
                return "Efficiency ratio by Perry Kaufman is a ratio of the price direction to its volatility, helping to find more efficient, smoother, faster trends. It's defined as the net price change over a period divided by the sum of absolute price changes for the same period.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ER);
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return 0.3;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 0.6;
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
                return "ERPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ER.ashx";
            }
        }
    }
}
