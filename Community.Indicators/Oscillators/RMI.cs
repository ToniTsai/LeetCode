using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// RMI (courtesy dansmo)
    /// </summary>
    public class RMI : DataSeries
    {
        public RMI(DataSeries ds, int len1, int len2, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(len1, len2);
            if (FirstValidValue < 2) return;

            if (ds.Count < Math.Max(len1, len2))
                return;

            // dansmo version

            double u1 = 0;
            double d1 = 0;
            DataSeries u1Series = new DataSeries(ds, "u1Series(" + ds.Description + "," + len1 + "," + len2 +")");
            DataSeries d1Series = new DataSeries(ds, "d1Series(" + ds.Description + "," + len1 + "," + len2 + ")");
            DataSeries ugSeries = new DataSeries(ds, "ugSeries(" + ds.Description + "," + len1 + "," + len2 + ")");
            DataSeries dgSeries = new DataSeries(ds, "dgSeries(" + ds.Description + "," + len1 + "," + len2 + ")");
            DataSeries rmiSeries = new DataSeries(ds, "rmiSeries(" + ds.Description + "," + len1 + "," + len2 + ")");

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                if (ds[bar] > ds[bar - len1])
                    u1 = ds[bar] - ds[bar - len1];
                else
                    u1 = 0;

                if (ds[bar] < ds[bar - len1])
                    d1 = Math.Abs(ds[bar] - ds[bar - len1]);
                else
                    d1 = 0;

                u1Series[bar] = u1;
                d1Series[bar] = d1;
            }

            ugSeries = WilderMA.Series(u1Series, len2);
            dgSeries = WilderMA.Series(d1Series, len2);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = 100.0 - (100.0 / (1 + ugSeries[bar] / dgSeries[bar]));
            }

            // fundtimer's version

            /*for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = RSI.Series(Momentum.Series(ds, len1), len2)[bar];
            }*/

            // dansmo's version, rescaled

            /*double u1 = 0;
            double d1 = 0;
            DataSeries u1Series = new DataSeries(ds, "u1Series(" + ds.Description + "," + len1 + "," + len2 +")");
            DataSeries d1Series = new DataSeries(ds, "d1Series(" + ds.Description + "," + len1 + "," + len2 + ")");
            DataSeries ugSeries = new DataSeries(ds, "ugSeries(" + ds.Description + "," + len1 + "," + len2 + ")");
            DataSeries dgSeries = new DataSeries(ds, "dgSeries(" + ds.Description + "," + len1 + "," + len2 + ")");

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                if (ds[bar] > ds[bar - len1])
                    u1 = ds[bar] - ds[bar - len1];
                else
                    u1 = 0;

                if (ds[bar] < ds[bar - len1])
                    d1 = Math.Abs(ds[bar] - ds[bar - len1]);
                else
                    d1 = 0;

                u1Series[bar] = u1;
                d1Series[bar] = d1;
            }

            ugSeries = WilderMA.Series(u1Series, len2);
            dgSeries = WilderMA.Series(d1Series, len2);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                double rm = ugSeries[bar] / dgSeries[bar];
                base[bar] = rm / (1.0 + rm);
            }*/
        }

        public static RMI Series(DataSeries ds, int period, int smoothPeriod)
        {
            string description = string.Concat(new object[] { "RMI(", ds.Description, ",", period, ",", smoothPeriod, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (RMI)ds.Cache[description];
            }

            RMI _RMI = new RMI(ds, period, smoothPeriod, description);
            ds.Cache[description] = _RMI;
            return _RMI;
        }
    }

    public class RMIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static RMIHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(5, 1, 300), new RangeBoundInt32(13, 1, 300) };
            _paramNames = new string[] { "DataSeries", "Period", "Smoothing Period" };
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
                return "The Relative Momentum Index, developed by Roger Altman, is a variation of the RSI indicator.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(RMI);
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
                return "RMIPane";
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
                return 30;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 70;
            }
        }

        public override Color OscillatorOversoldColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override Color OscillatorOverboughtColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www2.wealth-lab.com/WL5WIKI/RMI.ashx";
            }
        }
    }
}