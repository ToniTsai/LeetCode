using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class SenkouSpanA : DataSeries
    {
        public SenkouSpanA(Bars bars)
            : base(bars, "Senkou Span A")
        {
            int p1 = 9;
            int p2 = 26;
            int p3 = 52;
            int p4 = 26;

            if (bars.Count < p3)
                return;

            DataSeries TenkanSen = ((Highest.Series(bars.High, p1) + Lowest.Series(bars.Low, p1)) / 2);
            DataSeries KijunSen = ((Highest.Series(bars.High, p2) + Lowest.Series(bars.Low, p2)) / 2);
            DataSeries ssA = ((TenkanSen + KijunSen) / 2) >> p4;

            base.FirstValidValue = p3;


            for (int bar = p3; bar < bars.Count; bar++)
            {
                base[bar] = ssA[bar];
            }
        }

        public static SenkouSpanA Series(Bars bars)
        {
            string description = string.Concat(new object[] { "SenkouSpanA()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (SenkouSpanA)bars.Cache[description];
            }

            SenkouSpanA _SenkouSpanA = new SenkouSpanA(bars);
            bars.Cache[description] = _SenkouSpanA;
            return _SenkouSpanA;
        }
    }

    public class SenkouSpanAHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SenkouSpanAHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.FromArgb(0, 0, 255);
            }
        }

        public override Color DefaultBandColor
        {
            get
            {
                return Color.FromArgb(0, 0, 255);
            }
        }

        public override string Description
        {
            get
            {
                return "Senkou Span A and B are components of the Ichimoku Kinko Hyo indicator that is used to measure momentum and future areas of support and resistance.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SenkouSpanA);
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
                return typeof(SenkouSpanB);
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www2.wealth-lab.com/WL5WIKI/Ichimoku.ashx";
            }
        }
    }

    public class SenkouSpanB : DataSeries
    {
        public SenkouSpanB(Bars bars)
            : base(bars, "Senkou Span B")
        {
            int p1 = 9;
            int p2 = 26;
            int p3 = 52;
            int p4 = 26;

            if (bars.Count < p3)
                return;

            DataSeries TenkanSen = ((Highest.Series(bars.High, p1) + Lowest.Series(bars.Low, p1)) / 2);
            DataSeries KijunSen = ((Highest.Series(bars.High, p2) + Lowest.Series(bars.Low, p2)) / 2);
            DataSeries ssA = ((TenkanSen + KijunSen) / 2) >> p4;
            DataSeries ssB = ((Highest.Series(bars.High, p3) + Lowest.Series(bars.Low, p3)) / 2) >> p4;

            base.FirstValidValue = p3;

            for (int bar = p3; bar < bars.Count; bar++)
            {
                base[bar] = ssB[bar];
            }
        }

        public static SenkouSpanB Series(Bars bars)
        {
            string description = string.Concat(new object[] { "SenkouSpanB()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (SenkouSpanB)bars.Cache[description];
            }

            SenkouSpanB _SenkouSpanB = new SenkouSpanB(bars);
            bars.Cache[description] = _SenkouSpanB;
            return _SenkouSpanB;
        }
    }

    public class SenkouSpanBHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static SenkouSpanBHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.FromArgb(255, 0, 0);
            }
        }

        public override Color DefaultBandColor
        {
            get
            {
                return Color.FromArgb(255, 0, 0);
            }
        }

        public override string Description
        {
            get
            {
                return "Senkou Span A and B are components of the Ichimoku Kinko Hyo indicator that is used to measure momentum and future areas of support and resistance.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SenkouSpanB);
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
                return typeof(SenkouSpanA);
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www2.wealth-lab.com/WL5WIKI/Ichimoku.ashx";
            }
        }
    }
}