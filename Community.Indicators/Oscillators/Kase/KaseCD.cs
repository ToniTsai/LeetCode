using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class KaseCD : DataSeries
    {
        public KaseCD(Bars bars, int fastPeriod, int slowPeriod, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(fastPeriod, slowPeriod);

            DataSeries RWH = new DataSeries(bars, "RWH");
            DataSeries RWL = new DataSeries(bars, "RWL");

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                RWH[bar] = (((bars.High[bar] - bars.Low[bar - slowPeriod])) / ((ATR.Series(bars, slowPeriod)[bar] * Math.Sqrt(slowPeriod))));
                RWL[bar] = (((bars.High[bar - slowPeriod] - bars.Low[bar])) / ((ATR.Series(bars, slowPeriod)[bar] * Math.Sqrt(slowPeriod))));
            }

            DataSeries Pk = Community.Indicators.FastSMA.Series(WMA.Series((RWH - RWL), fastPeriod), fastPeriod);
            DataSeries KCD = Pk - Community.Indicators.FastSMA.Series(Pk, slowPeriod);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = KCD[bar];
            }
        }

        public static KaseCD Series(Bars bars, int fastPeriod, int slowPeriod)
        {
            string description = string.Concat(new object[] { "KaseCD Oscillator(", fastPeriod, ",", slowPeriod, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (KaseCD)bars.Cache[description];
            }

            KaseCD _KaseCD = new KaseCD(bars, fastPeriod, slowPeriod, description);
            bars.Cache[description] = _KaseCD;
            return _KaseCD;
        }
    }

    public class KaseCDHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static KaseCDHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(3, 2, 300), new RangeBoundInt32(8, 2, 300) };
            _paramNames = new string[] { "Bars", "Fast Period", "Slow Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Histogram;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 2;
            }
        }

        public override string Description
        {
            get
            {
                return "Kase CD Oscillator by Cynthia Kase.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(KaseCD);
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
                return "KaseCD";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www.mql4.com/go?http://www.aspenres.com/documents/pdf/kasestudies.pdf";
            }
        }
    }

}
