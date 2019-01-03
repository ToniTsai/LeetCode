using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class VK_WL_Band : DataSeries
    {
        public VK_WL_Band(DataSeries ds, int IntervalSize, int IntervalCount, string description)
            : base(ds, description)
        {
            //base.FirstValidValue = period;

            if (ds.Count < Math.Max(IntervalSize, IntervalCount))
                return;

            double val = 0;
            for (int bar = IntervalSize * IntervalCount; bar < ds.Count; bar++)
            {
                val = 0;
                for (int sequenz = 0; sequenz < IntervalCount; sequenz++)
                {
                    val += (IntervalCount - 1 - sequenz + 1) * Lowest.Series(ds, IntervalSize)[bar - sequenz * IntervalSize];
                }
                val /= ((IntervalCount + 1) * (IntervalCount / 2));
                base[bar] = val;
            }
        }

        public static VK_WL_Band Series(DataSeries ds, int IntervalSize, int IntervalCount)
        {
            string description = string.Concat(new object[] { "VK WL Band(", ds.Description, ",", IntervalSize, ",", IntervalCount, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (VK_WL_Band)ds.Cache[description];
            }

            VK_WL_Band _VK_WL_Band = new VK_WL_Band(ds, IntervalSize, IntervalCount, description);
            ds.Cache[description] = _VK_WL_Band;
            return _VK_WL_Band;
        }
    }

    public class VK_WL_BandHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static VK_WL_BandHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(10, 2, 100) };
            _paramNames = new string[] { "Data series", "IntervalSize", "IntervalCount" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override Color DefaultBandColor
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
                return "Lower Band of the VKW Bands from the March 2005 issue of Active Trader Magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(VK_WL_Band);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(VK_WH_Band);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/VKWBands.ashx";
            }
        }
    }

    public class VK_WH_Band : DataSeries
    {
        public VK_WH_Band(DataSeries ds, int IntervalSize, int IntervalCount, string description)
            : base(ds, description)
        {
            //base.FirstValidValue = period;

            if (ds.Count < Math.Max(IntervalSize, IntervalCount))
                return;

            double val = 0;
            for (int bar = IntervalSize * IntervalCount; bar < ds.Count; bar++)
            {
                val = 0;
                for (int sequenz = 0; sequenz < IntervalCount; sequenz++)
                {
                    val += (IntervalCount - 1 - sequenz + 1) * Highest.Series(ds, IntervalSize)[bar - sequenz * IntervalSize];
                }
                val /= ((IntervalCount + 1) * (IntervalCount / 2));
                base[bar] = val;
            }
        }

        public static VK_WH_Band Series(DataSeries ds, int IntervalSize, int IntervalCount)
        {
            string description = string.Concat(new object[] { "VK WH Band(", ds.Description, ",", IntervalSize, ",", IntervalCount, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (VK_WH_Band)ds.Cache[description];
            }

            VK_WH_Band _VK_WH_Band = new VK_WH_Band(ds, IntervalSize, IntervalCount, description);
            ds.Cache[description] = _VK_WH_Band;
            return _VK_WH_Band;
        }
    }

    public class VK_WH_BandHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static VK_WH_BandHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(10, 2, 100) };
            _paramNames = new string[] { "Data series", "IntervalSize", "IntervalCount" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override Color DefaultBandColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override string Description
        {
            get
            {
                return "Higher Band of the VKW Bands from the March 2005 issue of Active Trader Magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(VK_WH_Band);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(VK_WL_Band);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/VKWBands.ashx";
            }
        }
    }

    public class VK_SL_Band : DataSeries
    {
        public VK_SL_Band(DataSeries ds, int IntervalSize, int IntervalCount, string description)
            : base(ds, description)
        {
            //base.FirstValidValue = period;

            if (ds.Count < Math.Max(IntervalSize, IntervalCount))
                return;

            double val = 0;
            for (int bar = IntervalSize * IntervalCount; bar < ds.Count; bar++)
            {
                val = 0;
                for (int sequenz = 0; sequenz < IntervalCount; sequenz++)
                {
                    val += Lowest.Series(ds, IntervalSize)[bar - sequenz * IntervalSize] / IntervalCount;
                }
                base[bar] = val;
            }
        }

        public static VK_SL_Band Series(DataSeries ds, int IntervalSize, int IntervalCount)
        {
            string description = string.Concat(new object[] { "VK SL Band(", ds.Description, ",", IntervalSize, ",", IntervalCount, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (VK_SL_Band)ds.Cache[description];
            }

            VK_SL_Band _VK_SL_Band = new VK_SL_Band(ds, IntervalSize, IntervalCount, description);
            ds.Cache[description] = _VK_SL_Band;
            return _VK_SL_Band;
        }
    }

    public class VK_SL_BandHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static VK_SL_BandHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(10, 2, 100) };
            _paramNames = new string[] { "Data series", "IntervalSize", "IntervalCount" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.LightBlue;
            }
        }

        public override Color DefaultBandColor
        {
            get
            {
                return Color.LightBlue;
            }
        }

        public override string Description
        {
            get
            {
                return "Lower Band of the VK Bands from the February 2005 issue of Active Trader Magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(VK_SL_Band);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(VK_SH_Band);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/VKBands.ashx";
            }
        }
    }

    public class VK_SH_Band : DataSeries
    {
        public VK_SH_Band(DataSeries ds, int IntervalSize, int IntervalCount, string description)
            : base(ds, description)
        {
            //base.FirstValidValue = period;

            if (ds.Count < Math.Max(IntervalSize, IntervalCount))
                return;

            double val = 0;
            for (int bar = IntervalSize * IntervalCount; bar < ds.Count; bar++)
            {
                val = 0;
                for (int sequenz = 0; sequenz < IntervalCount; sequenz++)
                {
                    val += Highest.Series(ds, IntervalSize)[bar - sequenz * IntervalSize] / IntervalCount;
                }
                base[bar] = val;
            }
        }

        public static VK_SH_Band Series(DataSeries ds, int IntervalSize, int IntervalCount)
        {
            string description = string.Concat(new object[] { "VK SH Band(", ds.Description, ",", IntervalSize, ",", IntervalCount, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (VK_SH_Band)ds.Cache[description];
            }

            VK_SH_Band _VK_SH_Band = new VK_SH_Band(ds, IntervalSize, IntervalCount, description);
            ds.Cache[description] = _VK_SH_Band;
            return _VK_SH_Band;
        }
    }

    public class VK_SH_BandHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static VK_SH_BandHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(10, 2, 200), new RangeBoundInt32(10, 2, 100) };
            _paramNames = new string[] { "Data series", "IntervalSize", "IntervalCount" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.LightCoral;
            }
        }

        public override Color DefaultBandColor
        {
            get
            {
                return Color.LightCoral;
            }
        }

        public override string Description
        {
            get
            {
                return "Higher Band of the VK Bands from the February 2005 issue of Active Trader Magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(VK_SH_Band);
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

        public override Type PartnerBandIndicatorType
        {
            get
            {
                return typeof(VK_SL_Band);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/VKBands.ashx";
            }
        }
    }
}