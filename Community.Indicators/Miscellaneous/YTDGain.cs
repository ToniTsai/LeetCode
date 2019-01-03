using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Windows.Forms;

namespace Community.Indicators
{
    public class YTDGain : DataSeries
    {
        public YTDGain(DataSeries ds, string description)
            : base(ds, description)
        {
            base.FirstValidValue = 1;

            int yr = ds.Date[0].Year;
            double LastYearClose = 0;

            for (int bar = base.FirstValidValue; bar < ds.Count; bar++)
            {
                if (ds.Date[bar].Year != yr)
                {
                    yr = ds.Date[bar].Year;
                    LastYearClose = ds[bar - 1];
                }

                if (LastYearClose != 0)
                    base[bar] = 100 * (ds[bar] / LastYearClose - 1);
                else
                    base[bar] = 0;
            }
        }

        public static YTDGain Series(DataSeries ds)
        {
            string description = string.Concat(new object[] { "Year-To-Date Gain(", ds.Description, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (YTDGain)ds.Cache[description];
            }

            YTDGain _YTDGain = new YTDGain(ds, description);
            ds.Cache[description] = _YTDGain;
            return _YTDGain;
        }
    }

    public class YTDGainHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static YTDGainHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close };
            _paramNames = new string[] { "Data Series" };
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
                return "This indicator calculates the YTD (Year To Date) change, in %, of a symbol's last Closing price compared to the last Close of the previous year.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(YTDGain);
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
                return "YTDGain";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/YTDGain.ashx";
            }
        }
    }
}