using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// DV2 Unbounded - Created by Robert Sucher
    /// </summary>
    public class DV2 : DataSeries
    {
        public DV2(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 2;

            DataSeries dv2 = bars.Close / AveragePrice.Series(bars) - 1;
            dv2 = (dv2 + (dv2 >> 1)) / 2d;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = dv2[bar];
            }
        }

        public static DV2 Series(Bars bars)
        {
            string description = string.Concat(new object[] { "DV2()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (DV2)bars.Cache[description];
            }

            DV2 _DV2 = new DV2(bars, description);
            bars.Cache[description] = _DV2;
            return _DV2;
        }
    }

    public class DV2Helper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DV2Helper()
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
                return "DV2 created by David Varadi is a short-term overbought/oversold indicator alternative to the 2-period RSI.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DV2);
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
                return "DV2";
            }
        }

        public override string URL
        {
            get
            {
                return "http://marketsci.wordpress.com/2009/07/15/varadi%E2%80%99s-rsi2-alternative-the-dv2/";
            }
        }
    }

    /// <summary>
    /// DV2 Bounded - Created by Michael Bytnar aka DartboardTrader
    /// </summary>
    public class PercentRange : DataSeries
    {
        public PercentRange(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            DataSeries r = ds * 0;
            r = ds / (Highest.Series(ds, period) - Lowest.Series(ds, period));

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = r[bar];
            }
        }

        public static PercentRange Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "PercentRange(", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (PercentRange)ds.Cache[description];
            }

            PercentRange _PercentRange = new PercentRange(ds, period, description);
            ds.Cache[description] = _PercentRange;
            return _PercentRange;
        }
    }

    public class DV2_Bounded : DataSeries
    {
        public DV2_Bounded(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = PercentRank.Series(ds, period)[bar];
            }
        }

        public static DV2_Bounded Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "DV2_Bounded(", ds.Description, ",", period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (DV2_Bounded)ds.Cache[description];
            }

            DV2_Bounded _DV2_Bounded = new DV2_Bounded(ds, period, description);
            ds.Cache[description] = _DV2_Bounded;
            return _DV2_Bounded;
        }
    }

    public class DV2_BoundedHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DV2_BoundedHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(252, 10, 300) };
            _paramNames = new string[] { "Data Series", "Period" };
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
                return "DV2 created by David Varadi is a short-term overbought/oversold indicator alternative to the 2-period RSI.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DV2_Bounded);
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
                return "DV2";
            }
        }

        public override string URL
        {
            get
            {
                return "http://marketsci.wordpress.com/2009/07/15/varadi%E2%80%99s-rsi2-alternative-the-dv2/";
            }
        }
    }

    /// <summary>
    /// DV2, PartialValue version - courtesy avishn
    /// </summary>
    public class DV2Partial : DataSeries
    {
        Bars bars;

        public DV2Partial(Bars bars, string description)
            : base(bars, description)
        {
            this.bars = bars;
            base.FirstValidValue = 1;
            DataSeries dv2 = bars.Close / (bars.High + bars.Low) * 2d - 1d;
            dv2 = (dv2 + (dv2 >> 1)) / 2d;
            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = dv2[bar];
            }
        }

        public static DV2Partial Series(Bars bars)
        {
            string description = string.Concat(new object[] { "DV2_avishn(", bars.Symbol, ")" });
            if (bars.Cache.ContainsKey(description))
            {
                return (DV2Partial)bars.Cache[description];
            }
            DV2Partial _DV2 = new DV2Partial(bars, description);
            bars.Cache[description] = _DV2;
            return _DV2;
        }

        public override void CalculatePartialValue()
        {
            if (bars.Count < (FirstValidValue + 1) || Double.IsNaN(bars.Close.PartialValue))
            {
                PartialValue = Double.NaN;
            }
            else
            {
                int lb = bars.Count - 1;
                PartialValue = ((bars.Close.PartialValue / (bars.High.PartialValue + bars.Low.PartialValue) * 2d - 1d)
                                + (bars.Close[lb] / (bars.High[lb] + bars.Low[lb]) * 2d - 1d)) / 2d;
            }
        }
    }

    public class DV2PartialHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DV2PartialHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
        }

        public override Color DefaultColor { get { return Color.Blue; } }
        public override string Description { get { return "DV2 (PartialValue version)"; } }
        public override Type IndicatorType { get { return typeof(DV2Partial); } }
        public override IList<object> ParameterDefaultValues { get { return _paramDefaults; } }
        public override IList<string> ParameterDescriptions { get { return _paramNames; } }
        public override string TargetPane { get { return "DV2"; } }
        public override string URL { get { return "http://marketsci.wordpress.com/2009/07/15/varadi%E2%80%99s-rsi2-alternative-the-dv2/"; } }

    }

    /// <summary>
    /// DV2 bounded, PartialValue version - courtesy avishn
    /// </summary>
    public class DV2Bounded : DataSeries
    {
        Bars bars;
        int period;
        DataSeries dv2;

        public DV2Bounded(Bars bars, int period, string description)
            : base(bars, description)
        {

            this.bars = bars;
            this.period = period;
            base.FirstValidValue = period + 2;

            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < period)
                return;

            dv2 = PrcRank.Series(DV2Partial.Series(bars), period);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = dv2[bar];
            }

        }

        public static DV2Bounded Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "DV2Bounded(", bars.Symbol, ",", period, ")" });
            if (bars.Cache.ContainsKey(description))
            {
                return (DV2Bounded)bars.Cache[description];
            }
            DV2Bounded _DV2Bounded = new DV2Bounded(bars, period, description);
            bars.Cache[description] = _DV2Bounded;
            return _DV2Bounded;
        }

        public override void CalculatePartialValue()
        {
            dv2.CalculatePartialValue();
            PartialValue = dv2.PartialValue;
        }
    }

    public class DV2BoundedHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DV2BoundedHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(252, 1, 300) };
            _paramNames = new string[] { "Bars", "Period" };
        }

        public override Color DefaultColor { get { return Color.Blue; } }
        public override string Description { get { return "DV2Bounded (PartialValue, corrected PercentRank version)"; } }
        public override Type IndicatorType { get { return typeof(DV2Bounded); } }
        public override IList<object> ParameterDefaultValues { get { return _paramDefaults; } }
        public override IList<string> ParameterDescriptions { get { return _paramNames; } }
        public override string TargetPane { get { return "DV2Bounded"; } }
        public override string URL { get { return "http://marketsci.wordpress.com/2009/07/15/varadi%E2%80%99s-rsi2-alternative-the-dv2/"; } }
    }
}