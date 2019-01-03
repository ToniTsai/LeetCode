using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators.Trailing
{
    public class SmoothedParabolic : DataSeries
    {
        public SmoothedParabolic(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period * 3;
            EMACalculation em = EMACalculation.Modern;

            DataSeries o = EMA.Series(bars.Open, period, em);
            DataSeries h = EMA.Series(bars.High, period, em);
            DataSeries l = EMA.Series(bars.Low, period, em);
            DataSeries c = EMA.Series(bars.Close, period, em);

            Bars newBars = new Bars(bars);
            for (int bar = 0; bar < bars.Count; bar++)
            {
                newBars.Add(Date[bar], o[bar], h[bar], l[bar], c[bar], bars.Volume[bar]);
            }

            DataSeries SmoothedPar = Parabolic2.Series(newBars, 0.02, 0.02, 0.2);

            for (int bar = base.FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = SmoothedPar[bar];
            }
        }

        public static SmoothedParabolic Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "SmoothedParabolic(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (SmoothedParabolic)bars.Cache[description];
            }

            SmoothedParabolic _SmoothedParabolic = new SmoothedParabolic(bars, period, description);
            bars.Cache[description] = _SmoothedParabolic;
            return _SmoothedParabolic;
        }
    }

    public class SmoothedParabolicHelper : IndicatorHelper
    {
        private static object[] _defaultValues = { BarDataType.Bars, new RangeBoundInt32(14, 2, 300) };
        private static string[] _descriptions = { "Bars", "Period" };

        public override Color DefaultColor
        {
            get
            {
                return Color.Navy;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Dots;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 3;
            }
        }

        public override string Description
        {
            get
            {
                return @"Smoothed Parabolic uses EMA-smoothed extreme points.";
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www2.wealth-lab.com/WL5Wiki/SmoothedParabolic.ashx";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(SmoothedParabolic);
            }
        }

        public override IList<object> ParameterDefaultValues
        {
            get
            {
                return _defaultValues;
            }
        }

        public override IList<string> ParameterDescriptions
        {
            get
            {
                return _descriptions;
            }
        }
    }
}