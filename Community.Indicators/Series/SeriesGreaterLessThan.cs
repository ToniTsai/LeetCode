using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class SeriesLessThan : DataSeries
    {
        public SeriesLessThan(DataSeries ds, double value, string description)
            : base(ds, description)
        {
            DataSeries Result = new DataSeries(ds, "proxy(" + value + ")" );
            Result[0] = -Double.MinValue;

            for (int bar = 1; bar < ds.Count; bar++)
            {
                if ((ds[bar] < value) && (ds[bar - 1] >= value)) // XU
                    Result[bar] = bar;
                else
                    if ((ds[bar] > value) && (ds[bar - 1] <= value)) // XO
                        Result[bar] = 0;
                    else
                        Result[bar] = Result[bar - 1];
            }

            for (int i = 0; i < ds.Count; i++)
            {
                base[i] = Result[i];
            }
        }

        public static SeriesLessThan Series(DataSeries ds, double value)
        {
            string description = string.Concat(new object[] { ds.Description, " Less Than ", value });
            if (ds.Cache.ContainsKey(description))
            {
                return (SeriesLessThan)ds.Cache[description];
            }

            SeriesLessThan _SeriesLessThan = new SeriesLessThan(ds, value, description);
            ds.Cache[description] = _SeriesLessThan;
            return _SeriesLessThan;
        }
    }

    public class SeriesLessThanHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SeriesLessThanHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.High, new RangeBoundDouble(0, 0, 300) };
            _paramNames = new string[] { "Data Series", "Value" };
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
                return "Series Less Than Count is virtually a CrossUnderValueBar analogue, returning  the number of the bar when the Series has crossed below a specified value.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SeriesLessThan);
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return false;
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
                return "SeriesLessPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CrossUnderValueBar.ashx";
            }
        }
    }

    public class SeriesGreaterThan : DataSeries
    {
        public SeriesGreaterThan(DataSeries ds, double value, string description)
            : base(ds, description)
        {
            DataSeries Result = new DataSeries(ds, "proxy(" + value + ")");
            Result[0] = -Double.MinValue;

            for (int bar = 1; bar < ds.Count; bar++)
            {
                if ((ds[bar] > value) && (ds[bar - 1] <= value)) // XO
                    Result[bar] = bar;
                else
                    if ((ds[bar] < value) && (ds[bar - 1] >= value)) // XU                    
                        Result[bar] = 0;
                    else
                        Result[bar] = Result[bar - 1];
            }

            for (int bar = 0; bar < ds.Count; bar++)
            {
                base[bar] = Result[bar];
            }
        }

        public static SeriesGreaterThan Series(DataSeries ds, double value)
        {
            string description = string.Concat(new object[] { ds.Description, " Greater Than ", value });
            if (ds.Cache.ContainsKey(description))
            {
                return (SeriesGreaterThan)ds.Cache[description];
            }

            SeriesGreaterThan _SeriesGreaterThan = new SeriesGreaterThan(ds, value, description);
            ds.Cache[description] = _SeriesGreaterThan;
            return _SeriesGreaterThan;
        }
    }

    public class SeriesGreaterThanHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SeriesGreaterThanHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.High, new RangeBoundDouble(0, 0, 300) };
            _paramNames = new string[] { "Data Series", "Value" };
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
                return "Series Greater Than is virtually a CrossOverValueBar analogue, returning the number of the bar when the Series has crossed above a specified value.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SeriesGreaterThan);
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return false;
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
                return "SeriesGreaterPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CrossOverValueBar.ashx";
            }
        }
    }
}
