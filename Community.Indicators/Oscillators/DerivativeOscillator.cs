using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class DerivativeOscillator : DataSeries
    {
        public DerivativeOscillator(DataSeries ds, int periodRSI, int periodEMA1, int periodEMA2, int periodSMA, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(Math.Max(Math.Max(periodEMA1, periodEMA2), periodSMA), periodRSI);
            if (FirstValidValue <= 1) return;

            EMACalculation em = EMACalculation.Modern;
            RSI rsi = RSI.Series(ds, periodRSI);
            EMA rsiEma1 = EMA.Series(rsi, periodEMA1, em);
            EMA rsiEma2 = EMA.Series(rsiEma1, periodEMA2, em);
            DataSeries rsiSma = Community.Indicators.FastSMA.Series(rsiEma2, periodSMA);
            DataSeries derivOsc = rsiEma2 - rsiSma;

            for (int bar = base.FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = derivOsc[bar];
            }
        }

        public static DerivativeOscillator Series(DataSeries ds, int periodRSI, int periodEMA1, int periodEMA2, int periodSMA)
        {
            string description = string.Concat(new object[] { "Derivative Oscillator (", ds.Description, ",", periodRSI, ",", periodEMA1, ",", periodEMA2, ",", periodSMA, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (DerivativeOscillator)ds.Cache[description];
            }

            DerivativeOscillator _DerivativeOscillator = new DerivativeOscillator(ds, periodRSI, periodEMA1, periodEMA2, periodSMA, description);
            ds.Cache[description] = _DerivativeOscillator;
            return _DerivativeOscillator;
        }
    }

    public class DerivativeOscillatorHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DerivativeOscillatorHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(14, 2, 300), new RangeBoundInt32(5, 2, 300), new RangeBoundInt32(3, 2, 300), new RangeBoundInt32(9, 2, 300) };
            _paramNames = new string[] { "Data Series", "RSI Period", "EMA1 Period", "EMA2 Period", "SMA Period" };
        }

        public override string TargetPane
        {
            get
            {
                return "DerivativeOscillatorPane";
            }
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Navy;
            }
        }

        public override string Description
        {
            get
            {
                return "Derivative Oscillator developed by Constance Brown is a triple smoothed RSI that incorporates two EMAs and one SMA. For its description, see its Wiki page.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DerivativeOscillator);
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
                return LineStyle.Histogram;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 3;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/DerivativeOscillator.ashx";
            }
        }
    }
}