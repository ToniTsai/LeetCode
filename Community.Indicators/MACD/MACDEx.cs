using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class MACDEx : DataSeries
    {
        public MACDEx(DataSeries ds, int period1, int period2, string description): base(ds, description)
        {
            base.FirstValidValue = Math.Max(period1, period2) * 3;

            if (FirstValidValue > ds.Count || FirstValidValue < 0) 
                FirstValidValue = ds.Count;
            if (ds.Count < Math.Max(period1,period2)) 
                    return;

            EMACalculation m = EMACalculation.Modern;
            EMA ema1 = EMA.Series(ds, period1, m);
            EMA ema2 = EMA.Series(ds, period2, m);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = ema1[bar] - ema2[bar];
            }
        }

        public static MACDEx Series ( DataSeries ds, int period1, int period2 )
        {
            string description = string.Concat(new object[] { "MACDEx(", ds.Description, ",", period1, ",", period2, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (MACDEx)ds.Cache[description];
            }

            MACDEx _MACDEx = new MACDEx(ds, period1, period2, description);
            ds.Cache[description] = _MACDEx;
            return _MACDEx;
        }
    }

    public class MACDExHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MACDExHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, 
                new RangeBoundInt32(12, 2, 300), new RangeBoundInt32(26, 2, 300) };
            _paramNames = new string[] { "Series", "Shorter EMA", "Longer EMA" };
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
                return "This is an 'extended' MACD indicator that lets you specify the periods for the 2 moving averages.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MACDEx);
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
                return "MACDEx";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MACDEx.ashx";
            }
        }
    }

    public class MACDEx_Histogram : DataSeries
    {
        public MACDEx_Histogram(DataSeries ds, int period1, int period2, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period1, period2) * 3;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < Math.Max(period1, period2))
                return;

            MACDEx macdex = new MACDEx(ds, period1, period2, description);
            MACDEx_Signal sigLine = new MACDEx_Signal(ds, period1, period2, description);
            DataSeries macdHist = macdex - sigLine;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = macdHist[bar];
            }
        }

        public static MACDEx_Histogram Series(DataSeries ds, int period1, int period2)
        {
            string description = string.Concat(new object[] { "MACDEx Histogram(", ds.Description, ",", period1, ",", period2, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (MACDEx_Histogram)ds.Cache[description];
            }

            MACDEx_Histogram _MACDEx_Histogram = new MACDEx_Histogram(ds, period1, period2, description);
            ds.Cache[description] = _MACDEx_Histogram;
            return _MACDEx_Histogram;
        }
    }

    public class MACDEx_HistogramHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MACDEx_HistogramHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, 
                new RangeBoundInt32(12, 2, 300), new RangeBoundInt32(26, 2, 300) };
            _paramNames = new string[] { "Series", "Shorter EMA", "Longer EMA" };
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
                return LineStyle.Histogram;
            }

        }

        public override string Description
        {
            get
            {
                return "This is a customizable MACD Histogram indicator.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MACDEx_Histogram);
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
                return "MACDEx";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MACDEx_Histogram.ashx";
            }
        }
    }

    public class MACDEx_Signal : DataSeries
    {
        public MACDEx_Signal(DataSeries ds, int period1, int period2, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(9, Math.Max(period1, period2)) * 3;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < Math.Max(period1, period2))
                return;

            EMACalculation m = EMACalculation.Modern;
            MACDEx macdex = new MACDEx(ds, period1, period2, description);
            EMA ema = EMA.Series(macdex, 9, m);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = ema[bar];
            }
        }

        public static MACDEx_Signal Series(DataSeries ds, int period1, int period2)
        {
            string description = string.Concat(new object[] { "MACDEx Signal(", ds.Description, ",", period1, ",", period2, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (MACDEx_Signal)ds.Cache[description];
            }

            MACDEx_Signal _MACDEx_Signal = new MACDEx_Signal(ds, period1, period2, description);
            ds.Cache[description] = _MACDEx_Signal;
            return _MACDEx_Signal;
        }
    }

    public class MACDEx_SignalHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MACDEx_SignalHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, 
                new RangeBoundInt32(12, 2, 300), new RangeBoundInt32(26, 2, 300) };
            _paramNames = new string[] { "Series", "Shorter EMA", "Longer EMA" };
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
                return "This is the Signal Line of the MACDEx indicator.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MACDEx_Signal);
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
                return "MACDEx";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MACDEx_Signal.ashx";
            }
        }
    }

    public class MACDEx_Signal3 : DataSeries
    {
        public MACDEx_Signal3(DataSeries ds, int period1, int period2, int period3, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period3, Math.Max(period1, period2)) * 3;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < Math.Max(period1, period2))
                return;

            EMACalculation m = EMACalculation.Modern;
            MACDEx macdex = new MACDEx(ds, period1, period2, description);
            EMA ema = EMA.Series(macdex, period3, m);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = ema[bar];
            }
        }

        public static MACDEx_Signal3 Series(DataSeries ds, int period1, int period2, int period3)
        {
            string description = string.Concat(new object[] { "MACDEx Signal(", ds.Description, ",", period1, ",", period2, ",", period3, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (MACDEx_Signal3)ds.Cache[description];
            }

            MACDEx_Signal3 _MACDEx_Signal = new MACDEx_Signal3(ds, period1, period2, period3, description);
            ds.Cache[description] = _MACDEx_Signal;
            return _MACDEx_Signal;
        }
    }

    public class MACDEx_Signal3Helper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MACDEx_Signal3Helper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, 
                new RangeBoundInt32(12, 2, 300), new RangeBoundInt32(26, 2, 300), new RangeBoundInt32(9, 2, 300) };
            _paramNames = new string[] { "Series", "Shorter EMA", "Longer EMA", "Signal EMA period" };
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
                return "This is the Signal Line of the MACDEx indicator. The only difference from MACDEx_Signal is that you can set the Signal EMA period.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MACDEx_Signal3);
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
                return "MACDEx";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MACDEx_Signal3.ashx";
            }
        }
    }

    public class MACDEx_Histogram3 : DataSeries
    {
        public MACDEx_Histogram3(DataSeries ds, int period1, int period2, int period3, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period1, period2) * 3;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < Math.Max(period1, period2))
                return;

            MACDEx macdex = new MACDEx(ds, period1, period2, description);
            MACDEx_Signal3 sigLine = new MACDEx_Signal3(ds, period1, period2, period3, description);
            DataSeries macdHist = macdex - sigLine;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = macdHist[bar];
            }
        }

        public static MACDEx_Histogram3 Series(DataSeries ds, int period1, int period2, int period3)
        {
            string description = string.Concat(new object[] { "MACDEx Histogram3(", ds.Description, ",", period1, ",", period2, ",", period3, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (MACDEx_Histogram3)ds.Cache[description];
            }

            MACDEx_Histogram3 _MACDEx_Histogram3 = new MACDEx_Histogram3(ds, period1, period2, period3, description);
            ds.Cache[description] = _MACDEx_Histogram3;
            return _MACDEx_Histogram3;
        }
    }

    public class MACDEx_Histogram3Helper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MACDEx_Histogram3Helper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(12, 2, 300), 
                new RangeBoundInt32(26, 2, 300), new RangeBoundInt32(9, 2, 300) };
            _paramNames = new string[] { "Series", "Shorter EMA", "Longer EMA", "Signal EMA period" };
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
                return "This is a customizable MACD Histogram indicator.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MACDEx_Histogram3);
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
                return "MACDEx";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/MACD.ashx";
            }
        }
    }
}