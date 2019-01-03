using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class Density : DataSeries
    {
        public Density(Bars bars, int NBars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = NBars;

            DataSeries Area = (Highest.Series(bars.High, NBars) - Lowest.Series(bars.Low, NBars));

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = Sum.Series(TrueRange.Series(bars), NBars)[bar] / Area[bar];
            }
        }

        public static Density Series(Bars bars, int NBars)
        {
            string description = string.Concat(new object[] { "Density(", NBars, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (Density)bars.Cache[description];
            }

            Density _Density = new Density(bars, NBars, description);
            bars.Cache[description] = _Density;
            return _Density;
        }
    }

    public class DensityHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DensityHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(5, 1, 10) };
            _paramNames = new string[] { "Bars", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Black;
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
                return "The Density indicator created by Michael R. Bryant calculates the density of a consolidation pattern over the past N bars.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Density);
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
                return "Density";
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www.breakoutfutures.com/Newsletters/Newsletter0803.htm";
            }
        }
    }
}