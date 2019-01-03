using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class VPT : DataSeries
    {
        public VPT(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 1;

            for (int bar = 1; bar < bars.Count; bar++)
            {
                base[bar] = ((bars.Close[bar] - bars.Close[bar - 1]) / bars.Close[bar - 1]) * bars.Volume[bar];
                base[bar] += base[bar - 1];
            }
        }

        public static VPT Series(Bars bars)
        {
            string description = string.Concat(new object[] { "VPT()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (VPT)bars.Cache[description];
            }

            VPT _VPT = new VPT(bars, description);
            bars.Cache[description] = _VPT;
            return _VPT;
        }
    }

    public class VPTHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static VPTHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
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
                return 2;
            }
        }

        public override string Description
        {
            get
            {
                return "The Volume/Price Trend is a cumulative momentum style indicator which ties together Volume with price action.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(VPT);
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
                return "VPT";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/VPT.ashx";
            }
        }
    }
}