using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class InSyncIndex : DataSeries
    {
        public InSyncIndex(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 20;

            if (bars.Count < 20)
                return;

            DataSeries BOLInSLB = StdDev.Series(bars.Close, 20, StdDevCalculation.Sample) * 2;
            BOLInSLB = Community.Indicators.FastSMA.Series(bars.Close, 20) - BOLInSLB;

            DataSeries BOLInSUB = StdDev.Series(bars.Close, 20, StdDevCalculation.Sample) * 2;
            BOLInSUB = Community.Indicators.FastSMA.Series(bars.Close, 20) + BOLInSUB;

            DataSeries BOLInS2 = BOLInSUB - BOLInSLB;
            BOLInS2 = (bars.Close - BOLInSLB) / BOLInS2;

            EMV emv = EMV.Series(bars, 13);
            DataSeries EMVSer = Community.Indicators.FastSMA.Series(emv, 10);
            DataSeries EMVInS2 = EMVSer - Community.Indicators.FastSMA.Series(EMVSer, 10);

            DataSeries MACDSer = Community.Indicators.FastSMA.Series(MACD.Series(bars.Close), 10);
            DataSeries MACDInS2 = MACD.Series(bars.Close) - MACDSer;

            int Period = 18;
            DataSeries DPOSeries = bars.Close - (Community.Indicators.FastSMA.Series(bars.Close, Period) >> ((Period / 2) + 1));

            DataSeries PDOSer = Community.Indicators.FastSMA.Series(DPOSeries, 10);
            DataSeries PDOInS2 = DPOSeries - PDOSer;

            DataSeries ROCSer = EMA.Series(ROC.Series(bars.Close, 10), 10, EMACalculation.Modern);
            DataSeries ROCInS2 = ROC.Series(bars.Close, 10) - ROCSer;

            DataSeries BOLInSLL = bars.Close * 0;
            DataSeries CCInS = bars.Close * 0;
            DataSeries EMVInSB = bars.Close * 0;
            DataSeries EMVInSS = bars.Close * 0;
            DataSeries MACDInSB = bars.Close * 0;
            DataSeries MACDInSS = bars.Close * 0;
            DataSeries MFIInS = bars.Close * 0;
            DataSeries PDOInSB = bars.Close * 0;
            DataSeries PDOInSS = bars.Close * 0;
            DataSeries ROCInSB = bars.Close * 0;
            DataSeries ROCInSS = bars.Close * 0;
            DataSeries RSIInS = bars.Close * 0;
            DataSeries STODInS = bars.Close * 0;
            DataSeries STOKInS = bars.Close * 0;

            for (int bar = base.FirstValidValue; bar < bars.Count; bar++)
            {
                if (BOLInS2[bar] < 0.05)
                    BOLInSLL[bar] = -5;
                else
                    if (BOLInS2[bar] > 0.95)
                        BOLInSLL[bar] = +5;

                if (CCI.Series(bars, 14)[bar] > +100)
                    CCInS[bar] = +5;
                else
                    if (CCI.Series(bars, 14)[bar] < -100)
                        CCInS[bar] = -5;

                if ((EMVInS2[bar] < 0) & (EMVSer[bar] < 0))
                    EMVInSB[bar] = -5;
                if ((EMVInS2[bar] > 0) & (EMVSer[bar] > 0))
                    EMVInSS[bar] = +5;

                if ((MACDInS2[bar] < 0) & (MACDSer[bar] < 0))
                    MACDInSB[bar] = -5;
                if ((MACDInS2[bar] > 0) & (MACDSer[bar] > 0))
                    MACDInSS[bar] = +5;

                if (MFI.Series(bars, 20)[bar] > 80)
                    MFIInS[bar] = +5;
                else
                    if (MFI.Series(bars, 20)[bar] < 20)
                        MFIInS[bar] = -5;

                if ((PDOInS2[bar] < 0) & (PDOSer[bar] < 0))
                    PDOInSB[bar] = -5;
                if ((PDOInS2[bar] > 0) & (PDOSer[bar] > 0))
                    PDOInSS[bar] = +5;

                if ((ROCInS2[bar] < 0) & (ROCSer[bar] < 0))
                    ROCInSB[bar] = -5;
                if ((ROCInS2[bar] > 0) & (ROCSer[bar] > 0))
                    ROCInSS[bar] = +5;

                if (RSI.Series(bars.Close, 14)[bar] > 70)
                    RSIInS[bar] = +5;
                else
                    if (RSI.Series(bars.Close, 14)[bar] < 30)
                        RSIInS[bar] = -5;

                if (StochD.Series(bars, 14, 3)[bar] > 80)
                    STODInS[bar] = +5;
                else
                    if (StochD.Series(bars, 14, 3)[bar] < 20)
                        STODInS[bar] = -5;

                if (StochK.Series(bars, 14)[bar] > 80)
                    STOKInS[bar] = +5;
                else
                    if (StochK.Series(bars, 14)[bar] < 20)
                        STOKInS[bar] = -5;

                base[bar] = 50 +
                    CCInS[bar] + BOLInSLL[bar] + RSIInS[bar]
                    + STODInS[bar] + MFIInS[bar] + EMVInSB[bar]
                    + EMVInSS[bar] + ROCInSS[bar] + ROCInSB[bar]
                    + STOKInS[bar] + MACDInSS[bar] + MACDInSB[bar]
                    + PDOInSS[bar - 10] + PDOInSB[bar - 10];
            }
        }

        public static InSyncIndex Series(Bars bars)
        {
            string description = string.Concat(new object[] { "InSyncIndex()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (InSyncIndex)bars.Cache[description];
            }

            InSyncIndex _InSyncIndex = new InSyncIndex(bars, description);
            bars.Cache[description] = _InSyncIndex;
            return _InSyncIndex;
        }
    }

    public class InSyncIndexHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static InSyncIndexHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
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
                return "The Insync Index by Norm North is a consensus indicator.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(InSyncIndex);
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
                return "InSyncIndex";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/InSyncIndex.ashx";
            }
        }
    }
}