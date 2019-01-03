using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Power : DataSeries
    {
        public Power(DataSeries ds, double y, string description)
            : base(ds, description)
        {
            for (int bar = 0; bar < ds.Count; bar++)
            {
                base[bar] = checked(Math.Pow(ds[bar], y));
            }
        }

        public static Power Series(DataSeries ds, double y)
        {
            string description = string.Concat(new object[] { "Power(", ds.Description, ",", y, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (Power)ds.Cache[description];
            }

            Power _Pow = new Power(ds, y, description);
            ds.Cache[description] = _Pow;
            return _Pow;
        }
    }

    public class PowerHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PowerHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundDouble(2, 0.5, 3) };
            _paramNames = new string[] { "Data Series", "Power" };
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
                return "Returns the specified Price Series raised to the specified power. This is not really an indicator per se, but a mathematical function.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Power);
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
                return "Power";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Power.ashx";
            }
        }
    }
}