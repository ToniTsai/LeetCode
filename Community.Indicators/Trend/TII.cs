using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class TII : DataSeries
    {
        public TII(DataSeries ds, int period, int ma_period, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period, ma_period);

            /* If price is above the moving average, a positive deviation is recorded, 
            and if price is below the moving average a negative deviation. 
            The deviation is simply the distance between price and the moving average.

            Once the deviations are calculated, TII is calculated as:
            ( Sum of Positive Dev ) / ( ( Sum of Positive Dev ) + ( Sum of Negative Dev ) ) * 100 */

            DataSeries pos = new DataSeries(ds, "Positive Deviations(" + ds.Description + "," + period + "," + ma_period + ")");
            DataSeries neg = new DataSeries(ds, "Negative Deviations(" + ds.Description + "," + period + "," + ma_period + ")");
            //Community.Indicators.SMA ma = Community.Indicators.SMA.Series(ds, ma_period); // MA <-- results in unstable values!!!
            DataSeries ma = WealthLab.Indicators.SMA.Series(ds, ma_period); // MA

            for (int i = FirstValidValue; i < ds.Count; i++)
            {
                double p_diff = ds[i] - ma[i];
                double n_diff = ma[i] - ds[i];
                pos[i] = (p_diff > 0) ? p_diff : 0;
                neg[i] = (n_diff > 0) ? n_diff : 0;
            }

            DataSeries SDPos = Sum.Series(pos, period);
            DataSeries SDNeg = Sum.Series(neg, period);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = SDPos[bar] / (SDPos[bar] + SDNeg[bar]) * 100d;
            }
        }

        public static TII Series(DataSeries ds, int period, int ma_period)
        {
            string description = string.Concat(new object[] { "TII(", ds.Description, ",", period, ",", ma_period, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (TII)ds.Cache[description];
            }

            TII _TII = new TII(ds, period, ma_period, description);
            ds.Cache[description] = _TII;
            return _TII;
        }
    }

    public class TIIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static TIIHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(30, 2, 300), new RangeBoundInt32(60, 2, 300) };
            _paramNames = new string[] { "Data Series", "Period", "MA Period" };
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
                return "Trend Intensity Index (TII) by M.H. Pee is used to indicate the strength of a current trend in the market.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(TII);
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 80.0;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return 20.0;
            }
        }


        public override string TargetPane
        {
            get
            {
                return "TII";
            }
        }


        public override bool IsOscillator
        {
            get
            {
                return true;
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
    }
}