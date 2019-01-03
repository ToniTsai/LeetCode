using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class GannSwingOscillator : DataSeries
    {
        public GannSwingOscillator(Bars bars, bool zero, string description)
            : base(bars, description)
        {
            bool SwingLo, SwingHi;
            base.FirstValidValue = 5;

            for (int bar = 5; bar < bars.Count; bar++)
            {
                SwingLo = (CumDown.Series(bars.Low, 1)[bar - 2] >= 2) &&
                    (CumUp.Series(bars.Low, 1)[bar] == 2);
                SwingHi = (CumUp.Series(bars.High, 1)[bar - 2] >= 2) &&
                    (CumDown.Series(bars.High, 1)[bar] == 2);

                if (SwingLo)
                    base[bar] = -1;
                else
                    if (SwingHi)
                        base[bar] = 1;
                    else
                        // Behavior choice
                        if (!zero)
                            base[bar] = this[bar - 1];
                        else
                            base[bar] = 0;
            }

        }

        public static GannSwingOscillator Series(Bars bars, bool zero)
        {
            string description = string.Concat(new object[] { "GannSwingOscillator(", zero, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (GannSwingOscillator)bars.Cache[description];
            }

            GannSwingOscillator _GannSwingOscillator = new GannSwingOscillator(bars, zero, description);
            bars.Cache[description] = _GannSwingOscillator;
            return _GannSwingOscillator;
        }
    }

    public class GannSwingOscillatorHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static GannSwingOscillatorHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new Boolean() };
            _paramNames = new string[] { "Bars", "True = with zero, False = discrete" };
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

        public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override string Description
        {
            get
            {
                return "The Gann Swing Oscillator helps define market swings.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(GannSwingOscillator);
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
                return "GannSwingOscillator";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/GannSwingOscillator.ashx";
            }
        }
    }

}
