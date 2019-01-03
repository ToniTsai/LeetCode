using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class TwiggsMoneyFlow : DataSeries
    {
        public TwiggsMoneyFlow(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;

            double k = (period - 1d) / period;
            double AD = 0;
            double V = 0;
            double TMF = 0;

            for (int bar = 1; bar < bars.Count; bar++)
            {
                double TrueHigh = Math.Max(bars.High[bar], bars.Close[bar - 1]);
                double TrueLow = Math.Min(bars.Low[bar], bars.Close[bar - 1]);

                if (bar > period)
                {
                    AD *= k;
                    V *= k;
                }

                if (TrueHigh > TrueLow)
                    AD = AD + bars.Volume[bar] * ((bars.Close[bar] - TrueLow) - (TrueHigh - bars.Close[bar])) / (TrueHigh - TrueLow);
                V += bars.Volume[bar];
                if (V > 0)
                    TMF = AD / V;

                base[bar] = TMF;
            }
        }

        public static TwiggsMoneyFlow Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "Twigg's Money Flow(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (TwiggsMoneyFlow)bars.Cache[description];
            }

            TwiggsMoneyFlow _TwiggsMoneyFlow = new TwiggsMoneyFlow(bars, period, description);
            bars.Cache[description] = _TwiggsMoneyFlow;
            return _TwiggsMoneyFlow;
        }
    }

    public class TwiggsMoneyFlowHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TwiggsMoneyFlowHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(21, 2, 300) };
            _paramNames = new string[] { "Bars", "Period" };
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

        public override string Description
        {
            get
            {
                return "Twigg's Money Flow, created by Colin Twiggs, is an improvement to Chaikin Money Flow. It is used to signal of potential breakouts and confirm trends.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TwiggsMoneyFlow);
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
                return "TwiggsMoneyFlow";
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www.incrediblecharts.com/indicators/twiggs_money_flow.php";
            }
        }
    }
}