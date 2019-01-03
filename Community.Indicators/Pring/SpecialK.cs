using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class SpecialK_Daily : DataSeries
    {
        public SpecialK_Daily(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 530;

            // Daily constants
            int[] a = { 10, 15, 20, 30, 50, 65, 75, 100, 195, 265, 390, 530 };
            int[] b = { 10, 10, 10, 15, 50, 65, 75, 100, 130, 130, 130, 195 };
            int[] c = { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 };

            DataSeries sKd = bars.Close - bars.Close;

            for (int k = 0; k < 12; k++)
            {
                sKd += Community.Indicators.FastSMA.Series(ROC.Series(bars.Close, a[k]), b[k]) * c[k];
            }

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = sKd[bar];
            }
        }

        public static SpecialK_Daily Series(Bars bars)
        {
            string description = string.Concat(new object[] { "Special K Daily()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (SpecialK_Daily)bars.Cache[description];
            }

            SpecialK_Daily _SpecialK_Daily = new SpecialK_Daily(bars, description);
            bars.Cache[description] = _SpecialK_Daily;
            return _SpecialK_Daily;
        }
    }

    public class SpecialK_DailyHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SpecialK_DailyHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
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
                return "The Special K by Martin Pring is a new momentum indicator that identifies primary trend reversals.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SpecialK_Daily);
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
                return "SpecialK Daily";
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www.pring.com/";
            }
        }
    }

    public class SpecialK_Weekly : DataSeries
    {
        public SpecialK_Weekly(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 104 * 3;

            // Weekly constants
            int[] A = { 4, 5, 6, 8, 10, 13, 15, 20, 39, 52, 78, 104 };
            int[] B = { 4, 5, 6, 6, 10, 13, 15, 20, 26, 26, 26, 39 };
            int[] C = { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 };

            DataSeries sKw = bars.Close - bars.Close;

            for (int k = 0; k < 12; k++)
            {
                sKw += EMA.Series(ROC.Series(bars.Close, A[k]), B[k], EMACalculation.Modern) * C[k];
            }

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = sKw[bar];
            }
        }

        public static SpecialK_Weekly Series(Bars bars)
        {
            string description = string.Concat(new object[] { "Special K Weekly()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (SpecialK_Weekly)bars.Cache[description];
            }

            SpecialK_Weekly _SpecialK_Weekly = new SpecialK_Weekly(bars, description);
            bars.Cache[description] = _SpecialK_Weekly;
            return _SpecialK_Weekly;
        }
    }

    public class SpecialK_WeeklyHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SpecialK_WeeklyHelper()
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
                return "The Special K by Martin Pring is a new momentum indicator that identifies primary trend reversals.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SpecialK_Weekly);
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
                return "SpecialK Weekly";
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www.pring.com/";
            }
        }
    }
}
