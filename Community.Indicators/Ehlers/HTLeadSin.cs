using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class HTLeadSin : DataSeries
    {
        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public HTLeadSin(DataSeries ds, string description)
            : base(ds, description)
        {
            DataSeries Value = HTDCPhase.Series(ds);

            for (int bar = 0; bar < ds.Count; bar++)
            {
                base[bar] = Math.Sin(DegreeToRadian(Value[bar] + 45));
            }
        }

        public static HTLeadSin Series(DataSeries ds)
        {
            string description = string.Concat(new object[] { "HTLeadSin(", ds.Description, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (HTLeadSin)ds.Cache[description];
            }

            HTLeadSin _HTLeadSin = new HTLeadSin(ds, description);
            ds.Cache[description] = _HTLeadSin;
            return _HTLeadSin;
        }
    }

    public class HTLeadSinHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static HTLeadSinHelper()
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

        public override int DefaultWidth
        {
            get
            {
                return 1;
            }
        }

        public override string Description
        {
            get
            {
                return "The HTLeadSin is the sine of the DC Phase at a specific bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(HTLeadSin);
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
                return "HTSin";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/HTLeadSin.ashx";
            }
        }
    }
}
