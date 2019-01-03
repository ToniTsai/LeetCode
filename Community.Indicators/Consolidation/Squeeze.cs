using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Squeeze : DataSeries
    {
        public Squeeze(Bars bars, DataSeries ds, int period, double dev, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;

            BBandUpper bbu = BBandUpper.Series(ds, period, dev);
            BBandLower bbl = BBandLower.Series(ds, period, dev);
            KeltnerUpper ku = KeltnerUpper.Series(bars, period, period);
            KeltnerLower kl = KeltnerLower.Series(bars, period, period);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = (bbu[bar] - bbl[bar]) - (ku[bar] - kl[bar]);
            }
        }

        public static Squeeze Series(Bars bars, DataSeries ds, int period, double dev)
        {
            string description = string.Concat(new object[] { "Squeeze(", ds.Description, ",", period, ",", dev, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (Squeeze)bars.Cache[description];
            }

            Squeeze _Squeeze = new Squeeze(bars, ds, period, dev, description);
            bars.Cache[description] = _Squeeze;
            return _Squeeze;
        }
    }

    public class SqueezeHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SqueezeHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, CoreDataSeries.Close, 
                new RangeBoundInt32(20, 2, 300), new RangeBoundDouble(2, 0.5, 5) };
            _paramNames = new string[] { "Bars", "Series", "Indicator Period", "Deviations" };
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
                return 2;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Histogram;
            }

        }

        public override string Description
        {
            get
            {
                return "The Squeeze indicator by John F. Carter measures a contraction of the Bollinger Bands " +
                    "inside the Keltner Bands. Reflecting the market consolidation, it could be a potential " +
                    "leading indicator of subsequent directional movement.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Squeeze);
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
                return "Squeeze";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Squeeze.ashx";
            }
        }
    }
}