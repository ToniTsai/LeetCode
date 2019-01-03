using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class TSI : DataSeries
    {
        public TSI(DataSeries ds, int period1, int period2, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period1, period2) * 3;
            if (FirstValidValue <= 1) return;

            DataSeries mtm = Momentum.Series(ds, 1);
            DataSeries absmtm = DataSeries.Abs(mtm);
            DataSeries Numer = EMA.Series(EMA.Series(mtm, period1, EMACalculation.Modern), period2, EMACalculation.Modern);
            DataSeries Denom = EMA.Series(EMA.Series(absmtm, period1, EMACalculation.Modern), period2, EMACalculation.Modern);
            DataSeries TS = Numer / Denom;
            DataSeries TSI = TS * 100;

            for (int bar = base.FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = TSI[bar];
            }
        }

        public static TSI Series(DataSeries ds, int period1, int period2)
        {
            string description = string.Concat(new object[] { "True Strength Index(", ds.Description, ",", period1, ",", period2, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (TSI)ds.Cache[description];
            }

            TSI _TSI = new TSI(ds, period1, period2, description);
            ds.Cache[description] = _TSI;
            return _TSI;
        }
    }

    public class TSIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TSIHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(25, 2, 300), new RangeBoundInt32(13, 2, 300) };
            _paramNames = new string[] { "Data Series", "Period1", "Period2" };
        }

        public override string TargetPane
        {
            get
            {
                return "TSIPane";
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
                return "The True Strength Index (TSI) developed by William Blau is a momentum-based oscillator designed to determine both the trend and overbought-oversold conditions.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TSI);
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
                return -25;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 25;
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
                return "http://www2.wealth-lab.com/WL5Wiki/TSI.ashx";
            }
        }
    }
}