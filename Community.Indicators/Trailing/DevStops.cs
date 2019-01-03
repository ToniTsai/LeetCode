using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class DevStops : DataSeries
    {
        public DevStops(Bars bars, int period, double trFactor, double sdFactor, string description)
            : base(bars, description)
        {
            TR2DSeries tr2d = TR2DSeries.Series(bars);
            this.FirstValidValue = period;

            DataSeries avg = Community.Indicators.FastSMA.Series(tr2d, period * 2);
            StdDev sd = StdDev.Series(tr2d, period * 2, StdDevCalculation.Sample);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = trFactor * avg[bar] + sdFactor * sd[bar];
            }
        }

        public static DevStops Series(Bars bars, int period, double trFactor, double sdFactor)
        {
            string description = string.Concat(new object[] { "DevStops(", period, ",", trFactor, ",", sdFactor, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (DevStops)bars.Cache[description];
            }

            DevStops _DevStops = new DevStops(bars, period, trFactor, sdFactor, description);
            bars.Cache[description] = _DevStops;
            return _DevStops;
        }
    }

    public class DevStopsHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DevStopsHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(20, 2, 300),
                new RangeBoundDouble(1, 1, 2), new RangeBoundInt32(1, 0, 4) };
            _paramNames = new string[] { "Bars", "Period", "trFactor", "sdFactor" };
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
                return 2;
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Dots;
            }

        }

        public override string Description
        {
            get
            {
                return "Kase DevStop was created by Cynthia Kase, www.kaseco.com. "
                 + "The Kase DevStop Distance indicator calculates the average distance of the DevStop from the price."
                 + "Usually stops are calculated relative highest price in trade for long trades "
                 + "and relative lowest price in trade for short trade.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DevStops);
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
                return "KaseDevStopDistance";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www.kaseco.com/";
            }
        }
    }

    public class TR2DSeries : DataSeries
    {
        public TR2DSeries(Bars bars, string description)
            : base(bars, description)
        {
            this.FirstValidValue = 2;
            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                double h3 = Math.Max(Highest.Series(bars.High, 2)[bar], bars.Close[bar - 2]);
                double l3 = Math.Min(Lowest.Series(bars.Low, 2)[bar], bars.Close[bar - 2]);
                base[bar] = h3 - l3;
            }
        }

        public static TR2DSeries Series(Bars bars)
        {
            string description = string.Concat(new object[] { "True Range Two Days()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (TR2DSeries)bars.Cache[description];
            }

            TR2DSeries _TR2DSeries = new TR2DSeries(bars, description);
            bars.Cache[description] = _TR2DSeries;
            return _TR2DSeries;
        }
    }

    public class TR2DSeriesHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TR2DSeriesHelper()
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
                return "True Range Two days is an extension of Wells Wilder's True Range. "
                + "It was proposed by Cynthia Ann Kase for use in her Kase Dev Stops.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TR2DSeries);
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
                return "TR2DSeries";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www.kaseco.com/";
            }
        }
    }

}
