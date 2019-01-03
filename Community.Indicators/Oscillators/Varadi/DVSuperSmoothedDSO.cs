using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class DVSuperSmoothedDSO : DataSeries
    {
        public DVSuperSmoothedDSO(Bars bars, int period1, int period2, string description)
            : base(bars, description)
        {
            StochK step1 = StochK.Series(bars, period1);
            DataSeries step2 = (step1 - Lowest.Series(step1, period1)) / (Highest.Series(step1, period1) - Lowest.Series(step1, period1));
            DataSeries step3 = Community.Indicators.FastSMA.Series(step2, period2);
            DataSeries DV_SSDSO = 0.85 * step3 + 0.15 * (step3 >> 1);

            base.FirstValidValue = Math.Max(period1, period2);
            if (FirstValidValue == 1) return;

            for (int bar = base.FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = DV_SSDSO[bar] * 100;
            }
        }

        public static DVSuperSmoothedDSO Series(Bars bars, int period1, int period2)
        {
            string description = string.Concat(new object[] { "DV Super Smoothed Double Stochastic(", period1, ",", period2, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (DVSuperSmoothedDSO)bars.Cache[description];
            }

            DVSuperSmoothedDSO _DVSuperSmoothedDSO = new DVSuperSmoothedDSO(bars, period1, period2, description);
            bars.Cache[description] = _DVSuperSmoothedDSO;
            return _DVSuperSmoothedDSO;
        }
    }

    public class DVSuperSmoothedDSOHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DVSuperSmoothedDSOHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(10, 1, 300), new RangeBoundInt32(3, 1, 300) };
            _paramNames = new string[] { "Bars", "Stochastic Period", "Smoothing Period" };
        }

        public override string TargetPane
        {
            get
            {
                return "DVSuperSmoothedDSOPane";
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
                return "DV Super Smoothed Double Stochastic Oscillator created by David Varadi is smooth and responsive to cycles. " +
                    "We have multipled the resulting number by 100 to be easily compared to Stochastic.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DVSuperSmoothedDSO);
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
                return "http://cssanalytics.wordpress.com/2009/09/11/calculation-dv-super-smoothed-double-stochastic-oscillator/";
            }
        }
    }
}