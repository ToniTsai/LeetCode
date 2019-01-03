using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class UltimateOsc2 : DataSeries
    {
        public UltimateOsc2(Bars bars, int period1, int period2, int period3, string description)
            : base(bars, description)
        {
            /*
            Calculate Today's "True Low (TL)". TL = the lower of today's low or yesterday's close.
            Calculate Today's "Buying Pressure (BP)". BP = Today's close - Today's TL.
            Calculate Today's "True Range (TR)". TR = the higher of 
                1.) Today's High - Today's Low; 
                2.) Today's High - Yesterday's Close; 
                3.) Yesterday's Close - Today's Low.
            Calculate BPSum1, BPSum2, and BPSum3 by adding up all of the BPs for each of the three specified time frames.
            Calculate TRSum1, TRSum2, and TRSum3 by adding up all of the TR's for each of the three specified time frames.

            The Raw Ultimate Oscillator (RawUO) is equal to:
            4 * (BPSum1 / TRSum1) + 2 * (BPSum2 / TRSum2) + (BPSum3 / TRSum3)
        
            The Final Ultimate Oscillator is equal to:
            ( RawUO / (4 + 2 + 1) ) * 100
            */

            base.FirstValidValue = Math.Max(Math.Max(period1, period2), period3);
            if (FirstValidValue == 1) return;

            if (bars.Count < Math.Max(period3, Math.Max(period1, period2)))
                return;

            for (int bar = base.FirstValidValue; bar < bars.Count; bar++)
            {
                double BPSum1 = 0; double BPSum2 = 0; double BPSum3 = 0;
                double TRSum1 = 0; double TRSum2 = 0; double TRSum3 = 0;
                for (int i = 0; i < period1; i++)
                {
                    BPSum1 += (bars.Close[bar - i] - Math.Min(bars.Low[bar - i], bars.Close[bar - i - 1]));
                    TRSum1 += Math.Max(Math.Max(bars.High[bar - i] - bars.Low[bar - i],
                        bars.High[bar - i] - bars.Close[bar - i - 1]), bars.Close[bar - i - 1] - bars.Low[bar - i]);
                }
                for (int i = 0; i < period2; i++)
                {
                    BPSum2 += (bars.Close[bar - i] - Math.Min(bars.Low[bar - i], bars.Close[bar - i - 1]));
                    TRSum2 += Math.Max(Math.Max(bars.High[bar - i] - bars.Low[bar - i],
                        bars.High[bar - i] - bars.Close[bar - i - 1]), bars.Close[bar - i - 1] - bars.Low[bar - i]);
                }
                for (int i = 0; i < period3; i++)
                {
                    BPSum3 += (bars.Close[bar - i] - Math.Min(bars.Low[bar - i], bars.Close[bar - i - 1]));
                    TRSum3 += Math.Max(Math.Max(bars.High[bar - i] - bars.Low[bar - i],
                        bars.High[bar - i] - bars.Close[bar - i - 1]), bars.Close[bar - i - 1] - bars.Low[bar - i]);
                }

                double RawUO = 4 * (BPSum1 / TRSum1) + 2 * (BPSum2 / TRSum2) + (BPSum3 / TRSum3);
                double val = (RawUO / (4 + 2 + 1)) * 100;

                if (!double.IsNaN(val))
                    base[bar] = val;
                else
                    //base[bar] = 0;
                    base[bar] = base[bar - 1];
            }
        }

        public static UltimateOsc2 Series(Bars bars, int period1, int period2, int period3)
        {
            string description = string.Concat(new object[] { "UltimateOsc2(", period1, ",", period2, ",", period3, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (UltimateOsc2)bars.Cache[description];
            }

            UltimateOsc2 _UltimateOsc2 = new UltimateOsc2(bars, period1, period2, period3, description);
            bars.Cache[description] = _UltimateOsc2;
            return _UltimateOsc2;
        }
    }

    public class UltimateOsc2Helper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static UltimateOsc2Helper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(7, 1, 300), new RangeBoundInt32(14, 1, 300), new RangeBoundInt32(28, 1, 300) };
            _paramNames = new string[] { "Bars", "Period1", "Period2", "Period3" };
        }

        public override string TargetPane
        {
            get
            {
                return "UltimateOsc2Pane";
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
                return "This is a version of Ultimate Oscillator by Larry Williams that accepts variable lookback periods.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(UltimateOsc2);
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
                return "http://www2.wealth-lab.com/WL5Wiki/UltimateOsc2.ashx";
            }
        }
    }

}
