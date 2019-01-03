using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class UpDownVolume : DataSeries
    {
        public UpDownVolume(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;

            DataSeries up = new DataSeries(bars, ("UpDownVolume_UpVol_temp(" + period + ")"));
            DataSeries dn = new DataSeries(bars, ("UpDownVolume_DownVol_temp(" + period + ")"));

            for (int bar = period; bar < bars.Count; bar++)
            {
                double pv = bars.Close[bar] * bars.Volume[bar];

                if (bars.Close[bar] > bars.Close[bar - 1])
                    up[bar] = pv;
                else
                    dn[bar] = pv;
            }

            // (total volume of stock on up days over the last 50 days) / (total volume of stock on down days over the last 50 days.)
            DataSeries udRatio = Sum.Series(up, period) / Sum.Series(dn, period);

            for (int bar = period; bar < bars.Count; bar++)
            {
                base[bar] = udRatio[bar];
            }
        }

        public static UpDownVolume Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "Up/Down Volume(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (UpDownVolume)bars.Cache[description];
            }

            UpDownVolume _UpDownVolume = new UpDownVolume(bars, period, description);
            bars.Cache[description] = _UpDownVolume;
            return _UpDownVolume;
        }
    }

    public class UpDownVolumeHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static UpDownVolumeHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(50, 2, 300) };
            _paramNames = new string[] { "Bars", "Period" };
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
                return "The Up/Down Volume indicator calculates the ratio of (total volume on up days over the last N days) / (total volume on down days over the last N days.)";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(UpDownVolume);
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
                return "UpDownVolume";
            }
        }

        public override string URL
        {
            get
            {
                return "https://www.wealth-lab.com/Forum/Posts/30127";
            }
        }
    }
}