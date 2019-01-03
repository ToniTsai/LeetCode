using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class UDIDSRI : DataSeries
    {
        public UDIDSRI(DataSeries ds, int period, int power, int percentRankPeriod, int iteration, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(period, percentRankPeriod);

            DataSeries diff = ds - (ds >> 1);
            DataSeries mov = new DataSeries(ds, "mov(" + ds.Description + "," + period + "," + percentRankPeriod + ")");
            DataSeries movement = new DataSeries(ds, "movement(" + ds.Description + "," + period + "," + percentRankPeriod + ")");
            DataSeries r = DataSeries.Abs(ds / (ds >> 1) - 1);

            int u_iteration = iteration == 1 ? 1 : 2;

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                if (diff[bar] > 0)
                    mov[bar] = 1;

                if (diff[bar] < 0)
                    mov[bar] = -1;

                movement[bar] = mov[bar] * Math.Pow((1 + r[bar]), power);
            }

            DataSeries UDIDSRI_1st = Sum.Series(mov, period);
            UDIDSRI_1st = PercentRank.Series(UDIDSRI_1st, percentRankPeriod);

            DataSeries UDIDSRI_2nd = Sum.Series(movement, period);
            UDIDSRI_2nd = PercentRank.Series(UDIDSRI_2nd, percentRankPeriod);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = u_iteration == 1 ? UDIDSRI_1st[bar] : UDIDSRI_2nd[bar];
            }
        }

        public static UDIDSRI Series(DataSeries ds, int period, int power, int percentRankPeriod, int iteration)
        {
            string description = string.Concat(new object[] { "UDIDSRI(", ds.Description, ",", period, ",", power, ",", percentRankPeriod, ",", iteration, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (UDIDSRI)ds.Cache[description];
            }

            UDIDSRI _UDIDSRI = new UDIDSRI(ds, period, power, percentRankPeriod, iteration, description);
            ds.Cache[description] = _UDIDSRI;
            return _UDIDSRI;
        }
    }

    public class UDIDSRIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static UDIDSRIHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(20, 2, 300), 
                new RangeBoundInt32(1, 1, 5), new RangeBoundInt32(50, 2, 300), new RangeBoundInt32(1, 1, 2) };
            _paramNames = new string[] { "Data Series", "Period", "Power", "Percent Rank Period", "Iteration" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.GreenYellow;
            }
        }

        public override string Description
        {
            get
            {
                return "Up/Down and Intensity Day Summation Rank (UDIDSRI) by qusma.com is an indicator intended to pick out bottoms.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(UDIDSRI);
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 0.8;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return 0.2;
            }
        }

        public override string TargetPane
        {
            get
            {
                return "UDIDSRI";
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

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5WIKI/UDIDSRI.ashx";
            }
        }
    }
}
