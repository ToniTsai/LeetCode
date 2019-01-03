using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Sqrt : DataSeries
    {
        public Sqrt(DataSeries ds, string description)
            : base(ds, description)
        {
            for (int bar = 0; bar < ds.Count; bar++)
            {
                base[bar] = Math.Sqrt(ds[bar]);
            }
        }

        public static Sqrt Series(DataSeries ds)
        {
            string description = string.Concat(new object[] { "Sqrt(", ds.Description, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (Sqrt)ds.Cache[description];
            }

            Sqrt _Sqrt = new Sqrt(ds, description);
            ds.Cache[description] = _Sqrt;
            return _Sqrt;
        }
    }

    public class SqrtHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SqrtHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close };
            _paramNames = new string[] { "Data Series" };
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
                return "Returns the square root of values from the specified Price Series. This is not really an indicator per se, but a mathematical function.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Sqrt);
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
                return "Sqrt";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Sqrt.ashx";
            }
        }
    }
}
