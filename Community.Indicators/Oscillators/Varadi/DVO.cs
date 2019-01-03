using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Linq;

namespace Community.Indicators
{
    /// <summary>
    /// DVO courtesy avishn
    /// </summary>
    public class DVO : DataSeries
    {
        Bars bars;

        double[] w;	// individual bar prices' weights
        double[] s;	// prior bars' weights

        public DVO(Bars bars, double[] w, double[] s, string description)
            : base(bars, description)
        {

            this.bars = bars;
            this.w = w;
            this.s = s;

            this.FirstValidValue = s.Length - 1;

            for (int bar = FirstValidValue; bar < bars.Count; bar++)
            {
                double v = 0d;
                for (int i = 0; i < s.Length; i++)
                {
                    int b = bar - s.Length + 1 + i;
                    v += (bars.Close[b] / (w[0] * bars.Open[b] + w[1] * bars.High[b] + w[2] * bars.Low[b] + w[3] * bars.Close[b]) - 1d) * s[i];
                }
                base[bar] = v;
            }

        }

        public static DVO Series(Bars bars, string w, string s)
        {
            return Series2(bars, w.Split(',').Select(v => Double.Parse(v)).ToArray(), s.Split(',').Select(v => Double.Parse(v)).ToArray());
        }

        public static DVO Series2(Bars bars, double[] w, double[] s)
        {
            string description = string.Concat(new object[] { "DVO(", bars.Symbol, ",[",
			                                   	String.Join(",", w.Select(v => v.ToString("F")).ToArray()), "],[",
			                                   	String.Join(",", s.Select(v => v.ToString("F")).ToArray()), "])" });
            if (bars.Cache.ContainsKey(description))
            {
                return (DVO)bars.Cache[description];
            }
            DVO _s = new DVO(bars, w, s, description);
            bars.Cache[description] = _s;
            return _s;
        }

        public override void CalculatePartialValue()
        {
            if (Double.IsNaN(bars.Close.PartialValue))
            {
                PartialValue = Double.NaN;
            }
            else
            {
                double v = 0d;
                for (int i = 0; i < s.Length - 1; i++)
                {
                    int b = bars.Count - s.Length + 1 + i;
                    v += bars.Close[b] / (w[0] * bars.Open[b] + w[1] * bars.High[b] + w[2] * bars.Low[b] + w[3] * bars.Close[b]) * s[i];
                }
                v += bars.Close.PartialValue / (w[0] * bars.Open.PartialValue + w[1] * bars.High.PartialValue + w[2] * bars.Low.PartialValue + w[3] * bars.Close.PartialValue) * s[s.Length - 1];
                PartialValue = v - 1d;
            }
        }
    }

    public class DVOHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static DVOHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, "0, 0.5, 0.5, 0", "0.5, 0.5" };
            _paramNames = new string[] { "Bars", "OHLC Weights", "Bar Weights" };
        }

        public override Color DefaultColor { get { return Color.Blue; } }
        public override string Description { get { return "DVO (PartialValue version)"; } }
        public override Type IndicatorType { get { return typeof(DVO); } }
        public override IList<object> ParameterDefaultValues { get { return _paramDefaults; } }
        public override IList<string> ParameterDescriptions { get { return _paramNames; } }
        public override string TargetPane { get { return "DVO"; } }
        public override string URL { get { return "http://daveab.com/mr_swing/index.htm"; } }
    }
}