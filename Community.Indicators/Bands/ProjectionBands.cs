using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class PBandUpper : DataSeries
    {
        public PBandUpper(Bars bars, int period, string description)
            : base(bars, description)
        {
            this.FirstValidValue = period;

            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < period)
                return;

            DataSeries PBtop_slope = LinearRegSlope.Series(bars.High, period);
            DataSeries Result = new DataSeries(bars, "temporary_upper");

            for (int bar = period - 1; bar < bars.Count; bar++)
            {
                Result[bar] = bars.High[bar];
                for (int L_back = 0; L_back <= period - 1; L_back++)
                {
                    Result[bar] = Math.Max(Result[bar], bars.High[bar - L_back] + (PBtop_slope[bar] * (L_back)));
                }
                base[bar] = Result[bar];
            }
        }

        public static PBandUpper Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "Projection Band Upper(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (PBandUpper)bars.Cache[description];
            }

            PBandUpper _PBandUpper = new PBandUpper(bars, period, description);
            bars.Cache[description] = _PBandUpper;
            return _PBandUpper;
        }
    }

    public class PBandUpperHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PBandUpperHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 5, 300) };
            _paramNames = new string[] { "Bars", "Period" };
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
                return Color.Blue;
            }
        }

        public override string Description
        {
            get
            {
                return "Projection Bands were developed by Mel Widner. Based on linear regression channels, they are similar to other types of envelopes.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PBandUpper);
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
                return typeof(PBandLower);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ProjectionBands.ashx";
            }
        }
    }

    public class PBandLower : DataSeries
    {
        public PBandLower(Bars bars, int period, string description)
            : base(bars, description)
        {
            this.FirstValidValue = period;

            if (FirstValidValue > bars.Count || FirstValidValue < 0)
                FirstValidValue = bars.Count;
            if (bars.Count < period)
                return;

            DataSeries PBbot_slope = LinearRegSlope.Series(bars.Low, period);
            DataSeries Result = new DataSeries(bars, "temporary_lower");

            for (int bar = period - 1; bar < bars.Count; bar++)
            {
                Result[bar] = bars.Low[bar];
                for (int L_back = 0; L_back <= period - 1; L_back++)
                {
                    Result[bar] = Math.Min(Result[bar], bars.Low[bar - L_back] + (PBbot_slope[bar] * (L_back)));
                }
                base[bar] = Result[bar];
            }
        }

        public static PBandLower Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "Projection Band Lower (", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (PBandLower)bars.Cache[description];
            }

            PBandLower _PBandLower = new PBandLower(bars, period, description);
            bars.Cache[description] = _PBandLower;
            return _PBandLower;
        }
    }

    public class PBandLowerHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PBandLowerHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(14, 5, 300) };
            _paramNames = new string[] { "Bars", "Period" };
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
                return Color.Blue;
            }
        }

        public override string Description
        {
            get
            {
                return "Projection Bands were developed by Mel Widner. Based on linear regression channels, they are similar to other types of envelopes.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PBandLower);
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
                return typeof(PBandUpper);
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ProjectionBands.ashx";
            }
        }
    }
}