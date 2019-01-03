using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class PBFastOsc : DataSeries
    {
        public PBFastOsc(Bars bars, int period, int FSmooth, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(FSmooth, period) * 3;

            PBandLower PBL = PBandLower.Series(bars, period);
            PBandUpper PBU = PBandUpper.Series(bars, period);

            DataSeries A = bars.Close - PBL;
            DataSeries B = PBU - PBL;
            DataSeries Result = EMA.Series((A / B) * 100, FSmooth, EMACalculation.Modern);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = Result[bar];
            }
        }

        public static PBFastOsc Series(Bars bars, int period, int FSmooth)
        {
            string description = string.Concat(new object[] { "Projection Bands Fast Oscillator(", period, ",", FSmooth, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (PBFastOsc)bars.Cache[description];
            }

            PBFastOsc _PBFastOsc = new PBFastOsc(bars, period, FSmooth, description);
            bars.Cache[description] = _PBFastOsc;
            return _PBFastOsc;
        }
    }

    public class PBFastOscHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PBFastOscHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300), new RangeBoundInt32(5, 2, 300) };
            _paramNames = new string[] { "Bars", "Period", "Smoothing factor" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
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
                return "The Projection Oscillator by Mel Widner is a companion to his Projection Bands. Essentially, it's a Stochastic oscillator adjusted to linear regression slope, which makes it more responsive to short-term price moves.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PBFastOsc);
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
                return "PBOsc";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ProjectionBands.ashx";
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 80;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return 20;
            }
        }

    }

    public class PBSlowOsc : DataSeries
    {
        public PBSlowOsc(Bars bars, int period, int FSmooth, int SSmooth, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(SSmooth, Math.Max(FSmooth, period)) * 3;

            PBandLower PBL = PBandLower.Series(bars, period);
            PBandUpper PBU = PBandUpper.Series(bars, period);

            DataSeries A = bars.Close - PBL;
            DataSeries B = PBU - PBL;
            DataSeries Result = EMA.Series((A / B) * 100, FSmooth, EMACalculation.Modern);
            Result = EMA.Series(Result, SSmooth, EMACalculation.Modern);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = Result[bar];
            }
        }

        public static PBSlowOsc Series(Bars bars, int period, int FSmooth, int SSmooth)
        {
            string description = string.Concat(new object[] { "Projection Bands Slow Oscillator(", period, ",", FSmooth, ",", SSmooth, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (PBSlowOsc)bars.Cache[description];
            }

            PBSlowOsc _PBSlowOsc = new PBSlowOsc(bars, period, FSmooth, SSmooth, description);
            bars.Cache[description] = _PBSlowOsc;
            return _PBSlowOsc;
        }
    }

    public class PBSlowOscHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PBSlowOscHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300), new RangeBoundInt32(5, 2, 300), new RangeBoundInt32(3, 2, 300) };
            _paramNames = new string[] { "Bars", "Period", "Fast Smoothing", "Slow Smoothing" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
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
                return "The Projection Oscillator by Mel Widner is a companion to his Projection Bands. Essentially, it's a Stochastic oscillator adjusted to linear regression slope, which makes it more responsive to short-term price moves.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PBSlowOsc);
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
                return "PBOsc";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ProjectionBands.ashx";
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 80;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return 20;
            }
        }

    }

    public class PBBandWidth : DataSeries
    {
        public PBBandWidth(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;

            PBandLower PBL = PBandLower.Series(bars, period);
            PBandUpper PBU = PBandUpper.Series(bars, period);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = 200 * (PBU[bar] - PBL[bar]) / (PBU[bar] + PBL[bar]);
            }
        }

        public static PBBandWidth Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "Projection Bands Width(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (PBBandWidth)bars.Cache[description];
            }

            PBBandWidth _PBBandWidth = new PBBandWidth(bars, period, description);
            bars.Cache[description] = _PBBandWidth;
            return _PBBandWidth;
        }
    }

    public class PBBandWidthHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PBBandWidthHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) };
            _paramNames = new string[] { "Bars", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
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
                return "The Projection Band Width indicator is a companion to Mel Widner's Projection Bands, " +
                    "measuring the difference between the Upper and Lower Projection bands and hence, the underlying market's volatility.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PBBandWidth);
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
                return "PBBandWidth";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ProjectionBands.ashx";
            }
        }

        /*public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 80;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return 20;
            }
        }*/

    }
}