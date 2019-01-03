using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class KijunSen : DataSeries
    {
        public KijunSen(Bars bars)
            : base(bars, "KijunSen")
        {
            int p2 = 26;
            base.FirstValidValue = p2;

            if (bars.Count < p2)
                return;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
                base[bar] = ((Highest.Series(bars.High, p2)[bar] + Lowest.Series(bars.Low, p2)[bar]) / 2);
        }

        public static KijunSen Series(Bars bars)
        {
            string description = string.Concat(new object[] { "KijunSen()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (KijunSen)bars.Cache[description];
            }

            KijunSen _KijunSen = new KijunSen(bars);
            bars.Cache[description] = _KijunSen;
            return _KijunSen;
        }
    }

    public class KijunSenHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static KijunSenHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Purple;
            }
        }

        public override string Description
        {
            get
            {
                return "The Kijun Sen is a component of the Ichimoku Kinko Hyo indicator that is used to measure momentum and future areas of support and resistance.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(KijunSen);
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
