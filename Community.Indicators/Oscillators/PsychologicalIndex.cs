using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class PsychologicalIndex : DataSeries
    {
        public PsychologicalIndex(Bars bars, int period, string description)
            : base(bars, description)
        {
            base.FirstValidValue = period;

            DataSeries UpDay = Momentum.Series(bars.Close, 1);
            UpDay = ((UpDay / DataSeries.Abs(UpDay)) + 1) / 2;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                base[bar] = (WealthLab.Indicators.Sum.Series(UpDay, period)[bar] / period) * 100;
            }
        }

        public static PsychologicalIndex Series(Bars bars, int period)
        {
            string description = string.Concat(new object[] { "Psychological Index(", period, ")" });

            if (bars.Cache.ContainsKey(description))
            {
                return (PsychologicalIndex)bars.Cache[description];
            }

            PsychologicalIndex _PsychologicalIndex = new PsychologicalIndex(bars, period, description);
            bars.Cache[description] = _PsychologicalIndex;
            return _PsychologicalIndex;
        }
    }

    public class PsychologicalIndexHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static PsychologicalIndexHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, new RangeBoundInt32(12, 2, 300) };
            _paramNames = new string[] { "Bars", "Period" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Green;
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
                return "The Psychological Index is an overbought/oversold indicator described in the June 2000 Futures Magazine.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(PsychologicalIndex);
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
                return "PsychologicalIndex";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/PsychologicalIndex.ashx";
            }
        }

        public override bool IsOscillator
        {
            get
            {
                return true;
            }
        }

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 75;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return 25;
            }
        }

    }
}