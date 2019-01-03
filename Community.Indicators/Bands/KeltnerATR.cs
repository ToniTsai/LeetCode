using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class KeltnerATR_Upper : DataSeries
    {
        public KeltnerATR_Upper(Bars bars, int smaPeriod, int atrPeriod, double atrMult, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(smaPeriod, atrPeriod * 3);

            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < Math.Max(smaPeriod, atrPeriod))
                return;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = Community.Indicators.FastSMA.Series(WealthLab.Indicators.AveragePriceC.Series(bars), smaPeriod)[bar] + atrMult * ATR.Series(bars, atrPeriod)[bar];
            }
        }

        public static KeltnerATR_Upper Series(Bars bars, int smaPeriod, int atrPeriod, double atrMult)
        {
            string description = string.Concat(new object[] { "Keltner ATR Upper Band(", smaPeriod, ",", atrPeriod, ",", atrMult, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (KeltnerATR_Upper)bars.Cache[description];
            }

            KeltnerATR_Upper _KeltnerATR_Upper = new KeltnerATR_Upper(bars, smaPeriod, atrPeriod, atrMult, description);
            bars.Cache[description] = _KeltnerATR_Upper;
            return _KeltnerATR_Upper;
        }
    }

    public class KeltnerATR_UpperHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static KeltnerATR_UpperHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(20, 5, 300), new RangeBoundInt32(20, 5, 300), new RangeBoundDouble(3.0, 0.5, 10) };
            _paramNames = new string[] { "Bars", "SMA Period", "ATR Period", "ATR Multiple" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override Color DefaultBandColor
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
                return "Keltner Channels are used to identify overbought/oversold conditions as well as the trend strength of a market. This modification uses an ATR multiple to construct the bands.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(KeltnerATR_Upper);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(KeltnerATR_Lower);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/KeltnerATR_Upper.ashx";
            }
        }
    }

    public class KeltnerATR_Lower : DataSeries
    {
        public KeltnerATR_Lower(Bars bars, int smaPeriod, int atrPeriod, double atrMult, string description)
            : base(bars, description)
        {
            base.FirstValidValue = Math.Max(smaPeriod, atrPeriod * 3);

            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < Math.Max(smaPeriod, atrPeriod))
                return;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = Community.Indicators.FastSMA.Series(WealthLab.Indicators.AveragePriceC.Series(bars), smaPeriod)[bar] - atrMult * ATR.Series(bars, atrPeriod)[bar];
            }
        }

        public static KeltnerATR_Lower Series(Bars bars, int smaPeriod, int atrPeriod, double atrMult)
        {
            string description = string.Concat(new object[] { "Keltner ATR Lower Band(", smaPeriod, ",", atrPeriod, ",", atrMult, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (KeltnerATR_Lower)bars.Cache[description];
            }

            KeltnerATR_Lower _KeltnerATR_Lower = new KeltnerATR_Lower(bars, smaPeriod, atrPeriod, atrMult, description);
            bars.Cache[description] = _KeltnerATR_Lower;
            return _KeltnerATR_Lower;
        }
    }

    public class KeltnerATR_LowerHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static KeltnerATR_LowerHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(20, 5, 300), new RangeBoundInt32(20, 5, 300), new RangeBoundDouble(3.0d, 0.5d, 10d) };
            _paramNames = new string[] { "Bars", "SMA Period", "ATR Period", "ATR Multiple" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override Color DefaultBandColor
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
                return "Keltner Channels are used to identify overbought/oversold conditions as well as the trend strength of a market. This modification uses an ATR multiple to construct the bands.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(KeltnerATR_Lower);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(KeltnerATR_Upper);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/KeltnerATR_Lower.ashx";
            }
        }
    }
}
