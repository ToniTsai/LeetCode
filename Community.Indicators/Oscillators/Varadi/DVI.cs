using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// DVI implemented by Michael Bytnar (DartboardTrader)
    /// </summary>
    public class DVI : DataSeries
    {
        public DVI(DataSeries B, string description)
            : base(B, description)
        {
            base.FirstValidValue = 252;

            DataSeries C = B / Community.Indicators.FastSMA.Series(B, 3) - 1;
            DataSeries D = (Community.Indicators.FastSMA.Series(C, 5) + (Community.Indicators.FastSMA.Series(C, 100) / 10)) / 2;
            DataSeries E = Community.Indicators.FastSMA.Series(D, 5);
            DataSeries F = GreaterThan.Series(B, (B >> 1), 1, -1);
            DataSeries G = (Sum.Series(F, 10) + (Sum.Series(F, 100) / 10)) / 2;
            DataSeries H = Community.Indicators.FastSMA.Series(G, 2);
            // DVI [Magnitude]
            DataSeries I = PercentRank.Series(E, 252);
            // DVI [Stretch]
            DataSeries J = PercentRank.Series(H, 252);
            DataSeries DVI = (0.8 * I) + (0.2 * J);

            for (int bar = base.FirstValidValue; bar < B.Count; bar++)
            {
                base[bar] = DVI[bar];
            }
        }

        public static DVI Series(DataSeries B)
        {
            string description = string.Concat(new object[] { "DV Intermediate Oscillator (DVI)(", B.Description, ")" });

            if (B.Cache.ContainsKey(description))
            {
                return (DVI)B.Cache[description];
            }

            DVI _DVI = new DVI(B, description);
            B.Cache[description] = _DVI;
            return _DVI;
        }
    }

    public class DVIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DVIHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close };
            _paramNames = new string[] { "DataSeries" };
        }

        public override string TargetPane
        {
            get
            {
                return "DVIPane";
            }
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
                return "DV Intermediate Oscillator (DVI) by David Varadi is a very smooth momentum oscillator that can also be used as a trend indicator as well.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(DVI);
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
                return 0.25;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 0.75;
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
                return "http://cssanalytics.wordpress.com/2009/12/13/what-is-the-dvi/";
            }
        }
    }
}