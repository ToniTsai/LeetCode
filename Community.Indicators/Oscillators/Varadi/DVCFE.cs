using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// Composite Fractal Efficiency Indicator (CFE)
    /// </summary>
    public class DVCFE : DataSeries
    {
        public DVCFE(Bars bars, int period1, int period2, int rankPeriod, string description)
            : base(bars, description)
        {
            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < Math.Max(rankPeriod, Math.Max(period1, period2)))
                return;

            //Step 1
            DataSeries PriceChange = DataSeries.Abs(ROC.Series(bars.Close, 1));
            //Step 2
            DataSeries TenDayPriceChange = bars.Close - (bars.Close >> period1);
            DataSeries FirstRatio = (TenDayPriceChange / Sum.Series(PriceChange, period1)) * -1;
            //Step 3
            DataSeries TwoFiftyDayPriceChange = bars.Close - (bars.Close >> period2);
            DataSeries SecondRatio = (TwoFiftyDayPriceChange / Sum.Series(PriceChange, period2));
            //Step 4
            DataSeries Average = (FirstRatio + SecondRatio) / 2;
            DataSeries DVCFE = PrcRank.Series(Average, rankPeriod);

            base.FirstValidValue = Math.Max(Math.Max(period1, period2), rankPeriod);
            if (FirstValidValue == 1) return;

            for (int bar = base.FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = DVCFE[bar];
            }
        }

        public static DVCFE Series(Bars bars, int period1, int period2, int rankPeriod)
        {
            string description = string.Concat(new object[] { "DV Composite Fractal Efficiency Indicator (CFE)(", period1, ",", period2, ",", rankPeriod, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (DVCFE)bars.Cache[description];
            }

            DVCFE _DVCFE = new DVCFE(bars, period1, period2, rankPeriod, description);
            bars.Cache[description] = _DVCFE;
            return _DVCFE;
        }
    }

    public class DVCFEHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DVCFEHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(10, 2, 300), new RangeBoundInt32(250, 2, 500), new RangeBoundInt32(252, 2, 300), };
            _paramNames = new string[] { "Bars", "Period 1", "Period 2", "PercentRank Period" };
        }

        public override string TargetPane
        {
            get
            {
                return "DVCFEPane";
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
                return "Composite Fractal Efficiency oscillator created by David Varadi is a composite mean-reversion and trend indicator that measures fractal efficiency at two different time frames.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DVCFE);
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
                return 0.25;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 0.75;
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
                return "http://cssanalytics.wordpress.com/2009/11/16/code-for-composite-fractal-efficiency-indicator-cfe/";
            }
        }
    }
}