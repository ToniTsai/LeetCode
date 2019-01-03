using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;
using System.Windows.Forms;

namespace Community.Indicators
{
    public enum CoppockCalculation
    {
        Option1, Option2
    }

    public class Coppock : DataSeries
    {
        public Coppock(DataSeries ds, int ROCPeriod1, int ROCPeriod2, int MAPeriod, CoppockCalculation option, string description)
            : base(ds, description)
        {
            base.FirstValidValue = Math.Max(MAPeriod, Math.Max(ROCPeriod1, ROCPeriod2));

            if (FirstValidValue > ds.Count || FirstValidValue < 0)
                FirstValidValue = ds.Count;
            if (ds.Count < Math.Max(ROCPeriod1, ROCPeriod2))
                return;

            DataSeries Coppock = WMA.Series((ROC.Series(ds, ROCPeriod1) + ROC.Series(ds, ROCPeriod2)), MAPeriod);
            DataSeries Coppock2 = WMA.Series((Community.Indicators.FastSMA.Series(ds, 22) / Community.Indicators.FastSMA.Series(ds >> 250, 22) - 1), 150);

            for (int bar = FirstValidValue; bar < ds.Count; bar++)
            {
                if (option == CoppockCalculation.Option1)
                    base[bar] = Coppock[bar];
                else
                    base[bar] = Coppock2[bar];
            }
        }

        public static Coppock Series(DataSeries ds, int ROCPeriod1, int ROCPeriod2, int MAPeriod, CoppockCalculation option)
        {
            string description = string.Concat(new object[] { "Coppock(", ds.Description, ",", ROCPeriod1, ",", ROCPeriod2, ",", MAPeriod, ",", option, ")" });

            if (ds.Cache.ContainsKey(description))
            {
                return (Coppock)ds.Cache[description];
            }

            Coppock _Coppock = new Coppock(ds, ROCPeriod1, ROCPeriod2, MAPeriod, option, description);
            ds.Cache[description] = _Coppock;
            return _Coppock;
        }
    }

    public class CoppockHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static CoppockHelper()
        {
            //DataSeries Series = Close; int ROCPeriod1 = 22; int ROCPeriod2 = 250; int MAPeriod = 150;

            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundInt32(22, 2, 300), 
                new RangeBoundInt32(250, 2, 300), new RangeBoundInt32(150, 2, 300), CoppockCalculation.Option1 };
            _paramNames = new string[] { "Data Series", "ROC Period 1", "ROC Period 2", "MA Period", "Calculation option" };
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
                return "The Coppock Curve is a long-term momentum indicator used primarily to recognize major bottoms in the stock market.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(Coppock);
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
                return "Coppock";
            }
        }

        public override string URL
        {
            get
            {
                return @"http://www2.wealth-lab.com/WL5Wiki/Coppock.ashx";
            }
        }
    }
}
