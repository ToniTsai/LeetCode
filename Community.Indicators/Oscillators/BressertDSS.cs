using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class BressertDSS : DataSeries
    {
        public BressertDSS(Bars bars, int periodStochastic, int periodEMA, string description)
            : base(bars, description)
        {
            var X = EMAModern.Series(StochK.Series(bars, periodEMA), periodStochastic);
            var bDSS = EMAModern.Series(((X - Lowest.Series(X, periodEMA)) / (Highest.Series(X, periodEMA) - Lowest.Series(X, periodEMA)) * 100d), periodStochastic);

            base.FirstValidValue = Math.Max(periodEMA, periodStochastic);
            if (FirstValidValue == 1) return;

            if (bars.Count < Math.Max(periodStochastic, periodEMA))
                return;

            for (int bar = base.FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = bDSS[bar];
            }
        }

        public static BressertDSS Series(Bars bars, int periodStochastic, int periodEMA)
        {
            string description = string.Concat(new object[] { "BressertDSS(", periodStochastic, ",", periodEMA, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (BressertDSS)bars.Cache[description];
            }

            BressertDSS _BressertDSS = new BressertDSS(bars, periodStochastic, periodEMA, description);
            bars.Cache[description] = _BressertDSS;
            return _BressertDSS;
        }
    }

    public class BressertDSSHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static BressertDSSHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(13, 2, 300), new RangeBoundInt32(8, 2, 300) };
            _paramNames = new string[] { "Bars", "Stochastic Period", "EMA Period" };
        }

        public override string TargetPane
        {
            get
            {
                return "BressertDSSPane";
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
                return "Bressert DSS is a Stochastic-like oscillator created by Walter Bressert.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(BressertDSS);
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
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

        public override double OscillatorOversoldValue
        {
            get
            {
                return 20;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 80;
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
                return "http://www2.wealth-lab.com/WL5Wiki/BressertDSS.ashx";
            }
        }
    }

}
