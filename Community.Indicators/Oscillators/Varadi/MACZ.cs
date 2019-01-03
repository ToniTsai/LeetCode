using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class MACZ : DataSeries
    {
        public MACZ(DataSeries ds, int period1, int period2, double A, double B, StdDevCalculation sdc, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period1, period2);

            if (base.FirstValidValue < 2)
                base.FirstValidValue = 2;

            FirstValidValue *= 3; // EMA, the component of MACD, is an unstable indicator

            DataSeries sma = Community.Indicators.FastSMA.Series(ds, period2);
            StdDev sd = StdDev.Series(ds, period2, sdc);
            DataSeries ZScore = (ds - sma) / sd;
            DataSeries macd = EMA.Series(ds, period1, EMACalculation.Modern) - EMA.Series(ds, period2, EMACalculation.Modern);

            DataSeries MACZ = (ZScore * A) + (macd / sd * B);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = MACZ[bar];
            }
        }

        public static MACZ Series(DataSeries ds, int period1, int period2, double A, double B, StdDevCalculation sdc)
        {
            string description = string.Concat(new object[] { "MACZ(", ds.Description, ",", period1, ",", period2, ",", 
                A, ",", B, ",", sdc, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (MACZ)ds.Cache[description];
            }

            MACZ _MACZ = new MACZ(ds, period1, period2, A, B, sdc, description);
            ds.Cache[description] = _MACZ;
            return _MACZ;
        }
    }

    public class MACZHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MACZHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(15, 2, 300), new RangeBoundInt32(25, 2, 300), 
                new RangeBoundDouble(-2, -2, 2), new RangeBoundDouble(2, -2, 2),  StdDevCalculation.Sample };
            _paramNames = new string[] { "DataSeries", "Period1", "Period2", "A", "B", "StdDev calculation" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Purple;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 3;
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
                return "The MAC-Z by David Varadi is an oscillator that combines the separate mean-reversion and trend components within a given time frame.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MACZ);
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
                return "MACZ";
            }
        }

        public override string URL
        {
            get
            {
                return "http://cssanalytics.wordpress.com/2010/05/11/the-relationship-between-the-macd-and-z-score-creating-the-mac-z-score/";
            }
        }
    }
}