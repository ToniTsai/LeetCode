using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class SMI : DataSeries
    {
        /*
         * H = CurrentClose – (High MAX + Low MIN) / 2
         * 
         * HS1 =Exponential Moving Average(H,3)
         * HS2 = Exponential Moving Average(HS1,3)
         * 
         * DHL1 =Exponential Moving Average  (High MAX – Low MIN,3) 
         * DHL2 = Exponential Moving Average (High MAX – Low MIN, 3) / 2
         * 
         * SMI TODAY = (HS2 / DHL2) * 100
         */

        public SMI(Bars bars, int period1, int period2, int period3, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(Math.Max(period1, period2), period3);
            if (FirstValidValue <= 1) return;

            DataSeries h = Highest.Series(bars.High, period1);
            DataSeries l = Lowest.Series(bars.Low, period1);
            DataSeries c = (h + l) / 2;
            DataSeries H = bars.Close - c;
            DataSeries HS1 = EMA.Series(H, period2, EMACalculation.Modern);
            DataSeries HS2 = EMA.Series(HS1, period2, EMACalculation.Modern);
            DataSeries DHL1 = EMA.Series(h - l, period3, EMACalculation.Modern);
            DataSeries DHL2 = EMA.Series(h - l, period3, EMACalculation.Modern) / 2;
            DataSeries smi = (HS2 / DHL2) * 100;

            for (int bar = base.FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = smi[bar];
            }
        }

        public static SMI Series(Bars bars, int period1, int period2, int period3)
        {
            string description = string.Concat(new object[] { "SMI (", period1, ",", period2, ",", period3, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (SMI)bars.Cache[description];
            }

            SMI _SMI = new SMI(bars, period1, period2, period3, description);
            bars.Cache[description] = _SMI;
            return _SMI;
        }
    }

    public class SMIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SMIHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(10, 2, 300), new RangeBoundInt32(3, 2, 300), new RangeBoundInt32(3, 2, 300) };
            _paramNames = new string[] { "Bars", "Period 1", "Period 2", "Period 3" };
        }

        public override string TargetPane
        {
            get
            {
                return "SMIPane";
            }
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Fuchsia;
            }
        }

        public override string Description
        {
            get
            {
                return "Stochastic Momentum Index (SMI) by William Blau is advancement in the Stochastic Oscillator which shows the distance of current Close relative to the midpoint of High/Low Range.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SMI);
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

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 1;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return -40;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 40;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/SMI.ashx";
            }
        }
    }
}