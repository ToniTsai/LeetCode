using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class SeriesIsAbove : DataSeries
    {
        public SeriesIsAbove(DataSeries ds1, DataSeries ds2, int period, string description)
            : base(ds1, description)
        {
            int lp = Math.Min(ds1.Count, ds2.Count);
            DataSeries delta = ds1 - ds2;

            if (period > lp) return;

            int i = 0;
            for (int bar = period; bar < lp; bar++)
            {
                if (Lowest.Series(delta, period)[bar] > 0)
                    i++;
                else
                    i = 0;
                base[bar] = i;
            }
        }

        public static SeriesIsAbove Series(DataSeries ds1, DataSeries ds2, int period)
        {
            string description = string.Concat(new object[] { "Series is Above(", ds1.Description, ",", ds2.Description, ",", period, ")" });
            if (ds1.Cache.ContainsKey(description))
            {
                return (SeriesIsAbove)ds1.Cache[description];
            }

            SeriesIsAbove _SeriesIsAbove = new SeriesIsAbove(ds1, ds2, period, description);
            ds1.Cache[description] = _SeriesIsAbove;
            return _SeriesIsAbove;
        }
    }

    public class SeriesIsAboveHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SeriesIsAboveHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.High, CoreDataSeries.Close, new RangeBoundInt32(20, 5, 300) };
            _paramNames = new string[] { "1st Series", "2nd Series", "Period" };
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
                return "SeriesIsAbove returns true if Series1 has been above Series2 over the most-recent Period bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SeriesIsAbove);
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
                return "SeriesAboveBelowPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/SeriesIsAbove.ashx";
            }
        }
    }

    public class SeriesIsBelow : DataSeries
    {
        public SeriesIsBelow(DataSeries ds1, DataSeries ds2, int period, string description)
            : base(ds1, description)
        {
            int lp = Math.Min(ds1.Count, ds2.Count);
            DataSeries delta = ds2 - ds1;

            if (period > lp) return;

            int i = 0;
            for (int bar = period; bar < lp; bar++)
            {
                if (Lowest.Series(delta, period)[bar] > 0)
                    i++;
                else
                    i = 0;
                base[bar] = i;
            }
        }

        public static SeriesIsBelow Series(DataSeries ds1, DataSeries ds2, int period)
        {
            string description = string.Concat(new object[] { "Series is Below(", ds1.Description, ",", ds2.Description, ",", period, ")" });
            if (ds1.Cache.ContainsKey(description))
            {
                return (SeriesIsBelow)ds1.Cache[description];
            }

            SeriesIsBelow _SeriesIsBelow = new SeriesIsBelow(ds1, ds2, period, description);
            ds1.Cache[description] = _SeriesIsBelow;
            return _SeriesIsBelow;
        }
    }

    public class SeriesIsBelowHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SeriesIsBelowHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.High, CoreDataSeries.Close, new RangeBoundInt32(20, 5, 300) };
            _paramNames = new string[] { "1st Series", "2nd Series", "Period" };
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
                return "SeriesIsBelow returns true if Series1 has been below Series2 over the most-recent Period bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SeriesIsBelow);
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
                return "SeriesAboveBelowPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/SeriesIsBelow.ashx";
            }
        }
    }

    public class SeriesIsAboveValue : DataSeries
    {
        public SeriesIsAboveValue(DataSeries ds, double num, int period, string description)
            : base(ds, description)
        {
            int lp = ds.Count;
            DataSeries delta = ds - (double)num;
            delta += DataSeries.Abs(delta);
            delta /= delta;
            DataSeries aux = Sum.Series(delta, period) + 1.0e-9;  // keep in mind we're working with floating point numbers

            if (period > lp) return;

            for (int bar = period; bar < lp; bar++)
            {
                base[bar] = aux[bar];
            }
        }

        public static SeriesIsAboveValue Series(DataSeries ds, double num, int period)
        {
            string description = string.Concat(new object[] { "Series is Above Value(", ds.Description, ",", num, ",", period, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (SeriesIsAboveValue)ds.Cache[description];
            }

            SeriesIsAboveValue _SeriesIsAboveValue = new SeriesIsAboveValue(ds, num, period, description);
            ds.Cache[description] = _SeriesIsAboveValue;
            return _SeriesIsAboveValue;
        }
    }

    public class SeriesIsAboveValueHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SeriesIsAboveValueHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.High, new RangeBoundDouble(5, 2, 300), new RangeBoundInt32(20, 2, 300) };
            _paramNames = new string[] { "Data Series", "Value", "Period" };
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
                return "SeriesIsAboveValue returns true if DataSeries has been above a Value over the most-recent Period bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SeriesIsAboveValue);
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
                return "SeriesAboveBelowPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/SeriesIsAbove.ashx";
            }
        }
    }

    public class SeriesIsBelowValue : DataSeries
    {
        public SeriesIsBelowValue(DataSeries ds, double num, int period, string description)
            : base(ds, description)
        {
            int lp = ds.Count;
            DataSeries delta = (double)num - ds;
            delta += DataSeries.Abs(delta);
            delta /= delta;
            DataSeries aux = Sum.Series(delta, period) + 1.0e-9;  // keep in mind we're working with floating point numbers

            if (period > lp) return;

            for (int bar = period; bar < lp; bar++)
            {
                base[bar] = aux[bar];
            }
        }

        public static SeriesIsBelowValue Series(DataSeries ds, double num, int period)
        {
            string description = string.Concat(new object[] { "Series is Below Value(", ds.Description, ",", num, ",", period, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (SeriesIsBelowValue)ds.Cache[description];
            }

            SeriesIsBelowValue _SeriesIsBelowValue = new SeriesIsBelowValue(ds, num, period, description);
            ds.Cache[description] = _SeriesIsBelowValue;
            return _SeriesIsBelowValue;
        }
    }

    public class SeriesIsBelowValueHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SeriesIsBelowValueHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Low, new RangeBoundDouble(10, 2, 300), new RangeBoundInt32(20, 5, 300) };
            _paramNames = new string[] { "Data Series", "Value", "Period" };
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
                return "SeriesIsBelowValue returns true if DataSeries has been below a Value over the most-recent Period bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SeriesIsBelowValue);
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
                return "SeriesAboveBelowPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/SeriesIsBelow.ashx";
            }
        }
    }
}