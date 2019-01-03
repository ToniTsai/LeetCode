using System;
using System.Collections.Generic;
using System.Text;
using WealthLab;
using WealthLab.Indicators;
using System.Drawing;

namespace Community.Indicators
{
    public class LaguerreRSI : DataSeries
    {
        public LaguerreRSI(DataSeries ds, double gamma, string description)
            : base(ds, description)
        {
            double L0 = 0; double L0Prev = 0;
            double L1 = 0; double L1Prev = 0;
            double L2 = 0; double L2Prev = 0;
            double L3 = 0; double CU = 0; double CD = 0;

            DataSeries X0 = ds * (1 - gamma);

            for (int bar = 1; bar < ds.Count; bar++)
            {
                L0 = X0[bar] + gamma * L0Prev;
                L1 = -gamma * L0 + L0Prev + gamma * L1Prev;
                L2 = -gamma * L1 + L1Prev + gamma * L2Prev;
                L3 = -gamma * L2 + L2Prev + gamma * L3;

                CU = CD = 0;

                if (L0 >= L1)
                    CU = L0 - L1;
                else
                    CD = L1 - L0;

                if (L1 >= L2)
                    CU = CU + L1 - L2;
                else
                    CD = CD + L2 - L1;

                if (L2 >= L3)
                    CU = CU + L2 - L3;
                else
                    CD = CD + L3 - L2;

                if ((CU + CD) != 0)
                    base[bar] = CU / (CU + CD) * 100;

                L0Prev = L0;
                L1Prev = L1;
                L2Prev = L2;
            }
        }

        public static LaguerreRSI Series(DataSeries ds, double gamma)
        {
            string description = string.Concat(new object[] { "LaguerreRSI(", ds.Description, ",", gamma, ")" });
            if (ds.Cache.ContainsKey(description))
            {
                return (LaguerreRSI)ds.Cache[description];
            }

            LaguerreRSI _LaguerreRSI = new LaguerreRSI(ds, gamma, description);
            ds.Cache[description] = _LaguerreRSI;
            return _LaguerreRSI;
        }
    }

    public class LaguerreRSIHelper : IndicatorHelper
    {
        private static object[] _paramDefaults;
        private static string[] _paramNames;

        static LaguerreRSIHelper()
        {
            _paramDefaults = new object[] { CoreDataSeries.Close, new RangeBoundDouble(0.5, 0.1, 0.9) };
            _paramNames = new string[] { "Data Series", "Gamma" };
        }

        public override Color DefaultColor
        {
            get
            {
                return Color.Red;
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

        public override double OscillatorOverboughtValue
        {
            get
            {
                return 80.0;
            }
        }

        public override double OscillatorOversoldValue
        {
            get
            {
                return 20.0;
            }
        }

        public override string Description
        {
            get
            {
                return "LaguerreRSI by John F. Ehlers is a smooth filter which uses a short amount of data. It's built around an idea that to delay the low frequency components much more than the high frequency components.";
            }
        }

        public override Type IndicatorType
        {
            get
            {
                return typeof(LaguerreRSI);
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
                //return "LaguerreRSI";
                return "RSI";
            }
        }

        public override string URL
        {
            get
            {
                return "http://www2.wealth-lab.com/WL5Wiki/LaguerreRSI.ashx";
            }
        }
    }
}
