using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Log : DataSeries
    {
        public Log(DataSeries ds, double newBase, string description)
            : base(ds, description)
        {
            for (int bar = 0; bar < ds.Count; bar++)
            {
                base[bar] = checked(Math.Log(ds[bar], newBase));
            }
        }

        public static Log Series(DataSeries ds, double newBase)
        {
            string description = string.Concat(new object[] { "Log(", ds.Description, ",", newBase, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (Log)ds.Cache[description];
            }

            Log _Log = new Log(ds, newBase, description);
            ds.Cache[description] = _Log;
            return _Log;
        }
    }

    public class LogHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static LogHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundDouble(2, 0.1, 100) };
            _paramNames = new string[] { "Data Series", "Base" };
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
                return 1;
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return false;
            }
        }

        public override string Description
        {
            get
            {
                return "Returns the logarithm of a specified data series in a specified base. This is not really an indicator per se, but a mathematical function.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Log);
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
                return "Log";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Log.ashx";
            }
        }
    }
}
