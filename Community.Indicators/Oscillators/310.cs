using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class LBR3_10 : DataSeries
    {
        public LBR3_10(DataSeries ds, int period1, int period2, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period1, period2);
            Community.Indicators.FastSMA sma1 = Community.Indicators.FastSMA.Series(ds, period1);
            Community.Indicators.FastSMA sma2 = Community.Indicators.FastSMA.Series(ds, period2);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = sma1[bar] - sma2[bar];
            }
        }

        public static LBR3_10 Series(DataSeries ds, int period1, int period2)
        {
            string description = string.Concat(new object[] { "3/10 Oscillator(", ds.Description, ",", period1, ",", period2, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (LBR3_10)ds.Cache[description];
            }

            LBR3_10 _LBR3_10 = new LBR3_10(ds, period1, period2, description);
            ds.Cache[description] = _LBR3_10;
            return _LBR3_10;
        }
    }

    public class LBR3_10Helper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static LBR3_10Helper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(3, 2, 300), new RangeBoundInt32(10, 2, 300) };
            _paramNames = new string[] { "Data Series", "Shorter SMA", "Longer SMA" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
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
                return "Linda Raschke's 3/10 Oscillator is essentially a MACD built with shorter period simple moving averages.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(LBR3_10);
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
                return "LBR3_10";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/LBR3_10.ashx";
            }
        }
    }

    public class LBR3_10_Histogram : DataSeries
    {
        public LBR3_10_Histogram(DataSeries ds, int period1, int period2, int period3, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period1, Math.Max(period2, period3));
            LBR3_10 lbr3_10 = new LBR3_10(ds, period1, period2, description);
            LBR3_10_Signal sigLine = new LBR3_10_Signal(ds, period1, period2, period3, description);
            DataSeries lbrOscHist = lbr3_10 - sigLine;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = lbrOscHist[bar];
            }
        }

        public static LBR3_10_Histogram Series(DataSeries ds, int period1, int period2, int period3)
        {
            string description = string.Concat(new object[] { "3/10 Oscillator Histogram(", ds.Description, ",", period1, ",", period2, ",", period3, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (LBR3_10_Histogram)ds.Cache[description];
            }

            LBR3_10_Histogram _LBR3_10_Histogram = new LBR3_10_Histogram(ds, period1, period2, period3, description);
            ds.Cache[description] = _LBR3_10_Histogram;
            return _LBR3_10_Histogram;
        }
    }

    public class LBR3_10_HistogramHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static LBR3_10_HistogramHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(3, 2, 300), new RangeBoundInt32(10, 2, 300), new RangeBoundInt32(16, 2, 300) };
            _paramNames = new string[] { "Data Series", "Shorter SMA", "Longer SMA", "Signal Line SMA" };
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
                return 1;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Histogram;
            }

        }

        public override string Description
        {
            get
            {
                return "This is a customizable 3/10 Oscillator Histogram.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(LBR3_10_Histogram);
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
                return "LBR3_10";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/LBR3_10.ashx";
            }
        }
    }

    public class LBR3_10_Signal : DataSeries
    {
        public LBR3_10_Signal(DataSeries ds, int period1, int period2, int period3, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period3, Math.Max(period1, period2));
            LBR3_10 lbrOsc = new LBR3_10(ds, period1, period2, description);
            Community.Indicators.FastSMA sma = Community.Indicators.FastSMA.Series(lbrOsc, period3);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = sma[bar];
            }
        }

        public static LBR3_10_Signal Series(DataSeries ds, int period1, int period2, int period3)
        {
            string description = string.Concat(new object[] { "3/10 Oscillator Signal(", ds.Description, ",", period1, ",", period2, ",", period3, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (LBR3_10_Signal)ds.Cache[description];
            }

            LBR3_10_Signal _LBR3_10_Signal = new LBR3_10_Signal(ds, period1, period2, period3, description);
            ds.Cache[description] = _LBR3_10_Signal;
            return _LBR3_10_Signal;
        }
    }

    public class LBR3_10_SignalHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static LBR3_10_SignalHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(3, 2, 300), new RangeBoundInt32(10, 2, 300), new RangeBoundInt32(16, 2, 300) };
            _paramNames = new string[] { "Data Series", "Shorter SMA", "Longer SMA", "Signal SMA period" };
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
                return "This is the Signal Line of the 3/10 Oscillator. The Signal line SMA period is configurable.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(LBR3_10_Signal);
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
                return "LBR3_10";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/LBR3_10.ashx";
            }
        }
    }
}
