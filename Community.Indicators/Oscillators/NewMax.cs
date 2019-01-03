using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class NewMax : DataSeries
    {
        public NewMax(DataSeries ds, int period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period;
            double h, l, x, Value; l = 0;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                h = Highest.Series(ds, period)[bar];
                l = Lowest.Series(ds, period)[bar];
                x = ds[bar];
                if ((h - l) > 0)
                {
                    Value = (x - l) / (h - l) * 200 - 100;
                }
                else
                    Value = 0;

                base[bar] = Value;
            }
        }

        public static NewMax Series(DataSeries ds, int period)
        {
            string description = string.Concat(new object[] { "NewMax(", ds.Description, ",", period, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (NewMax)ds.Cache[description];
            }

            NewMax _NewMax = new NewMax(ds, period, description);
            ds.Cache[description] = _NewMax;
            return _NewMax;
        }
    }

    public class NewMaxHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static NewMaxHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(24, 2, 300) };
            _paramNames = new string[] { "DataSeries", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
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
                return "The NewMax indicator by Dr. Rene Koch finds new highs and new lows.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(NewMax);
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
                return "NewMax";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/NewMax.ashx";
            }
        }
    }
}