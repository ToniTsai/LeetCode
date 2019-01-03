using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class MCO : DataSeries
    {
        public MCO(Bars advBars, Bars decBars, int period1, int period2, string description)
            : base(advBars, description)
        {
            DataSeries Advancers = advBars.Close;
            DataSeries Decliners = decBars.Close;
            EMACalculation md = EMACalculation.Modern;

            base.FirstValidValue = Math.Max(period1, period2) * 3;
            if (FirstValidValue <= 1) return;

            DataSeries AD_Diff = Advancers - Decliners;
            DataSeries MCO = EMA.Series(AD_Diff, period1, md) - EMA.Series(AD_Diff, period2, md);

            for (int bar = base.FirstValidValue; bar < advBars.Count; bar++)
            {
                base[bar] = MCO[bar];
            }
        }

        public static MCO Series(Bars advBars, Bars decBars, int period1, int period2)
        {
            string description = string.Concat(new object[] { "McClellan Oscillator(", advBars.Symbol, ",", decBars.Symbol, ",", period1, ",", period2, ")" });

            if (advBars.Cache.ContainsKey(description))
            {
                return (MCO)advBars.Cache[description];
            }

            MCO _MCO = new MCO(advBars, decBars, period1, period2, description);
            advBars.Cache[description] = _MCO;
            return _MCO;
        }
    }

    public class MCOHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static MCOHelper()
        {
            _paramDefaults = new object[] { BarDataType.Bars, BarDataType.Bars, new RangeBoundInt32(19, 2, 300), new RangeBoundInt32(39, 2, 300) };
            _paramNames = new string[] { "Advancers (Bars)", "Decliners (Bars)", "EMA Period1", "EMA Period2" };
        }

        public override string TargetPane
        {
            get
            {
                return "MCOPane";
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
                return "McClellan Oscillator is a market breadth indicator that is based on the difference between the number of advancing and declining issues on the NYSE.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(MCO);
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
                return "http://www2.wealth-lab.com/WL5Wiki/MCO.ashx";
            }
        }
    }
}