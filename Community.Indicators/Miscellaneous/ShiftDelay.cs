using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Windows.Forms;

namespace Community.Indicators
{
    public class ShiftDelay : DataSeries
    {
        public ShiftDelay(DataSeries ds, int delay, string description)
            : base(ds, description)
        {
            base.FirstValidValue = ds.FirstValidValue + delay;

            if (ds.Count < ds.FirstValidValue + delay)
                return;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = ds[bar - delay];
            }
        }

        public static ShiftDelay Series(DataSeries ds, int delay)
        {
            string description = string.Concat(new object[] { "ShiftDelay(", ds.Description, ",", delay, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (ShiftDelay)ds.Cache[description];
            }

            ShiftDelay _ShiftDelay = new ShiftDelay(ds, delay, description);
            ds.Cache[description] = _ShiftDelay;
            return _ShiftDelay;
        }
    }

    public class ShiftDelayHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ShiftDelayHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(2, 1, 300) };
            _paramNames = new string[] { "DataSeries", "Delay" };
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
                return "The ShiftDelay Indicator delays any indicator by a period specified by user. It can dragged and dropped on any plotted series or indicator.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ShiftDelay);
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

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ShiftDelay.ashx";
            }
        }
    }
}
