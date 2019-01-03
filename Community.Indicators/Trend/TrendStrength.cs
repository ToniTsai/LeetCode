using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class TrendStrengthA : DataSeries
    {
        public TrendStrengthA(DataSeries ds, int periodStart, int periodEnd, int step, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(periodStart, periodEnd);

            int dummy, valueInt, i;
            double price;

            if (periodStart > periodEnd)
            {
                dummy = periodStart;
                periodStart = periodEnd;
                periodEnd = dummy;
            }

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                valueInt = 0;
                price = ds[bar];
                i = periodStart;
                do
                {
                    if (price > Community.Indicators.FastSMA.Series(ds, i)[bar])
                    {
                        valueInt++;
                    }
                    else
                    {
                        valueInt--;
                    }
                    i += step;
                }
                while (i <= periodEnd);

                base[bar] = valueInt / (((periodEnd - periodStart) / step) + 1d) * 100;
            }
        }

        public static TrendStrengthA Series(DataSeries ds, int periodStart, int periodEnd, int step)
        {
            string description = string.Concat(new object[] { "TrendStrengthA(", ds.Description, ",", periodStart, ",", periodEnd, ",", step, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (TrendStrengthA)ds.Cache[description];
            }

            TrendStrengthA _TrendStrength = new TrendStrengthA(ds, periodStart, periodEnd, step, description);
            ds.Cache[description] = _TrendStrength;
            return _TrendStrength;
        }
    }

    public class TrendStrengthAHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TrendStrengthAHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(30, 2, 300), new RangeBoundInt32(110, 2, 300), new RangeBoundInt32(20, 2, 300) };
            _paramNames = new string[] { "Data Series", "Period Start", "Period End", "Step" };
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
                return "TrendStrengthA compares the current price to the price of various SMAs.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TrendStrengthA);
            }
        }

        public override string TargetPane
        {
            get
            {
                return "TrendStrengthA";
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
                return "http://www2.wealth-lab.com/WL5Wiki/TrendStrengthA.ashx";
            }
        }
    }

    public class TrendStrengthB : DataSeries
    {
        public TrendStrengthB(DataSeries ds, int periodStart, int periodEnd, int step, string description)
            : base(ds, description)
        {
            base.FirstValidValue = periodEnd + 1;

            int dummy, i;
            double price, Value;

            if (periodStart > periodEnd)
            {
                dummy = periodStart;
                periodStart = periodEnd;
                periodEnd = dummy;
            }

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                Value = 0;
                price = ds[bar];
                i = periodStart;
                do
                {
                    Value = (Value + price / Community.Indicators.FastSMA.Series(ds, i)[bar] - 1); //relative change from day to day
                    i += step;
                }
                while (i <= periodEnd);

                base[bar] = 100.0 * Value / ((periodEnd - periodStart / step) + 1d);  //calc average
            }
        }

        public static TrendStrengthB Series(DataSeries ds, int periodStart, int periodEnd, int step)
        {
            string description = string.Concat(new object[] { "TrendStrengthB(", ds.Description, ",", periodStart, ",", periodEnd, ",", step, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (TrendStrengthB)ds.Cache[description];
            }

            TrendStrengthB _TrendStrength = new TrendStrengthB(ds, periodStart, periodEnd, step, description);
            ds.Cache[description] = _TrendStrength;
            return _TrendStrength;
        }
    }

    public class TrendStrengthBHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TrendStrengthBHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 300), new RangeBoundInt32(180, 2, 300), new RangeBoundInt32(40, 2, 300) };
            _paramNames = new string[] { "Data Series", "Period Start", "Period End", "Step" };
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
                return "TrendStrengthB calculates the average relative(%) distance from the current price to the price of various SMAs.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TrendStrengthB);
            }
        }

        public override string TargetPane
        {
            get
            {
                return "TrendStrengthB";
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
                return "http://www2.wealth-lab.com/WL5Wiki/TrendStrengthB.ashx";
            }
        }
    }

    public class TrendStrengthC : DataSeries
    {
        public TrendStrengthC(DataSeries ds, int periodStart, int periodEnd, int step, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(periodStart, periodEnd);

            int dummy, valueInt, i;
            double price;

            if (periodStart > periodEnd)
            {
                dummy = periodStart;
                periodStart = periodEnd;
                periodEnd = dummy;
            }

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                valueInt = 0;
                price = ds[bar];
                i = periodStart;
                do
                {
                    if (Community.Indicators.FastSMA.Series(ds, i)[bar] / Community.Indicators.FastSMA.Series(ds, i)[bar - 1] > 1)
                    {
                        valueInt++;
                    }
                    else
                    {
                        valueInt--;
                    }
                    i += step;
                }
                while (i <= periodEnd);

                base[bar] = valueInt / (((periodEnd - periodStart + 1) / step) + 1d) * 100;
            }
        }

        public static TrendStrengthC Series(DataSeries ds, int periodStart, int periodEnd, int step)
        {
            string description = string.Concat(new object[] { "TrendStrengthC(", ds.Description, ",", periodStart, ",", periodEnd, ",", step, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (TrendStrengthC)ds.Cache[description];
            }

            TrendStrengthC _TrendStrength = new TrendStrengthC(ds, periodStart, periodEnd, step, description);
            ds.Cache[description] = _TrendStrength;
            return _TrendStrength;
        }
    }

    public class TrendStrengthCHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TrendStrengthCHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(30, 2, 300), new RangeBoundInt32(110, 2, 300), new RangeBoundInt32(20, 2, 300) };
            _paramNames = new string[] { "Data Series", "Period Start", "Period End", "Step" };
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
                return "TrendStrengthC calculates the rate of rising vs. falling SMAs.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TrendStrengthC);
            }
        }

        public override string TargetPane
        {
            get
            {
                return "TrendStrengthC";
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
                return "http://www2.wealth-lab.com/WL5Wiki/TrendStrengthC.ashx";
            }
        }
    }

    public class TrendStrengthD : DataSeries
    {
        public TrendStrengthD(DataSeries ds, int periodStart, int periodEnd, int step, string description)
            : base(ds, description)
        {
            base.FirstValidValue = periodEnd + 1;

            int dummy, i;
            double price, Value;

            if (periodStart > periodEnd)
            {
                dummy = periodStart;
                periodStart = periodEnd;
                periodEnd = dummy;
            }

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                Value = 0;
                price = ds[bar];
                i = periodStart;
                do
                {
                    Value = Value + (Community.Indicators.FastSMA.Series(ds, i)[bar] / Community.Indicators.FastSMA.Series(ds, i)[bar - 1] - 1d); //use average rate of change of one day
                    i += step;
                }
                while (i <= periodEnd);

                base[bar] = 100.0 * Value / Convert.ToInt32((periodEnd - periodStart) / step + 1);
            }
        }

        public static TrendStrengthD Series(DataSeries ds, int periodStart, int periodEnd, int step)
        {
            string description = string.Concat(new object[] { "TrendStrengthD(", ds.Description, ",", periodStart, ",", periodEnd, ",", step, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (TrendStrengthD)ds.Cache[description];
            }

            TrendStrengthD _TrendStrength = new TrendStrengthD(ds, periodStart, periodEnd, step, description);
            ds.Cache[description] = _TrendStrength;
            return _TrendStrength;
        }
    }

    public class TrendStrengthDHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TrendStrengthDHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 300), new RangeBoundInt32(180, 2, 300), new RangeBoundInt32(40, 2, 300) };
            _paramNames = new string[] { "Data Series", "Period Start", "Period End", "Step" };
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
                return "TrendStrengthD calculates the average rate of change (ROC) of various SMAs.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TrendStrengthD);
            }
        }

        public override string TargetPane
        {
            get
            {
                return "TrendStrengthD";
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
                return "http://www2.wealth-lab.com/WL5Wiki/TrendStrengthD.ashx";
            }
        }
    }
}
