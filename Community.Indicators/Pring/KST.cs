using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Linq;

namespace Community.Indicators
{
    public class KST : DataSeries
    {
        public KST(DataSeries ds, int roc1, int roc2, int roc3, int roc4, int sma1, int sma2, int sma3, int sma4, string description)
            : base(ds, description)
        {
            List<int> lstParams = new List<int>(new int[] { roc1, roc2, roc3, roc4, sma1, sma2, sma3, sma4 });
            base.FirstValidValue = lstParams.Max();

            DataSeries RCMA1 = Community.Indicators.FastSMA.Series(ROC.Series(ds, roc1), sma1);
            DataSeries RCMA2 = Community.Indicators.FastSMA.Series(ROC.Series(ds, roc2), sma2);
            DataSeries RCMA3 = Community.Indicators.FastSMA.Series(ROC.Series(ds, roc3), sma3);
            DataSeries RCMA4 = Community.Indicators.FastSMA.Series(ROC.Series(ds, roc4), sma4);
            DataSeries kst = (RCMA1) + (RCMA2 * 2) + (RCMA3 * 3) + (RCMA4 * 4);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                base[bar] = kst[bar];
            }
        }

        public static KST Series(DataSeries ds, int roc1, int roc2, int roc3, int roc4, int sma1, int sma2, int sma3, int sma4)
        {
            string description = string.Concat(new object[] { "KST(", roc1, ",", roc2, ",", roc3, ",", roc4, ",",
                sma1, "," ,sma2, "," ,sma3, "," ,sma4, "," ,")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (KST)ds.Cache[description];
            }

            KST _KST = new KST(ds, roc1, roc2, roc3, roc4, sma1, sma2, sma3, sma4, description);
            ds.Cache[description] = _KST;
            return _KST;
        }
    }

    public class KSTHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static KSTHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, 
                new RangeBoundInt32(10, 2, 300), new RangeBoundInt32(15, 2, 300), new RangeBoundInt32(20, 2, 300), new RangeBoundInt32(30, 2, 300),
                new RangeBoundInt32(10, 2, 300), new RangeBoundInt32(10, 2, 300), new RangeBoundInt32(10, 2, 300), new RangeBoundInt32(15, 2, 300) };
            _paramNames = new string[] { "DataSeries", "ROC1 Period", "ROC2 Period", "ROC3 Period", "ROC4 Period",
                "SMA1 Period", "SMA2 Period", "SMA3 Period", "SMA4 Period",};
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

        public override string Description
        {
            get
            {
                return "KST (Know Sure Thing) by Martin Pring is a momentum oscillator based on the smoothed rate-of-change of four different periods.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(KST);
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

        public override string TargetPane
        {
            get
            {
                return "KST";
            }
        }

        public override string URL
        {
            get
            {
                return @"http://stockcharts.com/school/doku.php?id=chart_school:technical_indicators:know_sure_thing_kst";
            }
        }
    }
}
