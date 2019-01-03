using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class CrossOverBar : DataSeries
    {
        public CrossOverBar(DataSeries ds1, DataSeries ds2, string description)
            : base(ds1, description)
        {
            bool Crossover = false;
            base[0] = -1;

            base.FirstValidValue = Math.Max(ds1.FirstValidValue, ds2.FirstValidValue);

            for (int bar = 1; bar < ds1.Count; bar++)
            {
                Crossover = ((ds1[bar] > ds2[bar]) & (ds1[bar - 1] <= ds2[bar - 1]));

                if (Crossover)
                    base[bar] = bar;
                else
                    base[bar] = base[bar - 1];
            }
        }

        public static CrossOverBar Series(DataSeries ds1, DataSeries ds2)
        {
            string description = string.Concat(new object[] { "CrossOverBar(", ds1.Description, ",", ds2.Description, ")" });
            if (ds1.Cache.ContainsKey(description))
            {
                return (CrossOverBar)ds1.Cache[description];
            }

            CrossOverBar _CrossOverBar = new CrossOverBar(ds1, ds2, description);
            ds1.Cache[description] = _CrossOverBar;
            return _CrossOverBar;
        }
    }

    public class CrossOverBarHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CrossOverBarHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.High, CoreDataSeries.Close };
            _paramNames = new string[] { "1st Series", "2nd Series" };
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

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }

        }

        public override string Description
        {
            get
            {
                return "CrossOverBar returns the most recent bar where Series1 crossed over Series2.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CrossOverBar);
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
                return "CrossPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CrossOverBar.ashx";
            }
        }
    }

    public class CrossOverValueBar : DataSeries
    {
        public CrossOverValueBar(DataSeries ds, double value, string description)
            : base(ds, description)
        {
            bool Crossunder = false;
            base[0] = -1;

            base.FirstValidValue = ds.FirstValidValue;

            for (int bar = 1; bar < ds.Count; bar++)
            {
                Crossunder = ((ds[bar] > value) & (ds[bar - 1] <= value));

                if (Crossunder)
                    base[bar] = bar;
                else
                    base[bar] = base[bar - 1];
            }
        }

        public static CrossOverValueBar Series(DataSeries ds, double value)
        {
            string description = string.Concat(new object[] { "CrossOverValueBar(", ds.Description, ",", value.ToString(), ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (CrossOverValueBar)ds.Cache[description];
            }

            CrossOverValueBar _CrossOverValueBar = new CrossOverValueBar(ds, value, description);
            ds.Cache[description] = _CrossOverValueBar;
            return _CrossOverValueBar;
        }
    }

    public class CrossOverValueBarHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CrossOverValueBarHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundDouble(100, 1, 300) };
            _paramNames = new string[] { "Series", "Value" };
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

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }

        }

        public override string Description
        {
            get
            {
                return "CrossOverValueBar returns the most recent bar where Series crossed over Value.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CrossOverValueBar);
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
                return "CrossValuePane";
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

    public class CrossUnderBar : DataSeries
    {
        public CrossUnderBar(DataSeries ds1, DataSeries ds2, string description)
            : base(ds1, description)
        {
            bool Crossunder = false;
            base[0] = -1;

            base.FirstValidValue = Math.Max(ds1.FirstValidValue, ds2.FirstValidValue);

            for (int bar = 1; bar < ds1.Count; bar++)
            {
                Crossunder = ((ds1[bar] < ds2[bar]) & (ds1[bar - 1] >= ds2[bar - 1]));

                if (Crossunder)
                    base[bar] = bar;
                else
                    base[bar] = base[bar - 1];
            }
        }

        public static CrossUnderBar Series(DataSeries ds1, DataSeries ds2)
        {
            string description = string.Concat(new object[] { "CrossUnderBar(", ds1.Description, ",", ds2.Description, ")" });
            if (ds1.Cache.ContainsKey(description))
            {
                return (CrossUnderBar)ds1.Cache[description];
            }

            CrossUnderBar _CrossUnderBar = new CrossUnderBar(ds1, ds2, description);
            ds1.Cache[description] = _CrossUnderBar;
            return _CrossUnderBar;
        }
    }

    public class CrossUnderBarHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CrossUnderBarHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Low, CoreDataSeries.Close };
            _paramNames = new string[] { "1st Series", "2nd Series" };
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
                return 1;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }

        }

        public override string Description
        {
            get
            {
                return "CrossUnderBar returns the most recent bar where Series1 crossed under Series2.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CrossUnderBar);
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
                return "CrossPane";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/CrossUnderBar.ashx";
            }
        }
    }

    public class CrossUnderValueBar : DataSeries
    {
        public CrossUnderValueBar(DataSeries ds, double value, string description)
            : base(ds, description)
        {
            bool Crossunder = false;
            base[0] = -1;

            base.FirstValidValue = ds.FirstValidValue;

            for (int bar = 1; bar < ds.Count; bar++)
            {
                Crossunder = ((ds[bar] < value) & (ds[bar - 1] >= value));

                if (Crossunder)
                    base[bar] = bar;
                else
                    base[bar] = base[bar - 1];
            }
        }

        public static CrossUnderValueBar Series(DataSeries ds, double value)
        {
            string description = string.Concat(new object[] { "CrossUnderValueBar(", ds.Description, ",", value.ToString(), ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (CrossUnderValueBar)ds.Cache[description];
            }

            CrossUnderValueBar _CrossUnderValueBar = new CrossUnderValueBar(ds, value, description);
            ds.Cache[description] = _CrossUnderValueBar;
            return _CrossUnderValueBar;
        }
    }

    public class CrossUnderValueBarHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CrossUnderValueBarHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundDouble(100, 1, 300) };
            _paramNames = new string[] { "Series", "Value" };
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
                return 1;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }

        }

        public override string Description
        {
            get
            {
                return "CrossUnderValueBar returns the most recent bar where Series crossed under Value.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(CrossUnderValueBar);
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
                return "CrossValuePane";
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
}