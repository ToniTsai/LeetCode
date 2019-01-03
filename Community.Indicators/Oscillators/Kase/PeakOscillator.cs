using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class PeakOscillator : DataSeries
    {
        public PeakOscillator(Bars bars, int fastPeriod, int slowPeriod, string description)
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
            //DataSeries KCD = Pk - Community.Indicators.FastSMA.Series(Pk, slowPeriod);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = Pk[bar];
            }
        }

        public static PeakOscillator Series(Bars bars, int fastPeriod, int slowPeriod)
        {
            string description = string.Concat(new object[] { "PeakOscillator(", fastPeriod, ",", slowPeriod, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (PeakOscillator)bars.Cache[description];
            }

            PeakOscillator _PeakOscillator = new PeakOscillator(bars, fastPeriod, slowPeriod, description);
            bars.Cache[description] = _PeakOscillator;
            return _PeakOscillator;
        }
    }

    public class PeakOscillatorHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PeakOscillatorHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(3, 2, 300), new RangeBoundInt32(8, 2, 300) };
            _paramNames = new string[] { "Bars", "Fast Period", "Slow Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
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
                return "Peak Oscillator by Cynthia Kase.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PeakOscillator);
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
                return "PeakOscillator";
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
