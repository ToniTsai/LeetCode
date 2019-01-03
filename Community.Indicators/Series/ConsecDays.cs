using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// Glitch
    /// </summary>
    public class ConsecDaysDown : DataSeries
    {
        public ConsecDaysDown(DataSeries ds, double pct, string description)
            : base(ds, description)
        {
            DataSeries roc = ROC.Series(ds, 1);

            for (int i = ds.Count - 1; i >= 0; i--)
            {
                int cd = 0;
                int b = i;
                while (b >= 0 && roc[b] < -pct)
                {
                    cd++;
                    b--;
                }
                base[i] = cd;
            }
        }

        public static ConsecDaysDown Series(DataSeries ds, double pct)
        {
            string description = string.Concat(new object[] { "Consecutive Days Down(", ds.Description, ",", pct, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (ConsecDaysDown)ds.Cache[description];
            }

            ConsecDaysDown _ConsecDaysDown = new ConsecDaysDown(ds, pct, description);
            ds.Cache[description] = _ConsecDaysDown;
            return _ConsecDaysDown;
        }
    }

    public class ConsecDaysDownHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ConsecDaysDownHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundDouble(5.0, 0.5, 25) };
            _paramNames = new string[] { "Data Series", "Percent decline" };
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
                return 5;
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
                return "This indicator displays the number of consecutive days where prices declined by a specified percent or more.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ConsecDaysDown);
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
                return "ConsecDays";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ConsecDays.ashx";
            }
        }
    }

    /// <summary>
    /// Glitch
    /// </summary>
    public class ConsecDaysUp : DataSeries
    {
        public ConsecDaysUp(DataSeries ds, double pct, string description)
            : base(ds, description)
        {
            DataSeries roc = ROC.Series(ds, 1);

            for (int i = ds.Count - 1; i >= 0; i--)
            {
                int cd = 0;
                int b = i;
                while (b >= 0 && roc[b] > pct)
                {
                    cd++;
                    b--;
                }
                base[i] = cd;
            }
        }

        public static ConsecDaysUp Series(DataSeries ds, double pct)
        {
            string description = string.Concat(new object[] { "Consecutive Days Up(", ds.Description, ",", pct, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (ConsecDaysUp)ds.Cache[description];
            }

            ConsecDaysUp _ConsecDaysUp = new ConsecDaysUp(ds, pct, description);
            ds.Cache[description] = _ConsecDaysUp;
            return _ConsecDaysUp;
        }
    }

    public class ConsecDaysUpHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ConsecDaysUpHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundDouble(5.0, 0.5, 25) };
            _paramNames = new string[] { "Data Series", "Percent rise" };
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
                return 5;
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
                return "This indicator displays the number of consecutive days where prices rose by a specified percent or more.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ConsecDaysUp);
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
                return "ConsecDays";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ConsecDays.ashx";
            }
        }
    }
}