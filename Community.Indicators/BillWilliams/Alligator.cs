using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class AlligatorJaw : DataSeries
    {
        public AlligatorJaw(DataSeries ds, int period, int delay, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period + delay;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = ShiftedMA.Series(ds, period, delay, ChoiceOfMA.SMMA)[bar];
            }
        }

        public static AlligatorJaw Series(DataSeries ds, int period, int delay)
        {
            string description = string.Concat(new object[] { "Alligator's Jaw(", ds.Description, ",", period, ",", delay, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (AlligatorJaw)ds.Cache[description];
            }

            AlligatorJaw _AlligatorJaw = new AlligatorJaw(ds, period, delay, description);
            ds.Cache[description] = _AlligatorJaw;
            return _AlligatorJaw;
        }
    }

    public class AlligatorJawHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AlligatorJawHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(13, 1, 300), new RangeBoundInt32(8, 1, 300) };
            _paramNames = new string[] { "DataSeries", "Period", "Delay" };
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
                return "Component of Alligator by Bill Williams. Alligator's Jaw is a 13-period moving average of the AveragePrice, which is delayed 8 bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AlligatorJaw);
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
                return "http://www2.wealth-lab.com/WL5Wiki/Alligator.ashx";
            }
        }
    }

    public class AlligatorTeeth : DataSeries
    {
        public AlligatorTeeth(DataSeries ds, int period, int delay, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period + delay;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < period)
                return;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = ShiftedMA.Series(ds, period, delay, ChoiceOfMA.SMMA)[bar];
            }
        }

        public static AlligatorTeeth Series(DataSeries ds, int period, int delay)
        {
            string description = string.Concat(new object[] { "Alligator's Teeth(", ds.Description, ",", period, ",", delay, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (AlligatorTeeth)ds.Cache[description];
            }

            AlligatorTeeth _AlligatorTeeth = new AlligatorTeeth(ds, period, delay, description);
            ds.Cache[description] = _AlligatorTeeth;
            return _AlligatorTeeth;
        }
    }

    public class AlligatorTeethHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AlligatorTeethHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(8, 1, 300), new RangeBoundInt32(5, 1, 300) };
            _paramNames = new string[] { "DataSeries", "Period", "Delay" };
        }

        public override string Description
        {
            get
            {
                return "Component of Alligator by Bill Williams. Alligator's Teeth is a 8-period moving average of the AveragePrice, which is delayed 5 bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AlligatorTeeth);
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

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Alligator.ashx";
            }
        }
    }

    public class AlligatorLips : DataSeries
    {
        public AlligatorLips(DataSeries ds, int period, int delay, string description)
            : base(ds, description)
        {
            base.FirstValidValue = period + delay;

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < (period + delay))
                return;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = ShiftedMA.Series(ds, period, delay, ChoiceOfMA.SMMA)[bar];
            }
        }

        public static AlligatorLips Series(DataSeries ds, int period, int delay)
        {
            string description = string.Concat(new object[] { "Alligator's Lips(", ds.Description, ",", period, ",", delay, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (AlligatorLips)ds.Cache[description];
            }

            AlligatorLips _AlligatorLips = new AlligatorLips(ds, period, delay, description);
            ds.Cache[description] = _AlligatorLips;
            return _AlligatorLips;
        }
    }

    public class AlligatorLipsHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static AlligatorLipsHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(5, 1, 300), new RangeBoundInt32(2, 1, 300) };
            _paramNames = new string[] { "DataSeries", "Period", "Delay" };
        }

        public override string Description
        {
            get
            {
                return "Component of Alligator by Bill Williams. Alligator's Lips is a 5-period moving average of the AveragePrice, which is delayed 2 bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(AlligatorLips);
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

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/Alligator.ashx";
            }
        }
    }
}