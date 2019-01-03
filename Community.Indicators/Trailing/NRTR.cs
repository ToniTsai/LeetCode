using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class NRTR_Percent : DataSeries
    {
        public NRTR_Percent(Bars bars, double K, string description)
            : base(bars, description)
        {
            base.FirstValidValue = bars.FirstActualBar;

            int Trend = 0;
            double Reverse = 0;
            double HPrice = 0;
            double LPrice = 0;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                // Calculate the NRTR% Series
                if (Trend >= 0)
                {
                    HPrice = Math.Max(bars.Close[bar], HPrice);
                    Reverse = HPrice * (1 - K * 0.01);
                    if (bars.Close[bar] <= Reverse)
                    {
                        Trend = -1;
                        LPrice = bars.Close[bar];
                        Reverse = LPrice * (1 + K * 0.01);
                    }
                }
                if (Trend <= 0)
                {
                    LPrice = Math.Min(bars.Close[bar], LPrice);
                    Reverse = LPrice * (1 + K * 0.01);
                    if (bars.Close[bar] >= Reverse)
                    {
                        Trend = 1;
                        HPrice = bars.Close[bar];
                        Reverse = HPrice * (1 - K * 0.01);
                    }
                }
                base[bar] = Reverse;
            }
        }

        public static NRTR_Percent Series(Bars bars, double K)
        {
            string description = string.Concat(new object[] { "NRTR% (", K, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (NRTR_Percent)bars.Cache[description];
            }

            NRTR_Percent _NRTR_Percent = new NRTR_Percent(bars, K, description);
            bars.Cache[description] = _NRTR_Percent;
            return _NRTR_Percent;
        }
    }

    public class NRTR_PercentHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static NRTR_PercentHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundDouble(10, 0.5, 50) };
            _paramNames = new string[] { "Bars", "Percentage" };
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
                return "The NRTR % by Konstantin Kopyrkin is a trailing reverse indicator based on the percentage of price.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(NRTR_Percent);
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

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/NRTRPct.ashx";
            }
        }
    }

    public class NRTR_WATR : DataSeries
    {
        public NRTR_WATR(Bars bars, int lookback, double multiple, string description)
            : base(bars, description)
        {
            base.FirstValidValue = lookback;

            int Trend = 0;
            double Reverse = 0;
            double HPrice = 0;
            double LPrice = 0;

            DataSeries K = WMA.Series(TrueRange.Series(bars), lookback) * multiple;

            for (int bar = base.FirstValidValue; bar < bars.Count; bar++)
            {
                // Calculate the NRTR_WATR Series
                if (Trend >= 0)
                {
                    HPrice = Math.Max(bars.Close[bar], HPrice);
                    Reverse = HPrice - K[bar];

                    if (bars.Close[bar] <= Reverse)
                    {
                        Trend = -1;
                        LPrice = bars.Close[bar];
                        Reverse = LPrice + K[bar];
                    }
                }
                if (Trend <= 0)
                {
                    LPrice = Math.Min(bars.Close[bar], LPrice);
                    Reverse = LPrice + K[bar];

                    if (bars.Close[bar] >= Reverse)
                    {
                        Trend = 1;
                        HPrice = bars.Close[bar];
                        Reverse = HPrice - K[bar];
                    }
                }

                base[bar] = Reverse;
            }
        }

        public static NRTR_WATR Series(Bars bars, int lookback, double multiple)
        {
            string description = string.Concat(new object[] { "NRTR_WATR(", lookback, ",", multiple, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (NRTR_WATR)bars.Cache[description];
            }

            NRTR_WATR _NRTR_WATR = new NRTR_WATR(bars, lookback, multiple, description);
            bars.Cache[description] = _NRTR_WATR;
            return _NRTR_WATR;
        }
    }

    public class NRTR_WATRHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static NRTR_WATRHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(20, 5, 300), new RangeBoundDouble(3, 0.5, 10) };
            _paramNames = new string[] { "Bars", "WATR period", "WATR Multiplier" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 2;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Dotted;
            }
        }

        public override string Description
        {
            get
            {
                return "The NRTR_WATR indicator by Konstantin Kopyrkin is an adaptive variation of the trailing reverse technique (NRTR%.)";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(NRTR_WATR);
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

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/NRTR_WATR.ashx";
            }
        }
    }
}