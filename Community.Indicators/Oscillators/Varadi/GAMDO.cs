using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    /// <summary>
    /// GAMDO thanks to avishn
    /// </summary>
    public class GAMDO : DataSeries
    {
        /*
			The creation of the oscillator involves taking the arithmetic average of the percentage returns (or 1-day ROCs) over the last 5 days.  
			A geometric average is created by simply adding 1 to the percentage returns and taking the product of this series over the last 5 days 
			(the cumulative return). 
			The geometric average is derived by subtracting 1 from the cumulative return. 
			Then we subtract the arithmetic return from the geometric return to find the divergence. 
			This divergence is smoothed twice using a 3-day average to create smooth signals (note that the raw divergence produces signals in the same direction). 
			Finally this smoothed divergence is “bounded,” by taking the PERCENTRANK of the series going back 1-year.
		*/

        public GAMDO(Bars bars, string description)
            : base(bars, description)
        {
            base.FirstValidValue = 252;
            int period = 5;

            DataSeries ArAvg = Community.Indicators.FastSMA.Series(bars.Close / (bars.Close >> 1), period) - 1d;
            DataSeries GeomAvg = GMA.Series(bars.Close / (bars.Close >> 1), period) - 1d;
            DataSeries Diver = GeomAvg - ArAvg;
            DataSeries SmoothedDiver = Community.Indicators.FastSMA.Series(Diver, 3);

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = PercentRank.Series(SmoothedDiver, 252)[bar];
            }
        }

        public static GAMDO Series(Bars bars)
        {
            string description = string.Concat(new object[] { "GAMDO()" });

            if (bars.Cache.ContainsKey(description))
            {
                return (GAMDO)bars.Cache[description];
            }

            GAMDO _GAMDO = new GAMDO(bars, description);
            bars.Cache[description] = _GAMDO;
            return _GAMDO;
        }
    }

    public class GAMDOHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static GAMDOHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars };
            _paramNames = new string[] { "Bars object" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Violet;
            }
        }

        public override int DefaultWidth
        {
            get
            {
                return 2;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return -0.5;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 0.5;
            }
        }

        public override string Description
        {
            get
            {
                return "Geometric and Arithmetic Mean Divergence Oscillator (GAMDO) created by David Varadi is a short-term overbought/oversold oscillator.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(GAMDO);
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
                return "GAMDO";
            }
        }

        public override string URL
        {
            get
            {
                return "http://cssanalytics.wordpress.com/2009/09/25/geometric-and-arithmetic-mean-divergence-oscillator-gamdo/";
            }
        }
    }
}