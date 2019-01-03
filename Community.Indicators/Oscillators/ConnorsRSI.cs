using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class ConnorsRSI : DataSeries
    {
        public ConnorsRSI(DataSeries ds, int periodRSI, int periodStreak, int periodPR, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(Math.Max(periodRSI, periodStreak), periodPR);
            if (FirstValidValue <= 1) return;

            ConsecDaysDown cdd = ConsecDaysDown.Series(ds, 0);
            ConsecDaysUp cdu = ConsecDaysUp.Series(ds, 0);
            DataSeries streak = new DataSeries(ds, "streak");

            for (int bar = 0; bar < ds.Count; bar++)
            {
                streak[bar] = cdd[bar] > 0 ? -cdd[bar] : cdu[bar] > 0 ? cdu[bar] : 0;
            }

            RSI rsi3 = RSI.Series(ds, periodRSI);
            RSI rsiStreak = RSI.Series(streak, periodStreak);
            ROC ret = ROC.Series(ds, 1);
            DataSeries pr = PercentRank.Series(ret, periodPR) * 100.0;
            DataSeries connorsRSI = (rsi3 + rsiStreak + pr) / 3;

            for (int bar = base.FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = connorsRSI[bar];
            }
        }

        public static ConnorsRSI Series(DataSeries ds, int periodRSI, int periodStreak, int periodPR)
        {
            string description = string.Concat(new object[] { "Connors RSI (", ds.Description, ",", periodRSI, ",", periodStreak, ",", periodPR, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (ConnorsRSI)ds.Cache[description];
            }

            ConnorsRSI _ConnorsRSI = new ConnorsRSI(ds, periodRSI, periodStreak, periodPR, description);
            ds.Cache[description] = _ConnorsRSI;
            return _ConnorsRSI;
        }
    }

    public class ConnorsRSIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static ConnorsRSIHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(3, 2, 300), new RangeBoundInt32(2, 2, 300), new RangeBoundInt32(100, 2, 300) };
            _paramNames = new string[] { "Data Series", "RSI Period", "Streak Period", "PercentRank Period" };
        }

        public override string TargetPane
        {
            get
            {
                return "ConnorsRSIPane";
            }
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Violet;
            }
        }

        public override string Description
        {
            get
            {
                return "Connors RSI developed by Larry Connors is a composite indicator comprised of three components: price momentum, duration of up/down trend, and relative magnitude of price changed. The resulting momentum oscillator's interpretation is similar to RSI.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(ConnorsRSI);
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

        public override double OscillatorOversoldValue
        {
            get
            {
                return 20;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 80;
            }
        }

        public override Color OscillatorOversoldColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override Color OscillatorOverboughtColor
        {
            get
            {
                return Color.Blue;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/ConnorsRSI.ashx";
            }
        }
    }
}
