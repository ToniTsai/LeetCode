using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class FractalUpBar : DataSeries
    {
        /// <summary>
        /// returns the first bar of the High in a fractal sequence if detected on the current bar
        /// normally you'd pass High for the DataSeries
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        private int GetFractalUpBar(int bar, DataSeries ds)
        {
            if (bar < 4) return -1;		// A fractal is a 5-bar pattern, minimum
            int fractalbar = bar - 2;
            double fVal = ds[fractalbar];

            if (fVal <= ds[bar] || fVal <= ds[bar - 1])		// then not a fractal
                return -1;

            int kount = 0;
            for (int n = bar - 3; n >= 0; n--)
            {
                if (ds[n] == fVal)
                {
                    fractalbar = n;
                    kount = 0;					// reset count for matches
                }
                else if (ds[n] > fVal)
                    break;
                else if (ds[n] < fVal)
                {
                    kount++;
                    if (kount == 2)
                        return fractalbar;
                }
            }
            return -1;
        }

        public FractalUpBar(DataSeries ds, string description)
            : base(ds, description)
        {
            base.FirstValidValue = 5;

            double lastF = 0;
            for (int bar = 5; bar < ds.Count; bar++)
            {
                double f = GetFractalUpBar(bar, ds);
                if (f > 0)
                    lastF = f;

                this[bar] = lastF;
            }
        }

        public static FractalUpBar Series(DataSeries ds)
        {
            string description = "FractalUpBar(" + ds.Description + ")";

            if (ds.Cache.ContainsKey(description))
            {
                return (FractalUpBar)ds.Cache[description];
            }

            FractalUpBar fractalUpBar = new FractalUpBar(ds, description);
            ds.Cache[description] = fractalUpBar;
            return fractalUpBar;
        }
    }

    public class FractalUpBarHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static FractalUpBarHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.High };
            _paramNames = new string[] { "DataSeries" };
        }

        public override string Description
        {
            get
            {
                return "Fractals per Bill Williams in 'Trading Chaos, Second Edition' are an indication of a change in market direction/behavior.  An up (or buy) fractal is a minimum of 5-bar pattern where the center bar's high is greater than the high of the two bars on either side of it. FractalUpBar returns the first bar of the High in the most recent fractal sequence as of the current bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(FractalUpBar);
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

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/FractalUpBar.ashx";
            }
        }


        public override string TargetPane
        {
            get
            {
                return "FractalBarPane";
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }
        }
    }

    public class FractalUp : DataSeries
    {
        public FractalUp(DataSeries ds, string description)
            : base(ds, description)
        {
            DataSeries fracUpBar = FractalUpBar.Series(ds);
            for (int n = 0; n < ds.Count; n++)
            {
                if (fracUpBar[n] > 0)
                {
                    base.FirstValidValue = n;
                    break;
                }
            }

            for (int bar = base.FirstValidValue; bar < ds.Count; bar++)
            {
                int fb = (int)fracUpBar[bar];
                this[bar] = ds[fb];
            }
        }

        public static FractalUp Series(DataSeries ds)
        {
            string description = "FractalUp(" + ds.Description + ")";

            if (ds.Cache.ContainsKey(description))
            {
                return (FractalUp)ds.Cache[description];
            }

            FractalUp FractalUp = new FractalUp(ds, description);
            ds.Cache[description] = FractalUp;
            return FractalUp;
        }

        public static double Value(int bar, DataSeries ds)
        {
            DataSeries fracUpBar = FractalUpBar.Series(ds);
            int fb = (int)fracUpBar[bar];
            return ds[fb];
        }
    }

    public class FractalUpHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static FractalUpHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.High };
            _paramNames = new string[] { "DataSeries" };
        }

        public override string Description
        {
            get
            {
                return "Fractals per Bill Williams in 'Trading Chaos, Second Edition' are an indication of a change in market direction/behavior.  An up (or buy) fractal is a minimum of 5-bar pattern where the center bar's high is greater than the high of the two bars on either side of it. FractalUp returns the value of the most recent up/buy fractal as of the current bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(FractalUp);
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

        public override Color DefaultColor
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
                return "http://www2.wealth-lab.com/WL5Wiki/FractalUp.ashx";
            }
        }


        public override string TargetPane
        {
            get
            {
                return "P";
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
    }

    public class FractalDownBar : DataSeries
    {
        /// <summary>
        /// returns the first bar of the Low in a fractal sequence if detected on the current bar
        /// normally you'd pass Low for the DataSeries
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="ds"></param>
        /// <returns></returns>
        private int GetFractalDownBar(int bar, DataSeries ds)
        {
            if (bar < 4) return -1;		// A fractal is a 5-bar pattern, minimum
            int fractalbar = bar - 2;
            double fVal = ds[fractalbar];

            if (fVal >= ds[bar] || fVal >= ds[bar - 1])		// then bar - 2 is not a fractal bar
                return -1;

            int kount = 0;
            for (int n = bar - 3; n >= 0; n--)
            {
                if (ds[n] == fVal)
                {
                    fractalbar = n;
                    kount = 0;					// reset count for matches
                }
                else if (ds[n] < fVal)
                    break;
                else if (ds[n] > fVal)
                {
                    kount++;
                    if (kount == 2)
                        return fractalbar;
                }
            }
            return -1;
        }

        public FractalDownBar(DataSeries ds, string description)
            : base(ds, description)
        {
            base.FirstValidValue = 5;

            double lastF = 0;
            for (int bar = 5; bar < ds.Count; bar++)
            {
                double f = GetFractalDownBar(bar, ds);
                if (f > 0)
                    lastF = f;

                this[bar] = lastF;
            }
        }

        public static FractalDownBar Series(DataSeries ds)
        {
            string description = "FractalDownBar(" + ds.Description + ")";

            if (ds.Cache.ContainsKey(description))
            {
                return (FractalDownBar)ds.Cache[description];
            }

            FractalDownBar FractalDownBar = new FractalDownBar(ds, description);
            ds.Cache[description] = FractalDownBar;
            return FractalDownBar;
        }
    }

    public class FractalDownBarHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static FractalDownBarHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Low };
            _paramNames = new string[] { "DataSeries" };
        }

        public override string Description
        {
            get
            {
                return "Fractals per Bill Williams in 'Trading Chaos, Second Edition' are an indication of a change in market direction/behavior.  An down (or sell) fractal is a minimum of 5-bar pattern where the center bar's low is below the low of the two bars on either side of it. FractalDownBar returns the first bar of the Low in the most recent fractal sequence as of the current bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(FractalDownBar);
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

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/FractalDownBar.ashx";
            }
        }


        public override string TargetPane
        {
            get
            {
                return "FractalBarPane";
            }
        }

        public override LineStyle DefaultStyle
        {
            get
            {
                return LineStyle.Solid;
            }
        }
    }

    public class FractalDown : DataSeries
    {
        public FractalDown(DataSeries ds, string description)
            : base(ds, description)
        {
            DataSeries fracDnBar = FractalDownBar.Series(ds);

            for (int n = 0; n < ds.Count; n++)
            {
                if (fracDnBar[n] > 0)
                {
                    base.FirstValidValue = n;
                    break;
                }
            }

            for (int bar = base.FirstValidValue; bar < ds.Count; bar++)
            {
                int fb = (int)fracDnBar[bar];
                this[bar] = ds[fb];
            }
        }

        public static FractalDown Series(DataSeries ds)
        {
            string description = "FractalDown(" + ds.Description + ")";

            if (ds.Cache.ContainsKey(description))
            {
                return (FractalDown)ds.Cache[description];
            }

            FractalDown FractalDown = new FractalDown(ds, description);
            ds.Cache[description] = FractalDown;
            return FractalDown;
        }

        public static double Value(int bar, DataSeries ds)
        {
            DataSeries fracDnBar = FractalDownBar.Series(ds);
            int fb = (int)fracDnBar[bar];
            return ds[fb];
        }
    }

    public class FractalDownHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static FractalDownHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Low };
            _paramNames = new string[] { "DataSeries" };
        }

        public override string Description
        {
            get
            {
                return "Fractals per Bill Williams in 'Trading Chaos, Second Edition' are an indication of a change in market direction/behavior.  An up (or buy) fractal is a minimum of 5-bar pattern where the center bar's low is less than the low of the two bars on either side of it. FractalDown returns the value of the most recent down/sell fractal as of the current bar.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(FractalDown);
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

        public override Color DefaultColor
        {
            get
            {
                return Color.Fuchsia;
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/FractalDown.ashx";
            }
        }


        public override string TargetPane
        {
            get
            {
                return "P";
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
    }
}