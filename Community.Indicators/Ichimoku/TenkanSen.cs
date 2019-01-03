using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class TenkanSen : DataSeries
    {
        public TenkanSen(Bars bars)
            : base(bars, "TenkanSen")
        {
            int p1 = 9;
            base.FirstValidValue = p1;

            if (bars.Count < p1)
                return;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
                base[bar] = ((Highest.Series(bars.High, p1)[bar] + Lowest.Series(bars.Low, p1)[bar]) / 2);
        }

        public static TenkanSen Series(Bars bars)
        {
            string description = string.Concat(new object[] { "TenkanSen()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (TenkanSen)bars.Cache[description];
            }

            TenkanSen _TenkanSen = new TenkanSen(bars);
            bars.Cache[description] = _TenkanSen;
            return _TenkanSen;
        }
    }

    public class TenkanSenHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TenkanSenHelper()
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

        public override string Description
        {
            get
            {
                return "The Tenkan Sen is a component of the Ichimoku Kinko Hyo indicator that is used to measure momentum and future areas of support and resistance.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TenkanSen);
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
                return @"http://www2.wealth-lab.com/WL5WIKI/Ichimoku.ashx";
            }
        }
    }
}
